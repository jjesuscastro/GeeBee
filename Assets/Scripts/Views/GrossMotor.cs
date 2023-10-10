using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GrossMotor : MonitoringTest
{
    [SerializeField]
    private TMP_Text timerView;
    [SerializeField]
    private AnimatedImage animatedImage;
    [SerializeField]
    private AudioSource musicPlayer;
    /**
     * Used for timing each skill in the test.
     */
    [SerializeField]
    private CountDownTimer countDownTimer;

    /**
     * Clicked if the skill cannot be performed
     */
    [SerializeField]
    private Button naButton;
    [SerializeField]
    private Sprite[] jumpingJacks;
    [SerializeField]
    private Sprite[] jumpInPlace;
    [SerializeField]
    private Sprite[] runInPlace;
    [SerializeField]
    private Sprite[] hopInPlace;
    [SerializeField]
    private Sprite[] oneFootBalance;
    [SerializeField]
    private Sprite[] walkInPlace;
    [SerializeField]
    private Sprite[] marchInPlace;
    [SerializeField]
    private Sprite[] jogInPlace;

    /**
     * Contains the gross motor test that the child will
     * take.
     */
    private GrossMotorTest grossMotorTest;

    /**
     * Constructor.
     *
     * @see MonitoringTestFragment#intro
     */
    private void Start()
    {
        intro = "Let\'s do some fun exercises!";
        Popup.instance.YesButton().RemoveAllListeners();
        Popup.instance.YesButton().AddListener(delegate
        {
            grossMotorTest.getCurrentSkill().setSkillPassed();
            goToNextQuestion();
        });

        Popup.instance.NoButton().RemoveAllListeners();
        Popup.instance.NoButton().AddListener(delegate
        {
            grossMotorTest.getCurrentSkill().setSkillFailed();
            goToNextQuestion();
        });

        grossMotorTest = new GrossMotorTest(jumpingJacks, jumpInPlace, runInPlace, hopInPlace, oneFootBalance, walkInPlace, marchInPlace, jogInPlace);
        grossMotorTest.makeTest();
        startTest();
    }

    /**
     * Called when NAButton is clicked. Stops the current skill and starts the next.
     */
    public void onNAButtonClick()
    {
        if (countDownTimer != null)
        {
            countDownTimer.CancelCountDown();
        }
        grossMotorTest.getCurrentSkill().setSkillSkipped();
        goToNextQuestion();
        grossMotorTest.skipTest(timerView);
    }

    /**
     * Called when the activity has detected the user's press of the back key.
     * Ends the grossmotor test.
     */
    public void onBackPressed()
    {
        grossMotorTest.endTest();
    }

    /**
     * Updates the end test attributes of the test fragment namely
     * {@link #isEndEmotionHappy}, {@link #endstringResource},
     * and {@link #endTime} depending on the result of the test.
     *
     * @param result test result of the patient. 0 if passed, 1 if fail, 2 if n/a.
     * @see MonitoringTestFragment
     */
    private void updateTestEndRemark(int result)
    {
        switch (result)
        {
            case 0:
                isEndEmotionHappy = true;
                endStringResource = "Your gross motor skills are great!";
                this.endTime = 3000;
                break;
            case 1:
                isEndEmotionHappy = false;
                endStringResource = "Work on your gross motor skills! Try to exercise more and play outside.";
                endTime = 5000;
                break;
            default:
                isEndEmotionHappy = true;
                endStringResource = "That\'s okay! Let\'s move on to the next test.";
                endTime = 4000;
                break;
        }

        Popup.instance.OkayButton().RemoveAllListeners();
        Popup.instance.OkayButton().AddListener(onTestComplete.Invoke);
        Popup.instance.OpenPopup(endStringResource, PopupType.CONFIRMATION);
    }

    /**
     * Starts the test.
     */
    private void startTest()
    {
        grossMotorTest.setCurrentSkill(0);
        displaySkill(0);
    }

    /**
     * Ends the test and sends result to the
     * activity this fragment is attached to.
     */
    private void endTest()
    {
        string resultstring = grossMotorTest.getAllResults() + "\nOverall: " + grossMotorTest.getFinalResult();
        Record record = Record.instance;

        grossMotorTest.endTest();
        hideAnswerButtons();

        record.grossMotor = grossMotorTest.getIntFinalResult();
        record.grossMotorRemark = grossMotorTest.getFinalResult();

        naButton.gameObject.SetActive(false);
        updateTestEndRemark(grossMotorTest.getIntFinalResult());
    }

    /**
     * Displays the skill as determined by the GrossMotorTest on the screen
     *
     * @param i index to know how many skill test has been done.
     */
    private void displaySkill(int i)
    {
        GrossMotorSkill gms = grossMotorTest.getCurrentSkill();
        string durationstring = gms.getDuration().ToString("00");

        Popup.instance.OkayButton().AddListener(delegate
        {
            countDownTimer.StartCountDown();
            Popup.instance.OkayButton().RemoveAllListeners();
        });
        Popup.instance.OpenPopup(gms.getInstruction() + " for " + durationstring + " seconds.", PopupType.CONFIRMATION);

        animatedImage.SetSprites(gms.getSkillResImage());

        countDownTimer.SetCountDownTimer(5, 1);
        timerView.text = countDownTimer.GetSeconds().ToString("00");
        countDownTimer.onTick.RemoveAllListeners();
        countDownTimer.onTick.AddListener(delegate
        {
            timerView.text = countDownTimer.GetSeconds().ToString("00");
        });

        countDownTimer.onFinish.RemoveAllListeners();
        countDownTimer.onFinish.AddListener(delegate
        {
            countDownTimer.StopCountDown();
            grossMotorTest.performSkill(i, musicPlayer, countDownTimer, timerView, naButton);
            countDownTimer.StartCountDown();
        });

        hideAnswerButtons();
    }

    /**
     * Allows the test to move to the next question or ends the test
     */
    private void goToNextQuestion()
    {
        grossMotorTest.getCurrentSkill().setTested();
        int currentSkill = grossMotorTest.getCurrentSkillNumber();
        if (currentSkill < 2)
        {
            currentSkill++;
            grossMotorTest.setCurrentSkill(currentSkill);
            countDownTimer.StopCountDown();
            displaySkill(currentSkill);
        }
        else if (currentSkill == 2)
        {
            endTest();
        }
    }

    /**
     * Hides the yes and no buttons
     */
    private void hideAnswerButtons()
    {
        naButton.gameObject.SetActive(true);
    }
}
