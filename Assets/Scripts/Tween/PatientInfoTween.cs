using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientInfoTween : MonoBehaviour
{
    private Vector2 endPosition;
    private void Tween()
    {
        LeanTween.move(gameObject.GetComponent<RectTransform>(), endPosition, .15f);
    }

    private void OnEnable()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        endPosition = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = new Vector2(225, 0);
        LeanTween.cancel(gameObject);
        Tween();
    }
}
