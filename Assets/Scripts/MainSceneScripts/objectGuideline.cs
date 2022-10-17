using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System;
using UnityEngine.UI;

public class objectGuideline : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> Obj;

    [SerializeField]
    private GameObject player;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject obj in Obj)
        {
            if (Vector3.Distance(player.transform.position, obj.transform.position) <= 5.0f)
            {

            }
        }
    }
}
