using UnityEngine;

public class MusicPlayerEnd : MonoBehaviour
{
    [SerializeField]
    AudioClip Track1;

    [SerializeField]
    AudioClip Track2;

    [SerializeField]
    AudioClip Track3;

    [SerializeField]
    AudioClip Track4;

    [SerializeField]
    AudioSource MusicPlayer;

    [SerializeField]
    GameObject RoundEndObject;

    private bool FadeIn;
    private bool FadeOut;

    private float ElapsedTime = 0;
    private float EndTime = 2;
    private float Percentage = 0;

    public void Awake()
    {
        FadeOut = false;
        FadeIn = true;

        if(RoundEndObject.GetComponent<EndScript>().cause == "Sustainability")
        {
            MusicPlayer.clip = Track1;
            MusicPlayer.Play();
        }
        else if (RoundEndObject.GetComponent<EndScript>().cause == "Unsustainability")
        {
            MusicPlayer.clip = Track2;
            MusicPlayer.Play();
        }
        else if (RoundEndObject.GetComponent<EndScript>().cause == "Output")
        {
            MusicPlayer.clip = Track3;
            MusicPlayer.Play();
        }
        
        //if just a normal round end
        else
        {
            MusicPlayer.clip = Track4;
            MusicPlayer.Play();
        }

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
        ElapsedTime += Time.deltaTime;
        Percentage = 1 - ElapsedTime / EndTime;
        MusicPlayer.volume = Mathf.Lerp(0, 1, Percentage);
    }
  
    public void Update()
    {
        if(FadeIn == true)
        {
            MusicFadeIn();
            if(ElapsedTime > EndTime)
            {
                FadeIn = false;
                ElapsedTime = 0;
                Percentage = 0;
                EndTime = 2;
            }
        }
        if(FadeOut == true)
        {
            MusicFadeOut();
            if(ElapsedTime > EndTime)
            {
                FadeOut = false;
                ElapsedTime = 0;
                Percentage = 0;
            }
        }
    }
}
