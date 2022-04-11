using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

/// <summary>
/// Helper class that resolves dependencies in the Unity scene.
/// </summary>
public class DependencyResolver
{
    public void FindObjects(IEnumerable<GameObject> allGameObjects, List<MonoBehaviour> injectables)
    {
        foreach (GameObject gameObject in allGameObjects)
        {
            foreach (MonoBehaviour component in gameObject.GetComponents<MonoBehaviour>())
            {
                Type componentType = component.GetType();
                bool hasInjectableProperties = componentType.GetProperties()
                    .Where(IsMemberInjectable)
                    .Any();
                if (hasInjectableProperties)
                    injectables.Add(component);
                else
                {
                    bool hasInjectableFields = componentType.GetFields()
                        .Where(IsMemberInjectable)
                        .Any();
                    if (hasInjectableFields)
                        injectables.Add(component);
                }

                if (componentType.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(IsMemberInjectable)
                    .Any())
                {
                    Debug.LogError("Private properties should not be marked with [Inject] attribute!", component);
                }

                if(componentType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(IsMemberInjectable)
                    .Any())
                {
                    Debug.LogError("Private fields should not be marked with [Inject] attributes!", component);
                }
            }
        }
    }

    private bool IsMemberInjectable(MemberInfo member)
    {
        return member.GetCustomAttributes(true)
            .Where(attribute => attribute is InjectAttribute)
            .Count() > 0;
    }

    private IEnumerable<IInjectableMember> FindInjectableMembers(MonoBehaviour injectable)
    {
        Type type = injectable.GetType();
        IEnumerable<IInjectableMember> injectableProperties = type.GetProperties()
            .Where(IsMemberInjectable)
            .Select(p => new InjectableProperty(p))
            .Cast<IInjectableMember>();

        IEnumerable<IInjectableMember> injectableFields = type.GetFields()
            .Where(IsMemberInjectable)
            .Select(f => new InjectableField(f))
            .Cast<IInjectableMember>();

        return injectableProperties.Concat(injectableFields);
    }

    private IEnumerable<GameObject> GetAncestors(GameObject fromGameObject)
    {
        for (Transform p = fromGameObject.transform.parent; p != null; p = p.parent)
            yield return p.gameObject;
    }

    private IEnumerable<MonoBehaviour> FindMatchingDependencies(Type injectionType, GameObject gameObject)
    {
        foreach (MonoBehaviour m in gameObject.GetComponents<MonoBehaviour>())
            if (injectionType.IsAssignableFrom(m.GetType()))
                yield return m;
    }

    private MonoBehaviour FindMatchingDependency(Type injectionType, GameObject gameObject, MonoBehaviour injectable)
    {
        MonoBehaviour[] matchingDependencies = FindMatchingDependencies(injectionType, gameObject).ToArray();
        if (matchingDependencies.Length == 1)
            return matchingDependencies[0];

        if (matchingDependencies.Length == 0)
            return null;

        Debug.LogError("Found multiple hierarchy dependencies that match injection type " + injectionType.Name + " to be injected into '" + injectable.name + "'. See following warnings.", injectable);

        foreach (MonoBehaviour m in matchingDependencies)
            Debug.LogWarning(" Duplicate dependencies: '" + m.name + "'.", m);

        return null;
    }

    private MonoBehaviour FindDependencyInHierarchy(Type injectionType, MonoBehaviour injectable)
    {
        foreach(GameObject g in GetAncestors(injectable.gameObject))
        {
            MonoBehaviour dependency = FindMatchingDependency(injectionType, g, injectable);
            if (g != null)
                return dependency;
        }
        return null;
    }

    public interface IInjectableMember
    {
        void SetValue(object owner, object value);

        string Name { get; }

        Type MemberType { get; }

        string Category { get; }

        InjectFrom InjectFrom { get; }
    }

    public class InjectableProperty : IInjectableMember
    {
        private PropertyInfo propertyInfo;

        public InjectableProperty(PropertyInfo property)
        {
            propertyInfo = property;
            InjectAttribute injectableAttribute = propertyInfo.GetCustomAttributes(typeof(InjectAttribute), false)
                .Cast<InjectAttribute>()
                .Single();
            this.InjectFrom = injectableAttribute.InjectFrom;
        }

        public void SetValue(object owner, object value)
        {
            propertyInfo.SetValue(owner, value, null);
        }

        public string Name => propertyInfo.Name;

        public Type MemberType => propertyInfo.PropertyType;

        public string Category => "property";

        public InjectFrom InjectFrom { get; private set; }
    }

    public class InjectableField : IInjectableMember
    {
        private FieldInfo fieldInfo;

        public InjectableField(FieldInfo field)
        {
            fieldInfo = field;
            InjectAttribute injectAttribute = fieldInfo.GetCustomAttributes(typeof(InjectAttribute), false)
                .Cast<InjectAttribute>()
                .Single();
            this.InjectFrom = injectAttribute.InjectFrom;
        }

        public void SetValue(object owner, object value)
        {
            fieldInfo.SetValue(owner, value);
        }

        public string Name => fieldInfo.Name;

        public Type MemberType => fieldInfo.FieldType;

        public string Category => "field";

        public InjectFrom InjectFrom { get; private set; }
    }

    private bool ResolveMemberDenpendencyInHierarchy(MonoBehaviour injectable, IInjectableMember injectableMember)
    {
        MonoBehaviour toInject = FindDependencyInHierarchy(injectableMember.MemberType, injectable);
        if (toInject != null)
        {
            try
            {
                Debug.Log("Injecting " + toInject.GetType().Name + " from hierarchy (GameObject: '" + toInject.gameObject.name + "') into " + injectable.GetType().Name + " at " + injectableMember.Category + " " + injectableMember.Name + " on GameObject '" + injectable.name + "'.", injectable);

                injectableMember.SetValue(injectable, toInject);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex, injectable);
            }

            return true;
        }
        else
            return false;
    }

    private bool ResolveMemberDependencyFromAnywhere(MonoBehaviour injectable, IInjectableMember injectableMember)
    {
        if (injectableMember.MemberType.IsArray)
            return ResolveArrayDependencyFromAnywhere(injectable, injectableMember);
        else
            return ResolveObjectWithDependencyFromAnywhere(injectable, injectableMember);
    }

    private static bool ResolveArrayDependencyFromAnywhere(MonoBehaviour injectable, IInjectableMember injectableMember)
    {
        Type elementType = injectableMember.MemberType.GetElementType();
        UnityEngine.Object[] toInject = GameObject.FindObjectsOfType(elementType);
        if (toInject != null)
        {
            try
            {
                Debug.Log("Injecting array of " + toInject.Length + " elements into " + injectable.GetType().Name + " at " + injectableMember.Category + " " + injectableMember.Name + " on GameObject '" + injectable.name + "'.", injectable);

                foreach (MonoBehaviour m in toInject.Cast<MonoBehaviour>())
                    Debug.Log("> Injecting object " + m.GetType().Name + " (GameObject: '" + m.gameObject.name + "').", injectable);

                Array typedArray = Array.CreateInstance(elementType, toInject.Length);
                Array.Copy(toInject, typedArray, toInject.Length);

                injectableMember.SetValue(injectable, typedArray);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex, injectable);
            }

            return true;
        }
        else
            return false;
    }

    private static bool ResolveObjectWithDependencyFromAnywhere(MonoBehaviour injectable, IInjectableMember injectableMember)
    {
        MonoBehaviour toInject = (MonoBehaviour)GameObject.FindObjectOfType(injectableMember.MemberType);
        if (toInject != null)
        {
            try
            {
                Debug.Log("Injecting object " + toInject.GetType().Name + " (GameObject: '" + toInject.gameObject.name + "') into " + injectable.GetType().Name + " at " + injectableMember.Category + " " + injectableMember.Name + " on GameObject '" + injectable.name + "'.", injectable);

                injectableMember.SetValue(injectable, toInject);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex, injectable);
            }

            return true;
        }
        else
            return false;
    }

    private void ResolveMemberDependency(MonoBehaviour injectable, IInjectableMember injectableMember)
    {
        if (injectableMember.InjectFrom == InjectFrom.Above)
        {
            if (!ResolveMemberDenpendencyInHierarchy(injectable, injectableMember))
            {
                Debug.LogError(
                    "Failed to resolve dependency for " + injectableMember.Category + ". Member: " + injectableMember.Name + ", MonoBehaviour: " + injectable.GetType().Name + ", GameObject: " + injectable.gameObject.name + "\r\n" +
                    "Failed to find a dependency that matches " + injectableMember.MemberType.Name + ".",
                    injectable
                );
            }
        }
        else if (injectableMember.InjectFrom == InjectFrom.Anywhere)
        {
            if (!ResolveMemberDependencyFromAnywhere(injectable, injectableMember))
            {
                Debug.LogError(
                   "Failed to resolve dependency for " + injectableMember.Category + ". Member: " + injectableMember.Name + ", MonoBehaviour: " + injectable.GetType().Name + ", GameObject: " + injectable.gameObject.name + "\r\n" +
                   "Failed to find a dependency that matches " + injectableMember.MemberType.Name + ".",
                   injectable
                );
            }
        }
        else
        {
            throw new ApplicationException("Unexpected use of InjectFrom enum: " + injectableMember.InjectFrom);
        }
    }

    private void ResolveDependencies(MonoBehaviour injectable)
    {
        IEnumerable<IInjectableMember> injectableProperties = FindInjectableMembers(injectable);
        foreach (IInjectableMember i in injectableProperties)
            ResolveMemberDependency(injectable, i);
    }

    public void ResolveScene()
    {
        GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();
        Resolve(allGameObjects);
    }

    public void Resolve(GameObject parent)
    {
        GameObject[] gameObjects = new GameObject[] { parent };
        Resolve(gameObjects);
    }

    public void Resolve(IEnumerable<GameObject> gameObjects)
    {
        List<MonoBehaviour> injectables = new List<MonoBehaviour>();
        FindObjects(gameObjects, injectables);

        foreach (MonoBehaviour injectable in injectables)
            ResolveDependencies(injectable);
    }
}
