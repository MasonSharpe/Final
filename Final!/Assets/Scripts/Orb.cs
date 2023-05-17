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
    GameManager gameManager;
    public LayerMask Connectban;
    public float orbVelocity = 5;
    public float movementSpeed = 5;
    public float dashSpeed = 2;
    public int remainingPierce = 0;
    public float health = 20;
    float pierceTimer = 0;
    public float essence = 6;
    Vector2 freezePosition;
    void Start()
    {
        gameManager = GetComponentInParent<GameManager>();
        Cursor.visible = false;
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        tr = GetComponent<TrailRenderer>();
        cameraRb = Camera.main.GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    private void Update()
    {
        print(cc.IsTouchingLayers(Connectban));
        Vector3 pos = Input.mousePosition;
        pos.z = -Camera.main.transform.position.z;
        pos = Camera.main.ScreenToWorldPoint(pos);
        pierceTimer -= Time.deltaTime;
        gameManager.comboLeft -= Time.deltaTime;
        if (gameManager.comboLeft < 0)
        {
            gameManager.combo = 0;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            freezePosition = transform.position;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (essence > 0)
            {
                gameManager.slowActive = true;
                essence -= Time.deltaTime * 3;
            }
            else
            {
                gameManager.slowActive = false;
                freezePosition = transform.position;
            }
        }
        else
        {
            gameManager.slowActive = false;
            essence = Mathf.Clamp(essence + Time.deltaTime * (1 + gameManager.combo / 20), 0, 6);
        }
        if (remainingPierce > 0 && pierceTimer < 0)
        {
            remainingPierce = 0;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isConnected)
            {
                isConnected = false;
                freezePosition = transform.position;
                cc.enabled = true;
                rb.velocity *= 2;
                tr.startColor = Color.red;
                Cursor.visible = true;
                remainingPierce = (int)Mathf.Round(rb.velocity.magnitude / 8) + 1;
                pierceTimer = 1 + (remainingPierce * 0.1f);
                Physics2D.IgnoreLayerCollision(7, 12, true);

               
            }
            else
            {
                if (!cc.IsTouchingLayers(Connectban))
                {
                    Physics2D.IgnoreLayerCollision(7, 12, false);
                }
                isConnected = true;
                tr.startColor = Color.cyan;
                Cursor.visible = false;
            }
        }
        if (isConnected)
        {

            mouseVelocity = ((Vector2)new Vector3(pos.x, pos.y, 0) - (Vector2)transform.position * orbVelocity) * (gameManager.slowActive ? 0.1f : 1);
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
        if (gameManager.slowActive && !isConnected)
        {
            transform.position = freezePosition;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            health -= 4;
        }
    }
}
