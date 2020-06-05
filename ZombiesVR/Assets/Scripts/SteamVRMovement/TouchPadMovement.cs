using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
public class TouchPadMovement : MonoBehaviour
{
    //Player stats for Movement
    [Header("Player Stats")]
    private float m_moveSpeed;
    [Tooltip("Walk Speed of the player when walking")]
    public float m_WalkSpeed = 1.5f;
    [Tooltip("Run Speed of the player when running")]
    public float m_RunSpeed = 2.5f;
    //The Set up items for player movement
    [Header("SetUp")]
    Player player;
    [Tooltip("The Vector 2 input for movement")]
    public SteamVR_Action_Vector2 touchPadInput;
    [Tooltip("The sprint input")]
    public SteamVR_Action_Boolean sprint;
    [Tooltip("The players camera")]
    public Transform cameraTransform;
    private CapsuleCollider capsuleCollider;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();//Get the player class on start up
        capsuleCollider = GetComponent<CapsuleCollider>();//Get the capsule collider on start up
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Checks input
        Vector3 movementDir = player.hmdTransform.TransformDirection(new Vector3(touchPadInput.axis.x, 0, touchPadInput.axis.y));
        //Moves Player
        transform.position += (Vector3.ProjectOnPlane(Time.deltaTime * movementDir * m_moveSpeed, Vector3.up));
        //Checks Height
        float distanceFromFloor = Vector3.Dot(cameraTransform.localPosition, Vector3.up);
        capsuleCollider.height = Mathf.Max(capsuleCollider.radius, distanceFromFloor);
        capsuleCollider.center = cameraTransform.localPosition - 0.5f * distanceFromFloor * Vector3.up;
    }
    private void Update()
    {
        //Checks if sprinting or not.
        if (sprint.state)//Is sprinting
        { m_moveSpeed = m_RunSpeed; }

        else//Is not sprinting
        { m_moveSpeed = m_WalkSpeed; }
    }
}
