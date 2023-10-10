using UnityEngine;
using UnityEngine.UI;

/**
 * The ChartHelper class manages the
 * administration of the visual
 * acuity test using the visual acuity chart
 *
 * @author Katrina Lacsamana
 * @since02/24/2016.
 */
public class ChartHelper
{
    /**
     * List of chart lines of the test.
     */
    private ChartLine[] chart;

    /**
     * Current line number tested.
     */
    private int currentLineNumber;

    /**
     * where the chart is shown.
     */
    private Image chartView;

    /**
     * Line where the user last read correctly
     */
    private ChartLine result;

    /**
     * Whether the test is done or not.
     */
    private bool isDone;

    /**
     * Whether the right eye is tested or not.
     */
    private bool isRightTested;

    /**
     * Whether the left eye is tested or not.
     */
    private bool isLeftTested;

    /**
     * Constructor.
     *
     * @param chartView       {@link #chartView}
     * @param chartPreference the preferred eye chart to
     *                        be used for the test.
     */
    public ChartHelper(Image chartView, Sprite[] charts)
    {
        chart = new ChartLine[charts.Length];
        initializeChartPreference(charts);
        this.chartView = chartView;
        this.isRightTested = false;
        this.isLeftTested = false;
    }

    /**
     * Initialize the {@link #chart} depending on the {@code chartPreference}.
     *
     * @param chartPreference preferred chart of the user.
     */
    private void initializeChartPreference(Sprite[] charts)
    {
        chart[0] = new ChartLine(1, 20, 200, charts[0]);
        chart[1] = new ChartLine(2, 20, 100, charts[1]);
        chart[2] = new ChartLine(3, 20, 70, charts[2]);
        chart[3] = new ChartLine(4, 20, 50, charts[3]);
        chart[4] = new ChartLine(5, 20, 40, charts[4]);
        chart[5] = new ChartLine(6, 20, 30, charts[5]);
        chart[6] = new ChartLine(7, 20, 25, charts[6]);
        chart[7] = new ChartLine(8, 20, 20, charts[7]);
        chart[8] = new ChartLine(9, 20, 15, charts[8]);
        chart[9] = new ChartLine(10, 20, 10, charts[9]);
        chart[10] = new ChartLine(11, 20, 5, charts[10]);
    }

    /**
     * Gets the chart before the current chart line.
     *
     * @return previous line.
     */
    private ChartLine getPreviousLine()
    {
        return chart[currentLineNumber - 1];
    }

    /**
     * Gets the current chart line.
     *
     * @return current chart line.
     */
    private ChartLine getCurrentLine()
    {
        return chart[currentLineNumber];
    }

    /**
     * Change line shown to the user.
     */
    public void goToNextLine()
    {
        if (currentLineNumber < 10)
        {
            currentLineNumber++;
            displayChartLine();
        }
        else if (currentLineNumber == 10 && result == null)
        {
            setResult();
        }
    }

    /**
     * Displays the current chart line.
     */
    private void displayChartLine()
    {
        chartView.sprite = getCurrentLine().getChartLineDrawable();
    }

    /**
     * Starts the test.
     */
    public void startTest()
    {
        result = null;
        isDone = false;
        currentLineNumber = 0;
        displayChartLine();
    }

    /**
     * Gets {@link #isDone}.
     *
     * @return {@link #isDone}
     */
    public bool IsDone()
    {
        return isDone;
    }

    /**
     * Sets {@link #result} depending on the current chart line.
     */
    public void setResult()
    {
        if (currentLineNumber == 0 || currentLineNumber == 10)
        {
            result = getCurrentLine();
        }
        else
        {
            result = getPreviousLine();
        }
        isDone = true;
    }

    /**
     * Gets {@link #result}
     *
     * @return
     */
    public ChartLine getResult()
    {
        return result;
    }

    /**
     * Gets {@link #isRightTested}.
     *
     * @return {@link #isRightTested}
     */
    public bool IsRightTested()
    {
        return isRightTested;
    }

    /**
     * Gets {@link #isLeftTested}.
     *
     * @return {@link #isLeftTested}
     */
    public bool IsLeftTested()
    {
        return isLeftTested;
    }

    /**
     * Sets {@link #isRightTested} to true.
     */
    public void setIsRightTested()
    {
        isRightTested = true;
    }

    /**
     * Sets {@link #isLeftTested} to true.
     */
    public void setIsLeftTested()
    {
        isLeftTested = true;
    }

    /**
     * Check whether two eyes were tested or not.
     *
     * @return true if both were tested. Else, false.
     */
    public bool isBothTested()
    {
        return isRightTested && isLeftTested;
    }


}
