using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolveScene : MonoBehaviour
{
    private void Awake()
    {
        DependencyResolver dependencyResolver = new DependencyResolver();
        dependencyResolver.ResolveScene();
    }
}
