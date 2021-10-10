using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watering : MonoBehaviour
{

    [SerializeField]
    private GameObject wateringpot;     

    [SerializeField]
    private GameObject wateringps;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = wateringpot.GetComponent<AudioSource>();
        audioSource.clip= Resources.Load("Audios/wateringpot") as AudioClip;
        //audioSource.Stop();
        audioSource.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (wateringpot.transform.rotation.eulerAngles.z > 70f && wateringpot.transform.rotation.eulerAngles.z > 0.0f)
        {
            wateringps.SetActive(true);
            audioSource.enabled = true;
        }
        else
        {
            wateringps.SetActive(false);
            audioSource.enabled = false;
        }
    }
}
