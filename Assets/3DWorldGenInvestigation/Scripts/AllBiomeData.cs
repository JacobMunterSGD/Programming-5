using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AllBiomeData", menuName = "ScriptableObjects/AllBiomeDataScriptableObjects", order = 1)]
public class AllBiomeData : ScriptableObject
{
    public List<BiomeData> BiomeList;
}

[Serializable]
public class BiomeData
{
    [SerializeField] string name;

    [SerializeField] private BiomeList biome;
    public BiomeList Biome
    {
        set { biome = value; }
        get { return biome; }
    }

    [SerializeField] private float increment;
    public float Increment
    {
        get { return increment; }  
        set { increment = value; }
    }

    [SerializeField] private int heightDifference;
    public int HeightDifference
    {
        get { return heightDifference; }
        set { heightDifference = value; }
    }    

}

public enum BiomeList
{
    noBiome,
    plains,
    mountains,
    wasteland
}
