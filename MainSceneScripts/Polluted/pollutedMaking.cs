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
    private int waterSectrue;
    private int airSectrue;
    public static int activeBadWater; //used in VRTK_BezierPointerRenderer script 
    private bool airWarning;
    private int badairCount;
    private bool airsizeUp;
    public static int pollutedPercent;
    private float lastMin;

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
            badairList.Add(badairParent.transform.GetChild(i).gameObject);
        foreach (GameObject badair in badairList)
            badair.transform.localScale = new Vector3(0, 0, 0);
        waterWarningImg.SetActive(false);
        airWarningImg.SetActive(false);
        sec = float.Parse(DateTime.Now.ToString("ss"));
        waterWarningImg.SetActive(false);
        waterSectrue = 0;
        airSectrue = 0;
        activeBadWater = 0;
        airSectrue = 0;
        pollutedPercent = 0;
        airWarning = false;
        airsizeUp = true;
        badairCount = 0;
        lastMin = 0.0f;
        badairParent.GetComponent<Animator>().enabled = true;
       
        //종료시점~시작시점까지의 polluted
        string lastTime = PlayerPrefs.GetString("SaveLastTime");
        System.DateTime lastDateTime = System.DateTime.Parse(lastTime);
        System.TimeSpan compareTime = System.DateTime.Now - lastDateTime;
        lastMin = float.Parse(compareTime.TotalMinutes.ToString()); //지난 시간의 minute

        if(lastMin/60 >= 6.0f)
        {
            for(int i = 0; i < 30; i++)
            {
                badwaterList[i].SetActive(true);
                activeBadWater++;
            }
            foreach (GameObject badair in badairList)
            {
                badair.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f); ///0.0005f 정도? 현재는 test용으로 빠르게 진행함.
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        sec = float.Parse(DateTime.Now.ToString("ss"));

        //water polluted
        if (sec % 4 == 0 && waterSectrue == 0)
            waterSectrue = 1;
        if (waterSectrue == 1)
        {
            foreach (GameObject badwater in badwaterList)
            {
                if (!badwater.activeSelf) //false일 때,
                {
                    badwater.SetActive(true);
                    activeBadWater += 1;
                    waterSectrue = 2;
                    break;
                }
            }
        }
        if (sec % 4 == 1 && waterSectrue == 2)
            waterSectrue = 0;

        if (activeBadWater == badwaterList.Count - 30)
        {
            waterWarningImg.SetActive(true);
        }
        else if (activeBadWater < badwaterList.Count - 30)
            waterWarningImg.SetActive(false);


        //air polluted
        if (sec % 2 == 0 && airSectrue == 0)
            airSectrue = 1;
        if (airSectrue == 1)
        {
            foreach (GameObject badair in badairList)
            {
                if (badair.transform.localScale.x <= 1.2f)
                    badair.transform.localScale += new Vector3(0.0005f, 0.0005f, 0.0005f); ///0.0005f 정도? 현재는 test용으로 빠르게 진행함.
            }
            airSectrue = 2;
        }
        if (sec % 2 == 1 && airSectrue == 2)
            airSectrue = 0;

        //기준
        badairCount = 0;
        foreach (GameObject badair in badairList)
        {
            if (badair.transform.localScale.x >= 0.8f) 
                badairCount += 1;
        }
        if (badairCount >= 3)
        {
            airWarning = true;
            airWarningImg.SetActive(true);
        }
        else
        {
            airWarning = false;
            airWarningImg.SetActive(false);
        }

        if ((badairCount + (activeBadWater / 10)) >= 13 && pollutedPercent!=4)
        {
            pollutedPercent = 3;
        }
        else if ((airWarningImg.activeSelf || waterWarningImg.activeSelf)&& pollutedPercent != 2)
            pollutedPercent = 1;
        else
            pollutedPercent = 0;

        /*
        if (airMoving)
        {
            StartCoroutine(Going());
            badairParent.GetComponent<Animator>().SetBool("moving", true);
        }
        */
    }

    //마을로 badair 이동 
    /*IEnumerator Going()
    {
        badairParent.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(7.2f);
        badairParent.GetComponent<Animator>().enabled = false;
        airWarning = true;
        airMoving = false;
        airSectrue = 0;
    }*/
}
