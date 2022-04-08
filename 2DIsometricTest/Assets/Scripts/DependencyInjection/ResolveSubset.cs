using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolveSubset : MonoBehaviour
{
    private void Awake()
    {
        DependencyResolver dr = new DependencyResolver();
        dr.Resolve(this.gameObject);
    }
}
