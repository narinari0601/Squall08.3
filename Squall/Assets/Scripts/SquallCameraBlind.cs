using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquallCameraBlind : MonoBehaviour
{
    //GamePlayManager gamePlayManager;
    GamePlayManager.WeatherStates currentstates;//現在の天気
    GamePlayManager.WeatherStates paststate;//１フレーム前の天気
    GamePlayManager.GamePlayStates currentstate2;//現在はマップを見てるか
    GamePlayManager.GamePlayStates paststate2;//さっきはマップを見てたか
    //Vector3 position;//視界阻害サイズ変更用
    public int direction;//ブロックごとの方向指定数字 0=した,1=うえ,2=みぎ,3=ひだり,4=,スプライトマスク
    int count;
    int cntfull;//スコール時に見える部分が小さくなる量（大きいと小さくなる）
    //int membercnt;//連れてる仲間の人数
    Transform transforms;
    GameObject player;//プレイヤー

    const float BlackMIN = 0.2114235f;//スコールの時の黒いやつの最小値
    const float BlackMAX = 0.6514235f;//スコールの時の黒いやつの最大値
    //菅原追加
    MemberList script;//メンバーリストを見るためのやつ
    private int memberCount = 0;//メンバーの人数をカウントするためのやつ
    private bool memberCheckFlag = false;//一回しか呼ばないためのフラグ
    private int memberCount2 = 0;//1f前の仲間の人数

    // Start is called before the first frame update
    void Start()
    {
        count = 21;
        currentstates = GamePlayManager.instance.Weather;
        transforms = GameObject.Find("View").transform;
        //gamePlayManager = GameObject.Find("GamePlayManager");
        cntfull = 21;     
    }

    // Update is called once per frame
    void Update()
    {
        
        if(cntfull>21)
        {
            cntfull = 21;
        }


        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            script = player.GetComponent<MemberList>();
        }   
        memberCount = script.memberList.Count - 1;//メンバーの数を確認(-1されてるのはリストにPlayerを含んでるから)
        //cntfull = 21 - memberCount;//仲間の数を常に代入、

        paststate = currentstates;
        currentstates = GamePlayManager.instance.Weather;

        if(currentstates == GamePlayManager.WeatherStates.Squall)
        {
            if(currentstate2 == GamePlayManager.GamePlayStates.Play && paststate2 == GamePlayManager.GamePlayStates.Map)
            {
                count = cntfull;
                transforms.localScale -= new Vector3(0.4f,0.4f,0.4f);
            }

            if (paststate == GamePlayManager.WeatherStates.Sign)
            {
                count = 0;
            }
            if (direction == 0)
            {
                if (count < cntfull)
                {
                    transform.Translate(Vector3.forward * 0.1f);
                    count++;
                }
            }
            if (direction == 1)
            {
                if (count < cntfull)
                {
                    transform.Translate(Vector3.back * 0.1f);
                    count++;
                }
            }
            if (direction == 2)
            {
                if  (count < cntfull)
                {
                    transform.Translate(Vector3.left*0.25f);
                    count++;
                }
            }
            if (direction == 3)
            {
                if (count < cntfull)
                {
                    transform.Translate(Vector3.right * 0.25f);
                    count++;
                }
            }
            if (direction == 4)
            {
                if (count <= cntfull)
                {
                    transforms.localScale -= new Vector3(0.02f, 0.02f, 0.02f);

                    count++;
                    memberCount2 = memberCount;
                }

                if (transforms.localScale.x <= BlackMIN)//もとに戻すときに最初の大きさより小さくならないようにするため
                {
                    transforms.localScale = new Vector3(BlackMIN, BlackMIN, BlackMIN);
                }

                if (memberCount > memberCount2) //スコール中に仲間が増えたら視界を広げ、減ったら視界を狭くする、
                {
                    transforms.localScale += new Vector3(0.08f, 0.08f, 0);
                    memberCount2 = memberCount;
                    cntfull -= 4;
                    count = 0;
                }
                else if (memberCount < memberCount2)
                {
                    transforms.localScale -= new Vector3(0.08f, 0.08f, 0);
                    memberCount2 = memberCount;
                    cntfull += 4;
                    count = 0;
                }

            }
        }
        else
        {
            if (currentstate2 == GamePlayManager.GamePlayStates.Play && paststate2 == GamePlayManager.GamePlayStates.Map)
            {
                count = cntfull;//+1消した
                transforms.localScale += new Vector3(0.4f, 0.4f, 0.4f);
            }
            if (paststate == GamePlayManager.WeatherStates.Squall)
            {
                count = 0;
            }
            if (direction == 0)
            {
                if (count < cntfull)
                {
                    transform.Translate(Vector3.back * 0.1f);
                    count++;
                }
            }
            if (direction == 1)
            {
                if (count < cntfull)
                {
                    transform.Translate(Vector3.forward * 0.1f);
                    count++;
                }
            }
            if (direction == 2)
            {
                if (count < cntfull)
                {
                    transform.Translate(Vector3.right * 0.25f);
                    count++;
                }
            }
            if (direction == 3)
            {
                if (count < cntfull)
                {
                    transform.Translate(Vector3.left * 0.25f);
                    count++;
                }
            }
            if (direction == 4)
            {
                if (count <= cntfull)
                {
                    transforms.localScale += new Vector3(0.02f,0.02f,0.02f);
                    count++;
                }

                if (transforms.localScale.x >= BlackMAX)//もとに戻すときに最初の大きさより大きくならないようにするため
                {
                    transforms.localScale = new Vector3(BlackMAX, BlackMAX, BlackMAX);
                }
                //memberCheckFlag = false;//フラグを戻してあげる
                if (memberCount > memberCount2) //スコール中に仲間が増えたら視界を広げ、減ったら視界を狭くする、
                {
                    memberCount2 = memberCount;
                    cntfull -= 4;
                }
                else if (memberCount < memberCount2)
                {
                    memberCount2 = memberCount;
                    cntfull += 4;
                }
            }

        }
    }
}
