using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boulder : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        var rigidbody = GetComponent<Rigidbody2D>();

        if (transform.position.y > 0)
        {
            rigidbody.AddForce(Vector2.down * 9.81f * 4.0f);

        } else
        {
            rigidbody.AddForce(Vector2.up * 9.81f * 4.0f);
        }

    }
}
