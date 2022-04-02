using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

// makes appears some objects given some points
public class Burst : Action
{
    [SerializeField] List<Transform> burstPoints;
    [SerializeField] GameObject prefab;
    [SerializeField] float timeToDeleteInstances;

    int nInstanciated;
    int nInstances;
    List<GameObject> instances;
    [SerializeField] bool cleanedInstances = false;

    public override void OnStart()
    {
        instances = new List<GameObject>();
        nInstanciated = 0;
        nInstances = burstPoints.Count;
        cleanedInstances = false;
        InstanceThemAll();
        StartCoroutine(CleanInstances(timeToDeleteInstances));
    }

    public override TaskStatus OnUpdate()
    {
        return cleanedInstances ? TaskStatus.Success : TaskStatus.Running;
    }

    private void InstanceThemAll()
    {
        foreach (var point in burstPoints)
        {
            var ins = Object.Instantiate(prefab, point.position, Quaternion.identity);
            instances.Add(ins);
        }
    }

    private IEnumerator CleanInstances(float time)
    {
        yield return new WaitForSeconds(time);
        foreach (var instance in instances)
        {
            Object.Destroy(instance.gameObject);
        }
        cleanedInstances = true;
    }
}
