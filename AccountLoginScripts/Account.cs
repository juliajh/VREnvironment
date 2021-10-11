using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.SqliteClient;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.SqliteClient;
using System.IO;
using System.Data;
using UnityEngine.UI;
using System.Data.SqlClient;
using System;
using System.Configuration;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Account : MonoBehaviour
{
    [SerializeField]
    private Text id_Text;

    [SerializeField]
    private GameObject uiPanel;

    [SerializeField]
    private Text infoText;

    [SerializeField]
    private GameObject characterPanel;

    public string cspublic = @"Data Source=DESKTOP-DHIREQV,1433;Initial Catalog=weather;User ID=sa;Password=dankook512@;";

    private int characterNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        uiPanel.SetActive(false);
        characterPanel.SetActive(true);
    }

    public void chooseChar(int num)
    {
        characterNum = num;
        uiPanel.SetActive(true);
        characterPanel.SetActive(false);
        //StartCoroutine(Account());
    }

    public void accountClick()
    {
        StartCoroutine(AccountEnter());
    }

    IEnumerator AccountEnter()
    {
        SqlDB.userid = id_Text.text;

        SqlConnection Sqlconn;
        SqlCommand cmd;
        using (Sqlconn = new SqlConnection())
        {
            Sqlconn.ConnectionString = cspublic;
            Sqlconn.Open();

            using (Sqlconn = new SqlConnection())
            {
                Sqlconn.ConnectionString = cspublic;

                try
                {
                    Sqlconn.Open();

                    cmd = new SqlCommand("CREATE DATABASE weather_" + SqlDB.userid + ";", Sqlconn);
                    cmd.ExecuteNonQuery();

                    //make tables for user
                    cmd = new SqlCommand("USE weather_" + SqlDB.userid + ";" +
                        "CREATE TABLE savedplant_table(plantName smallint, num smallint); " +
                        "CREATE TABLE player_table(character smallint not null, Coin smallint not null, WateringCanNum smallint not null);" +
                        " CREATE TABLE puzzle_table(name smallint not null, num smallint not null);", Sqlconn);
                    cmd.ExecuteNonQuery();

                    //set default
                    cmd = new SqlCommand("INSERT INTO savedplant_table VALUES(1, 0), (2, 0), (3, 0), (4, 0), (5, 0), (6, 0)", Sqlconn);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("INSERT INTO player_table VALUES(" + characterNum + ",100,0)", Sqlconn);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("INSERT INTO puzzle_table VALUES(0,0),(1,0),(2,0),(3,0),(4,0),(5,0),(6,0),(7,0),(8,0),(9,0),(10,0),(11,0)" +
                        ",(12,0),(13,0),(14,0),(15,0),(16,0)", Sqlconn);
                    cmd.ExecuteNonQuery();

                    Sqlconn.Close();

                    infoText.text = "닉네임을 생성했습니다.";

                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                    //succeed = false;
                }
            }
            
        }
        yield return new WaitForSeconds(2.0f);
    }

}
