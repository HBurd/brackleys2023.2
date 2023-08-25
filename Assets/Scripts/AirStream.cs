using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirStream : MonoBehaviour
{
    [SerializeField]
    float forceStrength = 5.0f;  // The strength of the air stream force

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // OnTriggerStay2D is called once per frame for every Collider2D other that is touching the trigger collider
    void OnTriggerStay2D(Collider2D other)
    {
        // Check if the GameObject has a Rigidbody2D component
        if (other.GetComponent<Rigidbody2D>() != null)
        {
            // Apply the force to the GameObject's Rigidbody2D
            other.GetComponent<Rigidbody2D>().AddForce(-forceStrength * transform.right);
        }
    }
}
