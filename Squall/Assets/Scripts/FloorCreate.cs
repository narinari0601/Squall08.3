using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCreate : MonoBehaviour
{
    [SerializeField,Header("X軸の枚数")]
    private int x = 1;
    [SerializeField,Header("X軸の間隔")]
    private float xInterval = 1.0f;
    [SerializeField,Header("Z軸の枚数")]
    private int z = 1;
    [SerializeField,Header("Z軸の間隔")]
    private float zInterval = 1.0f;

    [SerializeField, Header("作る対象")]
    private GameObject target = null;
    GameObject obj;


    public void Create()
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                obj = (GameObject)Instantiate(target, transform.position + new Vector3(xInterval * i, 0.0f, zInterval * j), Quaternion.identity);
                obj.name = target.name + "(" + i + "," + j + ")";
                obj.transform.parent = this.transform;
            }
        }
    }
}
