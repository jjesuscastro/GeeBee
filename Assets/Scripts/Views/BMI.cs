using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMI : MonitoringTest
{
    private int height;
    private int weight;
    private Record record;

    private void Start()
    {
        record = Record.instance;
    }

    public void UpdateHeight(string value)
    {
        height = value.Equals("") ? 0 : int.Parse(value);
    }

    public void UpdateWeight(string value)
    {
        weight = value.Equals("") ? 0 : int.Parse(value);
    }

    public void Submit()
    {
        Patient patient = Patient.instance;
        if (patient == null)
            return;

        int age = patient.GetAge();
        bool isGirl = patient.GetGender();
        float bmi = BMICalculator.computeBMIMetric(height, weight);
        int bmiResult = BMICalculator.getBMIResult(false, age, bmi);
        string bmiString = BMICalculator.getBMIResultString(false, age, bmi);

        updateTestEndRemark(bmi, bmiResult, bmiString);
    }

    private void updateTestEndRemark(float bmi, int bmiResult, string bmiString)
    {
        switch (bmiResult)
        {
            case 0:
                isEndEmotionHappy = false;
                endStringResource = "You are " + bmiString + ". Eat more healthy food!";
                break;
            case 1:
                isEndEmotionHappy = true;
                endStringResource = "You are " + bmiString + ". Keep eating healthy food!";
                break;
            default:
                isEndEmotionHappy = false;
                endStringResource = "You are " + bmiString + ". Eat healthy food!";
                break;
        }

        Popup.instance.OkayButton().AddListener(onTestComplete.Invoke);
        Popup.instance.OpenPopup(endStringResource, PopupType.CONFIRMATION);

        if (record == null)
            return;

        record.height = height;
        record.weight = weight;
        record.bmi = bmi;
    }
}
