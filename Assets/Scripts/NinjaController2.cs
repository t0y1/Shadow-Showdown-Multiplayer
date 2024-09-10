using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaController2 : MonoBehaviour
{
    public float moveSpeed ;
    public float jumpForce ;
    public float move;
    public float movey;

    private Rigidbody2D rb;
    [SerializeField] Animator anim;
    public BoxCollider2D parar;
    public BoxCollider2D agachar;

    public GameObject pared;
    public Transform paredT;

    private bool isGrounded = false;
    private bool isTouchingWall = false;
    bool isCrouching;
    public bool isHoldingWeapon = false;
    public bool derecha = true;
    public string weaponName;

    public float kita;
    public float kitaJ;
    public float saltoDoble;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        kita = moveSpeed;
        kitaJ = jumpForce;
        saltoDoble = kitaJ * 2;
    }

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Jump();
        Crouch();
        CheckHoldingWeapon();
        WallSlide();

        if (isTouchingWall && Input.GetKey(KeyCode.A))
        {
            jumpForce = saltoDoble;
        }
        else
        {
            jumpForce = kitaJ;
        }
    }

    void Move()
    {
        move = Input.GetAxisRaw("Horizontal2");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
        if (rb.velocity.x > 0)
        {
            derecha = true;
            GetComponent<SpriteRenderer>().flipX = false;
            transform.rotation = Quaternion.Euler(0, 0, 0); // Rotación normal
            anim.SetBool("Run", true);
        }
        else if (rb.velocity.x < 0)
        {
            derecha = false;
            GetComponent<SpriteRenderer>().flipX = false;
            transform.rotation = Quaternion.Euler(0, 180, 0);
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }

        if ((Input.GetKey(KeyCode.RightArrow) && transform.position.x < paredT.transform.position.x && transform.rotation.y == 0 && isTouchingWall) ||
                (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > paredT.transform.position.x && transform.rotation.y < 100 && isTouchingWall))
        {
            moveSpeed = 0;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 1.5f);
            anim.SetBool("IsWallSliding", true);

        }
        else
        {
            moveSpeed = kita;
            anim.SetBool("IsWallSliding", false);

        }

    }

    void Jump()
    {
        movey = Input.GetAxisRaw("Vertical2");
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isCrouching && (isTouchingWall || isGrounded))
        {
            anim.SetBool("IsPunching", false);
            anim.SetBool("IsJumping", true);
            isGrounded = false;
            rb.velocity = new Vector2(rb.velocity.x, movey * jumpForce);
        }
    }

    void Crouch()
    {
        isCrouching = Input.GetKey(KeyCode.DownArrow);
        if (isCrouching)
        {
            anim.SetBool("IsPunching", false);
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsCrouching", isCrouching);
            rb.velocity = new Vector2(0, -10f);
            agachar.enabled = true;
            parar.enabled = false;
        }
        else
        {
            anim.SetBool("IsCrouching", false);
            agachar.enabled = false;
            parar.enabled = true;
        }
    }

    void WallSlide()
    {
        if (isTouchingWall == true)
        {
        }

        else
        {
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            anim.SetBool("IsJumping", false);
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            anim.SetBool("IsJumping", false);
            isTouchingWall = true;
            pared = collision.gameObject;
            paredT = pared.GetComponent<Transform>();
        }

        if (collision.gameObject.CompareTag("Weapon") && Input.GetKey(KeyCode.DownArrow))
        {
            string newWeaponName = collision.gameObject.name.Replace("(Clone)", "2").Trim();

            if (isHoldingWeapon)
            {
                // Cambiar el arma actual por la nueva
                anim.SetBool("IsHolding" + weaponName, false);
            }

            // Agarrar la nueva arma
            weaponName = newWeaponName;
            collision.gameObject.transform.position = new Vector2(100, 0);  // Mover el arma agarrada fuera de la pantalla
            isHoldingWeapon = true;
            anim.SetBool("IsHolding" + weaponName, true);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = false;
            anim.SetBool("IsWallSliding", false);
        }
    }

    void CheckHoldingWeapon()
    {
        if (isHoldingWeapon)
        {
            anim.SetBool("IsPunching", false);
        }
    }
}
