using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trash_Manager : MonoBehaviour
{

    public static int correctTrashnum=0;
    public Dictionary<GameObject, Vector3> vectorOfTrash = new Dictionary<GameObject, Vector3>();

    [SerializeField]
    private GameObject Sheep;

    [SerializeField]
    private GameObject Player;

    private GameObject ps;

    [SerializeField]
    private GameObject plants;

    [SerializeField]
    private Text talkingText;

    [SerializeField]
    private GameObject talkPanel;

    [SerializeField]
    private GameObject Move;

    private TalkController tc;

    private bool walking=false;
    private bool finish=false;
    private string[] talkingContent;

    private AudioSource alarmaudio;
    private AudioSource sheepaudio;

    [SerializeField]
    private GameObject alarmPanel;

    public List<Vector3> trashposition = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        trashposition.Add(new Vector3(-5.36f, 1, -2.952f));
        trashposition.Add(new Vector3(-16.98f, 1, -7.75f));
        trashposition.Add(new Vector3(-23.93f, 1, -12.1f));
        trashposition.Add(new Vector3(-0.36f, 1, 4.5f));
        trashposition.Add(new Vector3(-23.39f, 1, -1.56f));
        
        trashposition.Add(new Vector3(-29.91f, 1, -20.77f));
        trashposition.Add(new Vector3(5.32f, 1, -19.4f));
        trashposition.Add(new Vector3(10.96f, 1, -12.47f));
        trashposition.Add(new Vector3(-25.11f, 1, 0.029f));
        trashposition.Add(new Vector3(-27f, 1, -5.51f));
        trashposition.Add(new Vector3(-21.25f, 1, -5.81f));
        trashposition.Add(new Vector3(0.6f, 1, -16.8f));
        trashposition.Add(new Vector3(-8.3f, 1, -7.99f));

        alarmaudio = alarmPanel.transform.GetChild(1).GetComponent<AudioSource>();
        sheepaudio = Sheep.transform.GetComponent<AudioSource>();

        sheepaudio.clip = Resources.Load("Audios/sheep-trash-talking") as AudioClip;
        sheepaudio.enabled = false;

        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Trash").Length; i++)
        {
            GameObject trash = GameObject.FindGameObjectsWithTag("Trash")[i];
            Vector3 randomposition = trashposition[Random.Range(0, trashposition.Count)];
            if (!vectorOfTrash.ContainsValue(randomposition))
            {
                trash.transform.localPosition = randomposition;
                vectorOfTrash.Add(trash, trash.transform.localPosition);
            }
            else
            {
                i--;
                
            }
        }

        talkingContent = new string[] { "너는 쓰레기를 분리수거할 줄 아는 아이구나.", "분리수거는 재활용의 첫 걸음이야.",
            "재활용을 하면 우리 자원들을 아낄 수 있어.","그러니 평소에도 꾸준히 분리수거 해줘야 해.", "약속할거지?",
            "약속했으니 내가 선물을 줄게.", "농장으로 가보면 작물들이 자라고 있을거야!" };
        walking = true;
        tc = new TalkController();
        tc.check = false;
        tc.text = talkingText;
        tc.Skip_delay = 1.0f;
        tc.delay = 0.07f;
        Move.SetActive(true);





    }

    // Update is called once per frame
    void Update()
    {
        if (correctTrashnum == 4)
        {
            if (walking)
            {
                Move.SetActive(false);
                Sheep.GetComponent<Animator>().SetTrigger("Walk");
                Sheep.transform.position = Vector3.Lerp(Sheep.transform.position, Player.transform.position, 0.005f);
                Sheep.transform.LookAt(Player.transform);

                if (Vector3.Distance(Sheep.transform.position, Player.transform.position) < 3.2f)
                {
                    Sheep.GetComponent<Animator>().ResetTrigger("Walk");
                    Sheep.GetComponent<Animator>().SetTrigger("Idle");
                    walking = false;

                    tc.text_exit = false;
                    tc.text_full = false;
                }

            }
            if (!walking && !finish)
            {
                talkPanel.SetActive(true);

                Move.SetActive(false);
                sheepaudio.enabled = true;

                if (!tc.check)
                {
                    StartCoroutine(tc.ShowText(talkingContent));
                    tc.check = true;
                }
                if (tc.text_full && tc.check)
                {
                    tc.text_full = false;
                    StartCoroutine(tc.ShowText(talkingContent));
                }

                if (tc.text_exit)
                {
                    talkingText.text = "";
                    talkPanel.SetActive(false);
                    tc.text_exit = false;
                    tc.text_full = false;
                    tc.check = false;
                    tc.flag = false;
                    tc.cnt = 0;
                    Move.SetActive(true);
                    finish = true;
                    walking = true;
                    correctTrashnum++;
                }
            }
        }

        if (finish)
        {
            for(int i=0;i< SqlDB.farmplantList.Count;i++)
            {
                Vector3 pos = new Vector3(SqlDB.farmplantList[i].x, 0, SqlDB.farmplantList[i].z);
                ParticleSystem plantps = Instantiate(Resources.Load("Prefabs/ParticleSystems/PlantGrowingPs"), pos, Quaternion.identity) as ParticleSystem;
            }

            float timer = 0;
            while (timer < 31f)
            {
                timer += Time.deltaTime;
            }

            for(int i=0; i < SqlDB.farmplantList.Count; i++)
                SqlDB.farmplantList[i].percent += 30;

            finish = false;
        }
    }

    IEnumerator retry()
    {
        alarmPanel.SetActive(true);
        Debug.Log("in");
        alarmPanel.GetComponent<Animator>().SetBool("alarm", true);
        alarmPanel.transform.GetChild(1).GetComponent<Text>().text = "다시 시도해봐!";
        alarmaudio.clip = Resources.Load("Audios/retry") as AudioClip;
        alarmaudio.Play();
        yield return new WaitForSeconds(alarmaudio.clip.length);
        alarmPanel.GetComponent<Animator>().SetBool("alarm", false);
    }
}
