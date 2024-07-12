using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    public int gridWidth = 100;
    public int gridHeight = 100;
    public float scale = 20f;
    public GameObject dummyPrefab;
    public Transform chunks;

    [Header("Debug")]
    public bool generate = false;
    public bool clear = false;

    private int halfWidth;
    private int halfHeight;

    // References
    private BuildingGenerator buildingGenerator;

    /*void Start()
    {
        // Cache
        buildingGenerator = GetComponent<BuildingGenerator>();

        InitializeGridSize();

        Generate();
    }*/

    void OnValidate()
    {
        InitializeGridSize();

        if (generate)
        {
            generate = false;

            StartCoroutine(ClearChunks(true));
        }

        if (clear)
        {
            clear = false;

            StartCoroutine(ClearChunks());
        }
    }

    void InitializeGridSize()
    {
        if (gridWidth > 9 && gridWidth % 2 == 1)
        {
            gridWidth++;
        }

        if (gridHeight > 9 && gridHeight % 2 == 1)
        {
            gridHeight++;
        }

        halfWidth = gridWidth / 2;
        halfHeight = gridHeight / 2;
    }

    void Generate()
    {
        if (buildingGenerator == null)
        {
            buildingGenerator = GetComponent<BuildingGenerator>();
        }

        int count = 0;

        for (int x = -halfWidth; x < halfWidth; x++)
        {
            for (int y = -halfHeight; y < halfHeight; y++)
            {
                GameObject newChunk = new GameObject() { name = "Chunk_" + count };

                newChunk.transform.parent = chunks;

                newChunk.transform.position = new Vector3((100 * x) + 50, 0, (100 * y) + 50);

                float xCoord = (float)x / gridWidth * scale;
                float yCoord = (float)y / gridHeight * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);

                if (sample > 1)
                {
                    sample = 1;
                }

                if (sample < 0)
                {
                    sample = 0;
                }

                int level = Mathf.FloorToInt(sample * 4);

                buildingGenerator.Generate(newChunk, level);

                count++;
            }
        }
    }

    private IEnumerator ClearChunks(bool shouldGenerate = false)
    {
        yield return null;

        if (Application.isPlaying)
        {
            for (int i = chunks.childCount - 1; i >= 0; i--)
            {
                Destroy(chunks.GetChild(i).gameObject);
            }
        }
        else
        {
            for (int i = chunks.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(chunks.GetChild(i).gameObject);
            }
        }

        if (shouldGenerate)
        {
            Generate();
        }
    }
}

