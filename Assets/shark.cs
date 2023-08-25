using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class shark : MonoBehaviour
{
    public GameObject rock;

    public Rigidbody2D body;

    void Start()
    {
    }

    void FixedUpdate()
    {

        Vector3 pos = body.transform.position;
        Vector3 target_pos = rock.transform.position;


        if (pos.y < 0)
        {
            if (Mathf.Abs(target_pos.x - pos.x) > 10 || pos.y > target_pos.y)
            {
                target_pos.y -= 16;
            }
            if (pos.y > target_pos.y)
            {
                target_pos.x += Mathf.Sign(pos.x - target_pos.x) * 16;
            }

            float speed = 1.0f;
            if (pos.x > target_pos.x)
            {
                body.AddForce(new Vector2(-speed, 0), ForceMode2D.Impulse);
            }
            else
            {
                body.AddForce(new Vector2(speed, 0), ForceMode2D.Impulse);
            }

            if (pos.y > target_pos.y)
            {
                body.AddForce(new Vector2(0, -speed), ForceMode2D.Impulse);
            }
            else
            {
                if (Mathf.Abs(rock.transform.position.x - pos.x) <= 10)
                {
                    body.AddForce(new Vector2(0, speed * 4), ForceMode2D.Impulse);
                } else
                {
                    body.AddForce(new Vector2(0, speed), ForceMode2D.Impulse);
                }
            }

            if (body.velocity.y > 4)
            {
                body.AddForce(new Vector2(speed * 0.5f * Mathf.Sign(body.velocity.x), -speed * 0.75f), ForceMode2D.Impulse);
            }
            else if (body.velocity.y < -4)
            {
                body.AddForce(new Vector2(speed * 0.5f * Mathf.Sign(body.velocity.x), speed * 0.75f), ForceMode2D.Impulse);
            }

            if (body.velocity.x > 4)
            {
                body.AddForce(new Vector2(-speed * 0.75f, speed * 0.5f * Mathf.Sign(body.velocity.y)), ForceMode2D.Impulse);
            }
            else if (body.velocity.x < -4)
            {
                body.AddForce(new Vector2(speed * 0.75f, speed * 0.5f * Mathf.Sign(body.velocity.y)), ForceMode2D.Impulse);
            }

            if (Vector3.Distance(pos, rock.transform.position) < 6.25f)
            {
                rock.GetComponent<Rigidbody2D>().AddForce(body.velocity.normalized * 100, ForceMode2D.Impulse);
            }

            if (Mathf.Abs(body.velocity.x) > 32)
            {
                body.velocity = new Vector2(32 * Mathf.Sign(body.velocity.x), body.velocity.y);
            }
            if (Mathf.Abs(body.velocity.y) > 64)
            {
                body.velocity = new Vector2(body.velocity.x, 64 * Mathf.Sign(body.velocity.y));
            }

            if (Vector3.Distance(pos, rock.transform.position) > 32)
            {
                if (body.velocity.x > 32)
                {
                    body.velocity = Vector3.Lerp(body.velocity, body.velocity.normalized * 32, 0.75f);
                }
            }
        }
        else
        {
            body.AddForce(new Vector2(0, -9.81f * 4.0f));
        }

    }
}
