using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupTween : MonoBehaviour
{
    private void Tween()
    {
        LeanTween.scale(gameObject, Vector3.one, 1f).setEaseSpring();
    }

    private void OnEnable()
    {
        LeanTween.cancel(gameObject);
        Transform transform = gameObject.transform;
        transform.localScale = Vector3.zero;
        Tween();
    }
}
