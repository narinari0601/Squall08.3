using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance = null;

    private AudioSource audioSource;

    [SerializeField,Header("音源")]
    private AudioClip[] audioClips = new AudioClip[0];

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);  //シーンを切り替えても消えない
        }
        else
        {
            Destroy(this.gameObject);
        }

        Initialize();
    }


    public void Initialize()
    {
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeBGM(int num, float volume)
    {
        audioSource.Stop();
        audioSource.clip = audioClips[num];
        audioSource.volume = volume;
        audioSource.Play();
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }
}
