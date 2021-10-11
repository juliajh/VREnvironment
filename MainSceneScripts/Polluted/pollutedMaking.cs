using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class pollutedMaking : MonoBehaviour
{
    [SerializeField]
    private GameObject badairParent;

    [SerializeField]
    private GameObject badwaterParent;

    [SerializeField]
    private GameObject waterWarningImg;

    [SerializeField]
    private GameObject airWarningImg;

    private List<GameObject> badwaterList;
    private List<GameObject> badairList;
    private float sec;
    private bool waterflag;
    private bool airflag;

    // Start is called before the first frame update
    void Start()
    {
        badwaterList = new List<GameObject>();
        for (int i = 0; i < badwaterParent.transform.childCount; i++)
        {
            badwaterList.Add(badwaterParent.transform.GetChild(i).gameObject);
        }
        foreach (GameObject badwater in badwaterList)
        {
            badwater.SetActive(false);
        }

        badairList = new List<GameObject>();
        for (int i = 0; i < badairParent.transform.childCount; i++)
        {
            badairList.Add(badairParent.transform.GetChild(i).gameObject);
        }
        foreach (GameObject badair in badairList)
        {
            badair.SetActive(false);
        }

        waterWarningImg.SetActive(false);
        airWarningImg.SetActive(false);


        if (SqlDB.waterpolluted != 0)
        {
            for (int i = 0; i < SqlDB.waterpolluted; i++)
            {
                badwaterList[i].SetActive(true);
            }
        }

        float num = SqlDB.airpolluted / 3;

        if (num >= 3)
        {
            for (int i = 0; i < Math.Truncate(num); i++)
            {
                badairList[i].SetActive(true);
            }
        }

    }

    

    // Update is called once per frame
    void Update()
    {
        sec = int.Parse(DateTime.Now.ToString("ss"));

        if (sec % 8 == 0 && !waterflag)
        {
            SqlDB.waterpolluted += 1;

            for (int i = 0; i < badwaterList.Count; i++)
            {
                if (!badwaterList[i].activeSelf)
                {
                    badwaterList[i].SetActive(true);
                    break;
                }
            }
            waterflag = true;
        }

        if (sec % 8 == 1)
            waterflag = false; 

        if (sec % 10 == 0 && !airflag) 
        {
            SqlDB.airpolluted += 1;

            for (int i = 0; i < badairList.Count; i++)
            {
                if (!badairList[i].activeSelf)   //꺼져있는 것 하나만 찾아서 켜기
                {
                    badairList[i].SetActive(true);
                    break;
                }
            }            

            airflag = true;
        }

        if (sec % 10 == 1)
            airflag = false; 


        //warning
        if (SqlDB.waterpolluted >=50)
        {
            waterWarningImg.SetActive(true);
        }
        else
        {
            waterWarningImg.SetActive(false);
        }

        if (SqlDB.airpolluted >= 50)
        {
            airWarningImg.SetActive(true);
        }
        else
        {
            airWarningImg.SetActive(false);
        }
    }
}
