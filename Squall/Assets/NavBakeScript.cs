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

    public void Initialize()
    {
        meshSurface = GetComponent<NavMeshSurface>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NavReBake()//再ベイク
    {
        meshSurface.BuildNavMesh();
    }
}
