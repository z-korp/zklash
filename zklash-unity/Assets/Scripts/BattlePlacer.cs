using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BattlePlacer : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject[] unitPrefabs; 

    float columnTileYOffset = 0.75f; 

    private int prefabIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        PlaceUnits();
    }

    public void PlaceUnits()
    {
        TileBase[] allTiles = tilemap.GetTilesBlock(tilemap.cellBounds);

        for (int x = 0; x < tilemap.cellBounds.size.x; x++)
        {
            bool isPlaced = false;
            for (int y = tilemap.cellBounds.size.y - 1; y >= 0 && !isPlaced; y--)
            {
                int tileIndex = x + y * tilemap.cellBounds.size.x;

                 if (allTiles[tileIndex] != null)
                {

                     if (prefabIndex >= unitPrefabs.Length)
                    {
                        break;
                    }
                    
                    GameObject prefabToPlace = unitPrefabs[prefabIndex];
               
                    prefabIndex++;

                    Vector3Int cellPosition = new Vector3Int(tilemap.cellBounds.xMin + x, tilemap.cellBounds.yMin + y, 0);
                    Vector3 cellCenterWorld = tilemap.GetCellCenterWorld(cellPosition);
                    Vector3 placePosition = cellCenterWorld + new Vector3(0, tilemap.cellSize.y - columnTileYOffset, 0);
                    
                    Instantiate(prefabToPlace, placePosition, Quaternion.identity);
                    break; // Arrêtez la recherche de tuiles une fois que vous avez instancié un objet
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
