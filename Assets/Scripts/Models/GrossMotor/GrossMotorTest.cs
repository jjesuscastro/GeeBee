using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
 * The GrossMotorTest class represents the
 * gross motor test that the child will
 * take. It generates a short list of skills
 * from the general list of skills, as well as
 * generates music for the test. The class
 * allows the test to be generated for each
 * user and allows the user to perform
 * the skill.
 *
 * @author Katrina Lacsamana
 * @since 03/21/2016
 */
public class GrossMotorTest
{

    /**
     * List of all possible grossmotor skills to be tested.
     */
    private GrossMotorSkill[] grossMotorSkills;

    /**
     * List of grossmotor skills to be tested.
     */
    private GrossMotorSkill[] testSkills;

    /**
     * Musicplayer to be used for playing music
     * while running the skillset.
     */
    private AudioSource musicPlayer;

    /**
     * Index of the current skill tested from {@link #testSkills}.
     */
    private int currentSkill;

    /**
     * Used in counting down the remaining time.
     */
    private CountDownTimer countDownTimer;

    /**
     * Constructor.
     *
     * @param context current context.
     */
    public GrossMotorTest(Sprite[] jumpingJacks, Sprite[] jumpInPlace, Sprite[] runInPlace, Sprite[] hopInPlace, Sprite[] oneFootBalance, Sprite[] walkInPlace, Sprite[] marchInPlace, Sprite[] jogInPlace)
    {
        // musicPlayer = new MusicPlayer(context);
        grossMotorSkills = new GrossMotorSkill[8];
        testSkills = new GrossMotorSkill[3];
        currentSkill = 0;
        grossMotorSkills[0] = new GrossMotorSkill("Jumping Jacks", "Jumping", "Do jumping jacks", 30, jumpingJacks);
        grossMotorSkills[1] = new GrossMotorSkill("Jump in Place", "Jumping", "Jump in place", 30, jumpInPlace);
        grossMotorSkills[2] = new GrossMotorSkill("Run in Place", "Running", "Run in place", 40, runInPlace);
        grossMotorSkills[3] = new GrossMotorSkill("Hop in Place", "Hopping", "Hop on one foot", 20, hopInPlace);
        grossMotorSkills[4] = new GrossMotorSkill("One Foot Balance", "Balance", "Stand and balance on one foot", 15, oneFootBalance);
        grossMotorSkills[5] = new GrossMotorSkill("Walk in Place", "Walking", "Walk in place", 60, walkInPlace);
        grossMotorSkills[6] = new GrossMotorSkill("March in Place", "Marching", "March in place", 60, marchInPlace);
        grossMotorSkills[7] = new GrossMotorSkill("Jog in Place", "Jogging", "Jog in place", 40, jogInPlace);
    }

    /**
     * Gets a random skill that hasnt been tested yet.
     *
     * @return skill to be tested next.
     */
    private GrossMotorSkill getRandomSkill()
    {
        System.Random random = new System.Random((int)nanoTime());
        bool isFound = false;
        GrossMotorSkill temp = null;

        while (!isFound)
        {
            temp = grossMotorSkills[random.Next(grossMotorSkills.Length - 1)];
            if (!checkSkillDuplicates(testSkills, temp))
            {
                break;
            }
        }
        return temp;
    }

    /**
     * Check whether the {@code array} contains {@code key}
     *
     * @param array List of skills to be examined
     * @param key   to be searched inside the {@code array}
     * @return
     */
    private bool checkSkillDuplicates(GrossMotorSkill[] array, GrossMotorSkill key)
    {
        foreach (GrossMotorSkill gms in array)
        {
            if (key == gms)
            {
                return true;
            }
        }
        return false;
    }

    /**
     * Initialize the values for {@link #testSkills}.
     */
    public void makeTest()
    {
        for (int i = 0; i < 3; i++)
        {
            testSkills[i] = getRandomSkill();
        }
    }

    /**
     * Sets {@link #currentSkill}.
     *
     * @param skillNumber new value
     */
    public void setCurrentSkill(int skillNumber)
    {
        currentSkill = skillNumber;
    }

    /**
     * Perform the skill with index {@code skillNumber}
     *
     * @param skillNumber Index of the skill in {@link #testSkills} to be performed.
     * @param timerView   where time is shown.
     * @param answers     contains the yes and no buttons.
     * @param NAButton    to be pressed when the user cant perform skill.
     */
    public void performSkill(int skillNumber, AudioSource musicPlayer, CountDownTimer cDownTimer, TMP_Text timerView, Button NAButton)
    {
        GrossMotorSkill skill = testSkills[skillNumber];
        this.musicPlayer = musicPlayer;
        countDownTimer = cDownTimer;
        countDownTimer.SetCountDownTimer(skill.getDuration(), 1);
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
            timerView.text = "";
            musicPlayer.Stop();

            Popup.instance.OpenPopup("Was the child ablee to perform the task?", PopupType.YESNO);
            NAButton.gameObject.SetActive(false);
        });

        musicPlayer.Play();
    }

    /**
     * Gets the skill at index {@link #currentSkill} in {@link #testSkills}.
     *
     * @return
     */
    public GrossMotorSkill getCurrentSkill()
    {
        return testSkills[currentSkill];
    }

    /**
     * Gets {@link #currentSkill}.
     *
     * @return {@link #currentSkill}
     */
    public int getCurrentSkillNumber()
    {
        return currentSkill;
    }

    /**
     * Get all the results of all the skills done.
     *
     * @return result string.
     */
    public string getAllResults()
    {
        string result = "";
        foreach (GrossMotorSkill gms in testSkills)
        {
            result += gms.getSkillName() + " : " + gms.getAssessment() + "\n";
        }
        return result;
    }

    /**
     * Gets the overall result of the test in string form.
     *
     * @return overall result.
     */
    public string getFinalResult()
    {
        int result = getIntFinalResult();

        switch (result)
        {
            case 0:
                return "Pass";
            case 1:
                return "Fail";
            default:
                return "N/A";
        }
    }

    /**
     * Gets the overall result of the test in int form.
     *
     * @return overall result.
     */
    public int getIntFinalResult()
    {
        int pass = 0;
        int na = 0;
        string assessment;
        foreach (GrossMotorSkill gms in testSkills)
        {
            assessment = gms.getAssessment();
            if (assessment.Equals("Pass"))
            {
                pass++;
            }
            else if (assessment.Equals("N/A"))
            {
                na++;
            }
        }
        if (pass >= 2)
        { //Pass
            return 0;
        }
        else if (na >= 2)
        { //N/A
            return 2;
        }
        else
        { //Fail
            return 1;
        }
    }

    /**
     * Ends the test.
     */
    public void endTest()
    {
        if (musicPlayer != null)
            musicPlayer.Stop();
    }

    /**
     * Skips the test
     *
     * @param timerView shows the remaining time.
     */
    public void skipTest(TMP_Text timerView)
    {
        if (countDownTimer != null)
        {
            countDownTimer.CancelCountDown();
        }
        timerView.text = "";

        if (musicPlayer != null)
            musicPlayer.Stop();
    }

    /**
        * Gets C# equivalent to Java System.nanoTime()
        */
    private long nanoTime()
    {
        long nano = 10000L * Stopwatch.GetTimestamp();
        nano /= TimeSpan.TicksPerMillisecond;
        nano *= 100L;
        return nano;
    }
}
