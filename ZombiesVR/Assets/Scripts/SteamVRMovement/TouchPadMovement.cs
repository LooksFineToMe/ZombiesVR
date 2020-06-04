﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
public class TouchPadMovement : MonoBehaviour
{
    [Header("Player Stats")]
    public float moveSpeed = 2f;
    [Header("SetUp")]
    public Player player;
    public SteamVR_Action_Vector2 touchPadInput;
    public SteamVR_Action_Boolean sprint;
    public Transform cameraTransform;
    private CapsuleCollider capsuleCollider;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 movementDir = player.hmdTransform.TransformDirection(new Vector3(touchPadInput.axis.x, 0, touchPadInput.axis.y));
        transform.position += (Vector3.ProjectOnPlane(Time.deltaTime * movementDir * moveSpeed, Vector3.up));

        float distanceFromFloor = Vector3.Dot(cameraTransform.localPosition, Vector3.up);
        capsuleCollider.height = Mathf.Max(capsuleCollider.radius, distanceFromFloor);

        capsuleCollider.center = cameraTransform.localPosition - 0.5f * distanceFromFloor * Vector3.up;


    }
    private void Update()
    {
        if (sprint.state)
        { moveSpeed = 2.5f; }
        else
        { moveSpeed = 1.5f; }
    }
}
