using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{

    public float MovementSpeed = 2.0f;

    public float JumpForce = 5.0f;

    private Rigidbody _Rigidbody;

    private bool IsGrounded;

    public float DistationToGround = 0.1f;


    public float RotationSmothing = 20f;

    public GameObject HandMeshes;

    private float pitch, yaw;

    public float SprintSpeed = 4.0f;

    private GameManager _GameManager;



    // Start is called before the first frame update
    private void Start()
    {
        _Rigidbody = GetComponent<Rigidbody>();
        _GameManager = FindObjectOfType<GameManager>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Jump()
    {
        _Rigidbody.AddForce(UnityEngine.Vector3.up * JumpForce, ForceMode.Impulse);
    }

    private void GroundCheck()
    {
        IsGrounded = Physics.Raycast(transform.position, UnityEngine.Vector3.down, DistationToGround);
    }

    private UnityEngine.Vector3 CalculateMovment()
    {
        float HorizontalDirection = Input.GetAxis("Horizontal");
        float VerticalDirection = Input.GetAxis("Vertical");

        UnityEngine.Vector3 Move = transform.right * HorizontalDirection + transform.forward * VerticalDirection;
        return _Rigidbody.transform.position + Move * Time.fixedDeltaTime * MovementSpeed;
    }

    private void FixedUpdate()
    {
        GroundCheck();

        if (Input.GetKey(KeyCode.Space) && IsGrounded ) Jump();

        _Rigidbody.MovePosition(CalculateMovment());

        SetRotation();

         if (Input.GetKey(KeyCode.LeftShift) && !_GameManager.IsStaminaRestoring)
        {
            _GameManager.Spe();
            _Rigidbody.MovePosition(CalculateSprint());

        }
    }

    private void OnDrawnGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (UnityEngine.Vector3.down * DistationToGround));
    }

    private void SetRotation()
    {
        yaw += Input.GetAxis("Mouse X");
        pitch -= Input.GetAxis("Mouse Y");

        pitch = Mathf.Clamp(pitch, -60, 90);

        UnityEngine.Quaternion SmoothRotation = UnityEngine.Quaternion.Euler(pitch, yaw, 0);

        HandMeshes.transform.rotation = UnityEngine.Quaternion.Slerp(HandMeshes.transform.rotation, SmoothRotation,
        RotationSmothing * Time.fixedDeltaTime);

        SmoothRotation = UnityEngine.Quaternion.Euler(0, yaw, 0);

        transform.rotation = UnityEngine.Quaternion.Slerp(transform.rotation, SmoothRotation, RotationSmothing * Time.fixedDeltaTime);
    }

    private UnityEngine.Vector3 CalculateSprint()
    {
        float HorizontalDirection = Input.GetAxis("Horizontal");
        float VerticalDirection = Input.GetAxis("Vertical");
        UnityEngine.Vector3 Move = transform.right * HorizontalDirection + transform.forward * VerticalDirection;
        return _Rigidbody.transform.position + Move * Time.fixedDeltaTime * SprintSpeed;

    }
}
