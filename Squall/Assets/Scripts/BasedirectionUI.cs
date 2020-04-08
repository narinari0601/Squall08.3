using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasedirectionUI : MonoBehaviour
{
    public GameObject baseCamp;//拠点
    public GameObject baseUI;//拠点ＵＩ
    public GameObject DistanceUI;//距離表示用テキストＵＩ
    GameObject player;//プレイヤー
    Text text;//表示する文字
    int distance;//拠点とＵＩの距離
    public Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        text = DistanceUI.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (baseCamp == null)
        {
            baseCamp = GameObject.FindWithTag("BaseCamp");
        }
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        baseUI.transform.position = camera.WorldToScreenPoint(baseCamp.transform.position);
        if(baseUI.transform.position.x < 60)
        {
            baseUI.transform.position = new Vector3(60,baseUI.transform.position.y,baseUI.transform.position.z);
        }
        if (baseUI.transform.position.x > 1220)
        {
            baseUI.transform.position = new Vector3(1220, baseUI.transform.position.y, baseUI.transform.position.z);
        }
        if (baseUI.transform.position.y < 50)
        {
            baseUI.transform.position = new Vector3(baseUI.transform.position.x,50, baseUI.transform.position.z);
        }
        if (baseUI.transform.position.y > 670)
        {
            baseUI.transform.position = new Vector3(baseUI.transform.position.x,670, baseUI.transform.position.z);
        }

        //拠点とＵＩの距離を求め,表示する
        distance = (int)Vector3.Distance(camera.WorldToScreenPoint(player.transform.position), camera.WorldToScreenPoint(baseCamp.transform.position))/10;
        text.text = (distance-5).ToString() + "m";

        if(GamePlayManager.instance.GameState == GamePlayManager.GamePlayStates.Map)
        {
            baseUI.SetActive(false);
        }
        else
        {
            baseUI.SetActive(true);
        }
    }
}
