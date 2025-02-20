using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFieldController : MonoBehaviour
{
    public GameObject inputPanel;

    public void open()
    {
        inputPanel.SetActive(true);
        //might have to add more functionality later
    }

    public void close()
    {
        inputPanel.SetActive(false);
    }
}
