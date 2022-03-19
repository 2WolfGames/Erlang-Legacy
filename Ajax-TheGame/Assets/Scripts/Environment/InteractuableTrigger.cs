using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core.Player;

public class InteractuableTrigger : MonoBehaviour
{
    [SerializeField] float detectorTime = 0.2f;

    [SerializeField] string trigger = "hover";

    float _detectorTime = 0.2f;

    Animator animator;

    bool triggerAnimation = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        _detectorTime = detectorTime;
    }

    void Update()
    {
        _detectorTime -= Time.deltaTime;

        if (_detectorTime < Mathf.Epsilon)
        {
            _detectorTime = detectorTime;
            triggerAnimation = true;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerAnimation && other.GetComponent<FXController>())
        {
            animator.SetTrigger(trigger);
            triggerAnimation = false;
        }
    }
}
