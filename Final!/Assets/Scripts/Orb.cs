using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Orb : MonoBehaviour
{
    Vector2 mouseVelocity;
    bool isConnected = true;
    Rigidbody2D rb;
    CircleCollider2D cc;
    Vector2 prevPosition1;
    Vector2 prevPosition2;
    Vector2 prevPosition3;
    Vector2 prevPosition4;
    Vector2 prevPosition5;
    public float orbVelocity = 5;
    void Start()
    {
        Cursor.visible = false;
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        prevPosition1 = transform.position;
        prevPosition2 = transform.position;
        prevPosition3 = transform.position;
        prevPosition4 = transform.position;
        prevPosition5 = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        prevPosition1 = prevPosition2;
        prevPosition2 = prevPosition3;
        prevPosition3 = prevPosition4;
        prevPosition4 = prevPosition5;
        prevPosition5 = transform.position;

    }

    private void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isConnected)
            {
                isConnected = false;
                cc.enabled = true;
                rb.velocity = mouseVelocity;
            }
            else
            {
                isConnected = true;
            // cc.enabled = false;
        }
        }
        if (isConnected)
        {
            transform.position = new Vector3(pos.x, pos.y, 0);
            mouseVelocity = ((Vector2)transform.position - prevPosition1) * orbVelocity;
        }
        else
        {

        }
        Camera.main.transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * Time.deltaTime;
    }
}
