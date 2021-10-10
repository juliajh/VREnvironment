using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
public class FarmWateringManager : MonoBehaviour
{
    private GameObject target;

    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private GameObject Wateringpot;
    [SerializeField]
    private GameObject Crops;
    [SerializeField]
    private GameObject ps;

    private bool watering;
    public GameObject ground;
    private List<GameObject> CropList;
    private GameObject currentPoint;
    private Vector3 potFirstPosition;
    private Vector3 firstPosition;
    private Quaternion firstRotation;
    public RuntimeAnimatorController growingAnim;
    public float maxSize;
    public float growFactor;
    public float waitTime;
    
    public Button Savebutton;
    public Button Stopwateringbutton;
    public GameObject wateringperson;
    

    // Start is called before the first frame update
    void Start()
    {
        CropList = new List<GameObject>();
        Wateringpot.SetActive(false);
        for (int i = 0; i < Crops.transform.childCount; i++)
            CropList.Add(Crops.transform.GetChild(i).gameObject);
        ps.SetActive(false);
        firstPosition = new Vector3(0, 0, 0);
        firstRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        Stopwateringbutton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            target = ClickObject3D();
            if (target != null)
            {
                
                if (target.name == "wateringpot")
                {
                    watering = true;
                    //positionOfScreen = Camera.main.WorldToScreenPoint(target.transform.position);
                    //offsetValue = target.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, positionOfScreen.z));
                }

                else if (target == wateringperson)
                {
                    Vector3 wateringposition = target.transform.position;
                    target.SetActive(false);
                    Player.GetComponent<Rigidbody>().useGravity = false;
                    firstPosition = Player.transform.position;
                    firstRotation = Player.transform.rotation;
                    Player.transform.position = wateringposition + new Vector3(0, 19f, 0);
                    Player.transform.rotation = Quaternion.Euler(new Vector3(30f, 180, 0));
                    Wateringpot.SetActive(true);
                    potFirstPosition = Wateringpot.transform.localPosition;
                    watering = false;
                    Savebutton.gameObject.SetActive(false);
                    Stopwateringbutton.gameObject.SetActive(true);

                }
                
            }
        }

        else if (Input.GetMouseButton(0))
        {
            if (watering)
            {
                RaycastHit hit;
                var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(Ray.origin, Ray.direction, out hit) && hit.collider.gameObject == ground)
                {
                    target.transform.position = hit.point;
                }

                /*foreach (GameObject c in CropList)
                {
                    Vector3 worldtoscreen = Camera.main.WorldToScreenPoint(c.transform.position);
                }
            }

        }

        else if (Input.GetMouseButtonUp(0))
        {
            if (target != null)
            {
                if (watering)
                {
                    /*
                    GameObject minPlant = new GameObject();
                    float minDist = 100f;
                    foreach (GameObject c in CropList)
                    {
                        Vector3 worldtoscreen = Camera.main.ScreenToWorldPoint(c.transform.position);
                        if (Vector2.Distance(target.transform.position,c.transform.position) <= minDist)
                        {
                            minPlant = c;
                            minDist = Vector3.Distance(target.transform.position, c.transform.position);
                        }
                    }
                    currentPoint = minPlant;
                    
                    currentPoint = Pot.selectedplant;
                    Pot.selectedplant = null;

                    if (currentPoint != null)
                    {
                        StartCoroutine(waterTiming());
                    }
                    else
                    {
                        Wateringpot.transform.localPosition = potFirstPosition;
                        target = null;
                        currentPoint = null;
                        watering = false;
                    }
                }
            }

        }
    }

    private GameObject ClickObject3D()
    {
        if (StoreManager.click == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                target = hit.collider.gameObject;
            }

            return target;

        }
        else return null;
    }

    IEnumerator waterTiming()
    {
        //GameObject currentcrop = null;

        ps.SetActive(true);  //작동 X

        /*
        foreach (GameObject c in CropList)
        {
            if (Vector3.Distance(currentPoint.transform.position, c.transform.position) <= 0.7f)
            {
                currentcrop = c;
                break;
            }
        }

        //ThirdPerson.SetActive(false);
        /*
        if (currentcrop != null)
        {
            currentcrop.AddComponent<Animator>();
            currentcrop.GetComponent<Animator>().runtimeAnimatorController = growingAnim;
        }
        yield return new WaitForSeconds(2.0f);
        foreach (GameObject crop in CropList)
        {
            if (Vector3.Distance(currentPoint.transform.position, crop.transform.position) <=0.7f)
            {
                crop.AddComponent<Animator>();
                crop.GetComponent<Animator>().runtimeAnimatorController = growingAnim;
                break;
            }
        }
        //wateringpot.transform.parent = maincamera.GetComponent<Transform>();
        //Wateringpot.transform.localPosition = wateringpotposition;
        //wateringpot.transform.localScale -= new Vector3(0.7f, 0.7f, 0.7f);
        
        

        yield return new WaitForSeconds(3.0f);
        Growtree(currentPoint);
        ps.SetActive(false);
        watering = false;
        Wateringpot.transform.localPosition = potFirstPosition;
        target = null;
        currentPoint = null;

    }

    private void Growtree(GameObject tree)
    {
        if (tree != null)
        {
            float timer = 0;
            if (maxSize <= tree.transform.localScale.x)
            {
                timer += Time.deltaTime;
                Debug.Log(tree.transform.localScale);
                tree.transform.localScale += new Vector3(1f, 1f, 1f) * Time.deltaTime * growFactor;
            }
        }

    }

    public void Stopwateringbuttonclick()
    {
        Player.GetComponent<Rigidbody>().useGravity = true;
        Player.transform.position = firstPosition;
        Player.transform.rotation = firstRotation;
        watering = false;
        Stopwateringbutton.gameObject.SetActive(false);
        wateringperson.SetActive(true);
    }

}

*/