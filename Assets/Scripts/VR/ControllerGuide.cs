using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGuide : MonoBehaviour
{
    public GameObject leftControllerModel;
    public GameObject rightControllerModel;

    private GameObject LtriggerButton;
    private GameObject Ltouchpad;
    private GameObject Lgrip;
    private GameObject LmenuButton;
    private GameObject RtriggerButton;
    private GameObject Rtouchpad;
    private GameObject Rgrip;
    private GameObject RmenuButton;

    //private List<GameObject> controllers = new List<GameObject>();
    //private bool Lflag = false;
    //private bool Rflag = false;

    //Light On Buttons
    public void LTriggerButtonLight()
    {
        while (LtriggerButton == null)
            SettingLtriggerbutton();

        if (LtriggerButton != null)
        {
            if (LtriggerButton.GetComponent<Outline>() == null )
            {
                LtriggerButton.AddComponent<Outline>();
                LtriggerButton.AddComponent<Outline>();
            }
        }
    }
    public void LtouchpadLight()
    {
        while (Ltouchpad == null)
            SettingLtouchpad();


        if (Ltouchpad != null)
        {
            if (Ltouchpad.GetComponent<Outline>() == null)
                Ltouchpad.AddComponent<Outline>();
        }
    }
    public void LgripLight()
    {
        while (Lgrip == null)
            SettingLgrip();

        if (Lgrip != null)
        {
            if (Lgrip.GetComponent<Outline>() == null)
                Lgrip.AddComponent<Outline>();
        }
    }
    public void LmenuButtonLight()
    {
        while (LmenuButton == null)
            SettingLmenubutton();

        if (LmenuButton != null)
        {
            if (LmenuButton.GetComponent<Outline>() == null)
                LmenuButton.AddComponent<Outline>();
        }
    }
    public void RTriggerButtonLight()
    {

        while (RtriggerButton == null)
            SettingRtriggerbutton();

        if (RtriggerButton != null)
        {
            if (RtriggerButton.GetComponent<Outline>() == null )
                RtriggerButton.AddComponent<Outline>();
        }
    }
    public void RtouchpadLight()
    {
        while (Rtouchpad == null)
            SettingRtouchpad();


        if (Rtouchpad != null)
        {
            if (Rtouchpad.GetComponent<Outline>() == null)
                Rtouchpad.AddComponent<Outline>();
        }
    }
    public void RgripLight()
    {
        while (Rgrip == null)
            SettingRgrip();

        if (Rgrip != null)
        {
            if (Rgrip.GetComponent<Outline>() == null)
                Rgrip.AddComponent<Outline>();
        }
    }

    public void RmenuButtonLight()
    {
        while (RmenuButton == null)
            SettingRmenuButton();

        if (RmenuButton != null)
        {
            if (RmenuButton.GetComponent<Outline>() == null)
                RmenuButton.AddComponent<Outline>();
        }
    }


    //Light Off Buttons
    public void offLmenuButton()
    {
        if(LmenuButton.GetComponent<Outline>()!=null)
        {
            Destroy(LmenuButton.GetComponent<Outline>());
        }
    }

    public void offLtriggerButton()
    {
        if (LtriggerButton.GetComponent<Outline>() != null)
        {
            Destroy(LmenuButton.GetComponent<Outline>());
        }
    }

    public void offLtouchpad()
    {
        if (Ltouchpad.GetComponent<Outline>() != null)
        {
            Destroy(Ltouchpad.GetComponent<Outline>());
        }
    }

    public void offLgrip()
    {
        if (Lgrip.GetComponent<Outline>() != null)
        {
            Destroy(Lgrip.GetComponent<Outline>());
        }
    }

    public void offRtouchpad()
    {
        if (Rtouchpad.GetComponent<Outline>() != null)
        {
            Destroy(Rtouchpad.GetComponent<Outline>());
        }
    }
    public void offRtriggerButton()
    {
        if (RtriggerButton.GetComponent<Outline>() != null)
        {
            Destroy(RtriggerButton.GetComponent<Outline>());
        }
    }

    public void offRgrip()
    {
        if (Rgrip.GetComponent<Outline>() != null)
        {
            Destroy(Rgrip.GetComponent<Outline>());
        }
    }

    public void offRmenuButton()
    {
        if (RmenuButton.GetComponent<Outline>() != null)
        {
            Destroy(RmenuButton.GetComponent<Outline>());
        }
    }



    //Find button from models - Settings
    private void SettingLmenubutton()
    {
        if (LmenuButton == null)
        {
            if (leftControllerModel.transform.childCount > 0 && leftControllerModel.transform.parent.gameObject.activeSelf)
            {
                LmenuButton = leftControllerModel.transform.GetChild(2).gameObject;
            }
        }
    }

    private void SettingLtriggerbutton()
    {
        if (LtriggerButton == null)
        {
            if (leftControllerModel.transform.childCount > 0 && leftControllerModel.transform.parent.gameObject.activeSelf)
            {
                LtriggerButton = leftControllerModel.transform.GetChild(17).gameObject;
            }
        }
    }

    private void SettingLtouchpad()
    {
        if (Ltouchpad == null)
        {
            if (leftControllerModel.transform.childCount > 0 && leftControllerModel.transform.parent.gameObject.activeSelf)
            {
                Ltouchpad = leftControllerModel.transform.GetChild(14).gameObject;
            }
        }
    }

    private void SettingLgrip()
    {
        if (Lgrip == null)
        {
            if (leftControllerModel.transform.childCount > 0 && leftControllerModel.transform.parent.gameObject.activeSelf)
            {
                Lgrip = leftControllerModel.transform.GetChild(9).gameObject;
            }
        }
    }


    private void SettingRtriggerbutton()
    {
        if (RtriggerButton == null)
        {
            if (rightControllerModel.transform.childCount > 0 && rightControllerModel.transform.parent.gameObject.activeSelf)
            {
                RtriggerButton = rightControllerModel.transform.GetChild(17).gameObject;
            }
        }
    }

    private void SettingRtouchpad()
    {
        if (Rtouchpad == null)
        {
            if (rightControllerModel.transform.childCount > 0 && rightControllerModel.transform.parent.gameObject.activeSelf)
            {
                Rtouchpad = rightControllerModel.transform.GetChild(16).gameObject;
            }
        }
    }

    private void SettingRgrip()
    {
        if (Rgrip == null)
        {
            if (rightControllerModel.transform.childCount > 0 && rightControllerModel.transform.parent.gameObject.activeSelf)
            {
                Rgrip = rightControllerModel.transform.GetChild(8).gameObject;
            }
        }
    }

    private void SettingRmenuButton()
    {
        if (RmenuButton == null)
        {
            if (rightControllerModel.transform.childCount > 0 && rightControllerModel.transform.parent.gameObject.activeSelf)
            {
                RmenuButton = rightControllerModel.transform.GetChild(2).gameObject;
            }
        }
    }
}
