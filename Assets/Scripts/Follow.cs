using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target = null;

    public bool constrain_x = false;
    public bool constrain_y = false;

    public bool follow_rotation = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target_pos = new Vector3(
            constrain_x ? transform.position.x : target.position.x,
            constrain_y ? transform.position.y : target.position.y,
            transform.position.z);
        transform.position = target_pos;

        if (follow_rotation)
        {
            transform.rotation = target.rotation;
        }
    }
}
