using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class VisualAcuity : MonitoringTest
{
    public int chartPreference = 2;

    [SerializeField]
    private Image chartView;
    [SerializeField]
    private Sprite[] tumblingECharts;
    [SerializeField]
    private Sprite[] snellenCharts;

    private ChartHelper helper;
    private VisualAcuityResult rightEyeResult;
    private VisualAcuityResult leftEyeResult;
    private Record record;

    // Start is called before the first frame update
    private void Start()
    {
        record = Record.instance;
        DistanceCalculator distanceCalculator = new DistanceCalculator();
        float distance = distanceCalculator.getUserDistance(chartView);

        intro = "Let's check your eyesight. Move " + distance.ToString("0.00") + " meters away from the tablet. Cover your left eye, then tell me what you see. Assistant, press the \"Okay\" button when the child is ready.";

        if (chartPreference == 1)
            helper = new ChartHelper(chartView, tumblingECharts);
        else
            helper = new ChartHelper(chartView, snellenCharts);

        helper.startTest();
        Popup.instance.OpenPopup(intro, PopupType.CONFIRMATION);
    }

    public void Yes()
    {
        helper.goToNextLine();
        if (helper.IsDone() && !helper.isBothTested())
            updateResults(helper);
    }

    public void No()
    {
        helper.setResult();
        if (helper.IsDone() && !helper.isBothTested())
            updateResults(helper);
    }

    /**
     * Gets the result from the {@code chartHelper} and sends it to the
     * activity that this fragment is attached to.
     *
     * @param chartHelper
     */
    private void updateResults(ChartHelper chartHelper)
    {
        if (!helper.IsRightTested() && rightEyeResult == null)
        {
            rightEyeResult = new VisualAcuityResult("Right", chartHelper.getResult());
            chartHelper.setIsRightTested();
            chartHelper.startTest();
            displayResults(rightEyeResult);
        }
        else if (!helper.IsLeftTested() && leftEyeResult == null)
        {
            leftEyeResult = new VisualAcuityResult("Left", chartHelper.getResult());
            chartHelper.setIsLeftTested();
            displayResults(leftEyeResult);
            updateTestEndRemark(leftEyeResult.getLineNumber(), rightEyeResult.getLineNumber());
        }
    }

    /**
     * Displays the results to the user.
     *
     * @param result visual acuity test result.
     */
    private void displayResults(VisualAcuityResult result)
    {
        string resultstring = "";

        if (leftEyeResult == null && rightEyeResult != null)
            resultstring = "Now, uncover your left eye. Then, cover your right eye.";

        if (rightEyeResult != null && leftEyeResult != null)
            Popup.instance.OkayButton().AddListener(onTestComplete.Invoke);
        Popup.instance.OpenPopup(resultstring, PopupType.CONFIRMATION);
    }

    /**
     * Updates the end test attributes of the test fragment namely
     * {@link #isEndEmotionHappy}, {@link #endstringResource},
     * and {@link #endTime} depending on the result of the test.
     *
     * @param lineNumberLeft  line number in the eye chart that the user is able to read correctly last using
     *                        just her left eye.
     * @param lineNumberRight line number in the eye chart that the user is able to read correctly last using
     *                        just her right eye.
     * @see MonitoringTestFragment
     */
    private void updateTestEndRemark(int lineNumberLeft, int lineNumberRight)
    {
        int lineNumLeft = lineNumberLeft;
        int lineNumRight = lineNumberRight;

        if (lineNumLeft < 8 || lineNumRight < 8)
        {
            isEndEmotionHappy = false;
            endStringResource = "You seem to have some problems seeing. Try to visit an eye doctor when you can!";
            endTime = 6000;
        }
        else
        {
            isEndEmotionHappy = true;
            endStringResource = "Your vision is great!";
            endTime = 3000;
        }

        Popup.instance.OpenPopup(endStringResource, PopupType.CONFIRMATION);

        if (record == null)
            return;

        record.visualAcuityRight = rightEyeResult.getVisualAcuity();
        record.visualAcuityLeft = leftEyeResult.getVisualAcuity();
    }
}
