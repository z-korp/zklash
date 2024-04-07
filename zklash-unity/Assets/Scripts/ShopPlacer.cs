
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;

public class ItemPlacer : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject[] unitPrefabs; // Préfabs pour les trois premiers éléments
    public GameObject[] objectPrefabs; // Préfabs pour les deux derniers éléments

    float columnTileYOffset = 0.75f;

    private uint unitCount = 0;

    private bool hasPlacedItems = false; // Flag to track if items have been placed

    void Update()
    {
        // Check if shopEntity is not null and items haven't been placed yet
        if (!string.IsNullOrEmpty(PlayerData.Instance.shopEntity) && !hasPlacedItems)
        {
            PlaceItemsAboveColumns();
            hasPlacedItems = true; // Ensure this block only runs once
        }
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
        if (shopEntity == null)
        {
            Debug.LogError("Shop entity not found");
            return;
        }
        var shop = GameManager.Instance.worldManager.Entity(shopEntity).GetComponent<Shop>();
        var roles = SplitRoles(shop.roles);
        Debug.Log($"Roles: {roles[0]}, {roles[1]}, {roles[2]}");
        var item = shop.items;
        Debug.Log($"Items: {item}");

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
                    uint index;
                    // Utilisez le compteur pour déterminer quel tableau de préfabs utiliser
                    if (unitCount < 3)
                    {
                        var name = (Role)roles[unitCount];
                        prefabToPlace = PrefabUtils.FindPrefabByName(unitPrefabs, PrefabMappings.NameToRoleMap[name]);
                        if (prefabToPlace == null)
                        {
                            Debug.LogError($"Prefab not found for role: {name}");
                            return;
                        }
                        index = unitCount;
                        unitCount++; // Incrémentez le compteur d'unités placées
                    }
                    else
                    {
                        var name = (Item)item;
                        prefabToPlace = PrefabUtils.FindPrefabByName(objectPrefabs, PrefabMappings.NameToItemMap[name]);
                        if (prefabToPlace == null)
                        {
                            Debug.LogError($"Prefab not found for item: {name}");
                            return;
                        }
                        index = 0;
                    }

                    Vector3Int cellPosition = new Vector3Int(tilemap.cellBounds.xMin + x, tilemap.cellBounds.yMin + y, 0);
                    Vector3 cellCenterWorld = tilemap.GetCellCenterWorld(cellPosition);
                    Vector3 placePosition = cellCenterWorld + new Vector3(0, tilemap.cellSize.y - columnTileYOffset, 0);

                    GameObject instance = Instantiate(prefabToPlace, placePosition, Quaternion.identity);
                    ElementData data = instance.GetComponent<ElementData>();
                    if (data != null)
                    {
                        var name = (Role)roles[unitCount];
                        if (name == Role.Knight)
                        {
                            data.currentHealth = 3;
                            data.currentDamage = 1;
                        }
                        else if (name == Role.Bowman)
                        {
                            data.currentHealth = 2;
                            data.currentDamage = 2;
                        }
                        else if (name == Role.Pawn)
                        {
                            data.currentHealth = 2;
                            data.currentDamage = 1;
                        }
                        data.indexFromShop = index;
                    }
                    break; // Arrêtez la recherche de tuiles une fois que vous avez instancié un objet
                }
            }
        }
    }
}
