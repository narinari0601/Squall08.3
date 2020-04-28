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

    private OverviewUI overviewUI;

    private PauseUI pauseUI;


    public WindDirectUI WindDirectUI { get => windDirectUI;}
    public MemberAliveUI MemberAliveUI { get => memberAliveUI; }
    public GameOverUI GameOverUI { get => gameOverUI; }
    public ScoreUpUI ScoreUpUI { get => scoreUpUI;}
    public BasedirectionUI BasedirectionUI { get => basedirectionUI;}
    public GameClearUI GameClearUI { get => gameClearUI; set => gameClearUI = value; }
    public OverviewUI OverviewUI { get => overviewUI; set => overviewUI = value; }
    public PauseUI PauseUI { get => pauseUI; set => pauseUI = value; }

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
        overviewUI= GetComponentInChildren<OverviewUI>();
        overviewUI.Initialize();
        pauseUI= GetComponentInChildren<PauseUI>();
        pauseUI.Initialize();
    }

    
    void Update()
    {
        
    }

    public void SetActiveAllPlayUI(bool value)
    {
        windDirectUI.SetActive(value);
        memberAliveUI.SetActive(value);
        scoreUpUI.SetActive(value);
    }

    public void SetActiveAllOverviewUI(bool value)
    {
        overviewUI.SetActive(value);
    }
    
}
