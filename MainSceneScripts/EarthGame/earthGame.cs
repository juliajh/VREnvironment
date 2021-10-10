using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class earthGame : MonoBehaviour
{
    [SerializeField]
    private GameObject earthWarningImg; //경보 울릴 때 보이는 지구본

    [SerializeField]
    private GameObject spaceship; //우주선

    [SerializeField]
    private GameObject SDK_CameraRig; //'나'

    [SerializeField]
    private GameObject fadeinoutImg;

    [SerializeField]
    private GameObject ship;


    private int min;
    private bool warning; //경보 울릴 때 true. 즉, 경보가 울린다는 의미
    private bool near;  //우주선과 내가 가까울 때 true. 즉, near 이 true이면 우주선을 타야함
    private bool checkbool; //투명도 조절 논리형 변수
    private Color color; //fadeinout에 필요

    void Start()
    {
        min = int.Parse(DateTime.Now.ToString("mm"));
        color = fadeinoutImg.GetComponent<Image>().color;   //fadeinout 할 때의 색깔
        color.a = Time.deltaTime * 100.0f;  //걸리는 시간
        fadeinoutImg.GetComponent<Image>().color = color;
        warning = false;
        near = false;
        earthWarningImg.SetActive(false);
        spaceship.SetActive(false);
        checkbool = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!warning) //경보가 안울릴 때,
        {
            min = int.Parse(DateTime.Now.ToString("mm"));
            if (min%60==12) //원래는 min % 60 == 58 인데 테스트용 편의를 위해 true
            {
                warning = true;
                earthWarningImg.SetActive(true); //earthWarnimgImg 활성화
                spaceship.SetActive(true); //spaceship 활성화
            }
        }
        else
        {
            float distance = Vector3.Distance(SDK_CameraRig.transform.position, spaceship.transform.position);
            if (distance <= 13.0f)
            {
                near = true;
                color.a = Time.deltaTime * 0.0f;
                fadeinoutImg.GetComponent<Image>().color = color;
                StartCoroutine("startspaceshipAnim");
            }
            if (near)
            {
                StartCoroutine("fadein"); //fadein 실행
            }
            

            if (checkbool)
            {
                SceneManager.LoadScene("MiniGame_0");
                checkbool = false;
            }
        }
    }

    IEnumerator fadein()  //
    {
        fadeinoutImg.SetActive(true); //fadeinoutImg 활성화
        for (int i = 55; i >= 0; i--)//i=55이고 i가 0보다 크거나같으면
        {
            color.a += Time.deltaTime * 0.028f; //
            fadeinoutImg.GetComponent<Image>().color = color;
            
        }
        if (color.a >= 3.9f)
            checkbool = true;
        yield return null;
    }

    IEnumerator startspaceshipAnim()
    {
        ship.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(0.8f);
    }
}
