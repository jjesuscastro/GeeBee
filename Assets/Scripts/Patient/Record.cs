using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Record : MonoBehaviour
{
    public string patientID { get { return recordData.patientID; } set { recordData.patientID = value; } }
    public int height { get { return recordData.height; } set { recordData.height = value; } }
    public int weight { get { return recordData.weight; } set { recordData.weight = value; } }
    public float bmi { get { return recordData.bmi; } set { recordData.bmi = value; } }
    public string visualAcuityRight { get { return recordData.visualAcuityRight; } set { recordData.visualAcuityRight = value; } }
    public string visualAcuityLeft { get { return recordData.visualAcuityLeft; } set { recordData.visualAcuityLeft = value; } }
    public string colorVision { get { return recordData.colorVision; } set { recordData.colorVision = value; } }
    public string hearingRight { get { return recordData.hearingRight; } set { recordData.hearingRight = value; } }
    public string hearingLeft { get { return recordData.hearingLeft; } set { recordData.hearingLeft = value; } }

    //0 = passed, 1 = failed, 2 = N/A
    public int grossMotor { get { return recordData.grossMotor; } set { recordData.grossMotor = value; } }
    public string grossMotorRemark { get { return recordData.grossMotorRemark; } set { recordData.grossMotorRemark = value; } }

    //0 = passed, 1 = failed
    public int fineMotorDominant { get { return recordData.fineMotorDominant; } set { recordData.fineMotorDominant = value; } }
    public int fineMotorNDominant { get { return recordData.fineMotorNDominant; } set { recordData.fineMotorNDominant = value; } }
    public int fineMotorHold { get { return recordData.fineMotorHold; } set { recordData.fineMotorHold = value; } }

    public string remarks { get { return recordData.remarks; } set { recordData.remarks = value; } }

    RecordData recordData;

    #region Singleton
    public static Record instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            recordData = new RecordData();
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("[Record.cs] - Multiple Record(s) found!");
            Destroy(gameObject);
        }
    }
    #endregion 

    public IEnumerator UploadRecord()
    {
        yield return StartCoroutine(recordData.UploadData(result =>
        {
            if (result)
            {
                Debug.Log(result);
            }
        }));
    }
}
