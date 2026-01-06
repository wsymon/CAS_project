using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;


//IMPORTANT: this class manages the opening and closing of ui AND their animations...no other script needed for open/close...
public class UIAnimationScript : MonoBehaviour
{
   [SerializeField]
   GameObject gobject;

    private Image[] images;
    private Button[] buttons;
    private TextMeshProUGUI[] texts;
    private Text[] elderlytexts;

    private float ElapsedTIme = 0;
    private float EndTime = 0.25f;
    private float Percentage = 0;

    public void Start()
    {
        images = gobject.GetComponentsInChildren<Image>();
        buttons = gobject.GetComponentsInChildren<Button>();
        texts = gobject.GetComponentsInChildren<TextMeshProUGUI>();
        if(gobject.GetComponentsInChildren<Text>().Count() > 0)
        {
            elderlytexts = gobject.GetComponentsInChildren<Text>();
        }
    }

    //call this function, will do all the rest
    public void UIAnimation()
    {
        if(gobject.activeSelf == true)
        {
           UICloseAnimation();
        }
        else
        {
            gobject.SetActive(true);
            UIOpenAnimation();
        }
    }

    public void UIOpenAnimation()
    {
        ElapsedTIme = 0;
        StartCoroutine(Animation("open"));
    }

    public void UICloseAnimation()
    {
        ElapsedTIme = 0;
        StartCoroutine(Animation("close"));
    }

    IEnumerator Animation(string direction)
    {
        if(direction == "open")
        {
            while(ElapsedTIme < EndTime)
            {
                ElapsedTIme += Time.deltaTime;
                Percentage = ElapsedTIme / EndTime;
                if(elderlytexts != null)
                {
                     foreach(Text et in elderlytexts)
                    {
                        et.color = new Color(0.188f, 0.231f, 0.333f,Mathf.Lerp(0, 1, Percentage));
                    }

                }
                if(buttons != null)
                {
                    foreach(Button b in buttons)
                {
                    try
                    {
                        b.image.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, Percentage));
                    }
                    finally{}
                }
                }
                if(texts != null)
                {
                    foreach(TextMeshProUGUI t in texts)
                {
                    try
                    {
                        t.color = new Color(0.188f, 0.231f, 0.333f, Mathf.Lerp(0, 1, Percentage));
                    }
                    finally{}
                }
                }
                if(images != null)
                {
                    foreach(Image i in images)
                    {
                    try
                        {                
                            i.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, Percentage));
                        }
                    finally{}
                }
                yield return null;
            }
        }
            ElapsedTIme = 0;
            Percentage = 0;
            OpeningConfirm();
    }
    
        if(direction == "close")
        {
            while(ElapsedTIme < EndTime)
            {
                ElapsedTIme += Time.deltaTime;
                Percentage = ElapsedTIme / EndTime;
                if(elderlytexts != null)
                {
                     foreach(Text et in elderlytexts)
                    {
                        et.color = new Color(0.188f, 0.231f, 0.333f,Mathf.Lerp(1, 0, Percentage));
                    }

                }
                if(buttons != null)
                {
                    foreach(Button b in buttons)
                {
                    try
                    {
                    b.image.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, Percentage));
                    }
                    finally{}
                }
                }
                if(texts != null)
                {
                    foreach(TextMeshProUGUI t in texts)
                {
                    try 
                    {
                        t.color = new Color(0.188f, 0.231f, 0.333f, Mathf.Lerp(1, 0, Percentage));
                    }
                    finally{}
                }
                foreach(Image i in images)
                {
                    try
                    {
                        i.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, Percentage));
                    }            
                    finally{}
                }
                }
                yield return null;
            }
            ElapsedTIme = 0;
            Percentage = 0;
            gobject.SetActive(false);
        }
    }

    public void OpeningConfirm()
    {
        if(elderlytexts != null)
                {
                     foreach(Text et in elderlytexts)
                    {
                        et.color = new Color(0.188f, 0.231f, 0.333f, 1);
                    }

                }
                if(buttons != null)
                {
                    foreach(Button b in buttons)
                {
                    try
                    {
                    b.image.color = new Color(1f, 1f, 1, 1);
                    }
                    finally{}
                }
                }
                if(texts != null)
                {
                    foreach(TextMeshProUGUI t in texts)
                {
                    try 
                    {
                        t.color = new Color(0.188f, 0.231f, 0.333f, 1);
                    }
                    finally{}
                }
                foreach(Image i in images)
                {
                    try
                    {
                        i.color = new Color(1, 1, 1, 1);
                    }            
                    finally{}
                }
            }
    }
}