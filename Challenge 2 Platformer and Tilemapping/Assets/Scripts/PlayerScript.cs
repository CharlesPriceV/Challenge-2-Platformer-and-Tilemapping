using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public float speed;
    public bool faceRight = true;
   
    public Text countText;
    public Text winText;
    public Text livesText;

    private Rigidbody2D rb2d;
    private int count;
    private int lives;

    public AudioSource musicSource;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;

    Animator anim;

    void Start()
    {
        musicSource.clip = musicClipOne;
        musicSource.Play();
        musicSource.loop = true;
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        count = 0;
        winText.text = "";
        lives = 3;
        SetCountText();
        SetLivesText();
    }

    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        if (hozMovement > 0 && !faceRight)
        {
            Flip();
        }
        else if (hozMovement < 0 && faceRight)
        {
            Flip();
        }
        float vertMovement = Input.GetAxis("Vertical");
            
        rb2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }

        else if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
            lives = lives - 1;

            if (count >= 8)
            {
                musicSource.clip = musicClipTwo;
                musicSource.Play();
                winText.text = "You win! Game created by Charles Price!";
            }

            else
            {
                SetLivesText();
            }
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            anim.SetInteger("State", 0);

            if (Input.GetKey(KeyCode.W))
            {
                anim.SetInteger("State", 2);
                rb2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }

            if (Input.GetKey(KeyCode.D))
            {
                anim.SetInteger("State", 1);

                if (Input.GetKey(KeyCode.W))
                {
                    anim.SetInteger("State", 2);
                }
            }

            if (Input.GetKey(KeyCode.A))
            {
                anim.SetInteger("State", 1);

                if (Input.GetKey(KeyCode.W))
                {
                    anim.SetInteger("State", 2);
                }
            }
        }
    }

    void Flip()
    {
        faceRight = !faceRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count == 4)
        {
            transform.position = new Vector2(50.0f, 50.0f);
            lives = 3;
            SetLivesText();
        }

        if (count >= 8)
        {
            musicSource.clip = musicClipTwo;
            musicSource.Play();
            winText.text = "You win! Game created by Charles Price!";
        }
    }

    void SetLivesText()
    {
        livesText.text = "Lives: " + lives.ToString();
        if (lives == 0)
        {
            winText.text = "You Lose! Game created by Charles Price!";
            Destroy(gameObject);
        }
    }
}
