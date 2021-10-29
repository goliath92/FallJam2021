using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCoolDown;
    private float horizontalInput;
    private bool grounded;
    
    
    
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        
        // karakteri, gidilen yöne doğru çevirme
        if(horizontalInput>0.01f)
            transform.localScale = Vector3.one;
        
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);
        
        //animator kontrolü için
        anim.SetBool("Run",horizontalInput!=0);
        anim.SetBool("Grounded",isGrounded());
        anim.SetBool("Wall",onWall());
        
        // Duvardan zıplamak için
        if (wallJumpCoolDown > 0.2f)
        {
            body.velocity = new Vector2(horizontalInput*speed, body.velocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }

            else
                body.gravityScale = 2;
            
            if (Input.GetKey(KeyCode.Space)) 
                Jump();
        }
        else
            wallJumpCoolDown += Time.deltaTime;
    }

    private void Jump()
    {
        // yere temas durumunda
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("Jump");
        }
        
        
        //Duvara değdiğinde ama havada iken
        else if (onWall()&&!isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Math.Sign(transform.localScale.x) * 10, 0); 
                transform.localScale = new Vector3(-Math.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                body.velocity = new Vector2(-Math.Sign(transform.localScale.x) * 3, 6);
            
            wallJumpCoolDown = 0;
            
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
       
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center,boxCollider.bounds.size,0,Vector2.down,0.1f,groundLayer);
        return raycastHit.collider != null;
        
    }
    
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center,boxCollider.bounds.size,0,new Vector2(transform.localScale.x,0),0.1f,wallLayer);
        return raycastHit.collider != null;
    }
}
 