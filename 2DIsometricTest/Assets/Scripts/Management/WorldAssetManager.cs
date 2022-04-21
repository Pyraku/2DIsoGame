using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldAssetManager : MonoBehaviour
{
    [SerializeField] private List<Asset> m_assets = new List<Asset>();

    public GameObject GetAsset(Asset.AssetKey key)
    {
        Asset a = m_assets.Find(x => x.key == key);
        return a.asset;
    }

    public GameObject[] CreateAssetList(Asset.AssetKey key , int amount, Transform parent)
    {
        GameObject[] assets = new GameObject[amount];
        for(int i = 0; i < amount; i++)
        {
            assets[i] = Instantiate(GetAsset(key), parent);
        }
        return assets;
    }

    public T[] CreateAssetList<T>(Asset.AssetKey key, int amount, Transform parent)
    {
        T[] assets = new T[amount];
        for (int i = 0; i < amount; i++)
            assets[i] = Instantiate(GetAsset(key), parent).GetComponent<T>();
        return assets;
    }
}

[System.Serializable]
public struct Asset
{
    public enum AssetKey
    {
        BaseFloor,
        BaseWall,
        BaseDoor,
        BaseCharacter,
    }
    public string assetName;

    public AssetKey key;

    public GameObject asset;
}