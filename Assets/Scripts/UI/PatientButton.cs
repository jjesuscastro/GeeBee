using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PatientButton : MonoBehaviour
{
    [SerializeField]
    private TMP_Text gender;
    [SerializeField]
    private TMP_Text patientName;
    [SerializeField]
    private TMP_Text birthday;

    public void SetPatientData(string gender, string patientName, string birthday)
    {
        this.gender.text = gender;
        this.patientName.text = patientName;
        this.birthday.text = birthday;
    }

    public string GetName()
    {
        return patientName.text;
    }

    public string GetBirthdate()
    {
        return birthday.text;
    }

    public string GetGender()
    {
        return gender.text;
    }
}
