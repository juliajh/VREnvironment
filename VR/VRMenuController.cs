using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System.Data.SqlClient;
using System;

public class VRMenuController : MonoBehaviour
{
    public GameObject leftController;
    public GameObject Move;
    int coincount;

    [SerializeField]
    private GameObject menupanel;

    [SerializeField]
    GameObject PlantCanvas;

    [SerializeField]
    GameObject puzzleCanvas;

    [SerializeField]
    GameObject CoinPanel;

    [SerializeField]
    GameObject wateringpot;

    [SerializeField]
    GameObject wateringcanText;

    public List<GameObject> BagPlantTextList;


    //public static bool wateringcanon;

    private string cspublic = "";

    // Start is called before the first frame update
    void Start()
    { 
        PlantCanvas.SetActive(false);
        CoinPanel.SetActive(false);
        puzzleCanvas.SetActive(false);
        wateringpot.SetActive(false);
        menupanel.SetActive(false);

        cspublic = @"Data Source=DESKTOP-DHIREQV,1433;Initial Catalog=weather;User ID=sa;Password=dankook512@; ";
    }

    // Update is called once per frame
    void Update()
    {

        if (SqlDB.readingDone)
        {
            CoinPanel.transform.GetChild(0).GetComponent<Text>().text = SqlDB.coin.ToString();
            wateringcanText.GetComponent<Text>().text = "X " + SqlDB.wateringcanNum.ToString();

            if (SqlDB.SavedplantList.Count == 6)
            {
                for (int i = 0; i < BagPlantTextList.Count; i++)
                {
                    BagPlantTextList[i].GetComponent<Text>().text = "X " + SqlDB.SavedplantList[i].Count.ToString();
                }
            }
        }

        bool menubuttonflag = leftController.GetComponent<VRTK.VRTK_ControllerEvents>().menuPressed;
        int count = leftController.GetComponent<VRTK.VRTK_ControllerEvents>().count;

        if (menubuttonflag)
        {
            if (count % 2 == 1)
            {
                menupanel.SetActive(true);
                Move.SetActive(false);
               // GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().LtouchpadLight();
            }
            else
            {
                Move.SetActive(true);
                menupanel.SetActive(false);
               // GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().Off(1);
                PlantCanvas.SetActive(false);
                puzzleCanvas.SetActive(false);
                CoinPanel.SetActive(false);
            }
        }
    }

    public void bagpanelon()
    {
        if (PlantCanvas.activeSelf)
        {
            PlantCanvas.SetActive(false);
            if (SceneManager.GetActiveScene().name == "MainScene")
                GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().offRtouchpad();
        }
        else  //키는 과정
        {
            if (!wateringpot.activeSelf)  //wateringpot off 일때만 키도록.
            {
                PlantCanvas.SetActive(true);
                puzzleCanvas.SetActive(false);
                CoinPanel.SetActive(false);
                
                if(SceneManager.GetActiveScene().name=="MainScene")
                    GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().RtouchpadLight();
            }
        }
        

    }

    public void coinbutton()
    {
        if (CoinPanel.activeSelf)  //켜져있다면
        {
            CoinPanel.SetActive(false);
        }
        else //꺼져있다면
        {
            if (!wateringpot.activeSelf)
            {
                CoinPanel.SetActive(true);
                puzzleCanvas.SetActive(false);
                PlantCanvas.SetActive(false);
               // GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().Off(5);
            }
        }
    }

    public void puzzleCanvasOn()
    {
        if (puzzleCanvas.activeSelf)
            puzzleCanvas.SetActive(false);
        else 
        {
            if(!wateringpot.activeSelf)
            {
                puzzleCanvas.SetActive(true);
                PlantCanvas.SetActive(false);
             //   GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().Off(5);
                CoinPanel.SetActive(false);
            }
        }
    }

    public void canbuttonclick()
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            if (!wateringpot.activeSelf)
            {
                if (SqlDB.wateringcanNum > 0)
                {
                    wateringpot.gameObject.SetActive(true);
                    PlantCanvas.SetActive(false);
                    CoinPanel.SetActive(false);
                    puzzleCanvas.SetActive(false);
                 //   GameObject.Find("VR_Manager").GetComponent<ControllerGuide>().Off(5);

                }
            }
            else
            {
                wateringpot.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Not MainScene");
        }
    }

    public void QuitBtn()
    {
        //StartCoroutine(SaveDb("time.sqlite"));
        //PlayerPrefs.SetString("SaveLastTime", System.DateTime.Now.ToString());
        //Debug.Log("종료 시간: " + System.DateTime.Now);

        StartCoroutine(SaveDb("sql_player"));
        StartCoroutine(SaveDb("sql_savedplant"));
        StartCoroutine(SaveDb("sql_puzzle"));
        StartCoroutine(SaveDb("sql_farmplant"));  //don't change order

        //delete weather_table elements 
        SqlConnection Sqlconn = new SqlConnection(cspublic);
        Sqlconn.Open();
        SqlCommand cmd = new SqlCommand("TRUNCATE TABLE weather_table", Sqlconn);
        cmd.ExecuteNonQuery();
        Sqlconn.Close();

        onDestroy();
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    
    private void onDestroy()
    {
        PlayerPrefs.SetString("SaveLastTime", System.DateTime.Now.ToString());
        //Debug.Log("종료 시간 : " + System.DateTime.Now.ToString());
    }
    

    IEnumerator SaveDb(string p)
    {
        if (p == "sql_savedplant")
        {
            SqlConnection Sqlconn = new SqlConnection(cspublic);
            foreach (SavedPlant plant in SqlDB.SavedplantList)
            {
                Sqlconn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE savedplant_table SET num=" + "'" + plant.Count + "'" +
            " " + "WHERE plantName=" + "'" + plant.Name + "'", Sqlconn);
                cmd.ExecuteNonQuery();
                Sqlconn.Close();
            }
        }
        else if (p == "sql_player")
        {
            SqlConnection Sqlconn = new SqlConnection(cspublic);
            Sqlconn.Open();
            SqlCommand cmd = new SqlCommand("UPDATE player_table SET Coin=" + "'" + SqlDB.coin + "'" + ",WateringCanNum=" + "'" + SqlDB.wateringcanNum + "'", Sqlconn);
            cmd.ExecuteNonQuery();
            Sqlconn.Close();
        }
        else if (p == "sql_puzzle")
        {
            SqlConnection Sqlconn = new SqlConnection(cspublic);
            for (int i = 0; i < SqlDB.puzzleList.Count; i++)
            {
                Sqlconn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE puzzle_table SET num=" + "'" + SqlDB.puzzleList[i] + "'" +
            " " + "WHERE name=" + "'" + i + "'", Sqlconn);
                cmd.ExecuteNonQuery();
                Sqlconn.Close();
            }
        }
        else if (p == "sql_farmplant")
        {
            SqlConnection Sqlconn = new SqlConnection(cspublic);

            foreach (FarmPlant fplant in SqlDB.farmplantList)
            {
                try
                {
                    Sqlconn.Open();
                    SqlCommand cmd;

                    Debug.Log("fplant.Name: "+fplant.Name+"  fplant.master: "+fplant.master);


                    if (SqlDB.farmplantList.IndexOf(fplant) == 0)
                    {
                        cmd = new SqlCommand("TRUNCATE table farmplant_table; INSERT INTO farmplant_table(Name, x, z, size,per,polluted,master) VALUES(" + fplant.Name + "," + fplant.x + "," + fplant.z + ","
                            + fplant.size + "," + fplant.percent + "," + fplant.polluted + ",'" + fplant.master + "');", Sqlconn);
                    }
                    
                    else
                    {
                        cmd = new SqlCommand("INSERT INTO farmplant_table(Name, x, z, size,per, polluted, master) VALUES(" + fplant.Name + "," + fplant.x + "," + fplant.z + "," + fplant.size + "," + fplant.percent + "," + fplant.polluted + "," + fplant.master + ");", Sqlconn);
                    }

                    cmd.ExecuteNonQuery();
                    Sqlconn.Close();
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }

            }
            
            yield return new WaitForSeconds(0.2f);
        }

        yield return null;
    }

}
