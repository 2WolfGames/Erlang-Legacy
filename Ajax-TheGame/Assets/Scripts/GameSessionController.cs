using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSessionController : MonoBehaviour
{
    Transform savePoint;
    Transform currentPoint;

    void Awake(){
        int numGameSessionControllers = FindObjectsOfType<GameSessionController>().Length;
        if (numGameSessionControllers > 1){
            Destroy(gameObject);
        } else{
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
