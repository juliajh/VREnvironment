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


//불러오는 작업
public class SqlSave : MonoBehaviour
{
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

    //for VRMenuController
    public static bool readingDone;

    private string cspublic = @"Data Source=DESKTOP-DHIREQV,1433;Initial Catalog=weather;User ID=sa;Password=dankook512@;";

    [SerializeField]
    GameObject CoinPanel;

    [SerializeField]
    GameObject wateringcanText;

    void Start()
    {
        try
        {
            readingDone = false;
            StartCoroutine(Main());
        }

        catch(Exception e)
        {
            Debug.Log(e.Message);
        }


    }

    IEnumerator Main()
    {
        yield return StartCoroutine(DBParsing("sql_player"));
        /*CoinPanel.transform.GetChild(0).GetComponent<Text>().text = coin.ToString();
        wateringcanText.GetComponent<Text>().text = "X " + wateringcanNum.ToString();*/

        yield return StartCoroutine(DBParsing("sql_savedplant"));
        /*for (int i = 0; i < BagPlantTextList.Count; i++)
            BagPlantTextList[i].GetComponent<Text>().text = "X " + SavedplantList[i].Count;*/
        yield return StartCoroutine(DBParsing("sql_puzzle"));
        yield return StartCoroutine(DBParsing("sql_farmplant"));
        readingDone = true;
    }

    IEnumerator DBParsing(string p)
    {
        if (p == "sql_savedplant")
        {
            using (SqlConnection Sqlconn = new SqlConnection())
            {
                Sqlconn.ConnectionString = cspublic;
                Sqlconn.Open();
                SavedplantList.Clear();

                using (SqlCommand cmd = new SqlCommand("USE[weather_"+AccountTest.userid+"];SELECT * FROM savedplant_table", Sqlconn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string a = reader["plantName"].ToString();
                        string b = reader["num"].ToString();
                        SavedplantList.Add(new SavedPlant(int.Parse(a), int.Parse(b)));
                    }

                    reader.Close();
                    Sqlconn.Close();

                    
                }
            }
        }
        else if (p == "sql_player")
        {
            using (SqlConnection Sqlconn = new SqlConnection())
            {
                Sqlconn.ConnectionString = cspublic;
                Sqlconn.Open();

                using (SqlCommand cmd = new SqlCommand("USE[weather_" + AccountTest.userid + "];SELECT * FROM player_table", Sqlconn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        coin = int.Parse(reader["Coin"].ToString());
                        wateringcanNum = int.Parse(reader["WateringCanNum"].ToString());
                    }

                    reader.Close();
                    Sqlconn.Close();
                }
            }
        }
        else if (p == "sql_puzzle")
        {
            using (SqlConnection Sqlconn = new SqlConnection())
            {
                Sqlconn.ConnectionString = cspublic;
                Sqlconn.Open();
                puzzleList.Clear();
                using (SqlCommand cmd = new SqlCommand("USE[weather_" + AccountTest.userid + "];SELECT * FROM puzzle_table", Sqlconn))
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
                    Sqlconn.Close();
                }
            }
        }
        else if (p == "sql_farmplant")
        {
            farmplantList.Clear();
            using (SqlConnection Sqlconn = new SqlConnection())
            {
                Sqlconn.ConnectionString = cspublic;
                Sqlconn.Open();

                using (SqlCommand cmd = new SqlCommand("USE[weather]; SELECT * FROM farmplant_table", Sqlconn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        farmplantList.Add(new FarmPlant(int.Parse(reader["Name"].ToString()), float.Parse(reader["x"].ToString()), float.Parse(reader["z"].ToString()), 
                            int.Parse(reader["size"].ToString()), int.Parse(reader["per"].ToString()), int.Parse(reader["polluted"].ToString()),reader["master"].ToString()));
                    }

                    reader.Close();
                    Sqlconn.Close();
                }
            }
            foreach (FarmPlant farm in farmplantList)
                Debug.Log(farm.Name);
        }
        yield return null;
    }

}

