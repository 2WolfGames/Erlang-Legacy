using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AjaxFX : MonoBehaviour
{
    float orientation = 0f;
    bool blockOrientationUpdate = false;

    void Update()
    {
        orientation = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        if (!blockOrientationUpdate)
        {
            HandleCharacterOrientation();
        }
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

    public void BlockOrientationUpdate()
    {
        blockOrientationUpdate = true;
    }

    public void UnblockOrientationUpdate()
    {
        blockOrientationUpdate = false;
    }

}
