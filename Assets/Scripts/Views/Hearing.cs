using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hearing : MonitoringTest
{
    public float disabledEarAlpha = 100;
    [SerializeField]
    private Image rightEar;
    [SerializeField]
    private Image leftEar;
    [SerializeField]
    private AudioSource audioSource;
    private HearingTest hearingTest;
    private bool shortcut = false;
    private bool testStarted = false;

    private void Start()
    {
        intro = "Let's find out how well you can hear. Please wear the earphones. Press the Yes button every time you hear a sound.";

        Popup.instance.OkayButton().AddListener(StartTest);
        Popup.instance.OpenPopup(intro, PopupType.CONFIRMATION);
    }

    private void StartTest()
    {
        testStarted = true;
        hearingTest = new HearingTest();
        double[] calibrationData = hearingTest.getCalibrationData();
        hearingTest.onEarSwitch.AddListener(EarSwitch);
        StartCoroutine(hearingTest.performTest(calibrationData, audioSource));
    }

    public void Yes()
    {
        if (!hearingTest.IsGap())
        { //if not pressed during gap
            if (hearingTest.HasCheated() <= 3)
            { //if <=3 cheating attempt (random presses) made, consider as correct answer
                Debug.Log("SetHeard");
                hearingTest.setHeard();
            } //if more than 3 cheat attempts, answer is no longer considered for that round (for z loop in hearing test)
        }
        else if (hearingTest.IsGap())
        { //if pressed during gap, consider as cheating attempt (max 3 allowed)
            Debug.Log("HasCheated");
            hearingTest.setCheated();
        }
    }

    private void EarSwitch()
    {
        Color color = rightEar.color;
        color.a = disabledEarAlpha;
        rightEar.color = color;

        color = leftEar.color;
        color.a = 1;
        leftEar.color = color;
    }

    private void Update()
    {
        if (testStarted && hearingTest.IsDone())
            endTest();
    }

    /**
    * Sends results to the activity this fragment is attached to.
    */
    private void endTest()
    {
        shortcut = false;
        stopTest();
    }

    /**
     * Stops the tests. Interrupts all the threads inside the
     * {@link #threads}.
     */
    private void stopTest()
    {
        testStarted = false;
        hearingTest.setIsNotRunning();
        updateTestEndRemark();
    }

    private void updateTestEndRemark()
    {
        Record record = Record.instance;
        if (!shortcut)
        {
            record.hearingRight = hearingTest.getPureToneAverageInterpretation("Right");
            record.hearingLeft = hearingTest.getPureToneAverageInterpretation("Left");
        }
        else
        {
            record.hearingRight = "Mild Hearing Loss";
            record.hearingLeft = "Mild Hearing Loss";
        }

        if (!record.hearingRight.Equals("Normal Hearing") || !record.hearingLeft.Equals("Normal Hearing"))
            endStringResource = "You seem to have some problems hearing. Try to visit an ear doctor when you can!";
        else
            endStringResource = "Your hearing is great!";

        Popup.instance.OkayButton().RemoveAllListeners();
        Popup.instance.OkayButton().AddListener(delegate
        {
            onTestComplete.Invoke();
            Popup.instance.OkayButton().RemoveAllListeners();
        });
        Popup.instance.OpenPopup(endStringResource, PopupType.CONFIRMATION);
    }

    /**
    * Called by the activity to skip the hearing test.
    * Sends dummy result to the activity this fragment is attached to.
    */
    public void endTestShortCut()
    { //For testing purposes only
        shortcut = true;
        stopTest();
    }
}
