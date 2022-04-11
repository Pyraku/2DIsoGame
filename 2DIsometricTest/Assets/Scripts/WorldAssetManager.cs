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
