using UnityEngine;
using System.IO;
using UnityEditorInternal;
using UnityEditor.Overlays;
using UnityEngine.Rendering;
using UnityEditor.Build.Content;


public class NewSave : MonoBehaviour
{
    title_functions title_Functions;
    [SerializeField]
    GameObject Camera;

    string playerName;
    void Awake()
    {
        title_Functions = Camera.GetComponent<title_functions>();
        string savepath = Application.dataPath + "\\Saves;" + ".save";
    }

    public void Save() {

       // playerName = Camera.GetComponent<title_functions>().Name;




    }
}
    

    
