using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Remarks : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputField;

    Patient currentPatient;
    Popup popup;
    Record record;

    private void Start()
    {
        currentPatient = Patient.instance;
        popup = Popup.instance;
        record = Record.instance;

        string intro = "Assistant, do you want to add remarks regarding the child\'s monitoring tests?";

        popup.NoButton().RemoveAllListeners();
        popup.NoButton().AddListener(SaveRecord);
        popup.YesButton().RemoveAllListeners();
        popup.OpenPopup(intro, PopupType.YESNO);
    }

    public void SaveRecord()
    {
        if (currentPatient == null || record == null)
            return;

        record.remarks = inputField.text;
        record.patientID = currentPatient.GetPatientID();

        StartCoroutine(UploadRecord());
    }

    IEnumerator UploadRecord()
    {
        yield return StartCoroutine(record.UploadRecord());

        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
