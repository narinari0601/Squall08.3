using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    bool gameoverflag;
    public GameObject panelobj;
    public Text tex;
    Image panel;
    float red, green, blue;
    float alpha;
    float membercount;
    float currentmember;
    // Start is called before the first frame update
    void Start()
    {
        gameoverflag = true ;
        panel = panelobj.GetComponent<Image>();
        red = panel.color.r;
        green = panel.color.g;
        blue = panel.color.b;
        alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentmember =0;
        foreach(var a in GamePlayManager.instance.CurrentStage.MemberControllers)
        {
            if (a.GetMemberState == MemberControl.MemberStates.isDaed)
            {
                currentmember++;
            }
        }
        if (currentmember == membercount )  
        {
            panel.color = new Color(red, green, blue, alpha);
            alpha += 0.025f;
            if (alpha > 1)
            {
                tex.text = "味方が全滅した...";
                alpha = 1;
                if (Input.GetKeyDown(KeyCode.A))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                

            }
         
        }
        else
        {
            alpha--;
            if (alpha < 0)
            {
                alpha = 0;
            }
        }
    }

    public void Initialize()
    {
        gameoverflag = false;
        panel = panelobj.GetComponent<Image>();
        red = panel.color.r;
        green = panel.color.g;
        blue = panel.color.b;
        alpha = 0;

        membercount = GamePlayManager.instance.CurrentStage.MemberControllers.Length;
    }
}
