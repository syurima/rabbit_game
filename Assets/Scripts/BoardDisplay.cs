using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardDisplay : MonoBehaviour
{
    public int width;
    public int height;
    public static BoardDisplay instance;

    public Tilemap floor;
    public Tilemap walls;
    public Tile WallRight;
    public Tile WallLeft;
    public Tile WallTop;
    public Tile WallBottom;
    public Tile WallTopLeft;
    public Tile WallTopRight;
    public Tile WallBottomLeft;
    public Tile WallBottomRight;

    public TileBase[] grassTiles;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Clear()
    {
        floor.ClearAllTiles();
        walls.ClearAllTiles();
    }

    public void DrawMap()
    {
        Clear();
        width = GameController.instance.mapSizeX;
        height = GameController.instance.mapSizeY;
        DrawFloor();
        DrawWalls();
    }

    public void DrawWalls()
    {
        Vector3Int position = new Vector3Int(0, 0, 0);
        
        // Top wall
        for (int x = 0; x <= width; x++)
        {
            position.Set(x, height, 0);
            walls.SetTile(position, WallTop);
        }

        // Bottom wall
        for (int x = -1; x <= width; x++)
        {
            position.Set(x, -1, 0);
            walls.SetTile(position, WallBottom);
        }

        // Left wall
        for (int y = 0; y < height; y++)
        {
            position.Set(-1, y, 0);
            walls.SetTile(position, WallLeft);
        }

        // Right wall
        for (int y = 0; y < height; y++)
        {
            position.Set(width, y, 0);
            walls.SetTile(position, WallRight);
        }

        // set corners
        position.Set(-1, -1, 0);
        walls.SetTile(position, WallBottomLeft);
        position.Set(width, -1, 0);
        walls.SetTile(position, WallBottomRight);
        position.Set(-1, height, 0);
        walls.SetTile(position, WallTopLeft);
        position.Set(width, height, 0);
        walls.SetTile(position, WallTopRight);
    }

    public void DrawFloor()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Set the tile at the position (x, y) to a random grass tile
                Vector3Int position = new Vector3Int(x, y, 0);
                floor.SetTile(position, GetRandomGrassTile());
            }
        }  
    }
    
    private bool IsOutOfBounds(int x, int y)
    {
        return x < 0 || x >= width || y < 0 || y >= height;
    }

    // private TileBase GetWallTile(int x, int y)
    // {
    //     // Determine the type of wall tile based on coords
    //     if (x == -1){
    //         if (y == -1) return WallTopLeft;
    //         if (y == height + 1) return WallBottomLeft;
    //         return WallLeft;
    //     }
    //     else if (x == width + 1){
    //         if (y == -1) return WallTopRight;
    //         if (y == height + 1) return WallBottomRight;
    //         return WallRight;
    //     }
    //     else if (y == -1) return WallTop;
    //     else if (y == height + 1) return WallBottom;

    //     return null; // No tile for other cases
    // }

    private TileBase GetRandomGrassTile()
    {
        // Return a random grass tile from the array
        return grassTiles[Random.Range(0, grassTiles.Length)];
    }
}
