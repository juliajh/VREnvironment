using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//keyboard moveing
public class MovePlayer : MonoBehaviour
{
    float moveHorizontal;
    float moveVectical;

    float movingSpeed=20f;
    float rotationSpeed=20f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVectical = Input.GetAxis("Vertical");

        transform.Translate(moveVectical * Time.deltaTime * movingSpeed * Vector3.forward);
        transform.Rotate(Vector3.up, moveHorizontal * Time.deltaTime * rotationSpeed);
    }
}
