using UnityEngine;

/**
 * The GrossMotorSkill class represents a
 * gross motor skill. It contains the name
 * of the skill, the type of skill,
 * instructions to complete the skill, and
 * the duration for which the skill will
 * be performed.
 *
 * @author Katrina Lacsmaana
 * @since 03/21/2016
 */
public class GrossMotorSkill
{

    /**
     * Name of the skill
     */
    private string skillName;

    /**
     * Skill type
     */
    private string type;

    /**
     * Instructions to do the skill
     */
    private string instruction;

    /**
     * Duration of the skill to be done in milliseconds (1s = 1000ms).
     */
    private int duration;

    /**
     * Image of the skill
     */
    private Sprite[] skillResImage;

    /**
     * Results of the test
     */
    private string assessment;

    /**
     * Whether the skill was already tested or not.
     */
    private bool isTested;

    /**
     * Constructor.
     *
     * @param skillName     {@link #skillName}
     * @param type          {@link #type}
     * @param instruction   {@link #instruction}
     * @param duration      {@link #duration}
     * @param skillResImage {@link #skillResImage}
     */
    public GrossMotorSkill(string skillName, string type, string instruction, int duration, Sprite[] skillResImage)
    {
        this.skillName = skillName;
        this.type = type;
        this.instruction = instruction;
        this.duration = duration;
        this.skillResImage = skillResImage;
        this.isTested = false;
        this.assessment = "No Results";
    }

    /**
     * Gets {@link #skillName}
     *
     * @return {@link #skillName}
     */
    public string getSkillName()
    {
        return skillName;
    }

    /**
     * Gets {@link #type}
     *
     * @return {@link #type}
     */
    public string getType()
    {
        return type;
    }

    /**
     * Gets {@link #instruction}
     *
     * @return {@link #instruction}
     */
    public string getInstruction()
    {
        return instruction;
    }

    /**
     * Gets {@link #duration}
     *
     * @return {@link #duration}
     */
    public int getDuration()
    {
        return duration;
    }

    /**
     * Gets {@link #skillResImage}
     *
     * @return {@link #skillResImage}
     */
    public Sprite[] getSkillResImage()
    {
        return skillResImage;
    }

    /**
     * Gets {@link #isTested)
     *
     * @return {@link #isTested}
     */
    public bool IsTested()
    {
        return isTested;
    }

    /**
     * Sets {@link #isTested)
     */
    public void setTested()
    {
        isTested = true;
    }

    /**
     * Sets {@link #assessment) to pass
     */
    public void setSkillPassed()
    {
        assessment = "Pass";
    }

    /**
     * Sets {@link #isTested) to fail
     */
    public void setSkillFailed()
    {
        assessment = "Fail";
    }

    /**
     * Sets {@link #isTested) to n/a
     */
    public void setSkillSkipped()
    {
        assessment = "N/A";
    }

    /**
     * Gets {@link #assessment}.
     *
     * @return {@link #assessment}
     */
    public string getAssessment()
    {
        return assessment;
    }

}