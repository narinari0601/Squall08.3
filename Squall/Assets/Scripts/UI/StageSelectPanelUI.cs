using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectPanelUI : MonoBehaviour
{
    [SerializeField,Header("ステージパネルプレハブ")]
    private GameObject stagePanelPrefab = null;

    void Start()
    {

    }

    public void Initialize()
    {
        var stageValue = StageSelectManager.instance.StageValue;

        for (int i = 1; i < stageValue + 1; i++)
        {
            var stage = Instantiate(stagePanelPrefab);
            stage.transform.SetParent(this.transform);
            stage.name = "Stage" + i + "Panel";
            var rank = StageData.StageRank(i - 1);
            stage.GetComponent<StagePanel>().Initialize(i, rank);
            StageSelectManager.instance.SelectObjcts[i - 1] = stage;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
