using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField]
    GameObject whiteImage;

    [SerializeField]
    GameObject redImage;

    public float force;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RectTransform rt = whiteImage.GetComponent<RectTransform>();
        Debug.Log(rt.localPosition);

        /*방법1
        Vector3 distance = new Vector3(rt.localPosition.x, rt.localPosition.y, -30f) - new Vector3(rt.localPosition.x, rt.localPosition.y, 0f);
        distance = distance.normalized;
        distance = distance * force;
        rb.AddForce(distance);
        */

        rt.localPosition = Vector3.Lerp(rt.localPosition, new Vector3(rt.localPosition.x, rt.localPosition.y, 0f), Time.deltaTime * 5f);
        rt.localScale = Vector3.Lerp(rt.localScale, new Vector3(2, 2, 2), Time.deltaTime * 5f);
    }
}
