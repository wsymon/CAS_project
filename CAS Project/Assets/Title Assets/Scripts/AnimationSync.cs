using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class AnimationSync : MonoBehaviour
{
    [SerializeField]
    Object AnimatedObject;


    [SerializeField]
    Sprite[] ClipsArray;

    //2 ints for the number of sprites in the array and the current disaplying sprite within the array
    private int SpriteCount = 0;
    private int CurrentState = 0;

    //on start opens the loop
    public void Start()
    {   
        //finds array length, sets display to first sprite
        SpriteCount = ClipsArray.Length - 1;
        AnimatedObject.GetComponent<Image>().sprite = ClipsArray[0];
        //start a timed loop
        StartCoroutine(AnimationLoop());
    }
    IEnumerator AnimationLoop()
    {
        //inside coroutine
        int x = 1;
        while (x == 1)
        {
            //waits 1 second
            yield return new WaitForSeconds((float)0.5);
            if (CurrentState == SpriteCount)
            {
                //state was 4, set to 1 and play
                CurrentState = 0;
                AnimatedObject.GetComponent<Image>().sprite = ClipsArray[0];
            }
            else
            {
                //if the state isn't 4, add 1 to current state and pay 
                CurrentState++;
                AnimatedObject.GetComponent<Image>().sprite = ClipsArray[CurrentState];
            }
        }
    }
}