using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 90f;
    public float velocityLimit = 10f;

    private Rigidbody2D playerRB;
    private Animator playerAnimator;

    private const string STATE_ALIVE = "isAlive";
    private const string STATE_ON_THE_GROUND = "isOnTheGround";


    public LayerMask groundMask;
    public LayerMask ignoreMask;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator.SetBool(STATE_ALIVE, true);
        playerAnimator.SetBool(STATE_ON_THE_GROUND, true);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(this.transform.position, Vector2.down * 1.75f, Color.red);
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Jump();
        }

        playerAnimator.SetBool(STATE_ON_THE_GROUND, IsTouchingTheGround());

    }



    private void FixedUpdate()
    {
        if (Mathf.Abs(playerRB.velocity.x) < velocityLimit && playerAnimator.GetBool(STATE_ALIVE))
        {
            if (Input.GetKey(KeyCode.A))
            {
                //playerRB.velocity = new Vector2(-90f, playerRB.velocity.y);
                playerRB.AddForce(new Vector2(-5f, 0), ForceMode2D.Impulse);
            }

            if (Input.GetKey(KeyCode.D))
            {
                //playerRB.velocity = new Vector2(+90f, playerRB.velocity.y);
                playerRB.AddForce(new Vector2(5f, 0), ForceMode2D.Impulse);
            }
        }            

    }

    void Jump()
    {
        if (IsTouchingTheGround() && playerAnimator.GetBool(STATE_ALIVE))
        {
            playerRB.velocity = Vector2.zero;
            playerRB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        
    }

    bool IsTouchingTheGround()
    {
        if (Physics2D.Raycast(this.transform.position, Vector2.down, 2f, groundMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void KillCharacter()
    {
        playerAnimator.SetBool(STATE_ALIVE, false);
    }

}
