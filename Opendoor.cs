using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Opendoor : MonoBehaviour
{
    [SerializeField]
    private GameObject Doorcoordinate;

    [SerializeField]
    private GameObject player;

    private bool opened;
    private GameObject Door;
    public bool scenechange;

    // Start is called before the first frame update
    void Start()
    {
        opened = false;
        scenechange = false;
        Door = Doorcoordinate.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainScene")
        {
            if (Vector3.Distance(player.transform.position, Doorcoordinate.transform.position) < 6.7f)
            {
                if (!opened)  //닫혀 있을 때 --> 열기
                {
                    Doorcoordinate.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                    opened = true;
                }
                if (opened && (Vector3.Distance(player.transform.position, Doorcoordinate.transform.position) < 5.7f))
                {
                    scenechange = true;
                    opened = false;
                }
            }
            else if (Vector3.Distance(player.transform.position, Doorcoordinate.transform.position) > 7.0f)
            {
                if (opened)  //열려있을 때 
                {
                    Doorcoordinate.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    opened = false;
                }

            }
        }
        else
        {
            if (Vector3.Distance(player.transform.position, Doorcoordinate.transform.position) < 6f)
            {
                if (!opened)  //닫혀 있을 때 --> 열기
                {
                    Doorcoordinate.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));

                    if (Door.name == "StoreOutsideDoor")
                    {
                        Doorcoordinate.transform.rotation = Quaternion.Euler(new Vector3(-182.187f, 180, 180));
                    }
                    else if(Door.name == "HouseOutsideDoor")
                    {
                        Doorcoordinate.transform.rotation = Quaternion.Euler(new Vector3(-180, 180, 180));
                    }
                    opened = true;
                }
                if (opened && (Vector3.Distance(player.transform.position, Doorcoordinate.transform.position) < 5.0f))
                {
                    scenechange = true;
                    opened = false;
                }
            }
            else if (Vector3.Distance(player.transform.position, Doorcoordinate.transform.position) > 6.5f)
            {
                if (opened)  //열려있을 때 
                {
                    if (Door.name == "StoreOutsideDoor")
                    {
                        Doorcoordinate.transform.rotation = Quaternion.Euler(new Vector3(-182.187f, 90, 180));
                    }
                    else if (Door.name == "HouseOutsideDoor")
                    {
                        Doorcoordinate.transform.rotation = Quaternion.Euler(new Vector3(-180, 90, 180));
                    }
                    opened = false;
                }

            }
        }

    }
}
