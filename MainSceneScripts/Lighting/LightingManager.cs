using System;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    //Scene References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    [SerializeField] private GameObject fireflies;
    //Variables
    [SerializeField, Range(0, 24)] private float TimeOfDay;

    int hour;
    int min;

    private void Start()
    {
        fireflies.SetActive(false);
    }

    private void Update()
    {
        if (Preset == null)
            return;

        hour = int.Parse(DateTime.Now.ToString("HH"));
        min = int.Parse(DateTime.Now.ToString("mm"));

        if (Application.isPlaying)
        {
            
            TimeOfDay = hour + (float)min / 60;
            TimeOfDay %= 24; //Modulus to ensure always between 0-24
            
            UpdateLighting(TimeOfDay / 24f);

        }
        
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }

        if (TimeOfDay >= 21)
        {
            fireflies.SetActive(true);
        }

        else
            fireflies.SetActive(false);
    }


    private void UpdateLighting(float timePercent)
    {
        //Set ambient and fog
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        //If the directional light is set then rotate and set it's color, I actually rarely use the rotation because it casts tall shadows unless you clamp the value
        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);

            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0f));
        }

    }

    //Try to find a directional light to use if we haven't set one
    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        //Search for lighting tab sun
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        //Search scene for light that fits criteria (directional)
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }
}