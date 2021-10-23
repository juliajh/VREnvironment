using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainHouseManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Door;

    [SerializeField]
    private GameObject puzzleInPanel;

    [SerializeField]
    private GameObject wateringcan;

    [SerializeField]
    private GameObject fadeinoutImg;

    [SerializeField]
    private GameObject rightController;


    public MaterialData[] Datas;
    //public static bool mainsceneIn = true;
    private List<GameObject> puzzleinPanelList;
    private bool checkbool;
    private Color color;
    private bool start;
    private bool rayHit;
    private GameObject target;


    // Start is called before the first frame update
    void Start()
    {
        /*
        if (!mainsceneIn)
        {
            Destroy(GameObject.Find("HouseManager").GetComponent<Opening>());
            mainsceneIn = false;
        }
        else
        {
            GameObject.Find("HouseManager").GetComponent<Opening>().enabled = true;
        }*/
        puzzleInPanel.SetActive(false);
        wateringcan.SetActive(false);
        puzzleinPanelList = new List<GameObject>();
        for (int i = 0; i < puzzleInPanel.transform.childCount; i++)
            puzzleinPanelList.Add(puzzleInPanel.transform.GetChild(i).gameObject);
        for (int i = 0; i < puzzleinPanelList.Count; i++)
        {
            puzzleinPanelList[i].SetActive(false);
        }
        start = true;
        checkbool = false;
        color = fadeinoutImg.GetComponent<Image>().color;
        rayHit = false;
        target = null;
        fadeinoutImg.SetActive(false);
        
    }

    // Update is called once per frame

    void Update()
    {
        if (start)
        {
            StartCoroutine(fadeout());
        }

        if (SqlDB.readingDone)
        {
            for (int i = 0; i < SqlDB.puzzleList.Count; i++)
            {
                if (SqlDB.puzzleList[i] == 1)
                {
                    puzzleinPanelList[i].SetActive(true);
                    puzzleinPanelList[i].GetComponent<Image>().material = Datas[SqlDB.pictNum].mats[i];
                }
            }
        }

        if (Door.GetComponent<Opendoor>().scenechange)
        {
            StartCoroutine("fadein");
            if (checkbool)
            {
                SceneManager.LoadScene("MainScene");
            }
        }
    }
    IEnumerator fadein()  
    { //fadeinoutImg 활성화
        fadeinoutImg.SetActive(true);
        //if(GameObject.Find("HouseManager").GetComponent<Opening>()!=null)
        //    GameObject.Find("HouseManager").GetComponent<Opening>().alarmPanel.SetActive(false);
        for (int i = 150; i >= 0; i--)
        {
            color.a += Time.deltaTime * 0.015f; 
            fadeinoutImg.GetComponent<Image>().color = color;
        }
        if (color.a >= 1.0f)
        {
            checkbool = true;
        }
        yield return null;
    }

    IEnumerator fadeout()  //
    {
        for (int i = 200; i >= 0; i--) //i=100이고 i가 0보다 크거나같으면
        {
            color.a -= Time.deltaTime * 0.005f;
            fadeinoutImg.GetComponent<Image>().color = color;
        }
        if (color.a <= 0.002f)
        {
            start = false;
            color.a = 0.0f;
            fadeinoutImg.GetComponent<Image>().color = color;
            fadeinoutImg.SetActive(false);  //fadeinoutImg 비활성화
        }
        yield return new WaitForSeconds(0.5f);

    }

    
}
