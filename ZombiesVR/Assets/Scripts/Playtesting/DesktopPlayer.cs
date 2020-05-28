using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//******************************************************************************************
//******************************************************************************************

//Orininal Code writen in JAVA Script converted into CSharp.                                              ****************
//Sourced from https://github.com/WiggleWizard/quake3-movement-unity3d/blob/master/CPMPlayer.js           ****************

//******************************************************************************************
//******************************************************************************************


public class DesktopPlayer : MonoBehaviour
{
    private CharacterController PlayerController;

    //player camera variables
    [Header("Camera")]
    [SerializeField] Transform m_PlayerView;                //Must be a camera
    [SerializeField] float cameraTiltSpeed;
    [SerializeField] float cameraStrafeAngle = 10f;
    [SerializeField] float m_PlayerViewOffset = 0.6f;       //the height of the camera
    [SerializeField] float xMouseSensitivity = 30.0f;
    [SerializeField] float yMouseSensitivity = 30.0f;
    [SerializeField] bool useCameraTilt;

    Vector3 pv;                             //to convert camera transform into a vector3

    //Camera Rotations
    private float c_rotX = 0.0f;
    private float c_rotY = 0.0f;

    //Frame Specific
    [SerializeField] float gravity = 20.0f;
    [SerializeField] float friction = 6.0f;                 //ground friction
    
    //contains the commands the user wishes upon the player character (ive never used this before)
    public class Cmd
    {
        public float fowardInput;
        public float strafeInput;
        public bool jumpInput;
        public bool jumpInputUp;
    }
    public Cmd playerInput; //player commands, stores wish commands that the player asks for (foward, back, jump, so on)

    //player movement variables
    [Header("Movement")]
    [SerializeField] float m_MoveSpeed = 7.0f;              //ground movespeed
    [SerializeField] float m_MoveAcceleration = 14.0f;      //ground acceleration
    [SerializeField] float m_MoveDeacceleration = 10.0f;    //deceleration that occurs when running on the ground
    [SerializeField] float m_AirAcceleration = 10.0f;       //air acceleration
    [SerializeField] float m_AirDeacceleration = 10.0f;     //Deceleration when opposite strafing
    [SerializeField] float m_AirControl = 0.3f;             //how precise the air control is
    [SerializeField] float m_SideStrafeAcceleration = 50.0f;//how fast acceleration occurs to get up to sideStrafeSpeed when side strafing
    [SerializeField] float m_SideStrafeSpeed = 1.0f;        //what the max speed to generate when side strafing
    [SerializeField] float m_JumpSpeed = 8.0f;              //the speed at which the character's up axis gains when hitting jump
    [SerializeField] bool limitDiagonalSpeed = false;
    [SerializeField] bool DebugText = true;


    private bool m_HoldJumpToBHop = false;                  //when enabled allows the player to just hold jump to keep behopping perfectly

    //player movement vectors
    private Transform m_Transform;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 moveDirectionNorm = Vector3.zero;
    private Vector3 playerVelocity = Vector3.zero;
    private float playerTopVelocity = 0.0f;

    private float m_RayDistance;
    private float m_SlideLimit;
    private Vector3 m_ContactPoint;

    //Question> player can queue the next jump just before they hit they ground
    private bool wishJump = false;

    private float playerFriction = 0.0f;

    //player status
    private bool isDead = false;

    private Vector3 playerSpawnPos;
    private Quaternion playerSpawnRot;
    [Header("Debug Text")]
    [SerializeField] Text FPS;
    [SerializeField] Text Speed;
    [SerializeField] Text TopSpeed;

    private float fpsDisplayRate = 4.0f;

    private float frameCount = 0f;
    private float dt = 0.0f;
    private float fps = 0.0f;

    private float inputModifyFactor;

    // Start is called before the first frame update
    void Start()
    {
        //hiding the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //placing the camera in the capsule collider
        pv = m_PlayerView.position;
        pv = transform.position;
        pv.y = transform.position.y + m_PlayerViewOffset;

        //make sure to grab the Character Controller Component
        PlayerController = GetComponent<CharacterController>();
        m_Transform = GetComponent<Transform>();

        m_RayDistance = PlayerController.height * 0.5f + PlayerController.radius;
        m_SlideLimit = PlayerController.slopeLimit - 0.1f;

        playerInput = new Cmd();

        playerSpawnPos = transform.position;
        playerSpawnRot = m_PlayerView.rotation;

        inputModifyFactor = (playerInput.fowardInput != 0.00f && playerInput.strafeInput != 0.0f && limitDiagonalSpeed) ? 0.01f : 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (DebugText)
        {
            //fps calculations
            frameCount++;
            dt += Time.deltaTime;
            if (dt > 1.0f / fpsDisplayRate)
            {
                fps = Mathf.Round(frameCount / dt);
                frameCount = 0;
                dt -= 1.0f / fpsDisplayRate;
            }
            PlayerStats();
        }

        if (Cursor.lockState != CursorLockMode.Locked)
        {
            if (Input.GetMouseButtonDown(0))
                Cursor.lockState = CursorLockMode.Locked;
        }

        CameraControllers();
        if(useCameraTilt)
            CameraTilt();

        QueueNextJump();

        if (PlayerController.isGrounded)
            GroundMovement();
        else if (!PlayerController.isGrounded)
            AirMovement();

        //move the controller
        PlayerController.Move(playerVelocity * Time.deltaTime);

        //need to move the camera after the player has been moved otherwise the camera will clip the player if going fast enough and will fall 1 frame behind the player.
        //set the camera's position to the transform
        pv = transform.position;
        pv.y = transform.position.y + m_PlayerViewOffset;

        //some sick math to calculate the top velocity
        var udp = playerVelocity;
        udp.y = 0.0f;
        if (udp.magnitude > playerTopVelocity)
            playerTopVelocity = udp.magnitude;

        if (Input.GetKeyDown(KeyCode.R) && isDead)
        {
            PlayerRespawn();
        }


    }

    void CameraControllers()
    {
        //camera rotation and control
        c_rotX -= Input.GetAxis("Mouse Y") * xMouseSensitivity * 0.02f;
        c_rotY += Input.GetAxis("Mouse X") * yMouseSensitivity * 0.02f;

        //clamp the camera rotations
        c_rotX = Mathf.Clamp(c_rotX, -75, 75);

        transform.rotation = Quaternion.Euler(0, c_rotY, 0);    //rotates the player collider
        m_PlayerView.rotation = Quaternion.Euler(c_rotX, c_rotY, 0); //Rotates the camera
    }

    void CameraTilt()
    {
        float angle = cameraStrafeAngle * -playerInput.strafeInput;
        Quaternion cameraRotation = m_PlayerView.rotation * Quaternion.Euler(0f, 0f, angle);

        m_PlayerView.rotation = Quaternion.Slerp(m_PlayerView.rotation, cameraRotation, cameraTiltSpeed);
    }

    //*****************MOVEMENT*************

     
    //set the movement direction based on the player input
    private void SetMovementDir()
    {
        float inputModifyFactor = (playerInput.fowardInput != 0.00f && playerInput.strafeInput != 0.0f && limitDiagonalSpeed) ? 0.01f : 1.0f;

        playerInput.fowardInput = Input.GetAxisRaw("Vertical") * inputModifyFactor;
        playerInput.strafeInput = Input.GetAxisRaw("Horizontal") * inputModifyFactor;
    }
    

    private void QueueNextJump()
    {
        playerInput.jumpInput = Input.GetButtonDown("Jump");
        playerInput.jumpInputUp = Input.GetButtonUp("Jump");

        if (m_HoldJumpToBHop)
        {
            wishJump = playerInput.jumpInput;
            return;
        }

        if (playerInput.jumpInput && !wishJump)
            wishJump = true;
        if (playerInput.jumpInputUp)
            wishJump = false;
    }
    //exec when the player is in the air
    void AirMovement()
    {
        Vector3 TargetDir;
        float TargetVel = m_AirAcceleration;
        float accel;

        SetMovementDir();

        TargetDir = new Vector3(playerInput.strafeInput, 0, playerInput.fowardInput);
        TargetDir = transform.TransformDirection(TargetDir);

        float TargetSpeed = TargetDir.magnitude;
        TargetSpeed *= m_MoveSpeed;

        TargetDir.Normalize();
        moveDirectionNorm = TargetDir;

        //CPM air control
        float TargetSpeed2 = TargetSpeed;

        if (Vector3.Dot(playerVelocity, TargetDir) < 0)
            accel = m_AirDeacceleration;
        else
            accel = m_AirAcceleration;
        //if the player is only strafing left or right

        if (playerInput.fowardInput == 0 && playerInput.strafeInput != 0)
        {
            if (TargetSpeed > m_SideStrafeSpeed)
                TargetSpeed = m_SideStrafeSpeed;

            accel = m_SideStrafeAcceleration;
        }


        Accelerate(TargetDir, TargetSpeed, accel);
        if (m_AirControl > 0.3f)
            AirControl(TargetDir, TargetSpeed2);
        //!CPM aircontrol

        //applying gravity
        playerVelocity.y -= gravity * Time.deltaTime;

    }

    private void AirControl(Vector3 TargetDir, float TargetSpeed)
    {
        float zSpeed;
        float speed;
        float dot;
        float k;

        //can't control movement if not moving forward or backward
        if (playerInput.fowardInput == 0 || TargetSpeed == 0)
            return;

        zSpeed = playerVelocity.y;
        playerVelocity.y = 0;
        //next two lines are equivalent to idTech's VectorNormalize()
        speed = playerVelocity.magnitude;
        playerVelocity.Normalize();

        dot = Vector3.Dot(playerVelocity, TargetDir);
        k = 32f;
        k *= m_AirControl * dot * dot * Time.deltaTime;

        //change direction while slowing down
        if (dot > 0)
        {
            playerVelocity.x = playerVelocity.x * speed + TargetDir.x * k;
            playerVelocity.y = playerVelocity.y * speed + TargetDir.y * k;
            playerVelocity.z = playerVelocity.z * speed + TargetDir.z * k;

            playerVelocity.Normalize();
            moveDirectionNorm = playerVelocity;
        }

        playerVelocity.x *= speed;
        playerVelocity.y *= zSpeed;     //note this line
        playerVelocity.z *= speed;
    }

    void GroundMovement()
    {
        float inputModifyFactor = (Input.GetAxisRaw("Vertical") != 0.00f && Input.GetAxisRaw("Horizontal") != 0.0f && limitDiagonalSpeed) ? 0.01f : 1.0f;

        Vector3 TargetDir;

        //Do not apply friction if the player is queueing the next jump

        if (!wishJump)
            ApplyFriction(1.0f);
        else
            ApplyFriction(0);

        SetMovementDir();

        TargetDir = new Vector3(playerInput.strafeInput, 0, playerInput.fowardInput);
        TargetDir = transform.TransformDirection(TargetDir) * m_MoveSpeed;
        TargetDir.Normalize();
        moveDirectionNorm = TargetDir;

        float TargetSpeed = TargetDir.magnitude;
        TargetSpeed *= m_MoveSpeed;

        Accelerate(TargetDir, TargetSpeed, m_MoveAcceleration);

        //reset the gravity velocity
        playerVelocity.y = 0;

        if (wishJump)
        {
            playerVelocity.y = m_JumpSpeed;
            wishJump = false;
        }
    }

    private void ApplyFriction(float t)
    {
        Vector3 vec = playerVelocity; //equivalent to VectorCopy();
        float speed;
        float newSpeed;
        float control;
        float drop;

        vec.y = 0.0f;
        speed = vec.magnitude;
        drop = 0.0f;

        //only if the playeer is on the ground then apply friction
        if (PlayerController.isGrounded)
        {
            control = speed < m_MoveDeacceleration ? m_MoveDeacceleration : speed;
            drop = control * friction * Time.deltaTime * t;
        }

        newSpeed = speed - drop;
        playerFriction = newSpeed;
        if (newSpeed < 0)
            newSpeed = 0;
        if (speed > 0)
            newSpeed /= speed;

        playerVelocity.x *= newSpeed;
            //playerVelocity.y *= newSpeed;
        playerVelocity.z *= newSpeed;
    }

    private void Accelerate(Vector3 TargetDir, float TargetSpeed, float accel)
    {
        float addSpeed;
        float accelSpeed;
        float currentSpeed;

        currentSpeed = Vector3.Dot(playerVelocity, TargetDir);
        addSpeed = TargetSpeed - currentSpeed;
        if (addSpeed <= 0)
            return;
        accelSpeed = accel * Time.deltaTime * TargetSpeed;
        if (accelSpeed > addSpeed)
            accelSpeed = addSpeed;

        playerVelocity.x += accelSpeed * TargetDir.x;
        playerVelocity.z += accelSpeed * TargetDir.z;
    }

    private void PlayerStats()
    {
        FPS.text = "FPS: " + fps;
        var ups = PlayerController.velocity;
        ups.y = 0;
        Speed.text = "Speed: " + Mathf.Round(ups.magnitude * 100) / 100 + "ups";
        TopSpeed.text = "Top Speed: " + Mathf.Round(playerTopVelocity * 100) / 100 + "ups";
    }

    void PlayerRespawn()
    {
        transform.position = playerSpawnPos;
        m_PlayerView.rotation = playerSpawnRot;
        c_rotX = 0.0f;
        c_rotY = 0.0f;
        playerVelocity = Vector3.zero;
        isDead = false;
    }
}
