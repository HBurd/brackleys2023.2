using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField]
    TMP_Text text;

    [SerializeField]
    Image image;

    [SerializeField]
    double slide_time = 1.0f;

    double slide_end_time = 0.0f;


    RectTransform image_tf;
    RectTransform panel_tf;

    Vector2 image_offscreen;
    Vector2 panel_offscreen;

    bool activated = false;

    void Update()
    {
        float t = 1.0f - (float)(slide_end_time - Time.timeAsDouble) / (float)slide_time;

        if (activated)
        {
            image_tf.anchoredPosition = Vector2.Lerp(image_offscreen, Vector2.zero, t);
            panel_tf.anchoredPosition = Vector2.Lerp(panel_offscreen, Vector2.zero, t);
        }
        else
        {
            image_tf.anchoredPosition = Vector2.Lerp(Vector2.zero, image_offscreen, t);
            panel_tf.anchoredPosition = Vector2.Lerp(Vector2.zero, panel_offscreen, t);

            if (t > 1.0f)
            {
                gameObject.SetActive(false);
                image.gameObject.SetActive(false);
            }
        }
    }

    public void Open(string new_text, Sprite sprite)
    {
        gameObject.SetActive(true);
        image.gameObject.SetActive(true);

        image.sprite = sprite;
        text.text = new_text;

        slide_end_time = Time.timeAsDouble + slide_time;

        image_tf = image.GetComponent<RectTransform>();
        image_offscreen = 500.0f * Vector2.left;

        panel_tf = GetComponent<RectTransform>();
        panel_offscreen = 300.0f * Vector2.down;

        Debug.Log("open");

        image_tf.anchoredPosition = image_offscreen;
        panel_tf.anchoredPosition = panel_offscreen;

        activated = true;
    }

    public void Close()
    {
        Debug.Log("close");
        activated = false;
        slide_end_time = Time.timeAsDouble + slide_time;
    }
}
