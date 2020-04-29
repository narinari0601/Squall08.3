using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperationUIScript : MonoBehaviour
{
    [SerializeField]
    private GameObject operationUI = null;

    void Start()
    {
        
    }

    public void Initialize()
    {
        SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActive(bool value)
    {
        if (value)
        {
            operationUI.SetActive(true);
        }

        else
        {
            operationUI.SetActive(false);
        }
    }
}
