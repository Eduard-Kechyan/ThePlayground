using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    public Vector2Int count = Vector2Int.zero;
    public Vector2Int width = Vector2Int.zero;
    public Vector2Int height = Vector2Int.zero;
    public float maxPos = 0;
    public int minDistanceOffset = 10;
    public int maxAttempts = 30;
    public GameObject buildingPrefab;
    public Material[] materials;

    private List<Vector2> points = new List<Vector2>();

    public void Generate(GameObject chunk,int level)
    {
        int randomCount = Random.Range(count.x, count.y + 1);

        GeneratePoints(randomCount);

        for (int i = 0; i < randomCount; i++)
        {
            int randomWidth = Random.Range(width.x, width.y + 1);
            int randomHeight = Random.Range(height.x, height.y + 1) * (level + 1);
            Vector3 randomPos = new Vector3(points[i].x, randomHeight / 2, points[i].y);

            GameObject newBuilding = Instantiate(buildingPrefab);

            newBuilding.transform.localScale = new Vector3(randomWidth, randomHeight, randomWidth);

            newBuilding.transform.position = randomPos + chunk.transform.position;

            newBuilding.transform.parent = chunk.transform;

            newBuilding.GetComponent<MeshRenderer>().material = materials[level];
        }
    }

    void GeneratePoints(int pointCount)
    {
        points.Clear();

        for (int i = 0; i < pointCount; i++)
        {
            bool validPoint = false;
            int attempts = 0;
            Vector2 point = Vector2.zero;

            while (!validPoint && attempts < maxAttempts)
            {
                point = new Vector2(Random.Range(-maxPos, maxPos), Random.Range(-maxPos, maxPos));
                validPoint = ValidatePoint(point);
                attempts++;
            }

            if (validPoint)
                points.Add(point);
        }
    }

    bool ValidatePoint(Vector2 newPoint)
    {
        foreach (Vector2 point in points)
        {
            if (Vector2.Distance(newPoint, point) < (width.y+minDistanceOffset))
                return false;
        }
        return true;
    }
}