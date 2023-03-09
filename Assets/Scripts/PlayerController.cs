using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] float maxSpeed;
    [SerializeField] Transform checkPoint;

    private float horizontalInput;
    private bool isOnGround;
    private bool isInWater;
    private Rigidbody2D playerRb;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
            Jump();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        playerRb.AddForce(horizontalInput * speed * Time.deltaTime * Vector2.right, ForceMode2D.Impulse);

        if(playerRb.velocity.magnitude >= maxSpeed)
        {
            playerRb.velocity = Vector2.ClampMagnitude(playerRb.velocity, maxSpeed);
        }
    }

    void Jump()
    {
        playerRb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
        isOnGround = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
            isInWater = true;

        if (collision.gameObject.CompareTag("Obstacle"))
            GameManager.Instance.CollideWithObstacles();

        if (collision.gameObject.CompareTag("Collectable"))
        {
            Destroy(collision.gameObject);
            GameManager.Instance.Score();
        }

        if (collision.gameObject.CompareTag("Water") && gameObject.CompareTag("Beach Ball"))
        {
            playerRb.gravityScale = -0.3f;
            maxSpeed = 3.0f;
        }

        if (collision.gameObject.CompareTag("Water") && gameObject.CompareTag("Red Ball"))
            maxSpeed = 3.0f;

        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            checkPoint.position = collision.gameObject.transform.position;
        }

        if (collision.gameObject.CompareTag("LevelComplete"))
        {
            gameObject.SetActive(false);
            GameManager.Instance.LevelCompleted();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
            isInWater = false;

        if (collision.gameObject.CompareTag("Water") && gameObject.CompareTag("Beach Ball"))
        {
            playerRb.gravityScale = 0.5f;
            maxSpeed = 3.0f;
        }

        if (collision.gameObject.CompareTag("Water") && gameObject.CompareTag("Red Ball"))
            maxSpeed = 7.5f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;

            if(gameObject.CompareTag("Beach Ball") && !isInWater)
            {
                maxSpeed = 7.5f;
                playerRb.gravityScale = 1.0f;
            }
        }

        if (collision.gameObject.CompareTag("BeachBallToRedBall") && gameObject.CompareTag("Beach Ball"))
            GameManager.Instance.ChangeBall(1, 0);

        if (collision.gameObject.CompareTag("RedBallToBeachBall") && gameObject.CompareTag("Red Ball"))
            GameManager.Instance.ChangeBall(0, 1);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isOnGround = false;
    }
}
