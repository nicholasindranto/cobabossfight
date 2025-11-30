using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// butuh character controller
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    #region CharacterController
    // reference ke charactercontroller
    private CharacterController characterController;
    #endregion

    #region Movement
    // ambil dari input action
    [SerializeField] private Vector2 input;

    // gerakan playernya
    [SerializeField] private Vector3 moveDirection;

    // kecepatan geraknya
    public float moveSpeed;
    #endregion

    #region Rotation
    // seberapa halus rotatenya
    private float smoothTime = 0.2f;

    // kecepatan sekarang
    private float currentVelocity;
    #endregion

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        ApplyMovement();

        ApplyRotation();
    }

    private void ApplyMovement()
    {
        // bikin biar gerak
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void ApplyRotation()
    {
        // cek ada inputnya nggak pakai sqrtmagnitude
        if (input.sqrMagnitude == 0) return;

        // hitung target anglenya pakai atan2
        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;

        // bikin rotasinya halus pakai smoothdampangle
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);

        // set ke rotationnya yang sumbu y saja
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    public void Move(InputAction.CallbackContext context)
    {
        // masukin ke inputnya
        input = context.ReadValue<Vector2>();

        // assign ke movementnya
        moveDirection = new Vector3(input.x, 0, input.y);
    }
}
