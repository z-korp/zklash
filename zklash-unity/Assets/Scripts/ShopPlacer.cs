using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemPlacer : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject prefabToPlace;
    float columnTileYOffset = 0.75f; 

    void Start()
{
    PlaceItemsAboveColumns();
}
 
    public void PlaceItemsAboveColumns()
    {
        TileBase[] allTiles = tilemap.GetTilesBlock(tilemap.cellBounds);

        for (int x = 0; x < tilemap.cellBounds.size.x; x++)
        {
            for (int y = tilemap.cellBounds.size.y - 1; y >= 0; y--)
            {
                int tileIndex = x + y * tilemap.cellBounds.size.x;
                if (allTiles[tileIndex] != null)
                {
                   
                    Vector3Int cellPosition = new Vector3Int(tilemap.cellBounds.xMin + x, tilemap.cellBounds.yMin + y, 0);
                    Vector3 cellCenterWorld = tilemap.GetCellCenterWorld(cellPosition);
                    
                    
                    Vector3 placePosition = cellCenterWorld + new Vector3(0, tilemap.cellSize.y - columnTileYOffset, 0);
                    
                   
                    Instantiate(prefabToPlace, placePosition, Quaternion.identity);
                    break; 
                }
            }
        }
    }
}
