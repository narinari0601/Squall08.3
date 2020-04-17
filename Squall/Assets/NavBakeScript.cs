using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavBakeScript : MonoBehaviour
{
    private NavMeshSurface meshSurface;
    // Start is called before the first frame update
    void Start()
    {
        meshSurface = GetComponent<NavMeshSurface>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            meshSurface.BuildNavMesh();
        }
    }
}
