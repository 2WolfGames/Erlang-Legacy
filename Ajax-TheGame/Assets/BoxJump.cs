using System.Collections;
using System.Collections.Generic;
using Core.Character.Player;
using UnityEngine;

public class BoxJump : MonoBehaviour
{
    float time;
    [SerializeField][Range(0.01f, 0.5f)] float threshold = 0.05f;

    Vector2 startPosition;
    Vector2 endPosition;
    void Start()
    {
        startPosition = transform.position;
        endPosition = BasePlayer.Instance.Feets.position;
    }

    void Update()
    {
        time += Time.deltaTime;
        transform.position = MathParabola.Parabola(startPosition, endPosition, 3f, time / 2f);

        // Debug.Log(Vector2.Distance(transform.position, endPosition));

        if (Vector2.Distance(transform.position, endPosition) <= threshold) Debug.Log("target");

        // Debug.Log(transform.position);
        // Debug.Log(endPosition);

        // if (Mathf.Abs(Vector2.Distance(transform.position, endPosition)) == 0)
        // {
        //     Debug.Log("target!!");
        // }

    }
}
