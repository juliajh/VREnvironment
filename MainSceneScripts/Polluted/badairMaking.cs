using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class badairMaking : MonoBehaviour
{
    [SerializeField]
    private GameObject badAirParent;

    private List<GameObject> BadAirList;
    private float sec;
    private int sectrue;
    public static bool warning;
    private bool moving;

    //Start is called before the first frame update
    void Start()
    {
        BadAirList = new List<GameObject>();
        for (int i = 0; i < badAirParent.transform.childCount; i++)
            BadAirList.Add(badAirParent.transform.GetChild(i).gameObject);
        foreach (GameObject badair in BadAirList)
            badair.transform.localScale = new Vector3(0, 0, 0);
        sec = float.Parse(DateTime.Now.ToString("ss"));
        sectrue = 0;
        warning = false;
        moving = false;
        badAirParent.GetComponent<Animator>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving && !warning)
        {
            sec = float.Parse(DateTime.Now.ToString("ss"));
            if (sec % 2 == 0 && sectrue == 0)
                sectrue = 1;
            if (sectrue==1)
            {
                foreach (GameObject badair in BadAirList)
                {
                    badair.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f); ///0.0005f 정도? 현재는 test용으로 빠르게 진행함.
                }
                sectrue = 2;
            }
            if (sec % 2 == 1 && sectrue == 2)
                sectrue = 0;

            if(BadAirList[0].transform.localScale.x >=0.9f)
                moving = true;
        }

        if(moving)
        {
            StartCoroutine(Going());
            badAirParent.GetComponent<Animator>().SetBool("moving", true);
        }
    }

    IEnumerator Going()
    {
        badAirParent.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(7.2f);
        badAirParent.GetComponent<Animator>().enabled = false;
        warning = true;
        moving = false;
        sectrue = 0;
    }

}
