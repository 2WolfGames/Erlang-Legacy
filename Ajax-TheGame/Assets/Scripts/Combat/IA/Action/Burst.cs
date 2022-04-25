// using System.Collections;
// using System.Collections.Generic;
// using BehaviorDesigner.Runtime.Tasks;
// using UnityEngine;

// // description:
// //      instanciate gameobject in the list of trasforms

// // todo: remake using child transform

// namespace Core.Combat.IA.Action
// {
//     public class Burst : BehaviorDesigner.Runtime.Tasks.Action
//     {
//         [SerializeField] GameObject pointsContainer;
//         [SerializeField] GameObject prefab;
//         [SerializeField] float timeToDeleteInstances;

//         int nInstances;
//         List<GameObject> instances;
//         List<Transform> points;
//         [SerializeField] bool cleanedInstances = false;

//         public override void OnStart()
//         {
//             instances = new List<GameObject>();
//             points = new List<Transform>(pointsContainer.GetComponentsInChildren<Transform>());
//             nInstances = points.Count;
//             cleanedInstances = false;
//             InstanceThemAll();
//             StartCoroutine(CleanInstances(timeToDeleteInstances));
//         }

//         public override TaskStatus OnUpdate()
//         {
//             return cleanedInstances ? TaskStatus.Success : TaskStatus.Running;
//         }

//         private void InstanceThemAll()
//         {
//             foreach (var point in points)
//             {
//                 var ins = Object.Instantiate(prefab, point.position, Quaternion.identity);
//                 instances.Add(ins);
//             }
//         }

//         private IEnumerator CleanInstances(float time)
//         {
//             yield return new WaitForSeconds(time);
//             foreach (var instance in instances)
//             {
//                 Object.Destroy(instance.gameObject);
//             }
//             cleanedInstances = true;
//         }
//     }
// }


