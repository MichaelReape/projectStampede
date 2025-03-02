//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class UIMenuManager : MonoBehaviour
//{
//    private static UIMenuManager instance;
//    public GameObject pauseMenu;
//    public GameObject saveMenu;
//    public static UIMenuManager UIMenuManagerInstance
//    {
//        get
//        { 
//            return instance;
//        }
//    }

//    private void Awake()
//    {
//        //singleton pattern to ensure only one instance of the WFCManager
//        if (instance != null && instance != this)
//        {
//            Destroy(this.gameObject);
//        }
//        else
//        {
//            instance = this;
//        }
//    }


//}
