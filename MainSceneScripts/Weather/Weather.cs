using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Weather : MonoBehaviour
{
    //public List<string> weathers = new List<string>();
    public GameObject RainObj;
    public GameObject SnowObj;
    public Light light;
    //public string APP_ID;
    public string weather;
    public WeatherData weatherInfo;


    // Start is called before the first frame update
    void Start()
    {
        CheckCityWeather("Seoul");
        RainObj.SetActive(false);
        SnowObj.SetActive(false);
    }

    public void CheckCityWeather(string city)
    {
        StartCoroutine(GetWeather(city));
    }

    IEnumerator GetWeather(string city)
    {
        city = UnityWebRequest.EscapeURL(city);
        string url = keys.weatherkey;

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        string json = www.downloadHandler.text;
        json = json.Replace("\"base\":", "\"basem\":");
        weatherInfo = JsonUtility.FromJson<WeatherData>(json);

        if (weatherInfo.weather.Length > 0)
        {
            weather = weatherInfo.weather[0].main;
        }

        if (weather == "Rain")
            RainObj.SetActive(true);
        else if (weather == "Snow")
        {
            SnowObj.SetActive(true);
        }
        else if (weather == "Clouds")
            light.intensity = 0.5f;
        else if (weather == "Clear")
            light.intensity = 1.2f;

    }
}
