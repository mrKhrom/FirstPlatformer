using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header ("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header ("Coyote Time")]
    [SerializeField] private float coyoteTime; //Time after leaving platform where player can still jump
    private float coyoteCounter; //How much time passed since player left platform

    [Header ("Multijump")]
    [SerializeField] private int extraJumps; //How many extra jumps player has
    private int jumpCounter; //How many jumps player has done since last time on the ground
    
    [Header ("Walljump")]
    [SerializeField] private int wallJumpX; //Horizontal wall jump power
    [SerializeField] private int wallJumpY; //Vertical wall jump power

    [Header ("Collision")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header ("SFX")]
    [SerializeField] private AudioClip jumpSound;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        //Flip player when moving left-right
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        //Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        //jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();        
        }

        //Adjustable jump height
        if (Input.GetKeyUp(KeyCode.Space) && body.linearVelocity.y > 0)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, body.linearVelocity.y / 2f);
        }

        if (onWall())
        {
            body.gravityScale = 0;
            body.linearVelocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 7;
            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

            if (isGrounded())
            {
                coyoteCounter = coyoteTime; //Reset coyote counter when on the ground
                jumpCounter = extraJumps; //Reset jump counter when on the ground
            }
            else
                coyoteCounter -= Time.deltaTime; //Start decfeasing coyote counter when not on the ground
        }
    }

    private void Jump()
    {
        if(coyoteCounter < 0 && !onWall() && jumpCounter <= 0)
            return;
        SoundManager.instance.PlaySound(jumpSound);
        
        if (onWall())
            WallJump();
        else
        {
            if (isGrounded())
                body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            else
            {
                //If not on the ground and coyote counter bigger than 0 do a normal jump
                if(coyoteCounter > 0)
                    body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
                else
                {
                    //If not on the ground and coyote counter smaller than 0 do a double jump
                    if (jumpCounter > 0)
                    {
                        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
                        jumpCounter--;
                    }
                }
            }
            coyoteCounter = 0; //Reset coyote to avoid double jump
        }
    }

    private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));    
    }


    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}