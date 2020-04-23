using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private WindDirectUI windDirectUI;

    private MemberAliveUI memberAliveUI;

    private GameOverUI gameOverUI;

    private ScoreUpUI scoreUpUI;

    private BasedirectionUI basedirectionUI;

    private GameClearUI gameClearUI;

    public WindDirectUI WindDirectUI { get => windDirectUI;}
    public MemberAliveUI MemberAliveUI { get => memberAliveUI; }
    public GameOverUI GameOverUI { get => gameOverUI; }
    public ScoreUpUI ScoreUpUI { get => scoreUpUI;}
    public BasedirectionUI BasedirectionUI { get => basedirectionUI;}
    public GameClearUI GameClearUI { get => gameClearUI; set => gameClearUI = value; }

    void Start()
    {
        
    }

    public void Initialize()
    {
        windDirectUI = GetComponentInChildren<WindDirectUI>();
        windDirectUI.Initialize();
        memberAliveUI = GetComponentInChildren<MemberAliveUI>();
        memberAliveUI.Initialize();
        gameOverUI = GetComponentInChildren<GameOverUI>();
        gameOverUI.Initialize();
        scoreUpUI = GetComponentInChildren<ScoreUpUI>();
        scoreUpUI.Initialize();
        basedirectionUI = GetComponentInChildren<BasedirectionUI>();
        basedirectionUI.Initialize();
        gameClearUI = GetComponentInChildren<GameClearUI>();
        gameClearUI.Initialize();
    }

    
    void Update()
    {
        
    }

    public void HiddenPlayUI()
    {
        windDirectUI.SetActive(false);
        memberAliveUI.SetActive(false);
        scoreUpUI.SetActive(false);
        gameClearUI.SetActive(false);
    }
}
