using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFieldController : MonoBehaviour
{
    public GameObject inputPanel;
    //open the input panel
    public void open()
    {
        inputPanel.SetActive(true);
    }
    //close the input panel
    public void close()
    {
        inputPanel.SetActive(false);
    }
}
