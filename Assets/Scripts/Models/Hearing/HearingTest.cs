using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/**
 * The HearingTest class functions
 * to help manage the proctoring of the
 * audiometry hearing test.
 * <p>
 * The following class is based on the TestProctoring.java class created by
 * Reece Stevens (2014). The source code is available under the MIT License and
 * is published through a public GitHub repository:
 * https://github.com/ReeceStevens/ut_ewh_audiometer_2014/blob/master/app/src/main/java/ut/ewh/audiometrytest/TestProctoring.java
 *
 * @author Reece Stevens
 * @author Katrina Lacsamana
 * @version 03/11/2016
 */
public class HearingTest
{

    public UnityEvent onEarSwitch = new UnityEvent();
    // private int duration = 1;
    private int sampleRate = 44100;
    private int numSamples = 1 * 44100;
    private int volume = 50;
    private int[] testingFrequencies = { 250, 500, 1000, 2000, 4000 };
    // private double mGain = 0.0044;
    // private double mAlpha = 0.9;

    /**
     * Whether the sound was heard or not.
     */
    private bool isHeard = false;

    /**
     * Whether the sound is in loop or not.
     */
    private bool inLoop = true;

    /**
     * Whether the test is done or not.
     */
    private bool isDone = false;

    /**
     * Whether the test is inside the gap between samples or not.
     */
    private bool isGap = false;

    /**
     * Number of times the user responded within the gap.
     */
    private int hasCheated = 0;

    /**
     * Whether sound is currently played.
     */
    private static bool isRunning = true;

    /**
     * Threshold for the right ear.
     */
    private double[] thresholdsRight;

    /**
     * Threshold for the left ear.
     */
    private double[] thresholdsLeft;

    /**
     * Constructor.
     */
    public HearingTest()
    {
        this.isHeard = false;
        this.inLoop = true;
        this.isDone = false;
        isRunning = true;
        thresholdsRight = new double[testingFrequencies.Length];
        thresholdsLeft = new double[testingFrequencies.Length];
    }

    /**
     * Get random gap time between sounds.
     *
     * @return gap time.
     */
    private int getRandomGapDuration()
    {
        int time;
        double random = UnityEngine.Random.Range(0, 1);
        if (random < 0.1)
        {
            time = 2000;
        }
        else if (random < 0.2 && random >= 0.1)
        {
            time = 3000;
        }
        else if (random < 0.3 && random >= 0.2)
        {
            time = 4000;
        }
        else if (random < 0.4 && random >= 0.3)
        {
            time = 5000;
        }
        else if (random < 0.5 && random >= 0.4)
        {
            time = 6000;
        }
        else if (random < 0.6 && random >= 0.5)
        {
            time = 2500;
        }
        else if (random < 0.7 && random >= 0.6)
        {
            time = 3500;
        }
        else if (random < 0.8 && random >= 0.7)
        {
            time = 4500;
        }
        else if (random < 0.9 && random >= 0.8)
        {
            time = 5500;
        }
        else
        {
            time = 6500;
        }
        return time;
    }

    /**
     * Gets calibration data.
     *
     * @param context current context.
     * @return calibration data.
     */
    public double[] getCalibrationData()
    {
        byte[] calibrationByteData = new byte[24];

        // try
        // {
        //     FileInputStream fis = context.openFileInput("HearingTestCalibrationPreferences");
        //     fis.read(calibrationByteData, 0, 24);
        //     fis.close();
        // }
        // catch (IOException e)
        // {
        // }

        double[] calibrationArray = new double[3];
        int counter = 0;
        for (int i = 0; i < calibrationArray.Length; i++)
        {
            byte[] tempByteBuffer = new byte[8];
            for (int j = 0; j < tempByteBuffer.Length; j++)
            {
                tempByteBuffer[j] = calibrationByteData[counter];
                counter++;
            }

            calibrationArray[i] = BitConverter.ToDouble(BitConverter.IsLittleEndian
                ? tempByteBuffer.Take(8).Reverse().ToArray()
                : tempByteBuffer.Take(8).ToArray(), 0);
        }
        return calibrationArray;
    }

    private double[] ConvertByteToDouble(byte[] byteArray)
    {
        double[] doubleArray = new double[byteArray.Length / 8]; //size of a float is 4 bytes

        Buffer.BlockCopy(byteArray, 0, doubleArray, 0, byteArray.Length);

        return doubleArray;
    }

    /**
     * Run the test.
     *
     * @param calibrationArray calibration data.
     */
    public IEnumerator performTest(double[] calibrationArray, AudioSource audioSource)
    {
        SoundHelper soundHelper = new SoundHelper(numSamples, sampleRate);
        int tempResponse;
        bool earSwitched = false;

        for (int ear = 0; ear < 2; ear++)
        {
            if (ear > 0 && !earSwitched)
            {
                earSwitched = true;
                if (onEarSwitch != null)
                    onEarSwitch.Invoke();
            }

            for (int freqIndex = 0; freqIndex < testingFrequencies.Length; freqIndex++)
            {
                int frequency = testingFrequencies[freqIndex];
                float increment = (float)(Mathf.PI) * frequency / sampleRate;
                int maxVolume = volume;
                int minVolume = 0;
                audioSource.volume = volume / 800f;

                //Loop for each individual sample using binary search algorithm
                for (; ; )
                {
                    tempResponse = 0;
                    int actualVolume = (minVolume + maxVolume) / 2;
                    if ((maxVolume - minVolume) < 50)
                    {
                        if (ear == 0)
                        {
                            // thresholdsRight[freqIndex] = actualVolume * calibrationArray[freqIndex];
                            thresholdsRight[freqIndex] = actualVolume;
                        }
                        else if (ear == 1)
                        {
                            // thresholdsLeft[freqIndex] = actualVolume * calibrationArray[freqIndex];
                            thresholdsLeft[freqIndex] = actualVolume;
                        }
                        break;
                    }
                    else
                    {
                        for (int z = 0; z < 3; z++)
                        {
                            isHeard = false;
                            hasCheated = 0;
                            if (!isRunning)
                            {
                                yield return new WaitForSeconds(0);
                            }

                            isGap = true;
                            float randomGap = getRandomGapDuration() / 1000;
                            yield return new WaitForSeconds(randomGap);

                            isGap = false;
                            soundHelper.playSound(audioSource, frequency);
                            audioSource.panStereo = ear == 0 ? 1.0f : -1.0f;
                            audioSource.Play();

                            yield return new WaitForSeconds(1f);

                            audioSource.Stop();
                            audioSource.volume /= 2;

                            if (isHeard)
                            {
                                tempResponse++;
                            }

                            //Check if first two test were positive, skips the third to speed up the test
                            if (tempResponse >= 2)
                            {
                                break;
                            }

                            //Check if first two were misses and skips the third
                            if (z == 1 && tempResponse == 0)
                            {
                                break;
                            }
                        }
                        //If response was 2/3, register as heard
                        if (tempResponse >= 2)
                        {
                            maxVolume = actualVolume;
                        }
                        else
                        {
                            minVolume = actualVolume;
                        }
                    } //Continue with test
                }
            }
            //Run
        }
        inLoop = false;
        isDone = true;
    }

    /**
     * Gets {@link #hasCheated}.
     *
     * @return {@link #hasCheated}
     */
    public int HasCheated()
    {
        return hasCheated;
    }

    /**
     * Sets {@link #hasCheated}.
     */
    public void setCheated()
    {
        hasCheated++;
    }

    /**
     * Gets {@link #isGap}
     *
     * @return {@link #isGap}
     */
    public bool IsGap()
    {
        return isGap;
    }

    /**
     * Sets {@link #isHeard} to true.
     */
    public void setHeard()
    {
        isHeard = true;
    }

    /**
     * Gets {@link #isHeard}.
     *
     * @return {@link #isHeard}.
     */
    public bool IsHeard()
    {
        return isHeard;
    }

    /**
     * Gets {@link #isDone}
     *
     * @return {@link #isDone}
     */
    public bool IsDone()
    {
        return isDone;
    }

    /**
     * Gets {@link #inLoop}.
     *
     * @return {@link #inLoop}
     */
    public bool isInLoop()
    {
        return inLoop;
    }

    /**
     * Sets {@link #isRunning} to false.
     */
    public void setIsNotRunning()
    {
        isRunning = false;
    }

    /**
     * Gets {@link #isRunning}.
     *
     * @return {@link #isRunning}
     */
    public bool IsRunning()
    {
        return isRunning;
    }

    /**
     * Get the average of the test results.
     *
     * @param testResults results of the test
     * @return average of the results
     */
    private double getPureToneAverage(double[] testResults)
    {
        double result = 0;
        foreach (double d in testResults)
        {
            result += d;
        }
        return (result / testResults.Length);
    }

    /**
     * Get string results per frequency.
     *
     * @param testResults
     * @return
     */
    private string getResultsPerFrequency(double[] testResults)
    {
        string result = "";

        for (int i = 0; i < testResults.Length; i++)
        {
            result += (testingFrequencies[i] + " Hz: " + testResults[i].ToString("0.00") + " db HL\n");

        }
        return result;
    }

    /**
     * Get the string value of the result.
     *
     * @param result result of the test
     * @return result
     */
    private string interpretPureToneAverage(double result)
    {
        if (result <= 12) // Heard all 12
        {
            return "Normal Hearing";
        }
        else if (result >= 13 && result <= 21) // Heard four 17
        {
            return "Mild Hearing Loss";
        }
        else if (result >= 22 && result <= 26) // Heard three 22
        {
            return "Moderate Hearing Loss";
        }
        else if (result >= 27 && result <= 31) // Heard two 27
        {
            return "Moderately-Severe Hearing Loss";
        }
        else if (result >= 32 && result <= 36) // Heard one 32
        {
            return "Severe Hearing Loss";
        }
        else // Heard none 37
        {
            return "Profound Hearing Loss";
        }
    }

    /**
     * Get the average result.
     *
     * @param testResults results
     * @return average result
     * @see #getResults()
     */
    private string getPureToneAverageResults(double[] testResults)
    {
        double ptaResult = getPureToneAverage(testResults);
        string result = "Pure Tone Average: " + ptaResult.ToString("0.00") + " dB HL" +
                "\nYou have " + interpretPureToneAverage(ptaResult) + ".";

        return result;
    }

    /**
     * Get results for both ears.
     *
     * @return results for both ears.
     */
    public string getResults()
    {
        string result = "Right Ear\n" + getResultsPerFrequency(thresholdsRight) + getPureToneAverageResults(thresholdsRight)
                + "\n\nLeft Ear\n" + getResultsPerFrequency(thresholdsLeft) + getPureToneAverageResults(thresholdsLeft);

        return result;
    }

    /**
     * Get the results for the specific ear
     *
     * @param ear ear you want to get the results of
     * @return results for the ear.
     */
    public string getPureToneAverageInterpretation(string ear)
    {
        double[] thresholds;
        if (ear.Equals("Right"))
        {
            thresholds = thresholdsRight;
        }
        else
        {
            thresholds = thresholdsLeft;
        }

        return interpretPureToneAverage(getPureToneAverage(thresholds));
    }


}

