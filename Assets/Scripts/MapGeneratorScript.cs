using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MapGeneratorScript : MonoBehaviour
{
    public const float MAP_SIZE = 19.5f;

    [SerializeField]
    private GameObject obstaclePrefab;

    [SerializeField]
    private GameObject objectivePrefab;

    [SerializeField]
    private GameObject objectiveParent;

    [SerializeField]
    private GameObject obstacleParent;

    private Vector2 bottomLeft, topRight;

    private float bottom = -6f;
    private float top = 8.5f;
    private int obstacleSpawnCount = 64;

    private List<Vector2> spawns;
    private List<GameObject> newObjectives;

    private List<Transform> oldMap = new();

    public float GetTopOfMap()
    {
        return top;
    }

    public void AddToSpawns(Vector2 spawn)
    {
        spawns = new List<Vector2>();
        newObjectives = new List<GameObject>();
        spawns.Add(spawn);
    }

    public List<GameObject> SpawnMap()
    {
        if (obstacleSpawnCount < 74)
        {
            obstacleSpawnCount += 5;
        }

        bottomLeft = new Vector2(Constants.Map.Left, bottom += MAP_SIZE);
        topRight = new Vector2(Constants.Map.Right, top += MAP_SIZE);

        bool obstaclesSpawned = false;
        bool objectivesSpawned = false;
        int tries = 0;

        foreach (Transform transform in obstacleParent.transform)
        {
            oldMap.Add(transform);
        }

        foreach (Transform transform in objectiveParent.transform)
        {
            oldMap.Add(transform);
        }

        while (!objectivesSpawned)
        {
            var newPos = new Vector2(Random.Range(bottomLeft.x, topRight.x), Random.Range(bottomLeft.y, topRight.y));

            if (!spawns.Any(os => Vector2.Distance(os, newPos) <= 2f))
            {
                var newObjective = Instantiate(objectivePrefab, newPos, objectivePrefab.transform.rotation);
                newObjective.layer = 7;
                newObjectives.Add(newObjective);
                spawns.Add(newPos);
                newObjective.transform.parent = objectiveParent.transform;
            }

            tries++;

            if (tries >= Constants.Map.MaxTry || spawns.Count == Constants.Map.ObjectiveSpawnCount)
            {
                objectivesSpawned = true;
            }
        }

        tries = 0;

        while (!obstaclesSpawned)
        {
            var newPos = new Vector2(Random.Range(bottomLeft.x, topRight.x), Random.Range(bottomLeft.y, topRight.y));

            if (!spawns.Any(os => Vector2.Distance(os, newPos) <= 1.5f))
            {
                var newObstacle = Instantiate(obstaclePrefab, newPos, obstaclePrefab.transform.rotation);
                newObstacle.layer = 7;
                spawns.Add(newPos);
                newObstacle.transform.parent = obstacleParent.transform;
            }

            tries++;

            if (tries >= Constants.Map.MaxTry || spawns.Count == obstacleSpawnCount)
            {
                obstaclesSpawned = true;
            }
        }

        newObjectives = newObjectives.OrderBy(o => o.transform.position.y).ToList();

        newObjectives.First().layer = 6;
        Invoke(nameof(DestroyOldMap), 10);

        //Debug.Log($"spawns: {spawns.Count}");
        //Debug.Log($"tries: {tries}");
        return newObjectives;
    }

    private void DestroyOldMap()
    {
        foreach (var obj in oldMap)
        {
            Destroy(obj.gameObject);
        }

        oldMap = new();
    }
}
