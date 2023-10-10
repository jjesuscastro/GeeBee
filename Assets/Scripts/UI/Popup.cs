using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Popup : MonoBehaviour
{
    [SerializeField]
    private GameObject popup;
    [SerializeField]
    private TMP_Text message;
    [SerializeField]
    private Button okay;
    [SerializeField]
    private Button yes;
    [SerializeField]
    private Button no;

    #region Singleton
    public static Popup instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("[Popup.cs] - Multiple Popup(s) found!");
            Destroy(gameObject);
        }
    }
    #endregion 

    public void OpenPopup(string messageStr, PopupType type)
    {
        if (type == PopupType.CONFIRMATION)
        {
            okay.gameObject.SetActive(true);
            yes.gameObject.SetActive(false);
            no.gameObject.SetActive(false);
        }
        else
        {
            okay.gameObject.SetActive(false);
            yes.gameObject.SetActive(true);
            no.gameObject.SetActive(true);
        }

        message.text = messageStr;
        popup.SetActive(true);
    }

    public Button.ButtonClickedEvent YesButton()
    {
        return yes.onClick;
    }

    public Button.ButtonClickedEvent NoButton()
    {
        return no.onClick;
    }

    public Button.ButtonClickedEvent OkayButton()
    {
        return okay.onClick;
    }
}

public enum PopupType
{
    CONFIRMATION, YESNO
}