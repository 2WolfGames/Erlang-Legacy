using UnityEngine;

public class SceneEntrance : MonoBehaviour
{
    [SerializeField] Transform entrancePoint;
    [SerializeField] Transform spawnPoint;

    public Vector3 GetEntrancePoint()
    {
        return entrancePoint.position;
    }

    public Vector3 GetSpawnPoint()
    {
        return spawnPoint.position;
    }
}
