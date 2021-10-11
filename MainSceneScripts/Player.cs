using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject rightController;
    public GameObject leftController;
    public List<GameObject> miniGames;
    public GameObject target=null;
    public bool rayHit;
    //public bool menubuttonpressed;
    //public static bool gamestartflag=true;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject StoreDoor;

    [SerializeField]
    private GameObject HouseDoor;

    [SerializeField]
    private GameObject wateringcan;

    [SerializeField]
    private GameObject fadeinoutImg;

    [SerializeField]
    private GameObject alarmPanel;

    [SerializeField]
    private GameObject testObj;


    private bool checkbool; //투명도 조절 논리형 변수
    private Color color;
    private bool start;
    private bool audioflag;
    private AudioSource alarmaudio;
    private AudioSource backgroundAudio;
    private int stepNum = 0;
    private Animator ani;

    public static List<GameObject> Plantobject = new List<GameObject>();


    public void Start()
    {
        stepNum = 0;
        alarmPanel.SetActive(false);
        alarmaudio = alarmPanel.transform.GetChild(1).GetComponent<AudioSource>();
        backgroundAudio = this.GetComponent<AudioSource>();

        wateringcan.SetActive(false);

        if (StoreManager.mainsceneIn)
        {
            player.transform.position = new Vector3(-55f, 0.9f, 0.8f);
            StoreManager.mainsceneIn = false;
        }
        else
        {
            player.transform.localPosition = new Vector3(38.1f, 0.9f, 33.5f);
        }
        
        start = true;
        checkbool = false;
        fadeinoutImg.SetActive(true);
        color = fadeinoutImg.GetComponent<Image>().color;
        Plantobject.Clear();
        LoadCrop();
        ani = alarmPanel.GetComponent<Animator>();

        //종료시점~시작시점까지의 polluted
        string lastTime = PlayerPrefs.GetString("SaveLastTime");
        System.DateTime lastDateTime = System.DateTime.Parse(lastTime);
        System.TimeSpan compareTime = System.DateTime.Now - lastDateTime;
        float lastMin = float.Parse(compareTime.TotalMinutes.ToString()); //지난 시간의 minute

        if (lastMin >=1f)
        {
            stepNum = -1;
        }

        backgroundAudio.clip = Resources.Load("Audios/background") as AudioClip;
        backgroundAudio.Play();

    }


    void Update()
    {
        if (start)
        {
            StartCoroutine("fadeout");
        }

        else if (stepNum == 1 && !audioflag)
        {
            GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().RmenuButtonLight();

            bool menupressed = rightController.GetComponent<VRTK.VRTK_ControllerEvents>().menuPressed;
            if (menupressed)
            {
                GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().offRmenuButton();
                GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().RgripLight();
                alarmPanel.transform.GetChild(1).GetComponent<Text>().text = "마을 곳곳에 쓰레기가 버려져 있어.\n 주워서 올바르게 분리수거 해줘.\n" +
                    "밑에 있는 큐브를 잡아볼래?";
                alarmaudio.clip = Resources.Load("Audios/gripTest") as AudioClip;
                alarmaudio.Play();
                StartCoroutine(waitForSoundEnd(alarmaudio.clip));
            }
        }

        else if (stepNum == 2 && !audioflag)
        {
            ani.SetBool("talking", false);
            if (rightController.GetComponent<VRTK.VRTK_InteractGrab>().GetGrabbedObject() == testObj)
            {
                GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().offRgrip();
                ani.SetBool("alarm", true);
                GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().RmenuButtonLight();
                alarmPanel.transform.GetChild(1).GetComponent<Text>().text = "참 잘했어!\n(다음으로 넘기려면 흰색 버튼을 누르세요.)";
                alarmaudio.clip = Resources.Load("Audios/goodJob") as AudioClip;
                alarmaudio.Play();
                StartCoroutine(waitForSoundEnd(alarmaudio.clip));
                
            }
        }

        else if (stepNum == 3 && !audioflag)
        {
            ani.SetBool("alarm", false);
            Destroy(testObj);
            bool menupressed = rightController.GetComponent<VRTK.VRTK_ControllerEvents>().menuPressed;
            if (menupressed)
            {
                ani.SetBool("talking", true);

                alarmPanel.transform.GetChild(1).GetComponent<Text>().text = "퍼즐 조각에게 다가가면 퍼즐을 얻을 수 있어.\n퍼즐을 다 채워서\n멸종 위기 동물에 대해 알아보자.\n(다음으로 넘기려면 흰색 버튼을 누르세요.)";
                alarmaudio.clip = Resources.Load("Audios/puzzle_guide") as AudioClip;
                alarmaudio.Play();

                StartCoroutine(waitForSoundEnd(alarmaudio.clip));
            }
        }

        else if (stepNum == 4 && !audioflag)
        {
            
            bool menupressed = rightController.GetComponent<VRTK.VRTK_ControllerEvents>().menuPressed;
            if (menupressed)
            {
                alarmPanel.transform.GetChild(1).GetComponent<Text>().text =
                    "마을을 정화하면 코인을 얻을 수 있어.\n" +
                    "공장에서 나쁜 물질들이 많아지면\n경보가 울려.\n" +
                    "경보를 무시하면 힘들게 키운 식물들이 아파하니 조심해!\n" +
                    "오리와 두더지 친구에게 가보면\n설명을 더 해줄거야!";
                alarmaudio.clip = Resources.Load("Audios/factory_guide") as AudioClip;
                alarmaudio.Play();

                StartCoroutine(waitForSoundEnd(alarmaudio.clip));
            }
        }

        else if (stepNum == 5 && !audioflag)
        {
            ani.SetBool("talking", false);
            GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().offRmenuButton();
            stepNum = 6;
        }

        //load to another scene
        if (HouseDoor.GetComponent<Opendoor>().scenechange)
        {
            StartCoroutine("fadein");
            if (checkbool)
            {
                SceneManager.LoadScene("MainHouse");
                checkbool = false;
            }
        }
        else if (StoreDoor.GetComponent<Opendoor>().scenechange)
        {
            StartCoroutine("fadein");
            if (checkbool)
            {
                SceneManager.LoadScene("StoreScene");
                checkbool = false;
            }
        }
    }

    //Reading crop txt informaition and load it on farm
    public void LoadCrop()
    {
        //yield return new WaitForSeconds(1f);

        for (int i = 0; i < SqlDB.farmplantList.Count; i++)
        {
            string name = "Prefabs/Plantings/Plant_" + SqlDB.farmplantList[i].Name.ToString() + "/Planting_" + SqlDB.farmplantList[i].Name.ToString() + "." + SqlDB.farmplantList[i].size;
            Vector3 position = new Vector3(SqlDB.farmplantList[i].x, -0.0068f, SqlDB.farmplantList[i].z);
            GameObject plant = Instantiate(Resources.Load<GameObject>(name), position, Quaternion.identity) as GameObject;
            Plantobject.Add(plant);
            plant.transform.parent = GameObject.Find("Plant_" + SqlDB.farmplantList[i].Name.ToString()).transform;
            plant.transform.tag = "Plant";
            plant.transform.localScale = new Vector3(1, 1, 1);

        }

    }
    
    IEnumerator fadein()  //
    { //fadeinoutImg 활성화
        fadeinoutImg.SetActive(true);
        for (int i = 150; i >= 0; i--)
        {
            color.a += Time.deltaTime * 0.015f;
            fadeinoutImg.GetComponent<Image>().color = color;
        }
        if (color.a >= 1.0f)
            checkbool = true;
        yield return null;
    }

    IEnumerator fadeout()  //
    {
        for (int i = 200; i >= 0; i--) //i=100이고 i가 0보다 크거나같으면
        {
            color.a -= Time.deltaTime * 0.005f;
            fadeinoutImg.GetComponent<Image>().color = color;
        }
        if (color.a <= 0.02f)
        {
            start = false;
            color.a = 0.0f;
            fadeinoutImg.GetComponent<Image>().color = color;
            fadeinoutImg.SetActive(false);  //fadeinoutImg 비활성화
        }
        yield return null;

        if (MainHouseManager.mainsceneIn && !StoreManager.mainsceneIn && stepNum == -1)
        {
            stepNum = 0;
            alarmPanel.SetActive(true);
            alarmPanel.transform.GetChild(1).GetComponent<Text>().text = "안녕! 나는 우주 친구 별이야!\n우리 마을에 온 것을 환영해!\n(다음으로 넘기려면 흰색 버튼을 누르세요.)";
            ani.Play("openingStart");
            alarmaudio.clip = Resources.Load("Audios/openMainScene") as AudioClip;
            alarmaudio.Play();
            MainHouseManager.mainsceneIn = false;
            //start = false;
            StartCoroutine(waitForSoundEnd(alarmaudio.clip));
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

