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


public class SavedPlant
{
    public int Name;
    public int Count;

    public SavedPlant(int name, int count)
    {
        Name = name;
        Count = count;
    }
}

public class FarmPlant
{
    public int Name;
    public float x;
    public float z;
    public int size;
    public int percent;
    public int polluted;
    public string master;

    public FarmPlant(int name, float x, float z, int size, int percent, int polluted, string master)
    {
        this.Name = name;
        this.x = x;
        this.z = z;
        this.size = size;  //단계
        this.percent = percent;
        this.polluted = polluted;
        this.master = master;
    }
}

public class SqlDB : MonoBehaviour
{
    [SerializeField]
    private GameObject cameraRig;

    public List<GameObject> characters;
    public List<string> accounts;
    public string cspublic = @"Data Source=DESKTOP-DHIREQV,1433;Initial Catalog=weather;User ID=sa;Password=dankook512@;";
    public static string userid = "";
    private bool succeed=true;

    //player table variables
    public static int coin;
    public static int wateringcanNum;

    //plant table variables
    public static List<SavedPlant> SavedplantList = new List<SavedPlant>();

    //puzzle table variables
    public static List<int> puzzleList = new List<int>();
    public static int pictNum;

    //farmplant table variables
    public static List<FarmPlant> farmplantList = new List<FarmPlant>();

    //weather table in the PlantEnvironment Script//
    public static int waterpolluted = 0;
    public static int airpolluted = 0;

    //for VRMenuController
    public static bool readingDone;

    private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        accounts = new List<string>();
        characters = new List<GameObject>();
        StartCoroutine(getAccounts());
    }

    void Update()
    {
        if (characters.Count > 0)
        {
            for (int k = 0; k < characters.Count; k++)
            {
                if (Vector3.Distance(cameraRig.transform.position, characters[k].transform.position) < 3.7f)
                {
                    StartCoroutine(Login(accounts[k]));
                }
            }
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

                player = Instantiate(Resources.Load<GameObject>("Prefabs/characters/character_" + num), new Vector3(4 * i, 0, 11), Quaternion.identity) as GameObject;
                player.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                player.transform.GetChild(0).GetComponent<TextMesh>().text = accounts[i];
                characters.Add(player);
            }
        }
        yield return null;
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
                Debug.Log("로그인에 실패했습니다.");
                succeed = false;
            }

            //yield return new WaitForSeconds(3f);

            if (succeed)
            {
                //StartCoroutine(Main());
                yield return StartCoroutine(DBParsing("sql_player"));
                yield return StartCoroutine(DBParsing("sql_savedplant"));
                yield return StartCoroutine(DBParsing("sql_puzzle"));
                yield return StartCoroutine(DBParsing("sql_farmplant"));
                readingDone = true;
                SceneManager.LoadScene("MainHouse");
            }
        }
        yield return null;
    }


    IEnumerator DBParsing(string p)
    {
        using (SqlConnection Sqlconn = new SqlConnection())
        {
            Sqlconn.ConnectionString = cspublic;
            try
            {
                Sqlconn.Open();

                if (p == "sql_savedplant")
                {
                    SavedplantList.Clear();
                    using (SqlCommand cmd = new SqlCommand("USE[weather_" + userid + "];SELECT * FROM savedplant_table", Sqlconn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string a = reader["plantName"].ToString();
                            string b = reader["num"].ToString();
                            SavedplantList.Add(new SavedPlant(int.Parse(a), int.Parse(b)));
                        }

                        reader.Close();
                    }
                }

                else if (p == "sql_player")
                {

                    using (SqlCommand cmd = new SqlCommand("USE[weather_" + userid + "];SELECT * FROM player_table", Sqlconn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            coin = int.Parse(reader["Coin"].ToString());
                            wateringcanNum = int.Parse(reader["WateringCanNum"].ToString());
                        }

                        reader.Close();
                    }

                }
                else if (p == "sql_puzzle")
                {
                    puzzleList.Clear();
                    using (SqlCommand cmd = new SqlCommand("USE[weather_" + userid + "];SELECT * FROM puzzle_table", Sqlconn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            int a = int.Parse(reader["name"].ToString());
                            int b = int.Parse(reader["num"].ToString());
                            //read 에 관한 내용 
                            if (a != 16)
                            {
                                puzzleList.Add(b);
                            }
                            else
                                pictNum = b;
                        }

                        reader.Close();
                    }

                }
                else if (p == "sql_farmplant")
                {
                    farmplantList.Clear();
                    using (SqlCommand cmd = new SqlCommand("USE[weather]; SELECT * FROM farmplant_table", Sqlconn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            farmplantList.Add(new FarmPlant(int.Parse(reader["Name"].ToString()), float.Parse(reader["x"].ToString()), float.Parse(reader["z"].ToString()),
                                int.Parse(reader["size"].ToString()), int.Parse(reader["per"].ToString()), int.Parse(reader["polluted"].ToString()), reader["master"].ToString()));
                        }

                        reader.Close();
                    }

                }
                Sqlconn.Close();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        yield return new WaitForSeconds(1.5f);
    }

}


