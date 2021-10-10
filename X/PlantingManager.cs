using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
public class PlantingManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Positions;

    [SerializeField]
    private GameObject wateringpot;

    public GameObject ThirdPerson;
    public GameObject Lines;
    public GameObject farmPlane;
    private List<Vector3> positionList;
    private List<GameObject> lineList;
    private GameObject seed;
    private bool plant;
    private GameObject target;
    

    // Start is called before the first frame update
    void Start()
    {        
        lineList = new List<GameObject>();
        for (int k = 0; k < Lines.transform.childCount; k++)
            lineList.Add(Lines.transform.GetChild(k).gameObject);
        foreach (GameObject l in lineList)
            l.SetActive(false);
        positionList = new List<Vector3>();
        for (int k = 0; k < Positions.transform.childCount; k++)
            positionList.Add(Positions.transform.GetChild(k).transform.position);

        seed = null;
        plant = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (StoreManager.havemoney == true)
        {
            ThirdPerson.transform.position = new Vector3(-35f, 0.1f, 0.4f);
            seed = Instantiate(StoreManager.treePrefabs[StoreManager.indexoftree]) as GameObject;
            seed.transform.position = ThirdPerson.transform.position;
            seed.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            StoreManager.havemoney = false;
            StoreManager.click = false;

            //foreach (GameObject l in lineList)
            //    l.SetActive(true);


            if (Input.GetMouseButtonDown(0))
            {
                target = ClickObject3D();
                if (target != null)
                {
                    if (target.name == "wateringpot")
                    {
                        //target 아예 인식 안됨
                        Debug.Log("IN");
                    }
                }
            }
            //if (Input.GetMouseButton(0))
            //{
            //    if(target!=null)
            //    {
                    
            //    }
            //    //if (target.Equals(seed))
            //    //{
            //    //    foreach (GameObject l in lineList)
            //    //        l.SetActive(true);

            //    //    RaycastHit hit;
            //    //    var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //    //    if (Physics.Raycast(Ray.origin, Ray.direction, out hit) && (hit.collider.gameObject == farmPlane || lineList.Contains(hit.collider.gameObject)))
            //    //    {
            //    //        target.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z - 0.1f);
            //    //    }
            //    //    plant = true;
            //    //}
            //}
            //else if (Input.GetMouseButtonUp(0))
            //{
            //    foreach (Vector3 pos in positionList)
            //    {
            //        if (Vector3.Distance(pos, target.transform.position) < 0.9f)
            //        {
            //            target.transform.position = pos;
            //            break;
            //        }
            //    }
            //    foreach (GameObject l in lineList)
            //        l.SetActive(false);
            //    plant = false;
            //    target = null;
            //}
        
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
}
*/