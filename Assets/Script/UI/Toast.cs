
using System;
using UnityEngine;
using UnityEngine.UI;

public class Toast : MonoBehaviour
{
    private Image image;
    private Text text;
    private float time = 4f;

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
        Destroy(gameObject,time);
    }

    private void Update()
    {
        var imageColor = image.color;
        imageColor.a -= Time.deltaTime / time;
        image.color = imageColor;
    }
}
