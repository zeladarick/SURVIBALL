using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    // Start is called before the first frame update    

    public LayerMask boundMask;
    public LayerMask ignoreMask;
    public int randomSeed;
    private Rigidbody2D ballRB;


    private int bounces;
    private float ballRayRadius = 1.5f;
    private float timeLeft = 0;
    private int actualSecond = 0; 
    private int ballSpeed = 3;

    private void Awake()
    {
        ballRB = GetComponent<Rigidbody2D>();
        Random.InitState(randomSeed);
        bounces = 0;
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(this.transform.position, Vector2.down * ballRayRadius, Color.red);
        Debug.DrawRay(this.transform.position, Vector2.up * ballRayRadius, Color.red);
        Debug.DrawRay(this.transform.position, Vector2.left * ballRayRadius, Color.red);
        Debug.DrawRay(this.transform.position, Vector2.right * ballRayRadius, Color.red);

        if (
            Physics2D.Raycast(this.transform.position, Vector2.down, ballRayRadius, boundMask) ||
            Physics2D.Raycast(this.transform.position, Vector2.up, ballRayRadius, boundMask) ||
            Physics2D.Raycast(this.transform.position, Vector2.right, ballRayRadius, boundMask) ||
            Physics2D.Raycast(this.transform.position, Vector2.left, ballRayRadius, boundMask)
        )
        {
            int forceBound = actualSecond;

            if (forceBound > 25)
            {
                forceBound = 25;
            }

            int rAngle = Random.Range(0, 360);
            int rForce = Random.Range(0, forceBound);

            float xComponent = rForce * Mathf.Sin(rAngle * Mathf.PI/180);
            float yComponent = rForce * Mathf.Cos(rAngle * Mathf.PI / 180);

            ballRB.AddForce(new Vector2(xComponent, yComponent), ForceMode2D.Impulse);

        }

        if (
            (Physics2D.Raycast(this.transform.position, Vector2.down, ballRayRadius, ignoreMask) ||
            Physics2D.Raycast(this.transform.position, Vector2.up, ballRayRadius, ignoreMask) ||
            Physics2D.Raycast(this.transform.position, Vector2.right, ballRayRadius, ignoreMask) ||
            Physics2D.Raycast(this.transform.position, Vector2.left, ballRayRadius, ignoreMask))
        )
        {            
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().KillCharacter();
        }

    }

    private void FixedUpdate()
    {

        timeLeft += Time.deltaTime;
        updateTimer(timeLeft);
    }

    private void updateTimer(float currentTime)
    {
        int newSecond = Mathf.FloorToInt(currentTime % 15);

        if (newSecond != actualSecond)
        {
            actualSecond = newSecond;
        }
    }


    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("COLLISION PLAYER");
            
        }
            
    }*/

}
