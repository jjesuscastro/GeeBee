using UnityEngine;
using UnityEngine.UI;

/**
 * The IshiharaHelper class functions to help
 * generate the test and conduct the test.
 * It contains all possible plates and options
 * that may be used in test generation.
 *
 * @author Katrina Lacsamana.
 * @since 03/04/2016
 */
public class IshiharaHelper
{

    /**
     * Array of all possible Ishihara plates that may be shown to the user.
     */
    private IshiharaPlate[] ishiharaPlates;

    /**
     * Keeps track of the user's score and contains
     * the different Ishihara plates.
     */
    private IshiharaTest ishiharaTest;

    /**
     * List of possible options for the user to choose from.
     */
    private Option[] options;

    /**
     * View where the Ishihara plate is shown.
     */
    private Image plateView;

    /**
     * Serves as the index counter on how many Ishihara plates were shown.
     */
    private int currentPlate;

    /**
     * List of image buttons for the {@link #options}
     */
    private Button[] buttonList;

    /**
     * Serves as the flag whether the test is on going or not.
     */
    private bool isDone;

    /**
     * Constructor.
     * <p>
     * Initializes the {@link #ishiharaPlates}, {@link #options},
     * and the {@link #ishiharaTest}.
     *
     * @param plateView  {@link #plateView}
     * @param buttonList {@link #buttonList}
     */
    public IshiharaHelper(Image plateView, Button[] buttonList, Sprite[] plates, Sprite[] buttonSprites)
    {
        this.plateView = plateView;
        this.buttonList = buttonList;

        ishiharaPlates = new IshiharaPlate[plates.Length];
        for (int i = 0; i < plates.Length; i++)
        {
            string[] spriteName = plates[i].name.Split('_');
            ishiharaPlates[i] = new IshiharaPlate(spriteName[0], int.Parse(spriteName[2]), plates[i]);
        }

        options = new Option[buttonSprites.Length];
        for (int i = 0; i < buttonSprites.Length; i++)
        {
            string[] spriteName = buttonSprites[i].name.Split('_');
            options[i] = new Option(spriteName[2], buttonSprites[i]);
        }

        ishiharaTest = new IshiharaTest(ishiharaPlates, options);
    }

    /**
     * Displays current plate on screen.
     */
    private void displayPlate()
    {
        plateView.sprite = getCurrentPlate().getIshiharaPlateDrawable();
    }

    /**
     * Displays possible answers on buttons on screen.
     */
    private void displayOptions()
    {
        for (int i = 0; i < 5; i++)
        {
            buttonList[i].image.sprite = getCurrentOptions()[i].getOptionDrawable();
        }
    }

    /**
     * Gets current IshiharaPlate
     *
     * @return current IshiharaPlate
     */
    private IshiharaPlate getCurrentPlate()
    {
        return ishiharaTest.getPlate(currentPlate);
    }

    /**
     * Gets possible answers for current IshiharaPlate
     *
     * @return possible answers
     */
    private Option[] getCurrentOptions()
    {
        return ishiharaTest.getOptions(currentPlate);
    }

    /**
     * Resets values and screen for start of test
     */
    public void startTest()
    {
        currentPlate = 0;
        ishiharaTest.generateTest();
        displayPlate();
        displayOptions();
        isDone = false;
    }

    /**
     * Determines course of action for next question as test progresses
     */
    public void goToNextQuestion()
    {
        if (currentPlate < 10)
        {
            currentPlate++;
            displayPlate();
            displayOptions();
        }
        else if (currentPlate == 10)
        {
            ishiharaTest.getScore();
            isDone = true;
        }
    }

    /**
     * Sets the answer of the user for the current plate
     *
     * @param i index of the answer of the user.
     */
    public void answerQuestion(int i)
    {
        ishiharaTest.checkAnswer(currentPlate, i);
    }

    /**
     * Get {@link #isDone()}
     *
     * @return {@link #isDone()}
     */
    public bool IsDone()
    {
        return isDone;
    }

    /**
     * Gets the final test score of user
     *
     * @return score of user.
     */
    public int getScore()
    {
        return ishiharaTest.getScore();
    }

    /**
     * Gets string interpretation of test score
     *
     * @return string interpretation
     */
    public string getResult()
    {
        if (isNormal())
        {
            return "Normal";
        }
        else
        {
            return "Abnormal";
        }
    }

    /**
     * Returns whether the color vision test result
     * of the person is normal or not.
     *
     * @return if higher or equal to 10, return true. Else, false.
     */
    public bool isNormal()
    {
        return getScore() >= 10;
    }

}
