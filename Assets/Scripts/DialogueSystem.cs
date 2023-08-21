using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField]
    TMP_Text text;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Open(DialogueSource dialogue)
    {
        gameObject.SetActive(true);
        text.text = dialogue.GetText();

    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
