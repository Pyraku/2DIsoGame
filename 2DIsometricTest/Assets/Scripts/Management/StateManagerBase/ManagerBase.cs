using System;
using System.Reflection;
using UnityEngine;

public abstract class ManagerBase<T> : MonoBehaviour
    where T : Enum
{
    protected State_Base<T> m_currentState = null;
    [SerializeField] private T m_state;
    public T State => m_state;

    private void Awake()
    {
        m_currentState = null;
    }

    private void Update()
    {
        if (m_currentState != null)
            m_currentState.UpdateState();
    }

    public void SetState(T newState)
    {
        if (m_currentState != null)
            m_currentState.ExitState();

        m_state = newState;

        Type newType = Type.GetType(newState.ToString());
        if(newType == null)
        {
            Debug.LogError("No relevant class has been created: " + newState.ToString());
            return;
        }

        object newInstance = Activator.CreateInstance(newType, new object[] { this });

        m_currentState = (State_Base<T>)newInstance;

        if (m_currentState == null)
        {
            Debug.LogError("New class is null: " + newState.ToString());
            return;
        }

        m_currentState.EnterState();
    }
}