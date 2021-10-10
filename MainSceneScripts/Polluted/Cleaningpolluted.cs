using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleaningpolluted : MonoBehaviour
{
    [SerializeField]
    private GameObject rightController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool triggerpress = rightController.GetComponent<VRTK.VRTK_ControllerEvents>().triggerpress();
        bool cleanpolluted = rightController.transform.GetChild(1).GetComponent<VRTK.VRTK_BezierPointerRenderer>().getcleanpolluted();

        if (triggerpress && cleanpolluted)
        {
            GameObject p = rightController.transform.GetChild(1).GetComponent<VRTK.VRTK_BezierPointerRenderer>().gettarget();
            if (p.transform.gameObject != null)
            {
                if (p.transform.parent.name == "waterPolluted")
                    p.SetActive(false);
            }
        }
    }
}
