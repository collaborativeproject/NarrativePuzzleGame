using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    HumanIdle,
    HumanMoving,
    LandAnimalMoving,
    WaterAnimalMoving,
    AirAnimalMoving,
}

public class PlayerController : MonoBehaviour
{
    //Common public Variables
    public PlayerState playerState = PlayerState.HumanIdle;

    //Human variables
    public float humanSpeed;
    public float humanJumpForce;
    public int humanExtraJumps;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    public float humanGravityScale;

    //Private human variables
    private float moveInput;
    private bool facingRight = true;
    private bool isGrounded;
    private int extraJumps;

    //Water animal variables
    public float waterSpeed;
    public float waterAnimalGravityScale;

    //Private animal variables
    private float waterMoveInputX;
    private float waterMoveInputY;

    //Common private variables
    private Rigidbody2D myRB;

    void Start()
    {
        //Getting reference to the rigidbody
        myRB = GetComponent<Rigidbody2D>();

        //Setting human jumps
        extraJumps = humanExtraJumps;

        //making the gravity scale always start as the human
        myRB.gravityScale = humanGravityScale;
    }

    void Update()
    {
        switch (playerState)
        {
            //This enters the player into the human idle state
            case PlayerState.HumanIdle:
            HumanIdle();
            break;

            //This enters the player into the human moving state
            case PlayerState.HumanMoving:
            HumanMoving();
            break;

            //This enters the player into the land animal moving state
            case PlayerState.LandAnimalMoving:
            LandAnimalsMoving();
            break;

            //This enters the player into the water animal moving state
            case PlayerState.WaterAnimalMoving:
            WaterAnimalMoving();
            break;

            //This enters the player into the air animal moving state
            case PlayerState.AirAnimalMoving:
            AirAnimalMoving();
            break;
        }
    }

    //All human idle actions
    void HumanIdle()
    {

    }

    //All human moving actions
    void HumanMoving()
    {
        //Setting human gravity scale
        myRB.gravityScale = humanGravityScale;

        //Setting isGrounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        //Setting the input axis
        moveInput = Input.GetAxisRaw("Horizontal");
        //Applying velocity to the character
        myRB.velocity = new Vector2(moveInput * humanSpeed, myRB.velocity.y);

        //If the player is moving right
        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        //If thge player is moving left
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }

        //Jumping inputs
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) && extraJumps > 0)
        {
            //Player jump
            myRB.velocity = Vector2.up * humanJumpForce;

            //Subtracting extra jumps
            extraJumps--;
        }
        //If you have run out
        else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) && extraJumps == 0 && isGrounded)
        {
            //Player jump
            myRB.velocity = Vector2.up * humanJumpForce;
        }

        //Resetting jump value
        if (isGrounded)
        {
            extraJumps = humanExtraJumps;
        }
    }

    //All land animal actions
    void LandAnimalsMoving()
    {
        
    }

    //All water animal actions
    void WaterAnimalMoving()
    {
        //Setting the gravity scale
        myRB.gravityScale = waterAnimalGravityScale;

        //Setting the input axis
        waterMoveInputX = Input.GetAxisRaw("Horizontal");
        waterMoveInputY = Input.GetAxisRaw("Vertical");
        //Applying velocity to the character
        myRB.velocity = new Vector2(waterMoveInputX * waterSpeed, waterMoveInputY * waterSpeed);

        //If the player is moving right
        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        //If thge player is moving left
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }
    }

    //All air animal actions
    void AirAnimalMoving()
    {
        
    }

    //Flipping the charcters rotations
    void Flip()
    {
        //If facing right is = to false and then vice versa
        facingRight = !facingRight;

        //Getting the players local scale
        Vector3 scaler = transform.localScale;

        //Simply flips the character
        scaler.x *= -1;

        //sets the scaler to apply here
        transform.localScale = scaler;
    }

    //Changing the player states depending on their current action
    void OnTriggerEnter2D(Collider2D theCol)
    {
        if (theCol.gameObject.CompareTag("Water"))
        {
            playerState = PlayerState.WaterAnimalMoving;
        }

        if (theCol.gameObject.CompareTag("Ground"))
        {
            playerState = PlayerState.HumanMoving;
        }
    }
}
