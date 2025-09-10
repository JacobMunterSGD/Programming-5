using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script makes the terrain
public class BasicTerrainGeneration : MonoBehaviour
{

	public GameObject cubePrefab;

    [Header("Grid")]

    [SerializeField] int mapWidth;
	[SerializeField] int mapLength;

	[SerializeField][Range(0, .2f)] float increment;

	[SerializeField] [Range(0, 50)] int HeightDifference;

	List<GameObject> cubes = new();

    [Header("Visuals")]

    [SerializeField] float colorMultiplier;

    [Header("BiomeData")]
    public AllBiomeData biomeData;

	private void Start()
	{
		GenerateCubes();
	}

	void GenerateCubes()
	{
		ClearCurrentCubes();

		Vector2 RandOffset = GetRandomOffset();

        // determine biomes
        GenerateBiomes();

		cubes = CreateCubes(false);

		void ClearCurrentCubes()
		{
            foreach (GameObject c in cubes)
            {
                Destroy(c);
            }

            cubes.Clear();
        }

		Vector2 GetRandomOffset()
		{
            int xRandOffset = Random.Range(-100, 100);
            int yRandOffset = Random.Range(-100, 100);

			return new Vector2(xRandOffset, yRandOffset);
        }

        List<GameObject> CreateCubes(bool generateBiome)
        {
            List<GameObject> tempCubes = new();

            for (int w = 0; w < mapWidth; w++)
            {
                for (int l = 0; l < mapLength; l++)
                {
                    int height = (int)(Mathf.PerlinNoise((w + RandOffset.x) * increment, (l + RandOffset.y) * increment) * HeightDifference);

                    GameObject cube = Instantiate(cubePrefab, new Vector3(w, height, l), Quaternion.identity);
                    tempCubes.Add(cube);

                    MeshRenderer mr = cube.GetComponent<MeshRenderer>();

                    float _colorMultiplier = 1 / (float)HeightDifference;

                    mr.material.color = new Color(height * _colorMultiplier, height * _colorMultiplier, height * _colorMultiplier);
                }
            }

            return tempCubes;
        }

        void GenerateBiomes()
        {

        }

    }

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			GenerateCubes();
		}
	}

    [ContextMenu("Generate Cubes")]
    public void GenerateCubesRandomly()
    {
        GenerateCubes();
    }

}
