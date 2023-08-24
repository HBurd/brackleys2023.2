using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Silly hack to get animator working with UI images
/// </summary>
public class ImageAnimate : MonoBehaviour
{
    Animator animator;
    UnityEngine.UI.Image image;
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        image = GetComponent<UnityEngine.UI.Image>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        image.sprite = sprite.sprite;
    }
}
