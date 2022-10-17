using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class AnimalGameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject duckParent;

    [SerializeField]
    private GameObject mole;

    public GameObject talkPanel;
    public animalTalkingManager talkManager;
    public GameObject player;
    public GameObject Move; //사용자 멈추게 하기 위한 Move
    public Text talkText;
    private GameObject animObj; //동물이 무엇인지
    private bool isAction; //얘기하고 있는 지 여부
    private GameObject previousAnim;
    public int talkindex;
    public int numofAnim;
    public static List<int> selectedAnimalNum;
    private Transform playerOriginParent;

    //animal talk
    private TalkController tc;

    // Start is called before the first frame update
    void Start()
    {
        selectedAnimalNum = new List<int>();
        animObj = null;
        isAction = false;
        talkindex = 0;
        talkPanel.SetActive(false);
        tc = new TalkController();
        previousAnim = null;
        tc.check = false;
        tc.text = talkText;
        tc.Skip_delay = 1.0f;
        tc.delay = 0.07f;
        numofAnim = 0;
        playerOriginParent = player.transform.parent;
        duckParent.GetComponent<Animator>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (vrPuzzleGame.settingStart)
            SettingAnimalPuzzle();
        if (!isAction) //일반적으로 걸어다니고 있을 때
        {
            if (Vector3.Distance(player.transform.position, mole.transform.position) <= 6.0f)
            {
                mole.GetComponent<Animator>().enabled = true;
                mole.GetComponent<Animator>().SetBool("attack", true);
            }
            else
            {
                mole.GetComponent<Animator>().SetBool("attack", false);
            }

            foreach (GameObject animal in talkManager.listOfAnimalObject)
            {
                
                if (Vector3.Distance(player.transform.position, animal.transform.position) <= 3.0f) //player와 동물의 거리가 가까우면
                {
                    if (previousAnim == animal)
                    {
                        break;
                    }
                    animObj = animal; //가까운 동물을 animObj로 지정한다.
                    numofAnim = talkManager.listOfAnimalObject.IndexOf(animObj);
                    isAction = true;
                    tc.text_exit = false;
                    tc.text_full = false;
                    AudioSource audio = animObj.GetComponent<AudioSource>();
                    audio.clip = talkManager.GetAudio(animObj);
                    audio.Play();
                }
            }
        }
        else //동물과 말하기 시작
        {
            talkPanel.SetActive(true);
            Move.SetActive(false);

            if (!tc.check) {
                StartCoroutine(tc.ShowText(talkManager.GetTalk(animObj)));
                tc.check = true;
            }
            if (tc.text_full && tc.check)
            {
                tc.text_full = false;
                StartCoroutine(tc.ShowText(talkManager.GetTalk(animObj)));
            }

            if (tc.text_exit)
            {
                if (selectedAnimalNum.Contains(talkManager.listOfAnimalObject.IndexOf(animObj)))
                    StartCoroutine(GivingPuzzle());
                talkText.text = "";
                talkPanel.SetActive(false);
                isAction = false;
                tc.text_exit = false;
                tc.text_full = false;
                tc.check = false;
                tc.flag = false;
                tc.cnt = 0;
                talkindex = 0;
                previousAnim = animObj;
                animObj = null;
                Move.SetActive(true);
                if (numofAnim == 4)
                {
                    StartCoroutine(DuckGoing());
                }
                if(numofAnim==3)
                {
                    StartCoroutine(moleGuideline());
                }
            }
        }
    }

    //GivingPuzzle 수정. 
    IEnumerator GivingPuzzle()
    {
        if (SqlDB.puzzleList[talkManager.listOfAnimalObject.IndexOf(animObj)] == 0)
        {
            vrPuzzleGame.allPuzzleList[talkManager.listOfAnimalObject.IndexOf(animObj)].SetActive(true);
            vrPuzzleGame.puzzlepsList[talkManager.listOfAnimalObject.IndexOf(animObj)].SetActive(true);
            yield return new WaitForSeconds(3.0f);
        }
        else
            yield return null;
    }

    IEnumerator DuckGoing()
    {
        Move.SetActive(false);
        player.transform.parent = talkManager.listOfAnimalObject[4].transform;
        duckParent.GetComponent<Animator>().enabled = true;
        talkManager.listOfAnimalObject[4].GetComponent<Animator>().SetTrigger("walk");
        duckParent.GetComponent<Animator>().SetTrigger("walkback");
        yield return new WaitForSeconds(10.2f);
        player.transform.parent = playerOriginParent;
        duckParent.GetComponent<Animator>().ResetTrigger("walkback");
        GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().RTriggerButtonLight();
        Move.SetActive(true);
        yield return new WaitForSeconds(7f);
        GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().offRtriggerButton();
        talkManager.listOfAnimalObject[4].GetComponent<Animator>().SetTrigger("idle");
        duckParent.GetComponent<Animator>().enabled = false;
    }

    IEnumerator moleGuideline()
    {
        GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().RgripLight();
        yield return new WaitForSeconds(7f);
        GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().offRgrip();

    }

    public void SettingAnimalPuzzle()
    {
        int k = 0;
        int randNum = 0;
        bool selected = true;
        selectedAnimalNum.Clear();

        while (k <= 3)
        {
            selected = true;
            System.Random rand = new System.Random();
            randNum = rand.Next(0, 8);

            if (k != 0)
            {
                foreach(int a in selectedAnimalNum)
                {
                    if (a == randNum)
                        selected = false;
                    if (!selected)
                        break;
                }
            }
            else
            {
                selectedAnimalNum.Add(randNum);
                selected = false;
                k++;
            }
            if (selected && selectedAnimalNum.Count >= 1)
            {
                selectedAnimalNum.Add(randNum);
                k++;
            }
        }

        //foreach (int i in selectedAnimalNum)
        //    Debug.Log("Selected Animal Num : " + i);

        //////////animal index random select over /////////////
        foreach(int i in selectedAnimalNum)
        {
            ///vrPuzzleGame.allPuzzleList[i].transform.position = talkManager.GetPosition(talkManager.listOfAnimalObject[i]);
            if (vrPuzzleGame.allPuzzleList[i].activeSelf)
            {
                vrPuzzleGame.allPuzzleList[i].SetActive(false);
                vrPuzzleGame.puzzlepsList[i].SetActive(false);
            }
            
        }
    }
}
