using UnityEngine;

/**
 * The Option class represents an option
 * from the multiple choice menu that
 * the user chooses answers from. The class
 * contains the shape of the Option, its
 * corresponding drawable resource.
 *
 * @author Katrina Lacsmana
 * @since 03/04/2016
 */
public class Option
{

    /**
     * Shape inside the option.
     */
    private string shape;

    /**
     * Drawable of the option with the specified shape.
     */
    private Sprite optionDrawable;

    /**
     * Flag to show whether the option has been
     * added before.
     */
    private bool isAdded;

    /**
     * Constructor.
     *
     * @param shape          {@link #shape}
     * @param optionDrawable {@link #optionDrawable}
     */
    public Option(string shape, Sprite optionDrawable)
    {
        this.shape = shape;
        this.optionDrawable = optionDrawable;
        this.isAdded = false;
    }

    /**
     * Gets {@link #shape}
     *
     * @return {@link #shape}
     */
    public string getShape()
    {
        return shape;
    }

    /**
     * Gets {@link #isAdded}
     *
     * @return {@link #isAdded}
     */
    public bool IsAdded()
    {
        return isAdded;
    }

    /**
     * Gets {@link #optionDrawable}
     *
     * @return {@link #optionDrawable}
     */
    public Sprite getOptionDrawable()
    {
        return optionDrawable;
    }

    /**
     * Sets {@link #isAdded} to true.
     */
    public void setAdded()
    {
        isAdded = true;
    }

    /**
     * Sets {@link #isAdded} to false.
     */
    public void resetAdded()
    {
        isAdded = false;
    }

}
