using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasedirectionUI : MonoBehaviour
{
    [SerializeField, Header("大元のパネル")]
    private GameObject basePanel = null;

    private GameObject baseCamp;//拠点
    public GameObject baseUI = null;//拠点ＵＩ
    public GameObject DistanceUI = null;//距離表示用テキストＵＩ
    GameObject player;//プレイヤー
    Text text;//表示する文字
    int distance;//拠点とＵＩの距離
    public Camera m_camera;
    private bool flag;

    //public GameObject campUI;

    BaseCamp campScript;

    public GameObject BaseCamp { get => baseCamp;}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize()
    {
        text = DistanceUI.GetComponent<Text>();

        //baseUI.SetActive(false);

        //if (baseCamp == null)
        //{
        //    baseCamp = GameObject.FindWithTag("BaseCamp");
        //}
        //if (player == null)
        //{
        //    player = GameObject.FindGameObjectWithTag("Player");
        //}

        baseCamp = GamePlayManager.instance.CurrentStage.BaseCamp;
        player = GamePlayManager.instance.CurrentStage.PlayerObj;
        campScript = baseCamp.GetComponent<BaseCamp>();

        SetActive(false);
    }

    public void SetActive(bool value)
    {
        if (value)
        {
            basePanel.SetActive(true);
        }

        else
        {
            basePanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        flag = campScript.IsCameraCheck();
        //float speed = 0.1f;

        //Vector3 relativePos = baseCamp.transform.position - baseUI.transform.position;
        //Quaternion rotation = Quaternion.LookRotation(relativePos);
        //baseUI.transform.rotation = Quaternion.Slerp(baseUI.transform.rotation, rotation, speed);

        //baseUI.transform.rotation = Quaternion.Euler(90, 0, 0);
        //if (baseCamp)
        //{
        //    //Quaternion lookRotation = Quaternion.LookRotation(baseCamp.transform.position - baseUI.transform.position, Vector3.right);
        //    //lookRotation.x = 0;
        //    //lookRotation.y = 0;
        //    //baseUI.transform.rotation = Quaternion.Lerp(baseUI.transform.rotation, lookRotation, 0.1f);
        //    Quaternion lookRotation = Quaternion.LookRotation(baseCamp.transform.position - baseUI.transform.position, Vector3.right);
        //    lookRotation.x = 0;
        //    lookRotation.y = 0;
        //    baseUI.transform.rotation = Quaternion.Lerp(baseUI.transform.rotation, lookRotation, 0.3f);
        //}
        //BaseUIjA();
        //if (baseCamp == null)
        //{
        //    baseCamp = GameObject.FindWithTag("BaseCamp");
        //}
        //if(player == null)
        //{
        //    player = GameObject.FindGameObjectWithTag("Player");
        //}
        baseUI.transform.position = m_camera.WorldToScreenPoint(baseCamp.transform.position);
        if(baseUI.transform.position.x < 60)//60
        {
            baseUI.transform.position = new Vector3(60,baseUI.transform.position.y,baseUI.transform.position.z);
        }
        if (baseUI.transform.position.x > 1180)//220
        {
            baseUI.transform.position = new Vector3(1180, baseUI.transform.position.y, baseUI.transform.position.z);
        }
        if (baseUI.transform.position.y < 45)//50
        {
            baseUI.transform.position = new Vector3(baseUI.transform.position.x,45, baseUI.transform.position.z);
        }
        if (baseUI.transform.position.y > 630)//70
        {
            baseUI.transform.position = new Vector3(baseUI.transform.position.x,630, baseUI.transform.position.z);
        }

        //拠点とＵＩの距離を求め,表示する
        distance = (int)Vector3.Distance(m_camera.WorldToScreenPoint(player.transform.position), m_camera.WorldToScreenPoint(baseCamp.transform.position))/10;
        text.text = (distance-5).ToString() + "m";

        //if(GamePlayManager.instance.GameState == GamePlayManager.GamePlayStates.Map)
        //{
        //    baseUI.SetActive(false);

        //}
        //else
        //{
        //    baseUI.SetActive(true);

        //}

        //if (flag == true || GamePlayManager.instance.GameState != GamePlayManager.GamePlayStates.Play)//campScript.IsCameraCheck()
        //{
        //    //baseUI.SetActive(false);
        //    SetActive(false);

        //}
        //else if(!flag&& GamePlayManager.instance.GameState == GamePlayManager.GamePlayStates.Play)
        //{
        //    //baseUI.SetActive(true);
        //    SetActive(true);
        //}

        if (!flag && GamePlayManager.instance.GameState == GamePlayManager.GamePlayStates.Play)
        {
            SetActive(true);
        }

        else
        {
            SetActive(false);
        }
        


        //var diff = (player.transform.position - baseUI.transform.position).normalized;

        //diff.x = 90;
        //diff.y = 0;
        //baseUI.transform.rotation = Quaternion.FromToRotation(Vector3.right, diff);
       


    }

}
