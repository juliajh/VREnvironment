﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBin : MonoBehaviour
{
    private bool flag;
    private void Start()
    {
        flag = false;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.parent != null)
        {
            if (collision.transform.gameObject.tag == "Trash")
            {
                int trashnum = Split_num(collision.transform.name);
                if ((trashnum ==3)||(trashnum==5) || (trashnum == 0))
                {
                    if (!flag)
                    {
                        Trash_Manager.correctTrashnum++;
                        Debug.Log("correctTrashnum:" + Trash_Manager.correctTrashnum);
                        flag = true;
                    }
                }
                else
                {
                    collision.transform.localPosition = GameObject.Find("Managers").transform.GetChild(3).GetComponent<Trash_Manager>().vectorOfTrash[collision.gameObject];
                    GameObject.Find("Managers").transform.GetChild(3).GetComponent<Trash_Manager>().StartCoroutine("retry");
                }
            }
        }
    }



    public int Split_num(string name)
    {
        string result = name.Substring(name.LastIndexOf("_") + 1);
        return int.Parse(result);
    }
}
