using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePatientTween : MonoBehaviour
{
    [SerializeField]
    private Button savePatient;

    [SerializeField]
    private Color startColor, endColor;

    public void Tween()
    {
        LeanTween.value(gameObject, 0, 1, 1.5f)
        .setEasePunch()
        .setOnUpdate((value) =>
        {
            savePatient.image.color = Color.Lerp(startColor, endColor, value);
        });
    }
}
