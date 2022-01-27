using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AjaxMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] CharacterController2D characterController;

    [SerializeField] float runSpeed = 100f;

    [SerializeField] float jumpForce = 100;

    float horizontalMovement = 0f;

    bool wantToJump = false;

    // called one per frame
    void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            wantToJump = true;
        }
    }


    // Time.fixedDeltaTime: represents how long was the last time
    // the function was called (period time)

    // Multiply by Time.fixedDeltaTime will ensure that Ajax
    // moves same amount no matter how often this function was called
    // It make consistences with multiple platforms
    void FixedUpdate()
    {
        float xSpeed = horizontalMovement * runSpeed * Time.deltaTime;
        float ySpeed = jumpForce * Time.deltaTime;
        characterController.Move(xSpeed, wantToJump ? ySpeed : 0f);
        wantToJump = false;
    }
}
