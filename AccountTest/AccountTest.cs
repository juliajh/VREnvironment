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

    [SerializeField]
    private GameObject uiPanel;

    [SerializeField]
    private GameObject characterPanel;

    [SerializeField]
    private Text id_Text;

    public string cspublic = @"Data Source=DESKTOP-DHIREQV,1433;Initial Catalog=weather;User ID=sa;Password=dankook512@;";
    public static string userid = "";
    private bool succeed;
    private List<GameObject> characters;
    private List<string> accounts;
    private GameObject player = null;
    private int characterNum;

    // Start is called before the first frame update
    void Start()
    {
        accounts = new List<string>();
        characters = new List<GameObject>();
        StartCoroutine(getAccounts());
        succeed = true;
        characterPanel.SetActive(false);
        characterNum = 0;
    }

    void Update()
    {
        for (int k = 0; k < characters.Count; k++)
        {
            if (Vector3.Distance(cameraRig.transform.position, characters[k].transform.position) < 3.2f)
            {
                StartCoroutine(Login(accounts[k]));
            }
        }
    }

    public void accountClick()
    {
        if (accounts.Contains(id_Text.text))
        {
            informText.text = "해당 닉네임이 이미 존재합니다.\n다른 닉네임을 사용하세요.";
            id_Text.text = "";
        }
        else if (!succeed)
        {
            informText.text = "닉네임이 적절하지 않습니다.\n 공백이 포함됐는지 확인해보세요.";
            id_Text.text = "";
        }
        else
        {
            userid = id_Text.text;
            uiPanel.SetActive(false);
            characterPanel.SetActive(true);
        }

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
                    if (reader["name"].ToString() != string.Empty)
                    {
                        string name = reader["name"].ToString();
                        accounts.Add(name.Substring(name.IndexOf("_") + 1));
                    }
                }
                    
                reader.Close();
            }
            Sqlconn.Close();
        }
        if (accounts != null)
        {
            for (int i = 0; i < accounts.Count; i++)
            {
                int num = 0;
                using (SqlConnection Sqlconn = new SqlConnection())
                {
                    Sqlconn.ConnectionString = cspublic;
                    Sqlconn.Open();

                    using (SqlCommand cmd = new SqlCommand("USE[weather_" + accounts[i] + "];SELECT character FROM player_table", Sqlconn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            num = int.Parse(reader["character"].ToString());
                        }

                        reader.Close();
                        Sqlconn.Close();


                    }
                }
                Debug.Log("accounts name [" + accounts.IndexOf(name) + "] : " + accounts);
                player = Instantiate(Resources.Load<GameObject>("Prefabs/characters/character_" + num), new Vector3(4 * i, 0, 20), Quaternion.identity) as GameObject;
                player.transform.GetChild(0).GetComponent<TextMesh>().text = accounts[i];
                characters.Add(player);
            }
        }
        yield return null;
    }
    IEnumerator Account()
    {
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

                    cmd = new SqlCommand("CREATE DATABASE weather_" + userid +  ";", Sqlconn);
                    cmd.ExecuteNonQuery();

                    //make tables for user
                    cmd = new SqlCommand("USE weather_" + userid + ";" +
                        "CREATE TABLE savedplant_table(plantName smallint, num smallint); " +
                        "CREATE TABLE player_table(character smallint not null, Coin smallint not null, WateringCanNum smallint not null);" +
                        " CREATE TABLE puzzle_table(name smallint not null, num smallint not null);", Sqlconn);
                    cmd.ExecuteNonQuery();

                    //set default
                    cmd = new SqlCommand("INSERT INTO savedplant_table VALUES(1, 0), (2, 0), (3, 0), (4, 0), (5, 0), (6, 0)", Sqlconn);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("INSERT INTO player_table VALUES("+characterNum+",100,0)", Sqlconn);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("INSERT INTO puzzle_table VALUES(0,0),(1,0),(2,0),(3,0),(4,0),(5,0),(6,0),(7,0),(8,0),(9,0),(10,0),(11,0)" +
                        ",(12,0),(13,0),(14,0),(15,0),(16,0)", Sqlconn);
                    cmd.ExecuteNonQuery();

                    Sqlconn.Close();

                    informText.text = "닉네임을 생성했습니다.";
                    player = Instantiate(Resources.Load<GameObject>("Prefabs/characters/character_" + characterNum), characterPanel.transform.position, Quaternion.identity) as GameObject;
                    characterPanel.SetActive(false);
                    player.transform.GetChild(0).GetComponent<TextMesh>().text = userid;
                    characters.Add(player);
                    accounts.Add(userid);
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                    succeed = false;
                }
            }
            yield return new WaitForSeconds(2.0f);
        }
    }

    IEnumerator Login(string playerName)
    {

        userid = playerName;

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
                succeed = false;
            }

            yield return new WaitForSeconds(3f);

            if (succeed)
                SceneManager.LoadScene("MainHouse");
        }
    }

    public void chooseChar(int num)
    {
        characterNum = num;
        StartCoroutine(Account());
    }
}
