using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    bool isJump = false;
    bool isDead = false;
    int idMove = 0;
    Animator anim;
    Rigidbody2D rb;
    public float jumpForce = 10;
    public GameObject Projectile; // object peluru
    public Vector2 projectileVelocity; // kecepatan peluru
    public Vector2 projectileOffset; // jarak posisi peluru dari posisi player
    public float cooldown = 0.5f; // jeda waktu untuk menembak
    bool isCanShoot = true; // memastikan untuk kapan dapat menembak
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        EnemyController.EnemyKilled = 0;
        //isCanShoot = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) // Menggunakan A untuk gerakan kiri
        {
            MoveLeft();
        }

        if (Input.GetKeyDown(KeyCode.D)) // Menggunakan D untuk gerakan kanan
        {
            MoveRight();
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) // Menggunakan W atau Spasi untuk loncat
        {
            Jump();
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            Idle();
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            Idle();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
        Move();
        Dead();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Kondisi ketika menyentuh tanah
        if (isJump)
        {
            anim.ResetTrigger("jump");
            if (idMove == 0) anim.SetTrigger("idle");
            isJump = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Kondisi ketika di udara
        anim.SetTrigger("jump");
        anim.ResetTrigger("run");
        anim.ResetTrigger("idle");
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
            // Kondisi ketika bergerak ke kanan
            if (!isJump && !anim.GetCurrentAnimatorStateInfo(0).IsName("run")) anim.SetTrigger("run");
            transform.Translate(1 * Time.deltaTime * 5f, 0, 0);
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        if (idMove == 2 && !isDead)
        {
            // Kondisi ketika bergerak ke kiri
            if (!isJump && !anim.GetCurrentAnimatorStateInfo(0).IsName("run")) anim.SetTrigger("run");
            transform.Translate(-1 * Time.deltaTime * 5f, 0, 0);
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void Jump()
    {
        if (!isJump && !isDead)
        {
            // Kondisi ketika loncat
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJump = true;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*if (collision.transform.tag.Equals("Peluru"))
        {
            isCanShoot = true;
        }*/

        if (collision.transform.tag.Equals("Enemy"))
        {
            SceneManager.LoadScene("Lvl_1");
            /*isDead = true;*/
        }
    }

    public void Idle()
    {
        // Kondisi ketika idle/diam
        if (!isJump)
        {
            anim.ResetTrigger("jump");
            anim.ResetTrigger("run");
            anim.SetTrigger("idle");
        }
        idMove = 0;
    }

    private void Dead()
    {
        if (!isDead)
        {
            if (transform.position.y < -10f)
            {
                isDead = true;
                SceneManager.LoadScene("Lvl_1");
            }
        }
    }

    public void Fire()
    {
        if (isCanShoot)
        {
            //Membuat projectile baru
            GameObject bullet = Instantiate(Projectile, (Vector2)transform.position - projectileOffset * transform.localScale.x, Quaternion.identity);

            // mengatur kecepatan dari projectile
            Vector2 velocity = new Vector2(projectileVelocity.x * transform.localScale.x, projectileVelocity.y);
            bullet.GetComponent<Rigidbody2D>().velocity = velocity * -1;

            //Menyesuaikan scale dari projectile dengan scale karakter
            Vector3 scale = transform.localScale;
            bullet.transform.localScale = scale * -1;

            StartCoroutine(CanShoot());
            anim.SetTrigger("shoot");
        }
    }

    IEnumerator CanShoot()
    {
        anim.SetTrigger("shoot");
        isCanShoot = false;
        yield return new WaitForSeconds(cooldown);
        isCanShoot = true;
    }
}
