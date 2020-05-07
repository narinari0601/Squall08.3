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

    private OperationUIScript operationUI;

    private MushroomValueUI mushroomValueUI;


    public WindDirectUI WindDirectUI { get => windDirectUI;}
    public MemberAliveUI MemberAliveUI { get => memberAliveUI; }
    public GameOverUI GameOverUI { get => gameOverUI; }
    public ScoreUpUI ScoreUpUI { get => scoreUpUI;}
    public BasedirectionUI BasedirectionUI { get => basedirectionUI;}
    public GameClearUI GameClearUI { get => gameClearUI;}
    public OverviewUI OverviewUI { get => overviewUI; }
    public PauseUI PauseUI { get => pauseUI;}
    public OperationUIScript OperationUI { get => operationUI;}
    public MushroomValueUI MushroomValueUI { get => mushroomValueUI; }

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
        operationUI= GetComponentInChildren<OperationUIScript>();
        operationUI.Initialize();
        mushroomValueUI= GetComponentInChildren<MushroomValueUI>();
        mushroomValueUI.Initialize();
    }

    
    void Update()
    {
        
    }

    public void UpdatePlayUI()
    {
        memberAliveUI.MemberCount();
        mushroomValueUI.MashValueUpdate();
    }

    /// <summary>
    /// 「常に」表示しておくPlayUIをアクティブに
    /// </summary>
    public void PlayUIActiveTrue()
    {
        memberAliveUI.SetActive(true);
        operationUI.SetActive(true);
        mushroomValueUI.SetActive(true);
    }

    public void PlayUIActiveFalse()
    {
        windDirectUI.SetActive(false);
        memberAliveUI.SetActive(false);
        //scoreUpUI.SetActive(false);
        operationUI.SetActive(false);
        mushroomValueUI.SetActive(false);
    }

    public void OverviewUIAllSetActive(bool value)
    {
        overviewUI.SetActive(value);
    }
    
}
