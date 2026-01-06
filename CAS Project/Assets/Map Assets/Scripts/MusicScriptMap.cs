using UnityEngine;

public class MusicScriptMap : MonoBehaviour
{
    [SerializeField]
    AudioClip Track1;

    [SerializeField]
    AudioClip Track2;

    [SerializeField]
    AudioClip Track3;

    [SerializeField]
    AudioSource MusicPlayer;

    private bool FadeIn;
    private bool FadeOut;

    private float ElapsedTime = 0;
    private float EndTime = 2;
    private float Percentage = 0;

    public void Start()
    {
        FadeOut = false;
        FadeIn = true;

        if(CurrentPlayerData.Round < 3)
        {
            MusicPlayer.clip = Track1;
            MusicPlayer.Play();
        }
        else if (CurrentPlayerData.Round < 6)
        {
            MusicPlayer.clip = Track2;
            MusicPlayer.Play();
        }
        else
        {
            MusicPlayer.clip = Track3;
            MusicPlayer.Play();
        }
        FadeIn = true;
    }

    public void MusicFadeIn()
    {
        ElapsedTime += Time.deltaTime;
        Percentage = ElapsedTime / EndTime;
        MusicPlayer.volume = Mathf.Lerp(0, 1, Percentage);
    }

    public void MusicFadeOut()
    {
        FadeOut = true;
        ElapsedTime += Time.deltaTime;
        Percentage = 1 - ElapsedTime / EndTime;
        MusicPlayer.volume = Mathf.Lerp(0, 1, Percentage);
    }

    public void MuteAndUnMute()
    {
        if(MusicPlayer.volume == 0.1f)
        {
            MusicPlayer.volume = 1;
        }
        else
        {
            MusicPlayer.volume = 0.1f;
        }
    }
  
    public void Update()
    {
        if(FadeIn == true)
        {
            if(ElapsedTime > EndTime)
            {
                FadeIn = false;
                ElapsedTime = 0;
                Percentage = 0;
                EndTime = 2;
            }
            else
            {
                MusicFadeIn();
            }
        }
        if(FadeOut == true)
        {
            if(ElapsedTime > EndTime)
            {
                FadeOut = false;
                ElapsedTime = 0;
                Percentage = 0;
            }
            else
            {
                MusicFadeOut();
            }
        }
    }
}