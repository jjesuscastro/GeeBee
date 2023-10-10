using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FineMotor : MonitoringTest
{
    [SerializeField]
    private GameObject cursor;
    [SerializeField]
    private Image imageViewPathToTrace;
    [SerializeField]
    private AudioSource mp;
    [SerializeField]
    private GameObject answerButtons;
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private AudioClip outsidePath;
    [SerializeField]
    private Sprite[] pathsSprites;
    private Camera mainCamera;

    /**
     * Color of the starting point of the path.
     * If path image color is updated, this must also
     * be updated.
     */
    private Color START_COLOR = new Color(0.03529412f, 0.7372549f, 0.8313726f);

    /**
     * Color of the end point of the path.
     * If path image color is updated, this must also
     * be updated.
     */
    private Color END_COLOR = new Color(0.9058824f, 0.1176471f, 0.3882353f);

    /**
     * Keeps track of which test the user is currently taking.
     */
    private int currentTest;  //0 for non dominant, dominant, ask assistance

    /**
         * Serves as the flag whether the test is on going or not.
         */
    private bool isTestOngoing = true;

    /**
     * Serves as the flag whether the test has started or not
     */
    private bool hasStarted = false;

    /**
     * Contains the helper class of the activity
     */
    private FineMotorHelper fineMotorHelper;

    private Popup popup;
    private Record record;

    private void Start()
    {
        mainCamera = Camera.main;
        popup = Popup.instance;
        record = Record.instance;
        intro = "Now, let's do this fun activity.";
        currentTest = 0;
        fineMotorHelper = new FineMotorHelper(imageViewPathToTrace, mp, outsidePath, pathsSprites);
        Popup.instance.OpenPopup(fineMotorHelper.setInstructions(0), PopupType.CONFIRMATION);
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Sprite sprite = imageViewPathToTrace.sprite;
            Touch touch = Input.GetTouch(0);
            Color pixel = GetPixel(touch, sprite);
            cursor.SetActive(true);
            // Debug.Log(pixel.r + " " + pixel.g + " " + pixel.b);

            if (hasStarted)
            { //if user have pressed start_color
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        break;
                    case TouchPhase.Ended: //go back to start if finger is lifted
                        hasStarted = false;
                        fineMotorHelper.doIfTouchIsUp();
                        break;
                    case TouchPhase.Moved:
                        cursor.transform.position = touch.position;
                        if (pixel == END_COLOR)
                        { //if user done
                            hasStarted = false;
                            Debug.Log("Complete");

                            switch (currentTest)
                            {
                                case 0:
                                    popup.OpenPopup(fineMotorHelper.doNextTest(currentTest++), PopupType.CONFIRMATION);
                                    break;
                                case 1:
                                    popup.YesButton().AddListener(delegate { Answer(true); });
                                    popup.NoButton().AddListener(delegate { Answer(false); });
                                    popup.OpenPopup(fineMotorHelper.doNextTest(currentTest++), PopupType.YESNO);
                                    break;
                                case 2: break;
                            }

                        }
                        else if (pixel.a == 0)
                        { //if touch is outside path
                            Debug.Log("Outside");
                            fineMotorHelper.doIfOutSideThePath();
                        }
                        else
                        {
                            fineMotorHelper.doIfWithinPath();
                        }
                        break;
                }
            }
            else
            {
                if (pixel == START_COLOR && (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved))
                {
                    hasStarted = true;
                    Debug.Log("true");
                }
            }
        }
        else
        {
            cursor.SetActive(false);
        }
    }

    public void SkipTest()
    {
        hasStarted = false;
        Debug.Log("Complete");

        switch (currentTest)
        {
            case 0:
                popup.OpenPopup(fineMotorHelper.doNextTest(currentTest++), PopupType.CONFIRMATION);
                break;
            case 1:
                popup.YesButton().AddListener(delegate { Answer(true); });
                popup.NoButton().AddListener(delegate { Answer(false); });
                popup.OpenPopup(fineMotorHelper.doNextTest(currentTest++), PopupType.YESNO);
                break;
            case 2: break;
        }
    }

    private Color GetPixel(Touch touch, Sprite sprite)
    {
        Color pixel = Color.white;
        Vector2 localPoint = Vector2.zero;
        Vector3 screenPoint;

        screenPoint = mainCamera.WorldToScreenPoint(touch.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, mainCamera, out localPoint);

        Vector3[] localCorners = new Vector3[4];
        rectTransform.GetLocalCorners(localCorners);
        float imgWidth = localCorners[3].x - localCorners[0].x;
        float imgHeight = localCorners[1].y - localCorners[0].y;

        Vector2 positionNormalizedForTexCoords;
        positionNormalizedForTexCoords.x = localPoint.x / imgWidth + 0.5f;
        positionNormalizedForTexCoords.y = localPoint.y / imgHeight + 0.5f;

        if (positionNormalizedForTexCoords.x >= 0 && positionNormalizedForTexCoords.x <= 1 &&
            positionNormalizedForTexCoords.y >= 0 && positionNormalizedForTexCoords.y <= 1)
        {
            pixel = sprite.texture.GetPixelBilinear(positionNormalizedForTexCoords.x, positionNormalizedForTexCoords.y);
        }

        return pixel;
    }

    /**
     * Sends results to the activity this fragment is attached to.
     */
    private void sendResults()
    {
        string resultstring;
        bool[] result = fineMotorHelper.getResults();

        if (isTestOngoing)
        { //this is to avoid double clicking
            isTestOngoing = false;
            resultstring = "Non dominant hand: " + result[0] +
                    "\nDominant hand: " + result[1] +
                    "\nUsing pen: " + result[2];

            imageViewPathToTrace.sprite = null;

            if (record != null)
            {
                record.fineMotorNDominant = result[0] ? 0 : 1;
                record.fineMotorDominant = result[1] ? 0 : 1;
                record.fineMotorHold = result[2] ? 0 : 1;
            }
        }

        updateTestEndRemark(fineMotorHelper.getResults());
    }

    /**
     * Updates the end test attributes of the test fragment namely
     * {@link #isEndEmotionHappy}, {@link #endstringResource},
     * and {@link #endTime} depending on the result of the test.
     *
     * @param results list of results from the different rounds of the finemotor test.
     * @see MonitoringTestFragment
     */
    private void updateTestEndRemark(bool[] results)
    {
        int numPass = 0;

        foreach (bool result in results)
        {
            if (result)
            {
                numPass++;
            }
        }

        if (numPass < 2)
        {
            isEndEmotionHappy = false;
            endStringResource = "You may need to work on your fine motor skills!";
            endTime = 4000;
        }
        else
        {
            isEndEmotionHappy = true;
            endStringResource = "Your fine motor skills are great!";
            endTime = 3000;
        }

        popup.OkayButton().AddListener(onTestComplete.Invoke);
        popup.OpenPopup(endStringResource, PopupType.CONFIRMATION);
    }

    public void Answer(bool answer)
    {
        fineMotorHelper.setResult(2, answer);
        sendResults();
    }
}
