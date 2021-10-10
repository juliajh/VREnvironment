using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkController : MonoBehaviour
{
    public float delay;
    public float Skip_delay;
    public int cnt;

    public int dialog_cnt;
    public string currentText;
    public int num = 0;

    public bool flag = false;
    public bool check = false;
    public bool text_exit = false;
    public bool text_full = false;
    public int space = 53;

    public Text text;
    public GameObject speechbubble;

    public IEnumerator ShowText(string[] textAll)
    {
        //모든텍스트 종료
        dialog_cnt = textAll.Length;
        if (cnt >= dialog_cnt)
        {
            text_exit = true;
            //StopCoroutine("ShowText");
            check = false;
        }
        else
        {
            //기존문구clear
            currentText = "";

            for (int i = 0; i < textAll[cnt].Length; i++)
            {
                //단어하나씩출력
                currentText = textAll[cnt].Substring(0, i + 1);   //'안녕' -> 안 / 안녕
                text.text = currentText;
                yield return new WaitForSeconds(delay);

            }

            //탈출시 모든 문자출력
            text.text = textAll[cnt];
            yield return new WaitForSeconds(Skip_delay);
            cnt++;
            //스킵_지연후 종료
            text_full = true;
        }
    }

    public IEnumerator ShowText_Onlyskip(string[] textAll)  //for mainhouse scene
    {
        //모든텍스트 종료
        dialog_cnt = textAll.Length;
        if (cnt >= dialog_cnt)
        {
            text_exit = true;
            //StopCoroutine("ShowText");
            check = false;
        }
        else
        {
            //기존문구clear
            currentText = "";

            //탈출시 모든 문자출력
            text.text = textAll[cnt];
            yield return new WaitForSeconds(Skip_delay);
            cnt++;
            //스킵_지연후 종료
            text_full = true;
        }
    }
}