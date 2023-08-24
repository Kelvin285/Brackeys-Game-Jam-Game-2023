using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiverController : MonoBehaviour
{
    public GameObject camera;
    public GameObject left_leg;
    public GameObject right_leg;

    private Vector2 look = new Vector2(0, 1);
    private Vector2 dir = new Vector2(0, 1);
    private float movement = 0;
    private float momentum = 0;
    private bool pressed = false;
    private void FixedUpdate()
    {
        var rigidbody = GetComponent<Rigidbody2D>();

        

        if (transform.position.y > 0)
        {
            rigidbody.AddForce(Vector2.down * 9.81f * 4.0f);

            dir = rigidbody.velocity;
        } else
        {
            if (rigidbody.velocity.magnitude > 8)
            {
                rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, rigidbody.velocity.normalized * 8, 0.75f);
            }
            rigidbody.AddForce(Vector2.up * 9.81f * 0.5f);

            if (Input.GetKey(Settings.DOWN) || Input.GetKey(Settings.UP) || Input.GetKey(Settings.RIGHT) || Input.GetKey(Settings.LEFT))
            {
                if (!pressed)
                {
                    pressed = true;
                    dir *= 0;
                    momentum = 2.0f;
                }
            }
            else
            {
                pressed = false;
            }

            if (Input.GetKey(Settings.DOWN))
            {
                dir += Vector2.down;
            }
            if (Input.GetKey(Settings.UP))
            {
                dir += Vector2.up;
            }
            if (Input.GetKey(Settings.LEFT))
            {
                dir += Vector2.left;
            }
            if (Input.GetKey(Settings.RIGHT))
            {
                dir += Vector2.right;
            }
            if (momentum > 0)
            {
                movement += Time.fixedDeltaTime * 16.0f * momentum;
                rigidbody.AddForce(look.normalized * 4.0f * momentum, ForceMode2D.Impulse);
                momentum -= Time.fixedDeltaTime;
            }
        }
        look = Vector2.Lerp(look, dir, 1.0f / 8.0f);


        


        float rot = 0;
        if (rigidbody.velocity.magnitude > 0)
        {
            rot = Mathf.Rad2Deg * Mathf.Atan2(look.y, look.x) - 90;
        }
        transform.rotation = Quaternion.Euler(0, 0, rot);
        camera.transform.position = Vector3.Lerp(camera.transform.position, transform.position - new Vector3(0, 0, 10), 1.0f / 4.0f);
        left_leg.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(movement) * 45.0f);
        right_leg.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(-movement) * 45.0f);
    }
}
