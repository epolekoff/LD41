using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMap : MonoBehaviour
{
    private const float ValidNeighborHeightDifference = 1;

    public int Width;
    public int Height;

    public GameObject[,] ObjectsOnTiles = new GameObject[50, 50];
    public Dictionary<Vector2, MapTile> MapTiles = new Dictionary<Vector2, MapTile>();

    private const float ShooterLerpTimePerTimeTraveled = 0.1f;

    private List<MapTile> m_highlightedTiles = new List<MapTile>();

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    /// <summary>
    /// Get the shooter on a tile, or null if there is none.
    /// </summary>
    public Shooter GetShooterOnTile(Vector2 tile)
    {
        GameObject objectOnTile = ObjectsOnTiles[(int)tile.x, (int)tile.y];

        if (objectOnTile == null)
        {
            return null;
        }

        Shooter shooter = objectOnTile.GetComponent<Shooter>();
        return shooter;
    }

    /// <summary>
    /// Move an object to a new tile. Record where it is now.
    /// </summary>
    public void MoveObjectToTile(Shooter shooter, int x, int y, bool firstTimeSetup = false, System.Action callback = null)
    {
        // Get the shooter's old position
        Vector2 oldPosition = shooter.TilePosition;

        // Move the object to the new position.
        ObjectsOnTiles[x, y] = shooter.gameObject;
        shooter.TilePosition = new Vector2(x, y);

        int distanceTraveled = (int)Mathf.Abs(oldPosition.x - x) + (int)Mathf.Abs(oldPosition.y - y);

        // Move the game object
        Vector3 desiredPosition = new Vector3(
            x * MapTile.Width,
            MapTiles[shooter.TilePosition].Height,
            y * MapTile.Width);
        if (firstTimeSetup)
        {
            shooter.transform.position = desiredPosition;
        }
        else
        {
            StartCoroutine(LerpObjectToPosition(shooter.transform, desiredPosition, distanceTraveled, callback));
        }

        // Delete the object from the old position
        if (!firstTimeSetup)
        {
            ObjectsOnTiles[(int)oldPosition.x, (int)oldPosition.y] = null;
        }
    }

    /// <summary>
    /// Get the neighbors of this tile.
    /// </summary>
    public List<Vector2> GetValidNeighbors(Vector2 tilePosition)
    {
        int x = (int)tilePosition.x;
        int y = (int)tilePosition.y;

        MapTile mapTile = MapTiles[tilePosition];

        List<Vector2> neighbors = new List<Vector2>();

        Vector2 left    = new Vector2(x - 1, y);
        Vector2 right   = new Vector2(x + 1, y);
        Vector2 up      = new Vector2(x, y - 1);
        Vector2 down    = new Vector2(x, y + 1);

        if (IsValidTile(left) && Mathf.Abs(MapTiles[left].Height - mapTile.Height) <= ValidNeighborHeightDifference)
            neighbors.Add(left);
        if (IsValidTile(right) && Mathf.Abs(MapTiles[right].Height - mapTile.Height) <= ValidNeighborHeightDifference)
            neighbors.Add(right);
        if (IsValidTile(up) && Mathf.Abs(MapTiles[up].Height - mapTile.Height) <= ValidNeighborHeightDifference)
            neighbors.Add(up);
        if (IsValidTile(down) && Mathf.Abs(MapTiles[down].Height - mapTile.Height) <= ValidNeighborHeightDifference)
            neighbors.Add(down);

        return neighbors;
    }

    /// <summary>
    /// Is it valid to move to this tile.
    /// </summary>
    public bool IsValidTile(Vector2 tilePosition)
    {
        // Check against the map bounds.
        if(tilePosition.x < 0 || tilePosition.x >= Width || 
            tilePosition.y < 0 || tilePosition.y >= Height)
        {
            return false;
        }

        // Check other player objects on the tile.
        if(GetShooterOnTile(tilePosition))
        {
            return false;
        }

        // Check if there are any obstacles on the tile.
        MapTile tile = MapTiles[tilePosition];
        if(tile.Obstacle != null)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Recurse through all neighbors and get a list of tiles in range, without duplicates.
    /// </summary>
    public List<Vector2> GetAllTilesInRange(Vector2 tile, int range)
    {
        // Get all tiles, filter out duplicates.
        HashSet<Vector2> tilesInRangeNoDuplicates = new HashSet<Vector2>();
        List<Vector2> allTilesInRange = GetAllTilesInRangeRecursive(tile, 0, range);
        foreach(var potentialTile in allTilesInRange)
        {
            tilesInRangeNoDuplicates.Add(potentialTile);
        }

        return new List<Vector2>(tilesInRangeNoDuplicates);
    }

    private List<Vector2> GetAllTilesInRangeRecursive(Vector2 tile, int depth, int maxDepth)
    {
        if(depth == maxDepth)
        {
            List<Vector2> list = new List<Vector2>();
            if(IsValidTile(tile))
            {
                list.Add(tile);
            }
            return list;
        }

        // Keep recursing.
        List<Vector2> totalList = new List<Vector2>();
        if (IsValidTile(tile))
        {
            totalList.Add(tile);
        }
        foreach (var neighbor in GetValidNeighbors(tile))
        {
            totalList.AddRange(GetAllTilesInRangeRecursive(neighbor, depth + 1, maxDepth));
        }
        return totalList;
    }

    /// <summary>
    /// Highlight the tiles that indicate this player's movement range.
    /// </summary>
    public void HighlightMovementRange(Shooter shooter, Player player)
    {
        ClearHighlightedTiles();

        HighlightState highlight = player.Number == shooter.TeamId ? HighlightState.Friendly : HighlightState.Enemy;

        List<Vector2> allTilesInRange = GetAllTilesInRange(shooter.TilePosition, shooter.Data.MoveRange);
        foreach (Vector2 tile in allTilesInRange)
        {
            if(MapTiles.ContainsKey(tile))
            {
                MapTile mapTile = MapTiles[tile];
                mapTile.ShowHighlight(highlight);
                m_highlightedTiles.Add(mapTile);
            }
        }
    }

    /// <summary>
    /// Hide the highlight on all tiles that have highlights.
    /// </summary>
    public void ClearHighlightedTiles()
    {
        foreach(var mapTile in m_highlightedTiles)
        {
            mapTile.ShowHighlight(HighlightState.None);
        }

        m_highlightedTiles.Clear();
    }

    /// <summary>
    /// Smoothely move shooter to the correct time.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LerpObjectToPosition(Transform transform, Vector3 desiredPosition, int distanceTraveled, System.Action callback)
    {
        float lerpTime = distanceTraveled * ShooterLerpTimePerTimeTraveled;
        Vector3 startPosition = transform.position;
        float timer = 0;

        while(timer < lerpTime)
        {
            timer += Time.deltaTime;
            var ratio = timer / lerpTime;

            transform.position = Vector3.Lerp(startPosition, desiredPosition, ratio);

            yield return new WaitForEndOfFrame();
        }

        if(callback != null)
        {
            callback();
        }
    }
}
