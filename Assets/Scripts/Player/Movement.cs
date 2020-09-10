using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public static Movement instance;
    
    public Rigidbody2D _myRb;
    public Animator animator;

    public float speed;
    public float jumpForce;
    private float moveInput;

    private bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;

    private bool canDash = true;
   
    public float dashingTime;
    
    private float startDashTime;

    private float _dashSpeed = 75;

    public int dashDamage;

    public Transform attackPos;
    public float attackRange;
    public LayerMask whatIsEnemies;


    public float attackRangeX;
    public float attackRangeY;

    Vector3 characterScale;
    float characterScaleX;

    public static bool isDashing = false;
    private void Start()
    {
        instance = this;
     //   _myRb.GetComponent<LayerMask>();
        _myRb = GetComponent<Rigidbody2D>();
        dashingTime = startDashTime;

        characterScale = transform.localScale;
        characterScaleX = characterScale.x;
    }

    private void FixedUpdate()
    {



        moveInput = Input.GetAxisRaw("Horizontal");
        _myRb.velocity = new Vector2(moveInput * speed, _myRb.velocity.y);

        if (Input.GetAxis("Horizontal") < 0)
        {
            characterScale.x = -characterScaleX;
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            characterScale.x = characterScaleX;
        }
        transform.localScale = characterScale;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            animator.SetBool("IsJumping", true);
        }

     

        if (Input.GetKey(KeyCode.C) && canDash) // wykonuje sie dash przez sekunde, zmienia dash na falsa, czeka 10 sekund i znowu mozna przez sekunde dashowac       yield return moze dashowac przez 2 sekundy potem zmienia na false i 10 sek czekania
        {
            
            StartCoroutine(WaitForDash());
          
        }

    }

   

    private IEnumerator WaitForDash()
    {
        isDashing = true;
       
        _myRb.gameObject.layer = LayerMask.NameToLayer("Untouchable");
     

        _myRb.velocity = new Vector2(moveInput * _dashSpeed, _myRb.velocity.y);
        yield return new WaitForSeconds(0.1f);
        canDash = false;
        isDashing = false;
        _myRb.gameObject.layer = LayerMask.NameToLayer("Player");
        yield return new WaitForSeconds(10);
        canDash = true;
      
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, new Vector3(attackRangeX, attackRangeY, 1));
    }


    private void Update()
    {
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (isGrounded == true && Input.GetKeyDown(KeyCode.UpArrow))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            _myRb.velocity = Vector2.up * jumpForce;
            animator.SetBool("IsJumping", true);
        }

        if (Input.GetKey(KeyCode.UpArrow) && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                _myRb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
                animator.SetBool("IsJumping", false);
            }
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            isJumping = false;
            animator.SetBool("IsJumping", false);
        }

     
    }

}
