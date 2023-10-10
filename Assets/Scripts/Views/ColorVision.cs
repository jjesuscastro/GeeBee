using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//NOTE: Done minus Record
public class ColorVision : MonitoringTest
{
    [SerializeField]
    private Image plateView;
    [SerializeField]
    private Button[] buttonList;
    [SerializeField]
    private Sprite[] ishiharaPlates;
    [SerializeField]
    private Sprite[] buttonSprites;

    private IshiharaHelper helper;
    private bool isTestOngoing;
    private Record record;

    // Start is called before the first frame update
    private void Start()
    {
        record = Record.instance;
        intro = "Now let's check how well you can see colors. Tell me the shape you see by pressing the buttons below.";
        helper = new IshiharaHelper(plateView, buttonList, ishiharaPlates, buttonSprites);
        isTestOngoing = true;
        helper.startTest();
        Popup.instance.OpenPopup(intro, PopupType.CONFIRMATION);
    }

    public void Answer(int i)
    {
        helper.answerQuestion(i);
    }

    /**
     * Allows test to either go to the next question and save results if the test is done.
     *
     * @param ishiharaHelper helper class of the fragment.
     */
    public void updateResults()
    {
        helper.goToNextQuestion();
        if (helper.IsDone() && isTestOngoing)
        {
            isTestOngoing = false;
            displayResults(helper.getScore());
            updateTestEndRemark(helper.isNormal());
        }
    }

    /**
     * Updates the end test attributes of the test fragment namely
     * {@link #isEndEmotionHappy}, {@link #endStringResource},
     * and {@link #endTime}  on the result of the test.
     *
     * @param normal test result of the patient. true if normal, false if not normal.
     * @see MonitoringTestFragment
     */
    private void updateTestEndRemark(bool normal)
    {
        if (normal)
        {
            this.isEndEmotionHappy = true;
            this.endStringResource = "Your vision is great!";
            this.endTime = 3000;
        }
        else
        {
            this.isEndEmotionHappy = false;
            this.endStringResource = "It looks like you\'re having problems seeing colors. Try to visit an eye doctor about it when you can!";
            this.endTime = 7000;
        }

        Popup.instance.OkayButton().AddListener(onTestComplete.Invoke);
        Popup.instance.OpenPopup(endStringResource, PopupType.CONFIRMATION);

        if (record == null)
            return;

        record.colorVision = helper.getResult();
    }

    /**
     * Sends the test results to the activity that this fragment is attached to.
     *
     * @param score patient's final test score.
     */
    private void displayResults(int score)
    {
        string resultString = "SCORE: " + score;

        if (score >= 10)
        {
            resultString += "You have NORMAL color vision.";
        }
        else
        {
            resultString += "You scored lower than normal.";
        }
    }
}
