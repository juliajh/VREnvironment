using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlantingOnMain : MonoBehaviour
{
    [SerializeField]
    private GameObject leftController;

    [SerializeField]
    private GameObject rightController;

    [SerializeField]
    private GameObject Store;

   // [SerializeField]
    //private GameObject player;

    [SerializeField]
    private GameObject Plants;

    //public static List<GameObject> plantList; //새로운 plant들 저장 

    [SerializeField]
    private GameObject plantMenu;

    private static int plantNum;
    private bool planting;
    private bool firstenter;  //해당 plant를 클릭했는지
    //private bool PlantonFarm; //Farm에 제대로 놓여졌는지
    private GameObject plant;  //selected plant
    private Vector3 clickPoint;
    private GameObject crop;
    private GameObject target;

    private int selectedPlantNum;
    private bool rayHit;
    private bool selected;


    // Start is called before the first frame update
    void Start()
    {
        //plantList = new List<GameObject>();
        planting = false;

        //firstenter = true;
        //PlantonFarm = true;
        selectedPlantNum = 0;
        selected = false;

    }

    // Update is called once per frame
    void Update()
    {
        rayHit = rightController.transform.GetChild(0).GetComponent<VRTK.VRTK_StraightPointerRenderer>().getrayHit();
        if (rayHit)
        {
            if (rightController.GetComponent<VRTK.VRTK_ControllerEvents>().touchpadout())
                target = rightController.transform.GetChild(0).GetComponent<VRTK.VRTK_StraightPointerRenderer>().getObj();
            rayHit = false;
        }


        if (target != null)
        {
            if (target.tag == "PlantMenu")
            {
                if (selected == false)
                {
                    GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().offRtouchpad();
                    selectedPlantNum = int.Parse(target.name.Substring(target.name.LastIndexOf("_") + 1));

                    if (SqlDB.SavedplantList[selectedPlantNum].Count > 0)
                    {

                        GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().RTriggerButtonLight();
                        string name = "Prefabs/Crops/Crop_" + selectedPlantNum;
                        crop = Instantiate(Resources.Load<GameObject>(name), rightController.transform.position + new Vector3(0, 0.15f, 0), Quaternion.identity) as GameObject;
                        crop.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        crop.transform.parent = rightController.transform;

                        selected = true;
                        planting = true;
                        target = null;

                        plantMenu.transform.GetChild(selectedPlantNum + 1).GetChild(0).GetComponent<Text>().text = "X " + (SqlDB.SavedplantList[selectedPlantNum].Count - 1).ToString();
                        SqlDB.SavedplantList[selectedPlantNum].Count--;
                        plantMenu.SetActive(false);

                    }


                }

            }
        }

        if (planting)
        {

            bool triggerout = rightController.GetComponent<VRTK.VRTK_ControllerEvents>().triggerout();
            bool infarm = rightController.transform.GetChild(1).GetComponent<VRTK.VRTK_BezierPointerRenderer>().getinfarm();

            if (triggerout && infarm)
            {
                Destroy(crop);
                GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().offRtriggerButton();
                string name = "Prefabs/Plantings/Plant_"+selectedPlantNum+"/Planting_" + selectedPlantNum + ".1";
                Vector3 plantposition = rightController.transform.GetChild(1).GetComponent<VRTK.VRTK_BezierPointerRenderer>().getposition();

                plant = Instantiate(Resources.Load<GameObject>(name), plantposition, Quaternion.identity) as GameObject;
                plant.transform.parent = Plants.transform.GetChild(selectedPlantNum).transform;
                plant.transform.position = plantposition;
                plant.transform.localScale = new Vector3(1, 1, 1);

                Planting();

                selected = false;
            }
        }
    }

    private void Planting()
    {
        plant.SetActive(true);
        //plant 가 의미하는게 새로운 gameobject
        SqlDB.farmplantList.Add(new FarmPlant(int.Parse(plant.name.Substring(plant.name.IndexOf("_")+1,1)), plant.transform.position.x, plant.transform.position.z, 1,0,0, SqlDB.userid));
        Player.Plantobject.Add(plant);

        plant = null;
        planting = false;

    }



}
