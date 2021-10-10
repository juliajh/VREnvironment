using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.SceneManagement;
using System;


public class PuzzleManager : MonoBehaviour
{
    //puzzle_1은 position_1에 들어가야지 정답!!

    [SerializeField]
    private GameObject puzzles;    //puzzle조각들의 parent gameobject
    [SerializeField]
    private GameObject Positions;   //plane부분에 퍼즐 조각이 들어갈 position
    [SerializeField]
    private GameObject Lines;

    public GameObject puzzlePlane;
    public MaterialData[] Datas;
    public GameObject ClearBox;
    public GameObject OverObject;
    public GameObject Opening;
    public GameObject OpenText;
    private GameObject[] PositionSet;
    private List<GameObject> puzzleList;  //puzzles의 조각들을 넣을 리스트
    private List<Vector3> puzzlesPosition; //puzzleList의 조각들 각각의 position. 틀렸을 때 되돌아 가려면 필요.
    private List<Vector3> positionList;
    private List<GameObject> LineList;  //Line들 넣을 List
    private bool[] Answers;
    int puzzleindex; //클릭한 puzzle의 list상의 인덱스
    int positionindex; //puzzle을 놓은 부분의 position의 list상의 인덱스
    private GameObject target;
    private bool puzzling;
    private bool playing;
    private bool positioning;
    private bool abovePlane;
    private bool start;
    int pictureNum;
    int leftNum;

    // Start is called before the first frame update
    void Start()
    {
        leftNum = 0;
        pictureNum = 0;
        puzzling = false;
        abovePlane = false;
        positioning = false;
        playing = false;
        start = true;
        target = null;
        puzzleList = new List<GameObject>();
        for (int i = 0; i < puzzles.transform.childCount; i++)
            puzzleList.Add(puzzles.transform.GetChild(i).gameObject);  //puzzleList에 puzzle조각들을 넣음.
        puzzlesPosition = new List<Vector3>();
        foreach (GameObject puzzle in puzzleList)
            puzzlesPosition.Add(puzzle.transform.position);  //puzzles들이 눕혀져있는 부분의 position을 넣음.
        LineList = new List<GameObject>();
        for (int l = 0; l < Lines.transform.childCount; l++)
            LineList.Add(Lines.transform.GetChild(l).gameObject); //line들
        foreach (GameObject l in LineList)
            l.SetActive(false);
        PositionSet = new GameObject[puzzleList.Count];
        for (int k = 0; k < puzzleList.Count; k++)
            PositionSet[k] = null;
        Answers = new bool[puzzleList.Count];
        for (int p = 0; p < puzzleList.Count; p++)
            Answers[p] = false;
        puzzleindex = 0;
        positionindex = 0;
        positionList = new List<Vector3>();
        for (int k = 0; k < Positions.transform.childCount; k++)
            positionList.Add(Positions.transform.GetChild(k).transform.position); //PuzzlePlane의 position들을 넣음
        foreach (GameObject puz in puzzleList)
            puz.SetActive(false);
        ClearBox.SetActive(false);
        OverObject.SetActive(false);
        Opening.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (start) //opening
        {
            StartCoroutine(Open());
            StartCoroutine(TextChange());
        }
        else //after the opening
        {
            if (!playing) //setting the puzzle sprite
            {
                foreach (GameObject puz in puzzleList)
                    puz.SetActive(true);
                StartCoroutine(Setting(pictureNum));
            }
            else
            {
                if (leftNum == 0) //starting a puzzle with smooth move of puzzles
                {
                    Vector3 velo = Vector3.zero;
                    for (int p = 0; p < puzzleList.Count; p++)
                    {
                        puzzleList[p].transform.position = Vector3.SmoothDamp(puzzleList[p].transform.position, puzzlesPosition[p], ref velo, 0.1f);
                        // puzzleList[p].transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    target = ClickObject();
                    if (target != null && puzzleList.Contains(target)) //if user click one of the puzzles
                    {
                        puzzling = true;
                        foreach (GameObject l in LineList)
                            l.SetActive(true);
                        puzzleindex = puzzleList.IndexOf(target);
                        //target.transform.rotation = Quaternion.Euler(new Vector3(45, 180, 0));
                        target.GetComponent<Collider>().enabled = false;
                        //target.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
                    }

                    if (leftNum == 0)
                        leftNum = 1;
                }
                else if (Input.GetMouseButton(0))
                {
                    if (puzzling) //if user click one of the puzzles
                    {
                        RaycastHit hit;
                        var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(Ray.origin, Ray.direction, out hit) && (hit.collider.gameObject == puzzlePlane || LineList.Contains(hit.collider.gameObject)))
                        {
                            target.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z - 0.1f);
                            Debug.Log(hit.collider.gameObject.name);
                        }
                        else
                        {
                            //Debug.Log(hit.collider.gameObject.name);
                            target.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
                        }
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    if (puzzling)
                    {
                        foreach (Vector3 pos in positionList)
                        {
                            if (Vector3.Distance(pos, target.transform.position) < 0.9f)
                            {
                                positionindex = positionList.IndexOf(pos);
                                positioning = true;
                                break;
                            }
                        }
                        if (positioning)
                        {
                            if (PositionSet[positionindex] != null)
                            {
                                PositionSet[positionindex].transform.position = puzzlesPosition[puzzleList.IndexOf(PositionSet[positionindex])];
                                PositionSet[positionindex].GetComponent<Collider>().enabled = true;
                                //PositionSet[positionindex].transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                                PositionSet[positionindex] = target;
                                target.transform.position = positionList[positionindex];
                            }
                            else
                            {
                                target.transform.position = positionList[positionindex];
                                PositionSet[positionindex] = target;
                            }
                            if (positionindex == puzzleindex)
                                Answers[positionindex] = true;
                            else
                            {
                                Answers[positionindex] = false;
                            }
                        }
                        else
                        {
                            target.transform.position = puzzlesPosition[puzzleList.IndexOf(target)];
                            target.GetComponent<Collider>().enabled = true;
                            //  target.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                        }
                        positioning = false;
                        puzzleindex = 0;
                        positionindex = 0;
                        target = null;
                        puzzling = false;
                        abovePlane = false;
                        foreach (GameObject l in LineList)
                            l.SetActive(false);
                        foreach (bool b in Answers)
                        {
                            if (!b)
                            {
                                leftNum = 1;
                                break;
                            }
                            else
                                leftNum = 2;
                        }
                        if (leftNum == 2)
                        {
                            ClearBox.SetActive(true);
                            StartCoroutine(NextPicture());
                            for (int p = 0; p < puzzleList.Count; p++)
                                Answers[p] = false;
                        }
                    }
                }
            }
        }
    }

    //return GameObject that the user click (usually 3D object)
    private GameObject ClickObject()
    {
        RaycastHit hit;
        GameObject target = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            target = hit.collider.gameObject;
        }
        return target;
    }

    //Opening
    IEnumerator Open()
    {
        Opening.SetActive(true);
        yield return new WaitForSeconds(6.0f);
        start = false;
        Opening.SetActive(false);
        OpenText.SetActive(false);
        
    }
    //Opening Text
    IEnumerator TextChange()
    {
        yield return new WaitForSeconds(2.7f);
        OpenText.GetComponent<TextMesh>().text = "너희가 그들을 지켜줘!!";
    }
    /* private Vector3 PlanePosition(GameObject target)
     {
         Vector3 objectposition = new Vector3(0, 0, 0);
         RaycastHit hit;
         Ray ray = new Ray(target.transform.position, Camera.main.transform.forward);
         //ray.origin = target.transform.position;
         if (Physics.Raycast(ray.origin, ray.direction, out hit) && hit.collider.gameObject == puzzlePlane)
         {
             Debug.Log(puzzlePlane + " =" + hit.point);
             objectposition = hit.point;
             abovePlane = true;
         }
         return objectposition;
     }*/

    //if the puzzle is over, move on to the next Pict
    IEnumerator NextPicture()
    {
        yield return new WaitForSeconds(3.0f);
        if (playing)
        {
            if (pictureNum < 2)
            {
                ClearBox.SetActive(false);
                pictureNum++;
                playing = false;
                leftNum = 0;
            }
            else
            {
                ClearBox.SetActive(false);
                OverObject.SetActive(true);
                StartCoroutine(SceneChange());
            }
        }
    }

    //setting the basic such as sprite of the picture
    IEnumerator Setting(int pictNum)
    {
        if (pictNum == 0)
        {
            for (int k = 0; k < puzzleList.Count; k++)
            {
                puzzleList[k].GetComponent<MeshRenderer>().material = Datas[pictNum].mats[k];
                //puzzleList[k].transform.rotation = Quaternion.Euler(new Vector3(45, 180, 0));
                puzzleList[k].transform.position = positionList[k];
            }

        }
        else if (pictNum < 3)
        {
            for (int k = 0; k < puzzleList.Count; k++)
            {
                puzzleList[k].GetComponent<MeshRenderer>().material = Datas[pictNum].mats[k];
            }
        }
        for (int k = 0; k < puzzleList.Count; k++)
            PositionSet[k] = null;
        foreach (GameObject all in puzzleList)
            all.GetComponent<Collider>().enabled = true;
        yield return new WaitForSeconds(2.0f);
        playing = true;
    }

    //if the game is over, change the scene to 'main'
    IEnumerator SceneChange()
    {
        yield return new WaitForSeconds(3.7f);
        SceneManager.LoadScene("MainScene");
        //Player.gamestartflag = false;
        //Player.coin += 10;
    }
}
