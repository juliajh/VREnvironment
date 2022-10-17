using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class airCleaningManager : MonoBehaviour
{
    [SerializeField]
    private GameObject airWarningImg;

    [SerializeField]
    private GameObject cleaningObj;

    [SerializeField]
    private GameObject badAirParent;

    private List<GameObject> badairList;

    [SerializeField]
    private GameObject SDK_Camera; //1,30,1로 사이즈 바꾸면 하늘을 나는 느낌. 대신 이 때 canvas는 끄는 것이 좋다. 

    [SerializeField]
    private GameObject SDK_CameraRig; //하늘에 떴을 때 useGravity 끄기 위해 꼭 필요!

    [SerializeField]
    private GameObject rightController;

    [SerializeField]
    private GameObject camera_eye;

    [SerializeField]
    private GameObject fadeinoutimg;

    private bool flying;
    private Vector3 cleanObjcurrentPosition;

    float time = 0;
    float fades = 1.0f;
    private Color color;
    private bool checkbool;
    private Vector3 originCamera;

    // Start is called before the first frame update
    void Start()
    {
        checkbool = false;
        cleanObjcurrentPosition = cleaningObj.transform.position;
        airWarningImg.SetActive(false);
        flying = false;
        badairList = new List<GameObject>();
        for (int i = 0; i < badAirParent.transform.childCount; i++)
            badairList.Add(badAirParent.transform.GetChild(i).gameObject);
        color = fadeinoutimg.GetComponent<Image>().color;
        originCamera = SDK_Camera.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    { 
        if (!flying) //날지 않고 있을 때
        {
            if (checkbool)
                StartCoroutine(fadeout());

            if (rightController.GetComponent<VRTK.VRTK_InteractGrab>().GetGrabbedObject() == cleaningObj) //cleaning하는 obj(물체)를 잡았을 경우 -> 하늘로 올라간다.
            {
                if (!checkbool)
                    StartCoroutine(fadein());
                //SDK_Camera.transform.localScale = new Vector3(1, 77, 1);  //스케일을 키우면 땅의 물체들과 부딪치기 때문에 하늘을 제대로 날지 못함.
                
                else
                {
                    SDK_CameraRig.GetComponent<Rigidbody>().useGravity = false; //to fly
                    SDK_Camera.transform.localPosition = new Vector3(originCamera.x, 30.0f, originCamera.z);
                    flying = true;
                }
            }
        }
        else //if flying
        {
            if (checkbool)
            {
                StartCoroutine(fadeout());
            }
            else if (rightController.GetComponent<VRTK.VRTK_InteractGrab>().GetGrabbedObject() == null) //혹시라도 놓쳤을 경우
            {
                StartCoroutine(fadein());
                SDK_CameraRig.GetComponent<Rigidbody>().useGravity = true;
                cleaningObj.transform.position = cleanObjcurrentPosition;
                SDK_Camera.transform.localPosition = originCamera;
            }
            else //물체를 가지고 하늘을 나는 동안의 interaction
            {
                if (cleaningObj.GetComponent<cleanObj>().trig) //cleaningObj가 air들과 부딪쳤을 경우,
                {
                    badairList[cleaningObj.GetComponent<cleanObj>().numOfAir].transform.localScale = new Vector3(0, 0, 0);
                    cleaningObj.GetComponent<cleanObj>().trig = false;
                    AudioSource cleanAudio = cleaningObj.GetComponent<AudioSource>();
                    cleanAudio.clip = Resources.Load("Audios/cloud") as AudioClip;
                    cleanAudio.Play();
                }
                
                /*if (numofair == 0) //모든 air들을 없앴을 경우 다시 제자리로 돌아오게끔.
                {
                    SDK_CameraRig.GetComponent<Rigidbody>().useGravity = true;
                    vrCanvas.SetActive(true);
                    airWarningImg.SetActive(false);
                    cleaningObj.transform.position = cleanObjcurrentPosition;
                    cleaningObj.SetActive(false);
                    flying = false;
                    pollutedMaking.airWarning = false;
                }*/
            }
        }
    }
    IEnumerator fadein()  //
    { //fadeinoutImg 활성화
        fadeinoutimg.SetActive(true);
        color = fadeinoutimg.GetComponent<Image>().color;

        for (int i = 150; i >= 0; i--)
        {
            color.a += Time.deltaTime * 0.015f;
            fadeinoutimg.GetComponent<Image>().color = color;
        }
        if (color.a >= 1.0f)
        {
            checkbool = true;
            flying = false;
        }
        yield return null;
    }
    IEnumerator fadeout()  //
    {
        for (int i = 200; i >= 0; i--) //i=100이고 i가 0보다 크거나같으면
        {
            color.a -= Time.deltaTime * 0.005f;
            fadeinoutimg.GetComponent<Image>().color = color;
        }
        if (color.a <= 0.002f)
        {
            color.a = 0.0f;
            fadeinoutimg.GetComponent<Image>().color = color;
            checkbool = false;
            fadeinoutimg.SetActive(false);  //fadeinoutImg 비활성화
        }
        yield return new WaitForSeconds(0.5f);
    }

    }
