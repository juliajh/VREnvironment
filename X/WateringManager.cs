using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/* 필요없음
public class WateringManager : MonoBehaviour
{
    //public GameObject potParent;
    public GameObject trees; //나무들의 parent
    public GameObject wateringpot;
    private GameObject target;
    private bool watering;    //주전자를 잡은 상태인지 아닌지
    private List<GameObject> treeList; //모든 나무들이 담겨있는 리스트 
    private List<GameObject> pointList; //나무들의 포인트들이 담겨있는 리스트
    private Vector3 offsetValue;
    private Vector3 positionOfScreen;
    private Vector3 wateringpotposition;
    private GameObject currentPoint;
    public GameObject ground;
    private Vector3 potFirstPosition;
    public Animation anim;
    public RuntimeAnimatorController growingAnim; //나무 자라는 animation
    public float maxSize;
    public float growFactor;
    public float waitTime;

    [SerializeField]
    ParticleSystem ps;
    [SerializeField]
    GameObject maincamera;
    [SerializeField]
    GameObject ThirdPerson;
    [SerializeField]
    GameObject TreeCamera;

    // Start is called before the first frame update
    void Start()
    {

        anim = wateringpot.GetComponent<Animation>();
        currentPoint = null;
        target = null;
        treeList = new List<GameObject>();
        pointList = new List<GameObject>();
        watering = false;
        for (int i = 0; i < trees.transform.childCount; i++)
            treeList.Add(trees.transform.GetChild(i).gameObject);
        foreach (GameObject tree in treeList)
        {
            tree.AddComponent<Animator>();
            tree.GetComponent<Animator>().runtimeAnimatorController = growingAnim;
        }
        for (int k = 0; k < treeList.Count; k++)
        {
            GameObject point = Resources.Load("Prefabs/" + "Treepoints") as GameObject;
            pointList.Add(Instantiate(point, treeList[k].transform.position, Quaternion.identity) as GameObject);
            pointList[k].transform.SetParent(treeList[k].transform);
        }
        foreach (GameObject p in pointList)
            p.SetActive(false);
        wateringpotposition = wateringpot.transform.localPosition;
        potFirstPosition = wateringpot.transform.position;
        TreeCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!StoreManager.click)
            {
                target = ClickObject3D();
                if (target != null)
                {
                    if (target.name == "wateringpot")
                    {
                        watering = true;
                        wateringpot.transform.localScale += new Vector3(1.0f, 1.0f, 1.0f);
                        //positionOfScreen = Camera.main.WorldToScreenPoint(target.transform.position);
                        //offsetValue = target.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, positionOfScreen.z));
                    }
                    else
                    {
                        watering = false;
                    }
                }
            }
        }

        else if (Input.GetMouseButton(0))
        {
            if (watering)
            {
                target.transform.parent = transform.root;

                foreach (GameObject p in pointList)
                    p.SetActive(true);

                RaycastHit hit;
                var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(Ray.origin, Ray.direction, out hit) && hit.collider.gameObject == ground)
                {
                    target.transform.position = hit.point;
                }

                foreach (GameObject p in pointList)
                {
                    Vector3 worldtoscreen = Camera.main.WorldToScreenPoint(p.transform.position);
                    if (Vector3.Distance(Input.mousePosition, new Vector3(worldtoscreen.x, worldtoscreen.y, 0)) <= 20f)
                    {
                        p.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 1);
                    }
                    else
                    {
                        p.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 1);
                    }
                }
            }

        }

        else if (Input.GetMouseButtonUp(0))
        {
            if (target != null)
            {
                if (watering)
                {
                    foreach (GameObject p in pointList)
                    {
                        Vector3 worldtoscreen = Camera.main.WorldToScreenPoint(p.transform.position);
                        if (Vector3.Distance(Input.mousePosition, new Vector3(worldtoscreen.x, worldtoscreen.y, 0)) <= 20f)
                        {
                            currentPoint = p;
                        }
                        p.SetActive(false);
                    }
                    if (currentPoint != null)
                    {
                        //wateringpot.transform.localScale -= new Vector3(1f, 1f, 1f);
                        StartCoroutine(waterTiming());
                    }
                    else
                    {
                        //wateringpot.transform.localScale -= new Vector3(1f, 1f, 1f);
                        foreach (GameObject pp in pointList)
                            pp.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 1);
                        wateringpot.transform.position = potFirstPosition;
                        target = null;
                        wateringpot.transform.parent = maincamera.GetComponent<Transform>();
                        wateringpot.transform.localPosition = wateringpotposition;
                        wateringpot.transform.localScale -= new Vector3(1.0f, 1.0f, 1.0f);
                        currentPoint = null;

                        watering = false;


                    }


                    //GameObject Instance = Instantiate(Resources.Load("Prefabs/" + "wateringpotins")) as GameObject;
                    //Instance.transform.position = p.transform.position;
                    //Instance.GetComponent<Animation>().Play();
                }
                /*wateringpot.transform.parent = maincamera.GetComponent<Transform>();
                wateringpot.transform.localPosition = wateringpotposition;
                wateringpot.transform.localScale -= new Vector3(0.7f, 0.7f, 0.7f);
                currentPoint = null;

                watering = false; //--> WaterTiming 안으로 넣음

            }
            
            wateringpot.transform.parent = maincamera.transform;
            wateringpot.transform.localPosition = wateringpotposition;
            currentPoint = null;

            watering = false;
        }
             
    }
    private GameObject ClickObject3D()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            target = hit.collider.gameObject;
        }

        return target;
    }

    IEnumerator waterTiming()
    {
        GameObject currenttree = null;
        
        ps.Play();
        //anim.Play("Watering");   //작동 X

        foreach (GameObject tree in treeList)
        {
            if (Vector3.Distance(currentPoint.transform.position, tree.transform.position) <= 0.7f)
            {
                currenttree = tree;
                break;
            }
        }

        ThirdPerson.SetActive(false);
        TreeCamera.SetActive(true);
        TreeCamera.transform.localPosition = currenttree.transform.position+new Vector3(0f,4f,-10f);
        wateringpot.transform.localPosition = currenttree.transform.position + new Vector3(1f, 1f, -1f);

        Debug.Log(wateringpot.transform.position);

        if (currenttree != null)
        { 
            currenttree.AddComponent<Animator>();
            currenttree.GetComponent<Animator>().runtimeAnimatorController = growingAnim;
        }
        yield return new WaitForSeconds(2.0f);
        foreach(GameObject tree in treeList)
        {
            if (Vector3.Distance(currentPoint.transform.position, tree.transform.position) <= 0.7f)
            {
                tree.AddComponent<Animator>();
                tree.GetComponent<Animator>().runtimeAnimatorController = growingAnim;
                break;
            }
        }
        wateringpot.transform.parent = maincamera.GetComponent<Transform>();
        wateringpot.transform.localPosition = wateringpotposition;
        wateringpot.transform.localScale -= new Vector3(0.7f, 0.7f, 0.7f);
        currentPoint = null;

        yield return new WaitForSeconds(3.0f);
        watering = false;
        TreeCamera.SetActive(false);
        ThirdPerson.SetActive(true);

    }

    private void Growtree(GameObject tree)
    {
        float timer = 0;
        if (maxSize <= tree.transform.localScale.x)
        {
            timer += Time.deltaTime;
            tree.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * growFactor;
        }

    }
}
*/


