using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicationArrow : MonoBehaviour
{
    public Vector3 originalPosition;
    public float MaxVertDeviation = 2;
    public bool up = false;
    public float spinSpeed = 10;
    public float moveSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;    
    }

    // Update is called once per frame
    void Update()
    {
        Transform pos = transform;
        pos.Rotate(0, spinSpeed * Time.deltaTime, 0);
        if(up)
        {
            pos.position = new Vector3(pos.position.x, pos.position.y + moveSpeed * Time.deltaTime, pos.position.z);
            transform.position = pos.position;
            if (pos.position.y > originalPosition.y + MaxVertDeviation)
            {
                up = false;
            }
        }  
        else
        {
            pos.position = new Vector3(pos.position.x, pos.position.y - moveSpeed * Time.deltaTime, pos.position.z);
            transform.position = pos.position;
            if (pos.position.y < originalPosition.y - MaxVertDeviation)
            {
                up = true;
            }
        }
    }
}
