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

public class AccountTest : MonoBehaviour
{
    [SerializeField]
    private Text informText;

    [SerializeField]
    private GameObject cameraRig;

    public Text id_Text;
    public Text pw_Text;

    public string cspublic = @"Data Source=DESKTOP-DHIREQV,1433;Initial Catalog=weather;User ID=sa;Password=dankook512@;";
    public static string userid = "";
    private bool succeedLogin;
    private bool succeedaccount;
    private List<GameObject> characters;
    private List<string> accounts;

    // Start is called before the first frame update
    void Start()
    {
        accounts = new List<string>();
        characters = new List<GameObject>();
        StartCoroutine(getAccounts());
        informText.text = "";
        succeedLogin = true;
    }

    void Update()
    {
        for (int k = 0; k < characters.Count; k++)
        {
            /*
            if (Vector3.Distance(cameraRig.transform.position, characters[k].transform.position) < 3.9f)
            {
                characters[k].transform.GetChild(0).GetComponent<TextMesh>().fontSize = 70;
                characters[k].transform.GetChild(0).GetComponent<TextMesh>().text = accounts[k] + "에 로그인하시겠습니까?";
                if(Vector3.Distance(cameraRig.transform.position, characters[k].transform.position) < 3.0f)
                {
                    StartCoroutine(Login());
                }
            }
            else if (Vector3.Distance(cameraRig.transform.position, characters[k].transform.position) > 5.0f)
            {
                
            }*/
            if (Vector3.Distance(cameraRig.transform.position, characters[k].transform.position) < 3.2f)
            {
                StartCoroutine(Login(accounts[k]));
            }
        }
    }

    public void accountClick()
    {
        StartCoroutine(Account());
    }

    public void loginClick()
    {
        StartCoroutine(Login(id_Text.text));

    }

    IEnumerator getAccounts()
    {
        using (SqlConnection Sqlconn = new SqlConnection())
        {
            Sqlconn.ConnectionString = cspublic;
            Sqlconn.Open();
            accounts.Clear();

            using (SqlCommand cmd = new SqlCommand("SELECT name FROM sys.databases WHERE (database_id>=6);", Sqlconn))
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string name = reader["name"].ToString();
                    accounts.Add(name.Substring(name.IndexOf("_")+1));
                }
                reader.Close();
            }
            Sqlconn.Close();
        }
        GameObject player = null;
        for (int i = 0; i < accounts.Count; i++)
        {
            Debug.Log("name : " + accounts[i]);
            player = Instantiate(Resources.Load<GameObject>("Prefabs/characters/character_" + i), new Vector3(4 * i, 0, 20), Quaternion.identity) as GameObject;
            player.transform.GetChild(0).GetComponent<TextMesh>().text = accounts[i];
            characters.Add(player);
        }
        yield return null;
    }
    
    
    IEnumerator Login(string playerName)
    {

        userid = playerName;
        if (userid=="")
        {
            userid = id_Text.text;

        }

        using (SqlConnection Sqlconn = new SqlConnection())
        {
            Sqlconn.ConnectionString = cspublic;

            try
            {
                Sqlconn.Open();
                Sqlconn.Close();
            }
            catch (Exception e)
            {
                informText.text = "로그인에 실패했습니다.";
                succeedLogin = false;
            }

            yield return new WaitForSeconds(3f);

            if (succeedLogin)
                SceneManager.LoadScene("MainHouse");
        }

    }
    

    IEnumerator Account()
    {
        SqlConnection Sqlconn;
        SqlCommand cmd;
        using (Sqlconn = new SqlConnection())
        {
            Sqlconn.ConnectionString = cspublic;
            Sqlconn.Open();

            
            if (accounts.Contains(id_Text.text))
            {
                informText.text = "해당 아이디가 이미 존재합니다.\n다른 아이디를 사용하세요.";
            }
            else
            {
                using (Sqlconn = new SqlConnection())
                {
                    Sqlconn.ConnectionString = cspublic;

                    try
                    {
                        Sqlconn.Open();

                        cmd = new SqlCommand("CREATE DATABASE weather_" + id_Text.text + ";", Sqlconn);
                        cmd.ExecuteNonQuery();

                        //make tables for user
                        cmd = new SqlCommand("USE weather_" + id_Text.text +";"+
                            "CREATE TABLE savedplant_table(plantName smallint, num smallint); " +
                            "CREATE TABLE player_table(Coin smallint not null, WateringCanNum smallint not null);" +
                            " CREATE TABLE puzzle_table(name smallint not null, num smallint not null);", Sqlconn);
                        cmd.ExecuteNonQuery();

                        //set default
                        cmd = new SqlCommand("INSERT INTO savedplant_table VALUES(1, 0), (2, 0), (3, 0), (4, 0), (5, 0), (6, 0)", Sqlconn);
                        cmd.ExecuteNonQuery();
                        cmd = new SqlCommand("INSERT INTO player_table VALUES(100,0) ", Sqlconn);
                        cmd.ExecuteNonQuery();
                        cmd = new SqlCommand("INSERT INTO puzzle_table VALUES(0,0),(1,0),(2,0),(3,0),(4,0),(5,0),(6,0),(7,0),(8,0),(9,0),(10,0),(11,0)" +
                            ",(12,0),(13,0),(14,0),(15,0),(16,0)", Sqlconn);
                        cmd.ExecuteNonQuery();

                        Sqlconn.Close();

                        informText.text = "계정을 생성했습니다.";
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message);
                    }

                }
                yield return null;
                //StartCoroutine(Login());
            }
                
        }

    }

    public void chooseChar()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject.name);
    }
}
