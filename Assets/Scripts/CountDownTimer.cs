using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CountDownTimer : MonoBehaviour
{
    public UnityEvent onTick;
    public UnityEvent onFinish;

    private float duration;
    private float tick;
    private float durationCountDown;
    private float tickCountDown;

    private bool isCounting = false;

    private void Update()
    {
        if (isCounting)
        {
            durationCountDown -= Time.deltaTime;
            tickCountDown -= Time.deltaTime;

            if (tickCountDown <= 0)
            {
                if (onTick != null)
                    onTick.Invoke();

                tickCountDown = tick;
            }

            if (durationCountDown <= 0)
            {
                if (onFinish != null)
                    onFinish.Invoke();

                durationCountDown = duration;
            }
        }
    }

    public float GetSeconds()
    {
        return durationCountDown % 60;
    }

    public void SetCountDownTimer(float durationInSeconds, float tickInSeconds)
    {
        duration = durationInSeconds;
        durationCountDown = duration;
        tick = tickInSeconds;
        tickCountDown = tick;
    }

    public void StartCountDown()
    {
        Debug.Log("Countdown started with " + duration + " seconds");
        isCounting = true;
    }

    public void StopCountDown()
    {
        isCounting = false;
    }

    public void CancelCountDown()
    {
        durationCountDown = duration;
        tickCountDown = tick;
        isCounting = false;
    }
}
