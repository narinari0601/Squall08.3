using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperationUIScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject OperationUI;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GamePlayManager.instance.GameState == GamePlayManager.GamePlayStates.Map)
        {
            OperationUI.SetActive(false);
        }
        else
        {
            OperationUI.SetActive(true);
        }
    }
}
