using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSessionController : MonoBehaviour
{
    GameObject ajax;
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
    private void Start() {
        ajax = FindObjectOfType<Core.Character.Player.BasePlayer>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetCurrentPoint(){
        ajax.transform.position = currentPoint.position;
    }

    public void SavePlayerCurrentPoint(Transform currentPointTransform){
        currentPoint = currentPointTransform;
    }
}
