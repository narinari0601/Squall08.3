using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MemberControl : MonoBehaviour
{
    public NavMeshAgent player;
    public Transform[] points;
    private int destPoint = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<NavMeshAgent>();
        player.autoBraking = false;
        GotoNextPoint();
    }

    public void GotoNextPoint()
    {
        if (points.Length == 0)
            return;
        player.destination = points[destPoint].transform.position;
        destPoint = (destPoint + 1) % points.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.pathPending && player.remainingDistance < 0.5f)
        {
            GotoNextPoint();
        }
    }
}
