using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager_mini0 : MonoBehaviour
{

    [SerializeField]
    private GameObject fadeinoutImg;

    [SerializeField]
    private GameObject rightController;

    [SerializeField]
    private GameObject things;

    [SerializeField]
    private GameObject effects;

    [SerializeField]
    private GameObject buttons;

    [SerializeField]
    private GameObject coin;

    [SerializeField]
    private GameObject menualPanel;

    public GameObject QuizPanel;
    public Text QuizText;
    public Text ButtonText_0;
    public Text ButtonText_1;
    public Text ButtonText_2;
    public Text AnswerNumText;
    public GameObject AnswerBox;
    public Text AnswerText;
    public QuizData[] questions;
    public static bool mini0_scenechange; //사용자가 minigame -> Main 으로 넘어갈 때 true 가 됨.
    private int min;
    private List<GameObject> Things;
    private List<GameObject> Effects;
    private List<GameObject> Buttons;

    private int quizNum;
    private bool clickTarget;
    private int clearNum;
    private int answerNum;
    private int buttonNum;

    private GameObject target;
    private bool rayHit;
    private bool coinover;
    private bool quizOn;
    private bool start;
    private bool checkbool; //fadeinout을 위한 boolean 

    private Color color;

    void Start()
    {
        Things = new List<GameObject>();
        for (int k = 0; k < things.transform.childCount; k++)
            Things.Add(things.transform.GetChild(k).gameObject);
        Effects = new List<GameObject>();
        for (int k = 0; k < effects.transform.childCount; k++)
            Effects.Add(effects.transform.GetChild(k).gameObject);
        Buttons = new List<GameObject>();
        for (int k = 0; k < buttons.transform.childCount; k++)
            Buttons.Add(buttons.transform.GetChild(k).gameObject);
        clickTarget = false;
        quizNum = 0;
        clearNum = 0;
        buttonNum = 5;
        AnswerBox.SetActive(false);
        QuizPanel.SetActive(false);
        menualPanel.SetActive(false);
        coin.SetActive(false);
        target = null;
        rayHit = false;
        quizOn = false;
        coinover = false;
        checkbool = false;
        start = true;
        color.a = Time.deltaTime * 0.0f;
        mini0_scenechange = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            StartCoroutine("Splash");
        }
        //opening 넣어야 함. bool 변수 open 써서 open일 때, opening canvas로 무엇을 해야할 지 소개하는 부분 필요.

        if (clearNum == 7)
        {
            if (!coinover)
            {
                StartCoroutine(Over());
                color = fadeinoutImg.GetComponent<Image>().color;
                color.a = Time.deltaTime * 0.0f;
                fadeinoutImg.GetComponent<Image>().color = color;
            }
            else
            {
                StartCoroutine("SceneChange");
                if (checkbool)
                {
                    Debug.Log("CheckBool = true");
                    mini0_scenechange = true;
                    SceneManager.LoadScene("MainScene");
                    checkbool = false;
                    coinover = false;
                }
            }
        }
        if (!quizOn)
        { 
            rayHit = rightController.GetComponent<VRTK.VRTK_StraightPointerRenderer>().getrayHit();

            if (rayHit)
            {
                if (rightController.GetComponent<VRTK.VRTK_ControllerEvents>().touchpadout())
                    target = rightController.GetComponent<VRTK.VRTK_StraightPointerRenderer>().getObj();
                if (Things.Contains(target))
                {
                    clickTarget = true;
                    Debug.Log(target);
                }
                else
                {
                    clickTarget = false;
                    rayHit = false;
                }
            }
            if (clickTarget)
            {
                QuizPanel.SetActive(true);
                quizNum = Things.IndexOf(target);
                SettingQuiz(quizNum);
                quizOn = true;
            }
        }
        else //if quizOn!! 퀴즈 풀기 시작~
        {
            rayHit = rightController.GetComponent<VRTK.VRTK_StraightPointerRenderer>().getrayHit();

            if (rayHit) //원래는 rayHit
            {
                /*if (rightController.GetComponent<VRTK.VRTK_ControllerEvents>().touchpadout())
                    target = rightController.GetComponent<VRTK.VRTK_StraightPointerRenderer>().getObj();*/
                if (ButtonClick.click)
                {
                    buttonNum = int.Parse(ButtonClick.ButtonNum);
                    if (buttonNum == questions[quizNum].answer)
                    {
                        AnswerBox.SetActive(true);
                        AnswerText.text = "!정답!";
                        StartCoroutine(ShowingAnswer());
                    }
                    else
                    {
                        AnswerBox.SetActive(true);
                        AnswerText.text = "오답! 정답은 " + (questions[quizNum].answer + 1) + "번";
                        StartCoroutine(ShowingAnswer());
                    }
                }
            }
        }
    }

    private void SettingQuiz(int quizNum)
    {
        QuizText.text = questions[quizNum].question;
        ButtonText_0.text = questions[quizNum].button_0;
        ButtonText_1.text = questions[quizNum].button_1;
        ButtonText_2.text = questions[quizNum].button_2;

        AnswerNumText.text = "0";
        answerNum = 0;
    }

    IEnumerator ShowingAnswer()
    {
        ButtonClick.click = false;
        yield return new WaitForSeconds(3.0f);
        AnswerBox.SetActive(false);
        QuizPanel.SetActive(false);
        Effects[quizNum].SetActive(false);
        Things[quizNum].SetActive(false);
        target = null;
        quizNum = 0;
        clearNum++;
        clickTarget = false;
        rayHit = false;
        quizOn = false;
        ButtonClick.click = false;
        Debug.Log("ClearNum : " + clearNum);
    }

    IEnumerator Splash()
    {
        color = fadeinoutImg.GetComponent<Image>().color;
        for (int i = 100; i >= 0; i--)  //i=100이고 i가 0보다 크거나같으면
        {
            color.a -= Time.deltaTime * 0.01f;
            fadeinoutImg.GetComponent<Image>().color = color;
        }
        if (color.a <= 0.5f)
        {
            start = false;
            fadeinoutImg.GetComponent<Image>().color = color;
            fadeinoutImg.SetActive(false); //fadeinoutImg 비활성화
        }
        yield return new WaitForSeconds(0.5f);

        if (!start)
        {
            menualPanel.SetActive(true);
            menualPanel.transform.GetChild(1).GetComponent<Text>().text = "큰일났어!! \n각종 오염물질들로 지구가 위험해!!\n오염물질들에 대한 퀴즈를 풀고 지구를 구해줘!";
            yield return new WaitForSeconds(8.0f);
            menualPanel.transform.GetChild(1).GetComponent<Text>().text = "왼쪽 터치패드를 돌려서 지구를 돌릴 수 있어";
            GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().LtouchpadLight();
            yield return new WaitForSeconds(7.0f);

            menualPanel.transform.GetChild(1).GetComponent<Text>().text = "오른쪽 레이저로 지구에서 오염이 일어나고 있는 곳을 클릭하면\n퀴즈가 화면에 뜨고 레이저로 퀴즈의 답을 클릭하면 돼!";
            GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().offLtouchpad();
            GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().RtouchpadLight();

            yield return new WaitForSeconds(8.0f);
            menualPanel.GetComponent<Animator>().SetBool("talking", false);
            GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().offRtouchpad();
        }
    }

    IEnumerator Over()
    {
        coin.SetActive(true);
        menualPanel.GetComponent<Animator>().SetBool("talking", true);
        menualPanel.transform.GetChild(1).GetComponent<Text>().text = "참 잘했어!! 지구를 살려줘서 고마워!\n코인을 줄테니 마을을 잘 가꿔줘!";
        yield return new WaitForSeconds(5.5f);
        coin.SetActive(false);
        SqlSave.coin += 20;
        coinover = true;
    }

    IEnumerator SceneChange()  //
    {
        fadeinoutImg.SetActive(true); //fadeinoutImg 활성화
        menualPanel.SetActive(false);
        color = fadeinoutImg.GetComponent<Image>().color;

        for (int i = 50; i >= 0; i--)  //i=55이고 i가 0보다 크거나같으면
        {
            color.a += Time.deltaTime * 0.07f;
            fadeinoutImg.GetComponent<Image>().color = color;
        }

        if (color.a >= 2.5f)
            checkbool = true;
        yield return null;
    }

}
