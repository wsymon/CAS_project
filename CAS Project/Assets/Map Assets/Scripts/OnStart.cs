using System.Security.Cryptography;
using UnityEngine;

public class OnStart : MonoBehaviour
{
    //field to add animated opening sequence object into 
    [SerializeField] Animator transition;
    
        
    void Awake()
    {
        animation_starter();

    }
    private void animation_starter()
    {
        //trigger 
        transition.SetTrigger("Start");
        Debug.Log("inside camera movement void start");
    }
}
