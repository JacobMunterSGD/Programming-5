using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTerrainGeneration : MonoBehaviour
{

	public GameObject cubePrefab;

	[SerializeField] int width;
	[SerializeField] int length;

	[SerializeField] float increment;

	[SerializeField] float timeBetweenCubes;

	List<GameObject> cubes = new();

	private void Start()
	{
		GenerateCubesNC();
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

	void GenerateCubesNC()
	{
		foreach(GameObject c in cubes)
		{
			Destroy(c);
		}

		cubes.Clear();

		for (int w = 0; w < width; w++)
		{
			for (int l = 0; l < length; l++)
			{
				int height = (int)(Mathf.PerlinNoise(w * increment, l * increment) * 10);

				print(height);

				GameObject cube = Instantiate(cubePrefab, new Vector3(w, height, l), Quaternion.identity);
				cubes.Add(cube);
			}
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			print("works");
			GenerateCubesNC();
		}
	}

}
