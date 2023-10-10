using System;
using UnityEngine;

/**
 * The SoundHelper class contains functionality to
 * generate and play sounds for the hearing test.
 *
 * @author Katrina Lacsamana
 * @since 03/11/2016
 */
public class SoundHelper
{

    /**
     * Number of samples.
     */
    private int numSamples;
    private int position;

    /**
     * Sample rate of the test.
     */
    private int sampleRate;

    private int frequency;

    /**
     * Constructor.
     *
     * @param numSamples {@link #numSamples}
     * @param sampleRate {@link #sampleRate}
     */
    public SoundHelper(int numSamples, int sampleRate)
    {
        this.numSamples = numSamples;
        this.sampleRate = sampleRate;
    }

    /**
     * Get AudioTrack to be played
     *
     * @param generatedSound generated sound made for the test.
     * @return sound to be played.
     */
    public AudioClip CreateClip(int frequency)
    {
        this.frequency = frequency;
        AudioClip audioClip = AudioClip.Create("Beep", sampleRate * 2, 2, sampleRate, true, OnAudioRead, OnAudioSetPosition);
        return audioClip;
    }

    void OnAudioRead(float[] data)
    {
        int count = 0;
        while (count < data.Length)
        {
            data[count] = Mathf.Sign(Mathf.Sin(2 * Mathf.PI * frequency * position / sampleRate));
            position++;
            count++;
        }
    }

    void OnAudioSetPosition(int newPosition)
    {
        position = newPosition;
    }

    private float[] ConvertByteToFloat(byte[] byteArray)
    {
        float[] floatArray = new float[byteArray.Length / 4]; //size of a float is 4 bytes

        Buffer.BlockCopy(byteArray, 0, floatArray, 0, byteArray.Length);

        return floatArray;
    }

    /**
     * Play sound.
     *
     * @param generatedSound generated sound made for the test.
     * @param ear            ear tested
     * @return track played.
     */
    public void playSound(AudioSource aSource, int frequency)
    {
        AudioClip audioClip = CreateClip(frequency);
        aSource.clip = audioClip;
    }

}
