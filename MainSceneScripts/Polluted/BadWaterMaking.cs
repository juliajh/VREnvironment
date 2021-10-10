using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class BadWaterMaking : MonoBehaviour
{
    [SerializeField]
    private GameObject badwaterParent;

    [SerializeField]
    private GameObject warningImg;

    private List<GameObject> badwaterList;
    private float sec;
    private bool getOn;
    private int sectrue;
    public static int activeBadWater;

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
        warningImg.SetActive(false);
        sec = float.Parse(DateTime.Now.ToString("ss"));
        sectrue = 0;
        activeBadWater = 0;
        getOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(activeBadWater<badwaterList.Count)
        {
            warningImg.SetActive(false);
            sec = float.Parse(DateTime.Now.ToString("ss"));
            if (sec % 4 == 0 && sectrue == 0)
                sectrue = 1;
            if(sectrue == 1)
            {
                activeBadWater = 0;
                foreach(GameObject badwater in badwaterList)
                {
                    if (badwater.activeSelf)
                    {
                        activeBadWater += 1;
                    }
                    else if (!badwater.activeSelf && !getOn) //false일 때,
                    {
                        badwater.SetActive(true);
                        activeBadWater += 1;
                        sectrue = 2;
                        getOn = true;
                    }
                }
                getOn = false;
            }
            if (sec % 5 == 1 && sectrue == 2)
                sectrue = 0;
        }
        else
        {
            warningImg.SetActive(true);
        }

    }
}
