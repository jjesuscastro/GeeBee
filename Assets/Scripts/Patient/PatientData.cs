using System.Collections;
using UnityEngine;
using Firebase.Database;

#pragma warning disable 612, 618

[System.Serializable]
public class PatientData
{
    public string patientID;
    public string firstName;
    public string lastName;
    public string gender;
    public string handedness;
    public string birthDate;
    public string dateCreated;

    public string GetFullName()
    {
        return firstName + " " + lastName;
    }

    public string Stringify()
    {
        return JsonUtility.ToJson(this);
    }

    public static PatientData Parse(string json)
    {
        return JsonUtility.FromJson<PatientData>(json);
    }

    public IEnumerator DownloadAllPatients(System.Action<string> callback = null)
    {
        var patients = FirebaseDatabase.DefaultInstance.GetReference("patients").GetValueAsync();
        yield return new WaitUntil(predicate: () => patients.IsCompleted);

        if (patients != null)
        {
            DataSnapshot snapshot = patients.Result;
            string result = snapshot.GetRawJsonValue();
            result = Database.FormatFirebaseData(result);
            callback.Invoke(result);
        }
    }

    public IEnumerator UploadData(System.Action<bool> callback = null)
    {
        System.DateTime dateNow = System.DateTime.Now;
        dateCreated = dateNow.ToString();
        patientID = firstName[0] + lastName +
                    dateNow.Day.ToString("00") + dateNow.Month.ToString("00") + dateNow.Year +
                    dateNow.Hour.ToString("00") + dateNow.Minute.ToString("00") +
                    dateNow.Second.ToString("00") + dateNow.Millisecond.ToString("00");

        string json = JsonUtility.ToJson(this);
        Database.dbReference.Child("patients").Child(patientID).SetRawJsonValueAsync(json);

        yield return new WaitForSeconds(2f);
    }

    public IEnumerator Delete(System.Action<bool> callback = null)
    {
        Database.dbReference.Child("patients").Child(patientID).RemoveValueAsync();

        yield return new WaitForSeconds(1f);
        callback.Invoke(true);
    }
}
