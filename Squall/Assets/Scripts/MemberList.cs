using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberList : MonoBehaviour
{
    public List<GameObject> memberList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        memberList.Add(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {        
        
    }
}
