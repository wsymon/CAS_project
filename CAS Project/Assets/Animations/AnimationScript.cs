using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AnimationScript : MonoBehaviour
{
    [SerializeField]
    GameObject Backdrop;

    //opening and closing are the bools used within while loop to check for animation call
    private bool Opening;
    private bool Closing;

    //these are to tell the start, end of animation progress to tell Mathf.Lerp how far along linear interpolation it is
    private float ElapsedTime;
    private float EndTime;
    private float PercentComplete;

    //setup for opening animation. objects attached to this script are reloaded each scene so this works fine
    void Start()
    {
        Backdrop.GetComponent<Image>().color.a.Equals(1);
        ElapsedTime = 0;
        EndTime = 2;
        Opening = true;
        Closing = false;
    }

    //point of this is that once per frame unity can figure out how many seconds have passed in order to check
    //animation length progress, but cannot do in normal while loop (which is computer speed not time.deltarune (IRL time per frame))
    void Update()
    {
        if(Opening == true)
        {
            if(ElapsedTime < EndTime)
            {
                SceneOpenAnimation();
            }
            else
            {
                Opening = false;
                Backdrop.GetComponent<Image>().color = new Color(255, 255, 255, 0);
                ElapsedTime = 0;
                PercentComplete = 0;
            }
        }

        if(Closing == true)
        {
            if(ElapsedTime < EndTime)
            {
                SceneCloseAnimation();
            }
            else
            {
                Closing = false;
                Backdrop.GetComponent<Image>().color = new Color(255, 255, 255, 1);
                ElapsedTime = 0;
                PercentComplete = 0;
            }
        }

    }
    //obv open and exit scene animation 
    public void SceneOpenAnimation()
    {
        PercentComplete = (ElapsedTime / EndTime);
        Backdrop.GetComponent<Image>().color = new Color(255, 255, 255, Mathf.Lerp(1, 0, PercentComplete));
        ElapsedTime += Time.deltaTime;  
        //Debug.Log(Backdrop.GetComponent<Image>().color.a);
        //Debug.Log(ElapsedTime + " " + EndTime + " " + PercentComplete);
    }
    public void SceneCloseAnimation()
    {
        Closing = true;
        ElapsedTime += Time.deltaTime;
        PercentComplete = 1 - (ElapsedTime / EndTime);
        Backdrop.GetComponent<Image>().color = new Color(255, 255, 255, Mathf.Lerp(1, 0, PercentComplete));
    }
}
