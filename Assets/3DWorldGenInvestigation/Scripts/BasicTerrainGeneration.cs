using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTerrainGeneration : MonoBehaviour
{

	public GameObject cubePrefab;

    [Header("Grid")]

    [SerializeField] int width;
	[SerializeField] int length;

	[SerializeField] float increment;

	[SerializeField] int HeightDifference;

	[SerializeField] float timeBetweenCubes;

	List<GameObject> cubes = new();

    [Header("Visuals")]

    [SerializeField] float colorMultiplier;


	private void Start()
	{
		GenerateCubesNC(false);
		//StartCoroutine(GenerateCubes());
	}

	IEnumerator GenerateCubes() // UPDATE WITH NEW CODE BEFORE USING AGAIN
	{
		for (int w = 0; w < width; w++)
		{
			for (int l = 0; l < length; l++)
			{
				int height = (int)(Mathf.PerlinNoise(w * increment, l * increment) * 10);

				print(height);

				Instantiate(cubePrefab, new Vector3(w, height, l), Quaternion.identity);

				yield return new WaitForSeconds(timeBetweenCubes);
			}
		}
	}

	void GenerateCubesNC(bool isRandom)
	{
		foreach(GameObject c in cubes)
		{
			Destroy(c);
		}

		cubes.Clear();

		int xRandOffset = 0;
		int yRandOffset = 0;

		if (isRandom)
		{
            xRandOffset = Random.Range(-100, 100);
            yRandOffset = Random.Range(-100, 100);
        }

		for (int w = 0; w < width; w++)
		{
			for (int l = 0; l < length; l++)
			{
				int height = (int)(Mathf.PerlinNoise((w + xRandOffset) * increment, (l+ yRandOffset) * increment) * HeightDifference);

				print(height);

				GameObject cube = Instantiate(cubePrefab, new Vector3(w, height, l), Quaternion.identity);
				cubes.Add(cube);

				MeshRenderer mr = cube.GetComponent<MeshRenderer>();

				float _colorMultiplier = 1 / (float)HeightDifference;

				print(_colorMultiplier);

				mr.material.color = new Color(height * _colorMultiplier, height * _colorMultiplier, height * _colorMultiplier);
			}
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			GenerateCubesNC(true);
		}
	}

}
