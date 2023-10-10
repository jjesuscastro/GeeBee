using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UI.Dates;

public class NewPatient : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField firstName;
    [SerializeField]
    private TMP_InputField lastName;
    [SerializeField]
    private DatePicker birthdate;
    [SerializeField]
    private Dropdown gender;
    [SerializeField]
    private Dropdown handedness;
    [SerializeField]
    private SavePatientTween savePatient;

    private bool validationFailed;

    public void CheckValues()
    {
        if (string.IsNullOrEmpty(firstName.text) || string.IsNullOrEmpty(lastName.text) || gender.value == 0 || handedness.value == 0)
            validationFailed = true;

        System.DateTime dateToday = System.DateTime.Today.Date;
        System.DateTime birthDate = System.DateTime.Today.Date;
        try
        {
            birthDate = System.DateTime.Parse(birthdate.Ref_InputField.text).Date;
        }
        catch
        {
            validationFailed = true;
        }

        if (System.DateTime.Equals(dateToday, birthDate))
            validationFailed = true;

        if (!validationFailed)
            SavePatient(birthDate);
        else
            savePatient.Tween();
    }

    private void SavePatient(System.DateTime birthDate)
    {
        PatientData newPatient = new PatientData();
        newPatient.firstName = firstName.text;
        newPatient.lastName = lastName.text;
        newPatient.birthDate = birthDate.ToString();
        newPatient.gender = gender.options[gender.value].text;
        newPatient.handedness = handedness.options[handedness.value].text;

        StartCoroutine(newPatient.UploadData(result =>
        {
            if (result)
            {
                Debug.Log(result);
            }
        }));
    }
}
