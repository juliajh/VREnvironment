using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using UnityEngine.UI;
using UnityEngine;


/*
//Connect with Mssql --> read weather_table at SqlServer and add the weathers at List weathers
public class MssqlConnect : MonoBehaviour  
{
    private string cs;
    public List<string> weathers = new List<string>();
    public float rain;
    //public static bool weatherReadingDone = false;

    private void Start()
    {
        cs = @"Data Source=DESKTOP-DHIREQV,1433;Initial Catalog=weather;User ID=sa;Password=dankook512512; ";
        FromSql();
    }


    public void FromSql()
    {
        SqlConnection Sqlconn = new SqlConnection(cs);
        Sqlconn.Open();
        SqlCommand cmd = new SqlCommand("SELECT * FROM weather_table", Sqlconn);
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            weathers.Add(reader["weather"].ToString());
            if (reader["weather"].ToString() == "Rain")
                rain += float.Parse(reader["rain"].ToString());
        }

        reader.Close();
        Sqlconn.Close();
//weatherReadingDone = true;
    }
}

*/