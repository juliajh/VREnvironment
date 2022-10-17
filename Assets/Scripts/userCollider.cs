using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userCollider : MonoBehaviour
{
    vrPuzzleGame vrpuzzlegame;

    [SerializeField]
    private GameObject coinpanel;

    // Start is called before the first frame update
    void Start()
    {
        vrpuzzlegame = GameObject.Find("GameManager").GetComponent<vrPuzzleGame>();

    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.parent != null)
        {
            if (collision.gameObject.transform.parent.name == "puzzles")
            {
                collision.gameObject.GetComponent<AudioSource>().Play();
                vrpuzzlegame.puzzleGet(collision.gameObject);
            }

            if (collision.gameObject.transform.tag== "Crop")
            {
                Debug.Log("crop eat");
                collision.gameObject.SetActive(false);
                SqlDB.coin += 30;
                coinpanel.GetComponent<Animation>().Play("Coin");
                coinpanel.GetComponent<AudioSource>().clip = Resources.Load("Audios/success") as AudioClip;
                coinpanel.GetComponent<AudioSource>().Play();

            }
        }
    }

}
