using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindDirectUI : MonoBehaviour
{
    private string[] directStrings = new string[4] { "↑", "↓", "←", "→" };

    [SerializeField]
    private GameObject windPanel = null; 

    [SerializeField]
    private Text currentDirect = null;

    [SerializeField]
    private Text secondDirect = null;

    [SerializeField]
    private Text thirdDirect = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize()
    {
        currentDirect.text = "";
        secondDirect.text = "";
        thirdDirect.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActive(bool value)
    {
        if (value)
        {
            windPanel.SetActive(true);
        }

        else
        {
            windPanel.SetActive(false);
        }
    }

    public void ChangeDirection(int current, int second, int third)
    {
        //currentDirect.text = directStrings[(int)squallDirArray[squallCount]];

        //var secondDir = squallDirArray[(squallCount + 1) % squallDirArray.Length];
        //var thirdDir = squallDirArray[(squallCount + 2) % squallDirArray.Length];
        //secondDirect.text = directStrings[(int)secondDir];
        //thirdDirect.text = directStrings[(int)thirdDir];

        currentDirect.text = directStrings[current];
        secondDirect.text = directStrings[second];
        thirdDirect.text = directStrings[third];
    }
}
