using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// This script makes the terrain
public class BasicTerrainGeneration : MonoBehaviour
{

	public GameObject cubePrefab;

    [Header("Grid")]

    [SerializeField] int mapWidth;
	[SerializeField] int mapLength;

	[SerializeField][Range(0, .2f)] float increment;

	[SerializeField] [Range(0, 50)] int heightDifference;

	[SerializeField] List<CubeInfo> cubes = new();

    [Header("BiomeData")]
    public AllBiomeData biomeData;

    private float biomeGenerationIncrement;
    private int amountOfBiomes;

	private void Start()
	{
        amountOfBiomes = biomeData.BiomeList.Count - 1;

        foreach(BiomeData _biomeData in biomeData.BiomeList)
        {
            if (_biomeData.Biome == Biomes.noBiome)
            {
                biomeGenerationIncrement = _biomeData.Increment;
                break;
			}
        }

		GenerateCubes();
	}

	void GenerateCubes()
	{
		ClearCurrentCubes();

		Vector2 RandOffset = GetRandomOffset();

		cubes = CreateCubes();

        // NESTED FUNCTIONS FROM HERE

		void ClearCurrentCubes()
		{
            foreach (CubeInfo c in cubes)
            {
                Destroy(c.CubeGameObject);
            }

            cubes.Clear();
        }

		Vector2 GetRandomOffset()
		{
            int xRandOffset = Random.Range(-100, 100);
            int yRandOffset = Random.Range(-100, 100);

			return new Vector2(xRandOffset, yRandOffset);
        }

        List<CubeInfo> CreateCubes()
        {
            List<CubeInfo> tempCubes = new();

            for (int _width = 0; _width < mapWidth; _width++)
            {
                for (int _length = 0; _length < mapLength; _length++)
                {

                    //new Vector3(_width + RandOffset.x, _length + RandOffset.y, 0);

                    Biomes _tempBiome = GetBiome(new Vector2(_length, _width));

                    float _tempIncrement = increment;
                    int _tempHeightDifference = heightDifference;
                    Color _tempColor = Color.white;

                    foreach (BiomeData _BiomeData in biomeData.BiomeList)
                    {
                        if (_BiomeData.Biome == _tempBiome)
                        {
                            _tempIncrement = _BiomeData.Increment;
                            _tempHeightDifference = _BiomeData.HeightDifference;
                            _tempColor = _BiomeData.Color;
                            break;
						}
                    }

                    int height = (int)(Mathf.PerlinNoise((_width + RandOffset.x) * _tempIncrement, (_length + RandOffset.y) * _tempIncrement) * _tempHeightDifference);

                    Vector3 position = new Vector3(_width, height, _length);

                    GameObject _cube = Instantiate(cubePrefab, position, Quaternion.identity);

                    CubeInfo newCube = new CubeInfo(_cube, position, _tempColor, _tempBiome);

					tempCubes.Add(newCube);

                    MeshRenderer mr = _cube.GetComponent<MeshRenderer>();
                    mr.material.color = _tempColor;

					//float _colorMultiplier = 1 / (float)heightDifference;
					//mr.material.color = new Color(height * _colorMultiplier, height * _colorMultiplier, height * _colorMultiplier);
				}
			}

            return tempCubes;
        }

        Biomes GetBiome(Vector2 pos)
		{

            int biomeIndex = (int)(Mathf.PerlinNoise((pos.x + RandOffset.x) * biomeGenerationIncrement, (pos.y + RandOffset.y) * biomeGenerationIncrement) * amountOfBiomes);

            foreach (BiomeData _biomeData in biomeData.BiomeList)
            {
                if (biomeData.BiomeList[biomeIndex + 1] == _biomeData)
                {
                    return _biomeData.Biome;
				}
            }

            print("something's not working if we're here");
            return Biomes.plains;
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
