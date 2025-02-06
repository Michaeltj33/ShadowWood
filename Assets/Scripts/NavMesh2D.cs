using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMesh2D : MonoBehaviour
{
    private NavMeshSurface2d navMeshSurface2D;

    public bool BuidStart;
    // Start is called before the first frame update
    void Start()
    {
        if (BuidStart)
        {
            navMeshSurface2D = GetComponent<NavMeshSurface2d>();

            navMeshSurface2D.BuildNavMesh();
        }
    }
}
