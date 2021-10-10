using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightLight : MonoBehaviour
{
    int hour;
    int min;

    // Start is called before the first frame update
    void Start()
    {
        hour = int.Parse(DateTime.Now.ToString("HH"));
        min = int.Parse(DateTime.Now.ToString("mm"));
        Debug.Log(hour);
        Debug.Log(min);
        if ((hour>=7)&&(hour<=17))
        {
            this.GetComponent<Light>().color = new Color(1, 1, 1);
        }
        else if ((hour<=5)||(hour>=19))
        {
            this.GetComponent<Light>().color = new Color(0, 0, 0);
        }
        else if(hour==6)
        {
            float col = (float)(4.25 * (min + 1));
            this.GetComponent<Light>().color = new Color(col / 255f, col / 255f, col / 255f);
        }
        else if(hour==18)
        {
            float col = (float)(255 - 4.25 * (min + 1));
            this.GetComponent<Light>().color = new Color(col / 255f, col / 255f, col / 255f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        hour = int.Parse(DateTime.Now.ToString("HH"));
        min= int.Parse(DateTime.Now.ToString("mm"));

        if (hour == 6)
        {
            float col = (float)(4.25 * (min + 1));
            this.GetComponent<Light>().color = new Color(col / 255f, col / 255f, col / 255f);
        }
        else if (hour == 18)
        {
            float col = (float)(255 - 4.25 * (min + 1));
            this.GetComponent<Light>().color = new Color(col / 255f, col / 255f, col / 255f);
        }
    }
}
