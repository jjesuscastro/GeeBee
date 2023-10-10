
using UnityEngine;

/**
* The IshiharaPlate represents a plate
* to be used in the test. The class
* represents the plate through information
* about its shape, its style, and its
* corresponding drawable.
*
* @author Katrina Lacsamana
* @since 03/04/2016
*/
public class IshiharaPlate
{

    /**
     * Shape inside the plate.
     */
    private string shape;

    /**
     * Type of Ishihara plate. Types are depending
     * on the color combination of the plate.
     */
    private int style;

    /**
     * Drawable of the ishihira plate.
     */
    private Sprite ishiharaPlateDrawable;

    /**
     * Flag to show whether it has already been added
     * in the generated test. True if added, false if not.
     */
    private bool isAdded;

    /**
     * Constructor.
     *
     * @param shape                 {@link #shape}
     * @param style                 {@link #style}
     * @param ishiharaPlateDrawable {@link #ishiharaPlateDrawable}
     */
    public IshiharaPlate(string shape, int style, Sprite ishiharaPlateDrawable)
    {
        this.shape = shape;
        this.style = style;
        this.ishiharaPlateDrawable = ishiharaPlateDrawable;
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
     * Gets {@link #style}
     *
     * @return {@link #style}
     */
    public int getStyle()
    {
        return style;
    }

    /**
     * Gets {@link #ishiharaPlateDrawable}
     *
     * @return {@link #ishiharaPlateDrawable}
     */
    public Sprite getIshiharaPlateDrawable()
    {
        return ishiharaPlateDrawable;
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
     * Sets {@link #isAdded} to true
     */
    public void setAdded()
    {
        isAdded = true;
    }
}

