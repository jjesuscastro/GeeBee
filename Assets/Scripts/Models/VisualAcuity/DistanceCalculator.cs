using UnityEngine;
using UnityEngine.UI;

/**
 * The DistanceCalculator class determines
 * the size of the visual acuity chart to be used (maximum size depending on tablet)
 * and the required distance the user must be to perform the test.
 *
 * @author Katrina Lacsmana
 * @since 02/23/2016.
 */
public class DistanceCalculator
{

    /**
     * Height of the displayed chart.
     */
    private int displayHeight;

    /**
     * Sets {@link #displayHeight}.
     *
     * @param imageView to show the chart.
     */
    private void getDisplaySize(Image imageView)
    {
        displayHeight = (int)imageView.rectTransform.rect.height;
    }

    /**
     * Gets how far the user should be from the tablet while taking the test.
     *
     * @param context   current context
     * @param imageView to show the chart
     * @return distance of the user from the tablet.
     */
    public float getUserDistance(Image imageView)
    {
        getDisplaySize(imageView);

        float height = convertPixelsToMillimeter(displayHeight, Screen.dpi);
        float distanceMeters = (height / 88) * 6;
        return distanceMeters;
    }

    /**
     * Convert pixels to millimeter
     *
     * @param pixels to be converted.
     * @param dpi    value of dpi of the screen.
     * @return millimeter value.
     */
    private float convertPixelsToMillimeter(int pixels, float dpi)
    {
        return pixels / dpi * 25.4f;
    }

    /**
     * Convert meters to feet
     *
     * @param meters distance
     * @return distance in feet.
     */
    private float convertMetersToFeet(float meters)
    {
        return meters * 3.28084f;
    }

}
