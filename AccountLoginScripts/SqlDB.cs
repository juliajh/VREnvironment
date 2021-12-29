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
    public string cspublic = keys.cspublic;
    public static string userid = "";
    private bool succeed = true;

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
    public static bool firstUser = false;

    private GameObject player;
    private List<Vector3> characterPosition = new List<Vector3>();
    private List<Vector3> characterRotation = new List<Vector3>();

    private bool charChoose;
    private int selectedCharNum;

    // Start is called before the first frame update
    void Start()
    {
        characterPosition = new List<Vector3>();
        characterRotation = new List<Vector3>();
        accounts = new List<string>();
        characters = new List<GameObject>();

        characterPosition.Add(new Vector3(13.7f, -3.95f, -3.11f));
        characterPosition.Add(new Vector3(8f, -3.95f, -11.1f));
        characterPosition.Add(new Vector3(21.65f, -3.95f, 1.87f));
        characterPosition.Add(new Vector3(-7.74f, -3.95f, -11.54f));
        characterPosition.Add(new Vector3(2.17f, -3.95f, -9.22f));

        characterRotation.Add(new Vector3(0, -90f, 0));
        characterRotation.Add(new Vector3(0, 0, 0));
        characterRotation.Add(new Vector3(0, -90f, 0));
        characterRotation.Add(new Vector3(0, 90f, 0));
        characterRotation.Add(new Vector3(0, 49.21f, 0));

        StartCoroutine(getAccounts());

        charChoose = false;
        selectedCharNum = -1;
    }

    void Update()
    {
        if (characters.Count > 0)
        {
            if (!charChoose && selectedCharNum < 0)
            {
                for (int k = 0; k < characters.Count; k++)
                {
                    if (Vector3.Distance(cameraRig.transform.position, characters[k].transform.position) < 2.5f)
                    {
                        selectedCharNum = k;
                        charChoose = true;
                    }
                }
            }

            else if (charChoose && selectedCharNum > 0)
            {
                for (int k = 0; k < characters.Count; k++)
                {
                    if (k != selectedCharNum)
                    {
                        Destroy(characters[k]);
                    }
                    else
                    {
                        if (userid == accounts[k])
                        {
                            this.GetComponent<Opening>().enabled = true;
                            firstUser = true;
                        }
                        else
                        {
                            this.GetComponent<Opening>().enabled = false;
                            firstUser = false;
                        }
                        selectedCharNum = -1;
                        StartCoroutine(Login(accounts[k]));
                    }
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
                player = null;
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
                player = Instantiate(Resources.Load<GameObject>("Prefabs/characters/character_" + num), characterPosition[i], Quaternion.identity) as GameObject;
                player.transform.rotation = Quaternion.Euler(characterRotation[i]);
                player.transform.GetChild(0).GetComponent<TextMesh>().text = accounts[i];
                characters.Add(player);
                player.transform.parent = GameObject.Find("characters").transform;
            }
        }
        yield return null;
    }

    IEnumerator Login(string playerName)
    {
        userid = playerName;
        GameObject charPS = Instantiate(Resources.Load<GameObject>("Prefabs/ParticleSystems/characterPS"), characters[accounts.IndexOf(userid)].transform.position, Quaternion.identity) as GameObject;
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
            }
        }
        yield return new WaitForSeconds(2f);
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
        yield return null;
    }

}


