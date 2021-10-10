using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeinOUt : MonoBehaviour
{
    [SerializeField]
    private GameObject fadeinoutImg;

    private bool checkbool;
    private Color color;
    void Start()
    {
        checkbool = false;
        color = fadeinoutImg.GetComponent<Image>().color;   
        color.a = Time.deltaTime * 100.0f; 
        fadeinoutImg.GetComponent<Image>().color = color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator fadein()
    {
        fadeinoutImg.SetActive(true);
        for (int i = 55; i >= 0; i--)
        {
            color.a += Time.deltaTime * 0.028f; 
            fadeinoutImg.GetComponent<Image>().color = color;

        }
        if (color.a >= 3.9f)
            checkbool = true;
        yield return null;
    }

    IEnumerator fadeout() 
    {
        color = fadeinoutImg.GetComponent<Image>().color;

        for (int i = 100; i >= 0; i--)
        {
            color.a -= Time.deltaTime * 0.01f;
            fadeinoutImg.GetComponent<Image>().color = color;
            Debug.Log("color.a (in Main) : " + color.a);
        }
        if (color.a <= 0.7f)
        {
            Debug.Log("2.0f");
            GameManager_mini0.mini0_scenechange = false;
            fadeinoutImg.SetActive(false); 
        }
        yield return null;
    }
}



