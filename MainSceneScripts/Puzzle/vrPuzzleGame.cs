using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System;
using UnityEngine.UI;

public class vrPuzzleGame : MonoBehaviour
{
    /// <summary>
    /// 주석 5개 친 것 (/////)은 올바른 코드이지만 StartGame을 써야 하므로 StartGame을 먼저 실행하지 않으면 오류가 발생하는 코드. 따라서 전체적으로 정비 되고 나면 주석 없애기.
    /// </summary>
    [SerializeField]
    private GameObject allPuzzleParent; //바닥에 놓여진 puzzle들의 parent

    [SerializeField]
    private GameObject RightController;

    [SerializeField]
    private GameObject puzzleInPanel;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject alarmPanel;


   // [SerializeField]
    //private GameObject PuzzlePs;

    //[SerializeField]
    //private GameObject Effect;

    [SerializeField]
    private GameObject puzzleps;

    public MaterialData[] Datas;
    public static List<GameObject> allPuzzleList;
    private List<Vector3> puzzlesPositionList;
    private List<GameObject> positionList;
    private List<GameObject> puzzleinPanelList;
    private List<string> animalInfoList;
    //public static List<GameObject> EffectList;
    public static List<GameObject> puzzlepsList;

    public int pictNum; //동물이 무엇인지 확인하기 위한 num
    private int targetNum;
    private int puzzleInNum;

    public static bool settingStart; //settingStart
    private bool get;
    private float force=5f;
    private bool flag = false;

    private RectTransform rt;

    void Start()
    {
        animalInfoList = new List<string>();
        animalInfoList.Add("<검은손 긴팔 원숭이>\n멸종위기: 위기 단계\n서식지: 인도네시아의 수마트라 섬\n멸종위기이유: 서식지의 파괴, 사냥");
        animalInfoList.Add("<고릴라>\n멸종위기: 위기 단계\n서식지: 르완다\n멸종위기이유: 주민들의 화전 및 숯 제조로 인한 서식지 파괴");
        animalInfoList.Add("<필리핀독수리>\n멸종위기: 위급 단계\n서식지: 필리핀\n멸종위기이유: 농장의 확대, 무분별한 벌목, 광산 개발 등 서식지 파괴");
        animalInfoList.Add("<비단 마모셋>\n멸종위기: 관심필요 단계\n서식지: 브라질 북동부 해안\n멸종위기이유: 밀수");
        animalInfoList.Add("<말레이시벳>\n멸종위기: 위급 단계\n서식지: 인도\n멸종위기이유: 서식지 파괴, 사냥");
        animalInfoList.Add("<사막여우>\n멸종위기: 관심필요 단계\n서식지: 북아프리카 모래사막\n멸종위기이유: 모피제작, 애완용 포획");
        animalInfoList.Add("<원숭이올빼미>\n멸종위기: 관심필요 단계\n서식지: 전세계\n멸종위기이유: 서식지파괴, 밀렵");
        animalInfoList.Add("<반달가슴곰>\n멸종위기: 취약 단계\n서식지: 산지\n멸종위기이유: 전쟁으로 인한 서식지파괴, 밀렵");

        allPuzzleList = new List<GameObject>();
        for (int i = 0; i < allPuzzleParent.transform.childCount; i++)
            allPuzzleList.Add(allPuzzleParent.transform.GetChild(i).gameObject);
        positionList = new List<GameObject>();
        foreach (GameObject allP in allPuzzleList)
            allP.SetActive(true);
        puzzleinPanelList = new List<GameObject>();
        for (int i = 0; i < puzzleInPanel.transform.childCount; i++)
            puzzleinPanelList.Add(puzzleInPanel.transform.GetChild(i).gameObject);
        foreach (GameObject puzzle in puzzleinPanelList)
            puzzle.SetActive(false);
        puzzlesPositionList = new List<Vector3>();
        foreach (GameObject puzzle in allPuzzleList)
            puzzlesPositionList.Add(puzzle.transform.position);
        /*
        EffectList = new List<GameObject>();
        for (int k = 0; k < Effect.transform.childCount; k++)
            EffectList.Add(Effect.transform.GetChild(k).gameObject);*/
        puzzlepsList = new List<GameObject>();
        for (int k = 0; k < puzzleps.transform.childCount; k++)
            puzzlepsList.Add(puzzleps.transform.GetChild(k).gameObject);
        settingStart = true;
        get = false;
        puzzleInNum = 0;
        targetNum = 0;
        pictNum = 0;
        //PuzzlePs.SetActive(false);

    }


    void Update()
    {
        if (settingStart)
        {
            pictNum = SqlDB.pictNum;
            Debug.Log("pictNum" + SqlDB.pictNum);
            Setting(pictNum);
            settingStart = false;
        }

        if (get)
        {

            /*
            rb = puzzleinPanelList[targetNum].GetComponent<Rigidbody2D>();
            Debug.Log("rt.localPosition = "+rt.localPosition);
            Debug.Log("rt.localScale = " + rt.localScale);

            rt.localPosition = new Vector3(rt.localPosition.x, rt.localPosition.y, -100f);
            Vector3 distance = new Vector3(rt.localPosition.x, rt.localPosition.y, -0.1f) - new Vector3(rt.localPosition.x, rt.localPosition.y, -100f);
            distance = distance.normalized;
            distance= distance* force;
            rb.AddForce(distance);
            */
            if (!flag)
            {
                puzzleInNum++;
                rt.localPosition = new Vector3(rt.localPosition.x, rt.localPosition.y, -100f);
                rt.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                flag = true;
            }

            else
            {
                rt.localPosition = Vector3.Lerp(rt.localPosition, new Vector3(rt.localPosition.x, rt.localPosition.y, -0.1f), Time.deltaTime * 1.5f);
                rt.localScale = Vector3.Lerp(rt.localScale, new Vector3(0.5f, 0.5f, 0.5f), Time.deltaTime * 2f);
            }

            if (!puzzleInPanel.activeSelf)
            {
                //Destroy
                get = false;
                flag = false;
            }
        }

        if (puzzleInNum == 16)
        {
            Debug.Log("pictNum" + SqlDB.pictNum);
            StartCoroutine(clearEffect(pictNum));
            //Clear.SetActive(true);
            if (pictNum == 7)
                pictNum = 0;
            else
                pictNum++;
            SqlDB.pictNum = pictNum;
            for (int i = 0; i < SqlDB.puzzleList.Count; i++)
                SqlDB.puzzleList[i] = 0;
            for (int k = 0; k < allPuzzleList.Count; k++)
            {
                allPuzzleList[k].transform.position = puzzlesPositionList[k];
                allPuzzleList[k].SetActive(true);
            }
            puzzleInNum = 0;
            SqlDB.coin += 50;

        }
    }

    void Setting(int pictNum)
    {
        for (int k = 0; k < allPuzzleList.Count; k++)
        {
            allPuzzleList[k].GetComponent<MeshRenderer>().material = Datas[pictNum].mats[k];
            puzzleinPanelList[k].GetComponent<Image>().material = Datas[pictNum].mats[k];
            allPuzzleList[k].GetComponent<Collider>().enabled = true;
            puzzleinPanelList[k].SetActive(false);
        }
        for (int i = 0;i< SqlDB.puzzleList.Count;i++)
        {
            if (SqlDB.puzzleList[i]==1)
            {
                puzzleinPanelList[i].SetActive(true);
                allPuzzleList[i].SetActive(false);
                puzzlepsList[i].SetActive(false);
                puzzleInNum++;
            }
        }
    }

    public void puzzleGet(GameObject target)
    {
        if (target != null && allPuzzleList.Contains(target))
        {
            targetNum = allPuzzleList.IndexOf(target);
            SqlDB.puzzleList[targetNum] = 1;
            StartCoroutine(puzzleTimer(target));
            puzzlepsList[targetNum].SetActive(false);
            puzzleinPanelList[targetNum].SetActive(true); //이때, 기본적으로 puzzleinPanel 의 panel은 SetActive(false) 상태이어야 함! 
            //menu를 통해 puzzle을 클릭했을 경우에만 SetActive(true)가 되도록
            
            puzzleInPanel.SetActive(true);
            rt = puzzleinPanelList[targetNum].GetComponent<RectTransform>();
            get = true;
        }
    }

    IEnumerator puzzleTimer(GameObject target)
    {

        yield return new WaitForSeconds(3.0f);
        target.SetActive(false);
    }

    IEnumerator clearEffect(int pictNum)
    {
        alarmPanel.SetActive(true);
        alarmPanel.GetComponent<Animator>().SetBool("talking", true);
        alarmPanel.transform.GetChild(1).GetComponent<Text>().text = animalInfoList[pictNum];
        yield return new WaitForSeconds(10.0f);
        puzzleInPanel.SetActive(false);
        alarmPanel.GetComponent<Animator>().SetBool("talking", false);
        alarmPanel.GetComponent<Animator>().SetBool("alarm", true);
        yield return new WaitForSeconds(1.2f);
        alarmPanel.transform.GetChild(1).GetComponent<Text>().text = "50 코인을 획득했습니다";
        yield return new WaitForSeconds(3.0f);
        alarmPanel.GetComponent<Animator>().SetBool("alarm", false);
        alarmPanel.GetComponent<Animator>().SetBool("talking", true);
        alarmPanel.SetActive(false);
        for (int k = 0; k < puzzleinPanelList.Count; k++)
        {
            puzzleinPanelList[k].SetActive(false);
        }

        settingStart = true;
    }
}
