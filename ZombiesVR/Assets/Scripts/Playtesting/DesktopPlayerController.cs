using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DesktopPlayerController : MonoBehaviour
{
    [SerializeField] float m_MoveSpeed = 10f;
    [SerializeField] float m_Gravity = 20f;
    [SerializeField] bool active;

    public class CMD { public float fowardMovement; public float rightMovement; }

    private CMD cmd;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        cmd = new CMD();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        cmd.fowardMovement = Input.GetAxisRaw("Vertical");
        cmd.rightMovement = Input.GetAxisRaw("Horizontal");

        float inputModifyFactor = (cmd.fowardMovement != 0.0f && cmd.rightMovement != 0.0f) ? 0.75f : 1.0f;

        float fowardInput = m_MoveSpeed * cmd.fowardMovement * inputModifyFactor;
        rb.AddForce(transform.forward * fowardInput);

        float strafeInput = m_MoveSpeed * cmd.rightMovement * inputModifyFactor;
        rb.AddForce(transform.right * strafeInput);
    }
}
