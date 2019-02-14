using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public enum PlayerStates
{
    HumanMoving,
    TurtleMoving,
    RabbitMoving,
    DeerMoving
}

public class PlayerAnimalController : MonoBehaviour
{
    //Public variables
    [Header("Player common variables")]
    public int playerID = 0;
    public PlayerStates playerStates = PlayerStates.HumanMoving;

    [Header("Human variables")]
    public float moveSpeed;

    //Private variables
    private Player character;
    private Rigidbody myRB;
    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private Camera mainCamera;
    private Vector3 playerDirection;
    private Vector2 playerLookDirection;

    void Awake()
    {
        //Getting the Rewired player object for this gameObject and keeping it for the characters lifetime
        character = ReInput.players.GetPlayer(playerID);
    }

    void Start()
    {
        //Simply setting things at the start
        myRB = GetComponent<Rigidbody>();
        mainCamera = FindObjectOfType<Camera>();

        //Player directions for rotation and movement
        playerLookDirection.x = 0f;
        playerLookDirection.y = 1f;
    }

    void Update()
    {
        switch (playerStates)
        {
            //Human player states
            case PlayerStates.HumanMoving:
            HumanMovement();
            HumanRotation();
            break;
        }
    }

    void FixedUpdate()
    {
        //applies the velocity to the rigidbody
        myRB.velocity = moveVelocity;
    }

    void HumanMovement()
    {
        //Setting the vector3 equal to the inputs
        moveInput = new Vector3(character.GetAxisRaw("MoveHorizontal"), 0f, character.GetAxisRaw("MoveVertical"));
        //giving the player velocity
        moveVelocity = moveInput * moveSpeed;
        Debug.Log("Zoinks");
    }

    void HumanRotation()
    {
        //Human rotation code
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 7);
    }
}
