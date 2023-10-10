using System.Collections;
using UnityEngine;

#pragma warning disable 612, 618

[System.Serializable]
public class RecordData
{
    public string recordID;
    public string patientID;
    public int height;
    public int weight;
    public float bmi;
    public string visualAcuityLeft;
    public string visualAcuityRight;
    public string colorVision;
    public string hearingLeft;
    public string hearingRight;
    public int grossMotor;
    public string grossMotorRemark;
    public int fineMotorDominant;
    public int fineMotorNDominant;
    public int fineMotorHold;
    public string remarks;
    public string dateCreated;

    public string Stringify()
    {
        return JsonUtility.ToJson(this);
    }

    public IEnumerator UploadData(System.Action<bool> callback = null)
    {
        System.DateTime dateNow = System.DateTime.Now;
        dateCreated = dateNow.ToString();
        recordID = patientID + "_" +
                    dateNow.Day.ToString("00") + dateNow.Month.ToString("00") + dateNow.Year +
                    dateNow.Hour.ToString("00") + dateNow.Minute.ToString("00") +
                    dateNow.Second.ToString("00") + dateNow.Millisecond.ToString("00");

        string json = JsonUtility.ToJson(this);
        Database.dbReference.Child("records").Child(recordID).SetRawJsonValueAsync(json);

        yield return new WaitForSeconds(2f);
    }
}
