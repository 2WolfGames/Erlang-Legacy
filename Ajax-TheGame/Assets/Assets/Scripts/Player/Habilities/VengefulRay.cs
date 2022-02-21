using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VengefulRay : MonoBehaviour
{
    [SerializeField] float time;

    [SerializeField] List<LayerMask> targets;

    HashSet<GameObject> hitted = new HashSet<GameObject>();

}
