using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using System.Linq;
using System.Data.SqlClient;

public class PlantEnvironment : MonoBehaviour
{
    private string cs;
    private int plantPercent;
    private int plantName;
    private List<string> weathers = new List<string>();
    private float rain;
    private int polluted;
    private bool plantPolluted;

    [SerializeField]
    private GameObject Plants;

    [SerializeField]
    private GameObject Crops;

    // Start is called before the first frame update
    void Start()
    {
        cs = @"Data Source=DESKTOP-DHIREQV,1433;Initial Catalog=weather;User ID=sa;Password=dankook512@; ";
        FromSql();
        SQL_Plant();
        plantPolluted = false;
    }

    public void FromSql()
    {
        SqlConnection Sqlconn = new SqlConnection(cs);
        Sqlconn.Open();
        SqlCommand cmd = new SqlCommand("USE[weather];SELECT * FROM weather_table", Sqlconn);
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            weathers.Add(reader["weather"].ToString());
            if (reader["weather"].ToString() == "Rain")
                rain += float.Parse(reader["rain"].ToString());
            SqlDB.waterpolluted += int.Parse(reader["waterpolluted"].ToString());
            SqlDB.airpolluted += int.Parse(reader["airpolluted"].ToString());

        }

        if (SqlDB.airpolluted > 90)
            SqlDB.airpolluted = 90;
        if (SqlDB.waterpolluted > 90)
            SqlDB.waterpolluted = 90;

        reader.Close();
        Sqlconn.Close();
    }

    public void SQL_Plant()
    {
        foreach (string weather in weathers)
        {   
            if (weather == "Rain")
            {
                int rainPercent = ((int)Math.Ceiling(rain)) % 10;
                for (int i = 0; i < SqlDB.farmplantList.Count; i++)
                {
                    SqlDB.farmplantList[i].percent += rainPercent;
                }
            }
            else if (weather == "Clear")
            {
                for (int i = 0; i < SqlDB.farmplantList.Count; i++)
                {
                    SqlDB.farmplantList[i].percent += 5;
                }
            }
            else if (weather == "Cloud")
            {
                for (int i = 0; i < SqlDB.farmplantList.Count; i++)
                {
                    SqlDB.farmplantList[i].percent += 2;
                }
            }
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
        if (SqlDB.readingDone && SqlDB.farmplantList != null)
        {
            //check every plant's percent all the time
            for (int i = 0; i < SqlDB.farmplantList.Count; i++)
            {
                plantPercent = SqlDB.farmplantList[i].percent;
                plantName = SqlDB.farmplantList[i].Name;
                growPlant(Player.Plantobject[i]);
            }
        }

        polluted = SqlDB.airpolluted + SqlDB.waterpolluted;

        if (((polluted >= 61) && (polluted <= 109)) || ((polluted >= 121) && (polluted <= 169)))
            plantPolluted = false; 

        if (!plantPolluted)
        { 
            if (((polluted >= 50) && (polluted <= 59)) || ((polluted >= 110) && (polluted <= 120)) || ((polluted >= 170) && (polluted <= 179)))
            {
                for (int i = 0; i < SqlDB.farmplantList.Count; i++)
                {
                    SqlDB.farmplantList[i].polluted += 1;
                    pollutedPhase(Player.Plantobject[i]);
                }
            }
        }

        /*
        if (pollutedMaking.pollutedPercent % 2 == 1 && SqlDB.farmplantList != null)
        {
            //check every plant's percent when the polluted accur
            for (int i = 0; i < SqlDB.farmplantList.Count; i++)
            {
                pollutedPhase(Player.Plantobject[i]);
            }
            pollutedMaking.pollutedPercent += 1;  //2 or 4 
        }
        */
        
    }

    private void growPlant(GameObject plant)
    {
        if (plant != null)
        {
            if (plantPercent >= 30 && plantPercent<60)
            {
                if (int.Parse(plant.name.Substring(plant.name.IndexOf(".") + 1, 1)) == 1)
                {
                    GameObject newplant = Instantiate(Resources.Load("Prefabs/Plantings/Plant_" + plantName + "/Planting_" + plantName + "." + 2), plant.transform.position, Quaternion.identity) as GameObject;
                    Player.Plantobject[Player.Plantobject.IndexOf(plant)] = newplant;
                    Debug.Log("plantEnvironment's new plant in player.plantObject = " + Player.Plantobject[Player.Plantobject.IndexOf(newplant)]);
                    SqlDB.farmplantList[Player.Plantobject.IndexOf(newplant)].size = 2;
                    Destroy(plant);
                    newplant.transform.parent = Plants.transform.GetChild(plantName).transform;
                    newplant.transform.localScale = new Vector3(1, 1, 1);

                    AudioSource plantAudio = newplant.GetComponent<AudioSource>();
                    plantAudio.clip = Resources.Load("Audios/plant1step") as AudioClip;
                    plantAudio.Play();
                }
            }

            else if (plantPercent >= 60 && plantPercent <90)
            {
                if (int.Parse(plant.name.Substring(plant.name.IndexOf(".") + 1, 1)) == 2)
                {
                    GameObject newplant = Instantiate(Resources.Load("Prefabs/Plantings/Plant_" + plantName + "/Planting_" + plantName + "." + 3), plant.transform.position, Quaternion.identity) as GameObject;
                    Player.Plantobject[Player.Plantobject.IndexOf(plant)] = newplant;
                    SqlDB.farmplantList[Player.Plantobject.IndexOf(newplant)].size = 3;
                    Destroy(plant);
                    newplant.transform.parent = Plants.transform.GetChild(plantName).transform;
                    newplant.transform.localScale = new Vector3(1, 1, 1);

                    AudioSource plantAudio = newplant.GetComponent<AudioSource>();
                    plantAudio.clip = Resources.Load("Audios/plant1step") as AudioClip;
                    plantAudio.Play();
                }
            }

            else if(plantPercent>=90)
            {
                SqlDB.farmplantList.Remove(SqlDB.farmplantList[Player.Plantobject.IndexOf(plant)]);
                Player.Plantobject.Remove(plant);
                Destroy(plant);

                GameObject crop = Instantiate(Resources.Load("Prefabs/Crops/Crop_" + plantName), plant.transform.position + new Vector3(0, 0.3f, 0), Quaternion.identity) as GameObject;
                crop.transform.parent = Crops.transform.GetChild(plantName).transform;
                crop.transform.localScale = new Vector3(2, 2, 2);
                ParticleSystem cropps = Instantiate(Resources.Load("Prefabs/ParticleSystems/CropPs"), crop.transform.position, Quaternion.identity) as ParticleSystem;

                AudioSource plantAudio = crop.GetComponent<AudioSource>();
                plantAudio.clip = Resources.Load("Audios/plantallgrown") as AudioClip;
                plantAudio.Play();
            }

        }
    }


    private void pollutedPhase(GameObject plant)
    {
        if(SqlDB.farmplantList[Player.Plantobject.IndexOf(plant)].polluted == 1 )
        {
            int childnum = plant.transform.childCount;
            List<Material> materialList = new List<Material>();
            for (int i=1;i<childnum;i++)
            {
                GameObject childPlant = plant.transform.GetChild(i).gameObject;
                materialList.Add(childPlant.GetComponent<MeshRenderer>().material);
                materialList.Add(Resources.Load<Material>("Materials/rot"));
                childPlant.GetComponent<MeshRenderer>().materials = materialList.ToArray();
                materialList.Clear();
            }
        }
        else if(SqlDB.farmplantList[Player.Plantobject.IndexOf(plant)].polluted >= 2)
        {
            GameObject dirt = Instantiate(Resources.Load("Prefabs/Dirt"), plant.transform.position, Quaternion.identity) as GameObject;
            Destroy(plant);
            SqlDB.farmplantList.RemoveAt(Player.Plantobject.IndexOf(plant));
            Player.Plantobject.Remove(plant);
        }

        plantPolluted = true; 
    }

    private int Split_num(string name)
    {
        string result = name.Substring(name.LastIndexOf("_") + 1);
        result = result.Substring(0, 1);
        return int.Parse(result);
    }

}
