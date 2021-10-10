using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    public static string ButtonNum;
    public static bool click;
    public void clickButton()
    {
        ButtonNum = this.name.Substring(this.name.IndexOf("_") + 1);
        click = true;
        Debug.Log("click!");
    }
}
