
using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemPlacer : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject[] unitPrefabs; // Préfabs pour les trois premiers éléments
    public GameObject[] objectPrefabs; // Préfabs pour les deux derniers éléments
    float columnTileYOffset = 0.75f; 

    private int unitCount = 0;

    void Start()
    {
        PlaceItemsAboveColumns();
    }
 
    public void PlaceItemsAboveColumns()
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
                    GameObject prefabToPlace;
                    // Utilisez le compteur pour déterminer quel tableau de préfabs utiliser
                    if (unitCount < 3)
                    {
                        prefabToPlace = unitPrefabs[Random.Range(0, unitPrefabs.Length)];
                        unitCount++; // Incrémentez le compteur d'unités placées
                    }
                    else
                    {
                        prefabToPlace = objectPrefabs[Random.Range(0, objectPrefabs.Length)];
                    }
                   
                    Vector3Int cellPosition = new Vector3Int(tilemap.cellBounds.xMin + x, tilemap.cellBounds.yMin + y, 0);
                    Vector3 cellCenterWorld = tilemap.GetCellCenterWorld(cellPosition);
                    Vector3 placePosition = cellCenterWorld + new Vector3(0, tilemap.cellSize.y * columnTileYOffset, 0);
                    
                    Instantiate(prefabToPlace, placePosition, Quaternion.identity);
                    break; // Arrêtez la recherche de tuiles une fois que vous avez instancié un objet
                }
            }
        }
    }
}
