using UnityEngine;

public class MusicPlayerTitle : MonoBehaviour
{
   [SerializeField]
    AudioClip Track1;

    [SerializeField]
    AudioSource MusicPlayer;

    private bool FadeIn;
    private bool FadeOut = false;

    private float ElapsedTime = 0;
    private float EndTime = 3;
    private float Percentage = 0;

    public void Start()
    {
        MusicPlayer.clip = Track1;
        MusicPlayer.Play();
        FadeIn = true;
    }

    public void MusicFadeIn()
    {
        ElapsedTime += Time.deltaTime;
        Percentage = 1 - ElapsedTime / EndTime;
        MusicPlayer.volume = Mathf.Lerp(1, 0, Percentage);
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
