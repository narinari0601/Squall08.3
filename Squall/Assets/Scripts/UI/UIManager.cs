using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private WindDirectUI windDirectUI;

    private MemberAliveUI memberAliveUI;

    private GameOverUI gameOverUI;

    public WindDirectUI WindDirectUI { get => windDirectUI;}
    public MemberAliveUI MemberAliveUI { get => memberAliveUI; set => memberAliveUI = value; }
    public GameOverUI GameOverUI { get => gameOverUI; }
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
    }

    
    void Update()
    {
        
    }

    public void HiddenPlayUI()
    {
        windDirectUI.SetActive(false);
        memberAliveUI.SetActive(false);
    }
}
