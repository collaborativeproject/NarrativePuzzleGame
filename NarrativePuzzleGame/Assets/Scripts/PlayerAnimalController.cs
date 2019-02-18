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
    public GameObject humanMesh;

    [Header("Rabbit variables")]
    public float rabbitSpeed;
    public GameObject rabbitMesh;

    [Header("Turtle variables")]
    public float turtleSpeed;
    public GameObject turtleMesh;
    
    [Header("Turtle variables")]
    public float deerSpeed;

    [Header("Gems")]
    public GameObject gemOneGO;
    public Rigidbody gemOneRB;

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
            //Human states
            case PlayerStates.HumanMoving:
            HumanMesh();
            HumanMovement();
            HumanRotation();
                break;

            //Rabbit states
            case PlayerStates.RabbitMoving:
            RabbitMesh();
            RabbitMovement();
            RabbitDig();
                break;

            //Turtle states
            case PlayerStates.TurtleMoving:
            TurtleMesh();
            TurtleMovement();
            TurtlePush();
                break;

            //Deer states
            case PlayerStates.DeerMoving:
            DeerMovement();
            DeerPickup();
                break;
        }

        if (playerStates == PlayerStates.TurtleMoving)
        {
            gemOneRB.mass = 0.1f;
            gemOneRB.drag = 0;
        }
        else
        {
            gemOneRB.mass = 100f;
            gemOneRB.drag = 100f;
        }

        Flip();
    }

    void FixedUpdate()
    {
        //applies the velocity to the rigidbody
        myRB.velocity = moveVelocity;
    }

    /*
    *
    * ALL CODE RELATED TO HUMAN GOES HERE
    *
    */
    void HumanMesh()
    {
        //Setting human mesh active
        humanMesh.SetActive(true);
        rabbitMesh.SetActive(false);
        turtleMesh.SetActive(false);
    }

    void HumanMovement()
    {
        //Setting the vector3 equal to the inputs
        moveInput = new Vector3(character.GetAxisRaw("MoveHorizontal"), 0f, character.GetAxisRaw("MoveVertical"));
        //giving the player velocity and multiplies it by human speed
        moveVelocity = moveInput * moveSpeed;
    }

    void HumanRotation()
    {
        //Human rotation code
    }

    /*
    *
    * ALL HUMAN CODE ENDS HERE
    *
    */


    /*
    *
    * ALL CODE RELATED TO RABBIT GOES HERE
    *
    */
    void RabbitMesh()
    {
        //Setting rabbit mesh active
        rabbitMesh.SetActive(true);
        humanMesh.SetActive(false);
        turtleMesh.SetActive(false);
    }

    void RabbitMovement()
    {
        //Setting the vector3 equal to the inputs
        moveInput = new Vector3(character.GetAxisRaw("MoveHorizontal"), 0f, character.GetAxisRaw("MoveVertical"));
        //giving the player velocity and multiplies it by rabbit speed
        moveVelocity = moveInput * rabbitSpeed;
    }

    void RabbitDig()
    {

    }

    /*
    *
    * ALL RABBIT CODE ENDS HERE
    *
    */


    /*
    *
    * ALL CODE RELATED TO TURTLE GOES HERE
    *
    */
    void TurtleMesh()
    {
        //Setting turtle mesh active
        turtleMesh.SetActive(true);
        humanMesh.SetActive(false);
        rabbitMesh.SetActive(false);
    }

    void TurtleMovement()
    {
        //Setting the vector3 equal to the inputs
        moveInput = new Vector3(character.GetAxisRaw("MoveHorizontal"), 0f, character.GetAxisRaw("MoveVertical"));
        //giving the player velocity and multiplies it by turtle speed
        moveVelocity = moveInput * turtleSpeed;
    }

    void TurtlePush()
    {

    }

    /*
    *
    * ALL TURTLE CODE ENDS HERE
    *
    */

    /*
    *
    * ALL CODE RELATED TO DEER GOES HERE
    *
    */

    void DeerMovement()
    {
        //Setting the vector3 equal to the inputs
        moveInput = new Vector3(character.GetAxisRaw("MoveHorizontal"), 0f, character.GetAxisRaw("MoveVertical"));
        //giving the player velocity and multiplies it by turtle speed
        moveVelocity = moveInput * deerSpeed;
    }

    void DeerPickup()
    {

    }

    /*
    *
    * ALL DEER CODE ENDS HERE
    *
    */

    private void OnTriggerStay(Collider theCol)
    {
        //If the player is close to the bear cave then they trasnform into a rabbit
        if (theCol.gameObject.CompareTag("BearCave"))
        {
            playerStates = PlayerStates.RabbitMoving;
        }

        //If the player is close to water then they transform into a turtle
        if (theCol.gameObject.CompareTag("Water"))
        {
            playerStates = PlayerStates.TurtleMoving;
        }
    }

    private void OnTriggerExit(Collider theCol)
    {
        //If the player leaves the bear cave
        if (theCol.gameObject.CompareTag("BearCave"))
        {
            playerStates = PlayerStates.HumanMoving;
        }

        //If the player leaves the water
        if (theCol.gameObject.CompareTag("Water"))
        {
            playerStates = PlayerStates.HumanMoving;
        }
    }

    void Flip()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.eulerAngles = new Vector3(0, -90, 0);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.eulerAngles = new Vector3(0, 90, 0);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 7);
    }
}
