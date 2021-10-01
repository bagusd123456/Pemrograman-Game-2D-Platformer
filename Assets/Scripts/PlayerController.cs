using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    bool isJump = true;
    bool isDead = false;

    int idMove = 0;
    Animator anim;

    public GameObject Projectile;
    public Vector2 projectileVelocity;
    public Vector2 projectileOffset;
    public float cooldown = 0.5f;
    bool isCanShoot = true;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        isCanShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            Idle();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            Idle();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Fire();
        }
        Move();
        Dead();
    }

    void Fire()
    {
        // jika karakter dapat menembak
        if (isCanShoot)
        {
            //Membuat projectile baru
            GameObject bullet = Instantiate(Projectile, (Vector2)transform.position - projectileOffset * transform.localScale.x, Quaternion.identity);

            // mengatur kecepatan dari projectile
            Vector2 velocity = new Vector2(projectileVelocity.x * transform.localScale.x, projectileVelocity.y);
            bullet.GetComponent<Rigidbody2D>().velocity = velocity * -1;

            //Menyesuaikan scale dari projectile dengan scale karakter
            Vector3 scale = transform.localScale;
            bullet.transform.localScale = scale * - 1;

            StartCoroutine(CanShoot());
            anim.SetTrigger("isShoot");
        }
    }

    IEnumerator CanShoot()
    {
        anim.SetTrigger("isShoot");
        isCanShoot = false;
        yield return new WaitForSeconds(cooldown);
        isCanShoot = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("Peluru"))
        {
            isCanShoot = true;
        }
        if (collision.transform.tag.Equals("Enemy"))
        {
            SceneManager.LoadScene("Game Over");
            isDead = true;
        }
    }

private void OnCollisionStay2D(Collision2D collision)
    {
        if (isJump)
        {
            anim.ResetTrigger("isJump");
            if (idMove == 0) anim.SetTrigger("isIdle");
            isJump = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        anim.SetTrigger("isJump");
        anim.ResetTrigger("isRun");
        anim.ResetTrigger("isIdle");
        isJump = true;
    }

    public void MoveRight()
    {
        idMove = 1;
    }

    public void MoveLeft()
    {
        idMove = 2;
    }

    private void Move()
    {
        if (idMove == 1 && !isDead)
        {
            if (!isJump) anim.SetTrigger("isRun");
            transform.Translate(1 * Time.deltaTime * 5f, 0, 0);
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        if (idMove == 2 && !isDead)
        {
            if (!isJump) anim.SetTrigger("isRun");
            transform.Translate(-1 * Time.deltaTime * 5f, 0, 0);
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void Jump()
    {
        if (!isJump)
        {
            // Kondisi ketika Loncat
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 300f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag.Equals("Coin"))
        {
            Data.score += 15;
            Destroy(collision.gameObject);
        }
    }

    public void Idle()
    {
        // kondisi ketika idle/diam
        if (!isJump)
        {
            anim.ResetTrigger("isJump");
            anim.ResetTrigger("isRun");
            anim.SetTrigger("isIdle");
        }
        idMove = 0;
    }

    private void Dead()
    {
        if (!isDead)
        {
            if (transform.position.y < -10f)
            {
                // kondisi ketika jatuh
                isDead = true;
            }
        }
    }
}
