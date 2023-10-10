using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

/**
 * Serves as the helper class of FineMotorActivity
 * sets the conditions for each round of the test
 *
 * @author Mary Grace Malana
 * @since 25/03/2016
 */
public class FineMotorHelper
{
    /**
     * Max number of wrongs to pass the fine motor test.
     */
    private static int MAX_NUM_WRONG = 2;

    /**
     * View for the path.
     */
    private Image imageViewPathToTrace;

    /**
     * Media player of playing sounds.
     * NOTE: MediaPlayer is an AudioSource that plays the error sound when user goes outisde the path
     */
    // private MediaPlayer mp;
    private AudioSource mp;

    /**
     * Flag whether the user was outside the path or not.
     */
    private bool wasOutside = false;

    /**
     * Counter for the number of wrongs the user
     * has commited.
     */
    private int numWrongs = 0;

    /**
     * List of results for each round. result[i] is true if pass, false if fail
     */
    private bool[] result = new bool[3];

    /**
     * List of instructions for each round.
     */
    private string[] instructions;

    /*
     * List of Sprites for Paths
     */
    Sprite[] paths;

    /**
     * Constructor.
     *
     * @param context              current context.
     * @param imageViewPathToTrace {@link #imageViewPathToTrace}
     */
    public FineMotorHelper(Image imageViewPathToTrace, AudioSource mp, AudioClip outsidePath, Sprite[] pathsSprites)
    {
        this.imageViewPathToTrace = imageViewPathToTrace;
        this.mp = mp;

        paths = pathsSprites;
        int path = getRandomPathDrawable();
        // mp = MediaPlayer.create(context, R.raw.fine_motor_outside_path);
        // mp.setLooping(true);
        mp.clip = Resources.Load<AudioClip>("FineMotor/fine_motor_outside_path");

        imageViewPathToTrace.sprite = paths[path];
        imageViewPathToTrace.enabled = true;
        instructions = getInstructions(paths, path);
    }

    /**
     * Gets {@link #result}.
     *
     * @return {@link #result}
     */
    public bool[] getResults()
    {
        return result;
    }

    /**
     * Sets {@link #result}
     *
     * @param index index of the result to be set
     * @param isYes new value
     */
    public void setResult(int index, bool isYes)
    {
        result[index] = isYes;
    }

    /**
     * Called if the user is outside the path.
     */
    public void doIfOutSideThePath()
    {
        if (!wasOutside)
        {
            // mp.start();
            wasOutside = true;
            numWrongs++;
        }
    }

    /**
     * Check whether touch is within path.
     *
     * @return true if touch was outside, else false.
     */
    public bool doIfWithinPath()
    {
        if (wasOutside)
        {
            pauseMp();
            wasOutside = false;
            return true;
        }
        return false;
    }

    /**
     * Starts the next test. resets the variables
     *
     * @param currentTest index of the current test
     * @return instructions for the next test.
     */
    public string doNextTest(int currentTest)
    {
        pauseMp();
        result[currentTest] = numWrongs <= MAX_NUM_WRONG;
        numWrongs = 0;
        return setInstructions(currentTest + 1);
    }

    /**
     * Called when user lifted touch during the test.
     *
     * @return instructions when touch is lifted.
     */
    public string doIfTouchIsUp()
    {
        pauseMp();
        return "Don't lift your finger. Go back to start";
    }

    /**
     * Gets the instructions.
     *
     * @param index index in {@link #instructions}
     * @return instructions in specific index
     */
    public string setInstructions(int index)
    {
        return (instructions[index]);
    }

    /**
     * Get random path.
     *
     * @return path resource id
     */
    private int getRandomPathDrawable()
    {
        System.Random random = new System.Random((int)nanoTime());
        return random.Next(paths.Length);
    }

    /**
     * Get the instructions depending on the chosen path
     *
     * @param path path chosen
     * @return instructions for the specific path
     */
    private string[] getInstructions(Sprite[] paths, int path)
    {
        string[] instructionList = null;
        Sprite path1 = paths[0];
        switch (path)
        {
            case 0:
                instructionList = new string[]{"Help the butterfly go to the flowers. Use a finger from your non-dominant hand to trace the path.",
                        "Help the butterfly go to the flowers. Use the pen with your dominant hand to trace the path.",
                        "Assistant, has the user used the pen without difficulties?"
                };
                break;
            case 1:
                instructionList = new string[]{"Help the lion go to his friends. Use a finger of your non dominant hand to trace the path.",
                        "Help the lion go to his friends. Use the pen with your dominant hand to trace the path.",
                        "Assistant, has the user used the pen without difficulties?"
                };
                break;
        }
        return instructionList;

    }

    /**
     * Pauses the {@link #mp}
     */
    private void pauseMp()
    {
        if (mp.isPlaying)
        {
            mp.Pause();
        }
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

