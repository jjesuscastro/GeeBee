using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedImage : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private Sprite[] sprites;
    public float timeBetweenSprites = 1;
    public bool isPlaying = true;
    float timer;
    int index = 0;

    public void SetSprites(Sprite[] sprites)
    {
        this.sprites = sprites;
        timer = timeBetweenSprites;
        index = 0;
    }

    private void Update()
    {
        if (!isPlaying)
            return;

        if (sprites != null && sprites.Length > 0 && isPlaying)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                image.sprite = sprites[index];
                timer = timeBetweenSprites;
                index++;

                if (index == sprites.Length)
                    index = 0;
            }
            image.enabled = true;
        }
    }

    public void PlayPause(bool value)
    {
        isPlaying = value;
    }
}
