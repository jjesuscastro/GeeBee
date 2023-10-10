using UnityEngine;
using UnityEngine.Events;

/**
 * MonitoringTestFragment is an abstract class
 * used in MonitoringMainActivity.
 * This class are implemented by monitoring test fragments that has intro and results.
 *
 * @author Mary Grace Malana
 */
public abstract class MonitoringTest : MonoBehaviour
{
    /**
     * String resource of the intro regarding the monitoring test.
     */
    protected string intro;

    /**
     * String resource of the result remarks regarding the monitoring test.
     */
    protected string endStringResource;

    /**
     * How long in terms of milliseconds that the result remarks are uttered by the ECA.
     */
    protected int endTime;

    /**
     * End emotion to be expressed by the ECA about the result. If true, happy, else false.
     */
    protected bool isEndEmotionHappy; //true happy, false concern

    /**
     * If true the fragment has instruction prior to starting the test, else it has no
     * instruction prior to starting the test.
     *
     * @see com.geebeelicious.geebeelicious.fragments.VisualAcuityFragment
     * @see com.geebeelicious.geebeelicious.activities.MonitoringMainActivity#runTransition(int, String, Fragment, bool)
     */
    protected bool hasEarlyInstruction;

    public UnityEvent onTestComplete;

    /**
     * Gets {@link #intro}
     *
     * @return {@link #intro}
     */
    public string getIntro()
    {
        return intro;
    }

    /**
     * Gets {@link #endStringResource}
     *
     * @return {@link #endStringResource}
     */
    public string getEndStringResource()
    {
        return endStringResource;
    }

    /**
     * Gets {@link #endTime}
     *
     * @return {@link #endTime}
     */
    public int getEndTime()
    {
        return endTime;
    }

    /**
     * Gets {@link #isEndEmotionHappy}
     *
     * @return {@link #isEndEmotionHappy}
     */
    public bool IsEndEmotionHappy()
    {
        return isEndEmotionHappy;
    }

    /**
     * Gets {@link #hasEarlyInstruction}
     *
     * @return {@link #hasEarlyInstruction}
     */
    public bool HasEarlyInstruction()
    {
        return hasEarlyInstruction;
    }

    private void OnDisable()
    {
        Popup.instance.OkayButton().RemoveListener(onTestComplete.Invoke);
    }
}
