using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEntrance : MonoBehaviour
{
    [SerializeField] Transform entrancePoint;

    public Vector3 GetEntrancePoint(){
        return entrancePoint.position;
    }
}
