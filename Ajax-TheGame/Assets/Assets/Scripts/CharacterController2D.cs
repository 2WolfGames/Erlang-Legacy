using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    This class is thought to be generic,
    any character should use this class, it takes cares of
        - character orientation
    
    In future it may handle other common character eassues
*/

public class CharacterController2D : MonoBehaviour
{
    float orientation = 0f;

    void Update()
    {
        orientation = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        HandleCharacterOrientation();
    }

    void HandleCharacterOrientation()
    {
        int orientation = Mathf.RoundToInt(this.orientation);
        UpdateCharacterOrientation(orientation);
    }

    // -1 left, 1 right 
    void UpdateCharacterOrientation(int orientation)
    {
        if (orientation != -1 && orientation != 1) return;

        Vector3 characterScale = transform.localScale;
        characterScale.x = orientation;
        transform.localScale = characterScale;
    }

}
