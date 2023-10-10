/**
 * The VisualAcuityResult class represents a result
 * of the visual acuity test performed.
 * It can store which eye was tested and
 * the test result (represented by a ChartLine)
 *
 * @author Katrina Lacsamana
 * @since 02/24/2016
 */
public class VisualAcuityResult
{

    /**
     * Last line read by the user correctly.
     */
    private ChartLine resultLine;

    /**
     * Eye tested.
     */
    private string eye;

    /**
     * Constructor.
     *
     * @param eye        {@link #eye}
     * @param resultLine {@link #resultLine}
     */
    public VisualAcuityResult(string eye, ChartLine resultLine)
    {
        this.eye = eye;
        this.resultLine = resultLine;
    }

    /**
     * Gets {@link #eye}.
     *
     * @return {@link #eye}
     */
    public string getEye()
    {
        if (eye.Equals("Right"))
        {
            return "Right";
        }
        else
        {
            return "Left";
        }
    }

    /**
     * Gets result of the test.
     *
     * @return result.
     */
    public string getVisualAcuity()
    {
        return resultLine.getVisualAcuity();
    }

    /**
     * Gets line number of {@link #resultLine}.
     *
     * @return line number of {@link #resultLine}.
     */
    public int getLineNumber()
    {
        return resultLine.getLineNumber();
    }


}