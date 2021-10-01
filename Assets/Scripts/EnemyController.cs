using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    public bool isGrounded = false;
    public bool isFacingRight = false;
    public Transform batas1;
    public Transform batas2;

    float speed = 2;

    Rigidbody2D rb;
    Animator anim;
    public int HP = 1;
    bool isDie = false;
    public static int enemyKilled = 0;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isGrounded && !isDie)
        {
            if (isFacingRight)
                MoveRight();
            else
                MoveLeft();
            if (transform.position.x >= batas2.position.x && isFacingRight)
                Flip();
            else if (transform.position.x <= batas1.position.x && !isFacingRight)
                Flip();
        }
    }

    void MoveRight()
    {
        Vector3 pos = transform.position;
        pos.x += speed* Time.deltaTime;
        transform.position = pos;
        if (!isFacingRight)
        {
            Flip();
        }
    }
    void MoveLeft()
    {
        Vector3 pos = transform.position;
        pos.x -= speed * Time.deltaTime;
        transform.position = pos;
        if (isFacingRight)
        {
            Flip();
        }
    }
    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        isFacingRight = !isFacingRight;
    }

    void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            isDie = true;
            rb.velocity = Vector2.zero;
            anim.SetBool("isDie", true);
            Destroy(this.gameObject, 2);
            //Data.score += 20;
            enemyKilled++;

            if(enemyKilled == 3)
            {
                SceneManager.LoadScene("Congratulations");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
