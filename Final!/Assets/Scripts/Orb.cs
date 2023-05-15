using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Orb : MonoBehaviour
{
    Vector2 mouseVelocity;
    public bool isConnected = true;
    public Rigidbody2D rb;
    CircleCollider2D cc;
    Rigidbody2D cameraRb;
    TrailRenderer tr;
    public float orbVelocity = 5;
    public float movementSpeed = 5;
    public float dashSpeed = 2;
    public int remainingPierce = 0;
    public float health = 20;
    float pierceTimer = 0;
    void Start()
    {
        Cursor.visible = false;
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        tr = GetComponent<TrailRenderer>();
        cameraRb = Camera.main.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    private void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pierceTimer -= Time.deltaTime;
        if (remainingPierce > 0 && pierceTimer < 0)
        {
            remainingPierce = 0;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isConnected)
            {
                isConnected = false;
                cc.enabled = true;
                rb.velocity *= 2;
                tr.startColor = Color.red;
                Cursor.visible = true;
                remainingPierce = (int)Mathf.Round(rb.velocity.magnitude / 8) + 1;
                pierceTimer = 1 + (remainingPierce * 0.1f);
            }
            else
            {
                isConnected = true;
                tr.startColor = Color.cyan;
                Cursor.visible = false;
            }
        }
        if (isConnected)
        {
            //transform.position = new Vector3(pos.x, pos.y, 0);

            mouseVelocity = (Vector2)new Vector3(pos.x, pos.y, 0) - (Vector2)transform.position * orbVelocity;
            rb.velocity = mouseVelocity;
        }
        if (!isConnected && remainingPierce > 0)
        {
            cc.isTrigger = true;
        }
        else
        {
            cc.isTrigger = false;
        }
        cameraRb.velocity = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * movementSpeed;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            health -= 4;
        }
    }
}
