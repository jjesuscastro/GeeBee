using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using UnityEngine;

public class Patient : MonoBehaviour
{
    PatientData patientData;

    #region Singleton
    public static Patient instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("[Patient.cs] - Multiple Patient(s) found!");
            Destroy(gameObject);
        }
    }
    #endregion 

    public void SetPatientData(PatientData patientData)
    {
        this.patientData = patientData;
    }

    public string GetName()
    {
        return patientData.GetFullName();
    }

    public string GetPatientID()
    {
        return patientData.patientID;
    }

    public PatientData GetPatientData()
    {
        return patientData;
    }

    ///<summary>
    ///Returns true if patient is female
    ///</summary>
    public bool GetGender()
    {
        if (patientData == null || String.IsNullOrEmpty(patientData.patientID))
            CreateDefaultData();

        return patientData.gender.Equals("Female") ? true : false;
    }

    public int GetAge()
    {
        if (patientData == null || String.IsNullOrEmpty(patientData.patientID))
            CreateDefaultData();

        DateTime now = DateTime.Now;
        DateTime birthDate = DateTime.Now;
        try
        {
            birthDate = DateTime.ParseExact(patientData.birthDate, "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
        }
        catch
        {
            Debug.LogError("Cannot parse date of birth");
        }

        int a = (now.Year * 100 + now.Month) * 100 + now.Day;
        int b = (birthDate.Year * 100 + birthDate.Month) * 100 + birthDate.Day;

        return (a - b) / 10000;
    }

    void CreateDefaultData()
    {
        patientData = new PatientData();
        patientData.firstName = "Default";
        patientData.lastName = "Patient";
        patientData.gender = "Male";
        patientData.handedness = "Left-handed";
        patientData.birthDate = System.DateTime.Now.AddYears(-13).Date.ToString();
        System.DateTime dateNow = System.DateTime.Now;
        patientData.dateCreated = dateNow.ToString();
        patientData.patientID = patientData.firstName[0] + patientData.lastName +
                    dateNow.Day.ToString("00") + dateNow.Month.ToString("00") + dateNow.Year +
                    dateNow.Hour.ToString("00") + dateNow.Minute.ToString("00") +
                    dateNow.Second.ToString("00") + dateNow.Millisecond.ToString("00");
    }
}
