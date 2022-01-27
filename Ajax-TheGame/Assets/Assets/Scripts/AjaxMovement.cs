using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AjaxMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] CharacterController2D characterController;

    [SerializeField] float runSpeed = 30f;

    float horizontalMovement = 0f;

    // called one per frame
    private void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
    }


    // Time.fixedDeltaTime: represents how long was the last time
    // the function was called (period time)

    // Multiply by Time.fixedDeltaTime will ensure that Ajax
    // moves same amount no matter how often this function was called

    // It make consistence with multiple platforms
    private void FixedUpdate()
    {
        float xSpeed = horizontalMovement * runSpeed;
        characterController.Move(xSpeed, 0f);
    }
}
