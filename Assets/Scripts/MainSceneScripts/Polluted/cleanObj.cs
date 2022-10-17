using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cleanObj : MonoBehaviour
{
    public int numOfAir;
    public bool trig;

    private void Start()
    {
        numOfAir = 0;
        trig = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "badair")
        {
            numOfAir = int.Parse(collider.gameObject.name.Substring(collider.gameObject.name.IndexOf("_")+1));
            SqlDB.airpolluted -= 3;
            trig = true;
        }
    }

}
