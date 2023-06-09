using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Orb : MonoBehaviour
{
    Vector2 mouseVelocity;
    public bool isConnected = true;
    public Rigidbody2D rb;
    CircleCollider2D cc;
    CircleCollider2D gcc;
    Rigidbody2D cameraRb;
    public TrailRenderer tr;
    GameManager gameManager;
    public LayerMask Connectban;
    public LayerMask Disconnectban;
    public GameObject cameraMover;
    public float orbVelocity = 10;
    public float movementSpeed = 5;
    public float dashSpeed = 2;
    public int remainingPierce = 0;
    public float health = 20;
    float invincTimer = 0;
    public float pierceTimer = 0;
    public float essence = 8;
    public bool hasControl = true;
    Vector2 freezePosition;
    public AudioClip hitSound;
    public AudioClip deathSound;
    public AudioClip whishSound;
    public AudioClip comboSound;
    public AudioClip checkpointSound;

    private void Awake()
    {
        tr = GetComponent<TrailRenderer>();
    }

    void Start()
    {
       
        gameManager = GetComponentInParent<GameManager>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        gcc = GetComponentInChildren<CircleCollider2D>();
        cameraRb = Camera.main.GetComponentInParent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(7, 12, false);
        Physics2D.IgnoreLayerCollision(7, 16, true);
        gameManager.autoload.sfx.PlayOneShot(hitSound);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    private void Update()
    {
        gameManager.dTime = Time.deltaTime * (gameManager.slowActive ? 0.1f : 0.8f);
        gameManager.autoload.levelTime += gameManager.dTime;
        Vector3 pos = Input.mousePosition;
        pos.z = -Camera.main.transform.position.z;
        pos = Camera.main.ScreenToWorldPoint(pos);
        invincTimer -= gameManager.dTime;
        pierceTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (pierceTimer < 1 && !isConnected)
        {
            tr.enabled = false;
        }
        else
        {
            tr.enabled = true;
        }
        if (gameManager.inRoom)
        {
            gameManager.comboLeft -= gameManager.dTime;
        }
        if (gameManager.comboLeft < 0)
        {
            if (gameManager.combo > 2)
            {
                gameManager.autoload.sfx.PlayOneShot(comboSound, 2);
            }
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
            essence = Mathf.Clamp(essence + Time.deltaTime * (0.5f + gameManager.combo / 20), 0, 8);
        }
        if (remainingPierce > 0 && pierceTimer < 0)
        {
            remainingPierce = 0;
        }
        if ((Input.GetKeyDown(KeyCode.Space) || (Input.GetKey(KeyCode.Space) && !isConnected)) && hasControl)
        {
            
            if (isConnected)
            {
                gameManager.autoload.sfx.PlayOneShot(whishSound, 1.2f);
                isConnected = false;
                freezePosition = transform.position;
                cc.enabled = true;
                rb.velocity *= 2f;
                tr.startColor = Color.red;
                //Cursor.visible = true;
                remainingPierce = (int)Mathf.Round(rb.velocity.magnitude / 8) + 1;
                pierceTimer = 1 + (remainingPierce * 0.2f);
                Physics2D.IgnoreLayerCollision(7, 12, true);
                Physics2D.IgnoreLayerCollision(7, 16, false);

            }
            else if (!isConnected)
            {
                if (pierceTimer <= 1)
                {
                    gameManager.autoload.sfx.PlayOneShot(whishSound, 1.2f);
                    Physics2D.IgnoreLayerCollision(7, 12, false);
                    Physics2D.IgnoreLayerCollision(7, 16, true);
                    isConnected = true;
                    tr.startColor = Color.cyan;
                    // Cursor.visible = false;
                }
            }
        }
        if (isConnected)
        {
            mouseVelocity = ((Vector2)new Vector3(pos.x, pos.y, 0) - (Vector2)transform.position) * (gameManager.slowActive ? 0.1f : 1) * 1.3f;
            rb.velocity = mouseVelocity * 1.1f;
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
            takeDamage(3);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Damaging")
        {
            takeDamage(4);
        }
        if (collision.gameObject.tag == "Health")
        {
            health = 20;
            gameManager.autoload.sfx.PlayOneShot(hitSound);
            gameManager.IncreaseCombo();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Kill Barrier")
        {
            takeDamage(20);
        }
        if (collision.gameObject.tag == "Checkpoint")
        {
            gameManager.autoload.currentCheckpoint = collision.gameObject.transform.position;
            gameManager.autoload.completedRooms = gameManager.completedRooms;
            gameManager.autoload.sfx.PlayOneShot(checkpointSound);
            Destroy(collision.gameObject);
        }
    }

    void takeDamage(float amount)
    {
        if (invincTimer < 0)
        {
            StartCoroutine(Camera.main.gameObject.GetComponent<CameraShake>().Shake(0.2f, 0.2f));
            health -= amount;
            gameManager.comboLeft -= 2;
            invincTimer = 0.25f;
            gameManager.autoload.sfx.PlayOneShot(hitSound);
            if (health <= 0)
            {
                gameManager.autoload.sfx.PlayOneShot(deathSound, 2);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Destructable" && rb.velocity.magnitude > 5 && !isConnected)
        {
            Destroy(collision.gameObject);
            gameManager.comboLeft += 2;
        }
        if (collision.gameObject.tag == "Damaging")
        {
            takeDamage(4);
        }
        if (collision.gameObject.tag == "Kill Barrier")
        {
            takeDamage(20);
        }
    }
}
