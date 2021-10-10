using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    //private int buttonsNum=6; 
    //public static List<GameObject> treePrefabs;
    //public static bool click;   //true when the player choosed a crop
   //public static bool havemoney;   //true when the player has enough money
    public static int indexoftree;  
    public List<GameObject> buttonList=new List<GameObject>();
    public List<GameObject> plantmenuList = new List<GameObject>();
    //public GameObject Nomoneytextplace;
    public GameObject rightController;
    private GameObject target;

    [SerializeField]
    private GameObject StoreMenuPanel;

    [SerializeField]
    private GameObject BagPlantmenu;

    [SerializeField]
    private GameObject WateringcanText;

    [SerializeField]
    private GameObject coinText;

    [SerializeField]
    private GameObject Doorcoordinate;

    [SerializeField]
    private GameObject puzzleInPanel;

    [SerializeField]
    private GameObject fadeinoutImg;

    [SerializeField]
    private GameObject alarmPanel;

    public MaterialData[] Datas;
    private List<GameObject> puzzleinPanelList;
    public static bool mainsceneIn;
    private bool checkbool; //투명도 조절 논리형 변수
    private Color color;
    private bool start;
    private Animator panelAnim;
    private AudioSource audio;
    private bool audioflag = false;


    // Start is called before the first frame update
    void Start()
    {
        SqlSave.coin = 100;
        puzzleInPanel.SetActive(false);
        puzzleinPanelList = new List<GameObject>();
        for (int i = 0; i < puzzleInPanel.transform.childCount; i++)
            puzzleinPanelList.Add(puzzleInPanel.transform.GetChild(i).gameObject);
        for (int i = 0; i < puzzleinPanelList.Count; i++)
        {
            puzzleinPanelList[i].SetActive(false);
            puzzleinPanelList[i].GetComponent<Image>().material = Datas[SqlSave.pictNum].mats[i];
        }
        for (int i = 0; i < SqlSave.puzzleList.Count; i++)
        {
            if (SqlSave.puzzleList[i] == 1)
            {
                puzzleinPanelList[i].SetActive(true);
            }
        }

        mainsceneIn = false;
        start = true;
        checkbool = false;
        color = fadeinoutImg.GetComponent<Image>().color;
        fadeinoutImg.SetActive(true);

        coinText.GetComponent<Text>().text = SqlSave.coin.ToString();
        panelAnim = alarmPanel.GetComponent<Animator>();
        audio = alarmPanel.transform.GetChild(1).GetComponent<AudioSource>();
        alarmPanel.SetActive(false);

        for (int i = 0; i < StoreMenuPanel.transform.childCount; i++)
        {
            buttonList.Add(StoreMenuPanel.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < BagPlantmenu.transform.childCount; i++)
        {
            plantmenuList.Add(BagPlantmenu.transform.GetChild(i).gameObject);
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            StartCoroutine(fadeout());
        }
        bool touchpadout = rightController.transform.GetComponent<VRTK.VRTK_ControllerEvents>().touchpadout();

        if (touchpadout)
        {
            target = rightController.transform.GetChild(0).GetComponent<VRTK.VRTK_StraightPointerRenderer>().getObj();
            if (buttonList.Contains(target))
            {
                if (SqlSave.coin>=10)
                {
                    SqlSave.coin = SqlSave.coin - 10;
                    coinText.GetComponent<Text>().text = SqlSave.coin.ToString();
                    indexoftree = int.Parse(target.name.Substring(target.name.LastIndexOf("_") + 1));
                    GameObject selectedplant = plantmenuList[indexoftree];
                    
                    NumUp(selectedplant);
                    target = null;
                }

                else
                {
                    if (!audioflag)
                    {
                        panelAnim.SetBool("alarm", true);
                        //click = false;
                        alarmPanel.transform.GetChild(1).GetComponent<Text>().text = "코인이 부족해요.";
                        audio.clip = Resources.Load("Audios/coin") as AudioClip;
                        audio.Play();
                        StartCoroutine(waitForSoundEnd(audio.clip));
                    }
                }
            }
            
        }

        //문 근처에 가면 Main Scene으로 가기 구현
        if (Doorcoordinate.GetComponent<Opendoor>().scenechange)
        {
            StartCoroutine("fadein");
            if (checkbool)
            {
                mainsceneIn = true;
                SceneManager.LoadScene("MainScene");
                checkbool = false;
            }
        }
    }

    public void NumUp(GameObject selectedplant) //잘 돌아감. 문제X 
    {
        string text = selectedplant.transform.GetChild(0).GetComponent<Text>().text;
        int numofplant = int.Parse(text.Substring(text.LastIndexOf(" ") + 1));
        
        selectedplant.transform.GetChild(0).GetComponent<Text>().text = "X "+(numofplant + 1).ToString();

        SqlSave.SavedplantList[indexoftree].Count++;
    }

    public void wateringcan()
    {
        Debug.Log("watercan");
        if (SqlSave.coin >= 2)
        {
            SqlSave.coin = SqlSave.coin - 2;
            string text = WateringcanText.GetComponent<Text>().text;
            int numofcan = int.Parse(text.Substring(text.LastIndexOf(" ") + 1));
            WateringcanText.GetComponent<Text>().text = "X " + (numofcan + 1).ToString();
            coinText.GetComponent<Text>().text = SqlSave.coin.ToString();

            SqlSave.wateringcanNum++;
        }
        else
        {
            if (!audioflag)
            {
                panelAnim.SetBool("alarm", true);
                alarmPanel.transform.GetChild(1).GetComponent<Text>().text = "코인이 부족해요.";
                audio.clip = Resources.Load("Audios/coin") as AudioClip;
                audio.Play();
                StartCoroutine(waitForSoundEnd(audio.clip));
            }
        }
    }
    IEnumerator fadein()  //
    { //fadeinoutImg 활성화
        fadeinoutImg.SetActive(true);
        alarmPanel.SetActive(false);
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
        color = fadeinoutImg.GetComponent<Image>().color;

        for (int i = 100; i >= 0; i--) //i=100이고 i가 0보다 크거나같으면
        {
            color.a -= Time.deltaTime * 0.01f;
            fadeinoutImg.GetComponent<Image>().color = color;
        }
        if (color.a <= 0.3f)
        {
            start = false;
            color.a = 0.0f;
            fadeinoutImg.GetComponent<Image>().color = color;
            fadeinoutImg.SetActive(false);  //fadeinoutImg 비활성화
        }
        yield return new WaitForSeconds(0.5f);

        if (!start)
        {
            alarmPanel.SetActive(true);
            panelAnim.SetBool("talking", true);
            GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().RtouchpadLight();
            alarmPanel.transform.GetChild(1).GetComponent<Text>().text = "버튼을 눌러 씨앗과 물뿌리개를 사보세요." +
                "\n 씨앗은 10 코인, 물뿌리개는 2코인이에요." +
                "\n 씨앗과 물뿌리개로 예쁜 농장을 가꾸어봐요!";
            audio.clip = Resources.Load("Audios/storeOpening") as AudioClip;
            audio.Play();
            StartCoroutine(waitForSoundEnd(audio.clip));  //time for the user to see the light
            GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().offRtouchpad();
        }

    }

    IEnumerator waitForSoundEnd(AudioClip audioclip)
    {
        audioflag = true;
        yield return new WaitForSeconds(audioclip.length);
        audioflag = false;
        if (panelAnim.GetBool("alarm"))
            panelAnim.SetBool("alarm", false);
        else if (panelAnim.GetBool("talking"))
            panelAnim.SetBool("talking", false);
    }

}
