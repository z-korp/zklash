
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;

public enum Role {
    None,
    Knight,
    Bowman,
    Pawn,
    Torchoblin,
    Dynamoblin,
    Bomboblin,
}

public class ItemPlacer : MonoBehaviour
{
    
    public Tilemap tilemap;
    public GameObject[] unitPrefabs; // Préfabs pour les trois premiers éléments
    public GameObject[] objectPrefabs; // Préfabs pour les deux derniers éléments
    private Dictionary<Role, string> nameToRoleMap;
    float columnTileYOffset = 0.75f; 

    private int unitCount = 0;

    private bool hasPlacedItems = false; // Flag to track if items have been placed

    void Awake()
    {
        InitializeNameToRoleMap();
    }

    void Update()
    {
        // Check if shopEntity is not null and items haven't been placed yet
        if (!string.IsNullOrEmpty(PlayerData.Instance.shopEntity) && !hasPlacedItems)
        {
            PlaceItemsAboveColumns();
            hasPlacedItems = true; // Ensure this block only runs once
        }
    }

    void InitializeNameToRoleMap()
    {
        nameToRoleMap = new Dictionary<Role, string>
        {
            { Role.Knight, "Warrior_Blue" },
            { Role.Bowman, "Archer" },
            { Role.Pawn, "Pawn" },
            { Role.Torchoblin, "TorchoblinPrefab" },
            { Role.Dynamoblin, "Dynamite" },
            { Role.Bomboblin, "Bomboblin" },
        };
    }

    private GameObject FindPrefabByName(GameObject[] prefabs, string prefabName)
    {
        foreach (var prefab in prefabs)
        {
            if (prefab.name == prefabName)
            {
                return prefab;
            }
        }
        return null; // Return null if no prefab with the given name is found
    }

    private uint[] SplitRoles(uint roles)
    {
        string hexStr = roles.ToString("X6");
        List<string> hexParts = new List<string>();
        for (int i = 0; i < hexStr.Length; i += 2)
        {
            hexParts.Add(hexStr.Substring(i, 2));
        }

        // Convert each hex part back to decimal
        List<uint> decimals = hexParts.ConvertAll(part => Convert.ToUInt32(part, 16));

        // Convert List<uint> to uint[]
        return decimals.ToArray();
    }
 
    public void PlaceItemsAboveColumns()
    {
        var shopEntity = PlayerData.Instance.shopEntity;
        if(shopEntity == null)
        {
            Debug.LogError("Shop entity not found");
            return;
        }
        var shop = GameManager.Instance.worldManager.Entity(shopEntity).GetComponent<Shop>();
        var roles = SplitRoles(shop.roles);
        Debug.Log($"Roles: {roles[0]}, {roles[1]}, {roles[2]}");

        TileBase[] allTiles = tilemap.GetTilesBlock(tilemap.cellBounds);

        for (int x = 0; x < tilemap.cellBounds.size.x; x++)
        {
            bool isPlaced = false;
            for (int y = tilemap.cellBounds.size.y - 1; y >= 0 && !isPlaced; y--)
            {
                int tileIndex = x + y * tilemap.cellBounds.size.x;

                 if (allTiles[tileIndex] != null)
                {
                    GameObject prefabToPlace;
                    // Utilisez le compteur pour déterminer quel tableau de préfabs utiliser
                    if (unitCount < 3)
                    {
                        var name = (Role)roles[unitCount];
                        prefabToPlace = FindPrefabByName(unitPrefabs, nameToRoleMap[name]);
                        unitCount++; // Incrémentez le compteur d'unités placées
                    }
                    else
                    {
                        prefabToPlace = objectPrefabs[0];
                    }
                   
                    Vector3Int cellPosition = new Vector3Int(tilemap.cellBounds.xMin + x, tilemap.cellBounds.yMin + y, 0);
                    Vector3 cellCenterWorld = tilemap.GetCellCenterWorld(cellPosition);
                    Vector3 placePosition = cellCenterWorld + new Vector3(0, tilemap.cellSize.y - columnTileYOffset, 0);
                    
                    Instantiate(prefabToPlace, placePosition, Quaternion.identity);
                    break; // Arrêtez la recherche de tuiles une fois que vous avez instancié un objet
                }
            }
        }
    }
}
