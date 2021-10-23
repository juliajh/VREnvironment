using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Opening : MonoBehaviour
{
    public GameObject alarmPanel;

    [SerializeField]
    private GameObject rightController;

    [SerializeField]
    private GameObject leftController;

    private Animator panelAnim;
    private AudioSource audio;
    private int stepNum = 0;
    private bool audioflag = false;
    //private float lastMin;

    // Start is called before the first frame update
    void Start()
    {
        alarmPanel.SetActive(false);
        /*
        string lastTime = PlayerPrefs.GetString("SaveLastTime");
        System.DateTime lastDateTime = System.DateTime.Parse(lastTime);
        System.TimeSpan compareTime = System.DateTime.Now - lastDateTime;
        lastMin = float.Parse(compareTime.TotalMinutes.ToString()); //지난 시간의 minute*/

        stepNum = 0;
        alarmPanel.SetActive(true);
        panelAnim = alarmPanel.GetComponent<Animator>();
        panelAnim.SetBool("talking", true);
        audio = alarmPanel.transform.GetChild(1).GetComponent<AudioSource>();

        alarmPanel.transform.GetChild(1).GetComponent<Text>().text = "안녕하세요 저는 우주라고 해요.\n저희 마을에 오신 것을 환영해요.\n저희 마을은 푸른 나무도 많고\n예쁜 꽃들도 많은 살기 좋은 마을이에요.\n" +
            "다만 요즈음 환경 오염이 심해져서\n다들 걱정이 많아요...\n 저를 도와 마을을 정화시켜 주세요!\n(다음으로 넘기려면 흰색 버튼을 누르세요.)";
        audio.clip = Resources.Load("Audios/opening") as AudioClip;
        audio.Play();
        StartCoroutine(waitForSoundEnd(audio.clip));
    }

    // Update is called once per frame
    void Update()
    {
        if (stepNum == 1 && !audioflag)
        {
            GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().RmenuButtonLight();
            //when user clicks the skip button
            bool menuPressed = rightController.transform.GetComponent<VRTK.VRTK_ControllerEvents>().menuPressed;
            if (menuPressed)
            {
                GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().offRmenuButton();
                GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().LtouchpadLight();
                alarmPanel.transform.GetChild(1).GetComponent<Text>().text = "터치를 통해서 원하는 방향으로\n이동할 수 있어요.\n(다음으로 넘기려면 흰색 버튼을 누르세요.)";  //채우기
                audio.clip = Resources.Load("Audios/moving_touch") as AudioClip; 
                audio.Play();

                StartCoroutine(waitForSoundEnd(audio.clip));
                
            }
        }

        else if (stepNum == 2 && !audioflag)
        {
            GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().offLtouchpad();
            GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().RmenuButtonLight();

            bool menuPressed = rightController.transform.GetComponent<VRTK.VRTK_ControllerEvents>().menuPressed;
            if (menuPressed)
            {
                GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().offRmenuButton();
                GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().RtouchpadLight();
                alarmPanel.transform.GetChild(1).GetComponent<Text>().text = "레이저를 쏘아 클릭을 할 수 있어요.\n 식물 심거나 살 때 이용해보세요.";  //채우기
                audio.clip = Resources.Load("Audios/rightTouchpad_click") as AudioClip;  //채우기
                audio.Play();

                StartCoroutine(waitForSoundEnd(audio.clip));
            }
        }

        else if(stepNum==3)
        {
            GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().offRtouchpad();
            GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().RmenuButtonLight();

            bool menuPressed = rightController.transform.GetComponent<VRTK.VRTK_ControllerEvents>().menuPressed;
            if(menuPressed)
            {
                GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().offRmenuButton();
                GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().LmenuButtonLight();
                alarmPanel.transform.GetChild(1).GetComponent<Text>().text = "메뉴 버튼을 눌러보세요.";  
                audio.clip = Resources.Load("Audios/menubutton") as AudioClip;  
                audio.Play();
                stepNum++;
            }
        }


        else if (stepNum == 4 && !audioflag)
        {
            bool menupressed = leftController.GetComponent<VRTK.VRTK_ControllerEvents>().menuPressed;
            if (menupressed)
            {
                GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().offLmenuButton();
                GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().LtouchpadLight();
                alarmPanel.transform.GetChild(1).GetComponent<Text>().text =
                    "전원 : 게임을 저장하고 종료할 수 있어요.\n" +
                    "물뿌리개 : 가게에서 물뿌리개를 사서 식물에게 물을 줄 수 있어요.\n" +
                    "퍼즐: 동물 친구들에게 말을 걸어\n퍼즐을 모아보세요.\n" +
                    "코인: 코인을 이용하여 가게에서\n필요한 것을 사보세요.\n" +
                    "가방: 가게에서 식물을 사서\n정원을 가꾸어봐요.";  

                audio.clip = Resources.Load("Audios/menu_guide") as AudioClip;
                audio.Play();

                StartCoroutine(waitForSoundEnd(audio.clip));
            }
        }

        else if (stepNum == 5 && !audioflag)
        {
            GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().offLtouchpad();
            GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().LmenuButtonLight();

            bool menupressed = leftController.GetComponent<VRTK.VRTK_ControllerEvents>().menuPressed;
            if (menupressed)
            {
                GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().offLmenuButton();
                GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().LtouchpadLight();
                alarmPanel.transform.GetChild(1).GetComponent<Text>().text = "그럼 이제 마을로 나가볼까요??";

                audio.clip = Resources.Load("Audios/goOutside") as AudioClip;
                audio.Play();

                StartCoroutine(waitForSoundEnd(audio.clip));
            }
        }
        else if (stepNum == 6 && !audioflag)
        {
            alarmPanel.GetComponent<Animator>().SetBool("talking", false);
        }

    }

    IEnumerator waitForSoundEnd(AudioClip audioclip)
    {
        audioflag = true;
        yield return new WaitForSeconds(audioclip.length);
        stepNum++;
        audioflag = false;
        
    }
}
