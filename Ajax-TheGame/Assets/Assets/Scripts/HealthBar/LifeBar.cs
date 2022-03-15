using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBar : MonoBehaviour
{
    //// Const values ////
    const int cMaxLifeContainers = 9;
    const float cCoroutineShakeWait = 1.25f;
    const float cCoroutineFillLifeWait = 1f;
    const float cCoroutineReflectLifeWait = 0.35f;

    //// Serialized variables ////
    [SerializeField] GameObject lifePrefab;
    [SerializeField] GameObject emptyLifePrefab;
    [SerializeField] List<LifeContainer> lifeContainers;
    [SerializeField] ParticleSystem newLifeApearingParticleEffect;
    
    ////Global variables ////
    int totalLifes;
    int currentLifes;
    Queue<int> pendentChanges;
    bool modifying;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
