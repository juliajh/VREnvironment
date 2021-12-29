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

    [SerializeField]
    private GameObject rightController;

    [SerializeField]
    private InputField idInput;

    public string cspublic = keys.cspublic;

    private int characterNum = 0;
    private bool succeed;

    // Start is called before the first frame update
    void Start()
    {
        uiPanel.SetActive(false);
        characterPanel.SetActive(true);
        succeed = true;
    }

    void Update()
    {
        /*
        rayHit = rightController.transform.GetChild(0).GetComponent<VRTK.VRTK_StraightPointerRenderer>().getrayHit();
        if (rayHit)
        {
            if (rightController.GetComponent<VRTK.VRTK_ControllerEvents>().touchpadout())
                target = rightController.transform.GetChild(0).GetComponent<VRTK.VRTK_StraightPointerRenderer>().getObj();
            rayHit = false;
        }

        if (target != null)
        {
            if (target.tag == "Keys")
            {
                if (target.transform.parent.name == "Keys")  //keys
                {
                    if (target.transform.name == "Backspace")
                    {
                        if (idInput.text.Length > 0)
                        {
                            idInput.text = idInput.text.Substring(0, idInput.text.Length - 1);
                        }
                    }
                    else if (target.transform.name == "Enter")
                    {
                        accountClick();
                    }
                    else
                    {
                        idInput.text += target.transform.GetChild(0).GetComponent<Text>().text;
                    }
                    target = null;

                }
                else   //characters
                {
                    int num = int.Parse(target.transform.name.Substring(target.transform.name.IndexOf("_") + 1));
                    chooseChar(num);
                    target = null;
                }
            }
        }*/

    }

    public void chooseChar(int num)
    {
        characterNum = num;
        uiPanel.SetActive(true);
        characterPanel.SetActive(false);
    }

    public void accountClick()
    {
        StartCoroutine(AccountEnter());
    }

    IEnumerator AccountEnter()
    {
        SqlDB.userid = id_Text.text;
        if (this.GetComponent<SqlDB>().accounts.Count < 4)
        {

            SqlConnection Sqlconn;
            SqlCommand cmd;
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
                    succeed = true;
                }
                catch (Exception e)
                {
                    succeed = false;
                }
                if (!succeed)
                {
                    infoText.text = "닉네임을 다시 설정해주세요.";
                    SqlDB.userid = "";
                    id_Text.text = "";
                }
                else
                {
                    characterPanel.SetActive(false);
                    infoText.text = "닉네임을 생성했습니다.\n본인의 캐릭터를 찾아가서\n게임을 시작해보세요!";
                    GameObject player = Instantiate(Resources.Load<GameObject>("Prefabs/characters/character_" + characterNum), new Vector3(8.7f, -3.95f, 2), Quaternion.identity) as GameObject;
                    player.transform.parent = GameObject.Find("characters").transform;
                    player.transform.rotation = Quaternion.Euler(new Vector3(0, 180f, 0));
                    player.transform.GetChild(0).GetComponent<TextMesh>().text = id_Text.text;
                    this.GetComponent<SqlDB>().characters.Add(player);
                    this.GetComponent<SqlDB>().accounts.Add(id_Text.text);
                    SqlDB.userid = id_Text.text;

                }
            }
        }
        else
        {
            infoText.text = "계정이 너무 많습니다.";
        }
        uiPanel.SetActive(false);


        yield return null;
    }

}
