
using System;
using Effect;
using UnityEngine;
using UnityEngine.UI;

public class Toast : MonoBehaviour
{
    private Image image;
    private Text text;
    // private float time = 4f;

    private void Awake()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
    }
    
    public void Show(string msg)
    {
        text.text = msg;
    }

    private void Start()
    {
        var imageColor = image.color;
        imageColor.a = 0;
        var textColor = text.color;
        textColor.a = 0;
        this.CreateEffect().AddEffects(new[]
        {
            image.ColorTo(imageColor, 2f),
            text.ColorTo(textColor,2f)
        }).Play(() => { Destroy(gameObject); });
        // Destroy(gameObject,time);
    }
}
