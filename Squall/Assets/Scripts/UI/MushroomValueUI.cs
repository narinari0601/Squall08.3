using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MushroomValueUI : MonoBehaviour
{
    [SerializeField]
    private GameObject mashroomValuePanel = null;

    [SerializeField]
    private Text valueText = null;

    private string value;

    void Start()
    {
        
    }

    public void Initialize()
    {
        value = "0";
        valueText.text = "× " + value;
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
            mashroomValuePanel.SetActive(true);
        }

        else
        {
            mashroomValuePanel.SetActive(false);
        }
    }

    public void MashValueUpdate()
    {
        var mashCount = GamePlayManager.instance.CurrentStage.PlayerController.Mashcount;

        value = mashCount.ToString();
        valueText.text= "× " + value;
    }
}
