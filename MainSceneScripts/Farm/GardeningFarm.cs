using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardeningFarm : MonoBehaviour
{
    Rigidbody rigid;
    private GameObject selectedplant;

    [SerializeField]
    private GameObject wateringcan;

    private void Start()
    {
    }

    void OnParticleCollision(GameObject collision)
    {
        if (collision.transform.parent != null)
        {
            if (collision.transform.parent.gameObject.tag == "Plant")
            {
                selectedplant = collision.transform.parent.gameObject;
                StartCoroutine(Timer());
            }
        }

    }

    IEnumerator Timer()
    {
        
        yield return new WaitForSeconds(3.0f);
        wateringcan.SetActive(false);
        SqlDB.wateringcanNum--;
        SqlDB.farmplantList[Player.Plantobject.IndexOf(selectedplant)].percent += 10;



    }

}


