using UnityEngine;
using UnityEditor;

public class MapTileGeneratorEditorWindow : EditorWindow
{
    public GameObject Map;
    public GameObject MapTilePrefab;

    // Add menu item to the Window menu
    [MenuItem("Window/Map Tile Generator")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MapTileGeneratorEditorWindow window = (MapTileGeneratorEditorWindow)EditorWindow.GetWindow(typeof(MapTileGeneratorEditorWindow));
        window.Show();
    }

    void OnGUI()
    {
        Map = (GameObject)EditorGUILayout.ObjectField("Map", Map, typeof(GameObject), allowSceneObjects: true);
        MapTilePrefab = (GameObject)EditorGUILayout.ObjectField("Map Tile", MapTilePrefab, typeof(GameObject), allowSceneObjects: false);

        // Generate all of the map tiles.
        if (GUILayout.Button("Generate"))
        {
            GameMap gameMap = Map.GetComponent<GameMap>();
            for(int x = 0; x < gameMap.Width; x++)
            {
                for(int y = 0; y < gameMap.Height; y++)
                {
                    var mapTileObject = GameObject.Instantiate(
                        MapTilePrefab, 
                        new Vector3(
                            x * MapTile.Width, 
                            0,
                            y * MapTile.Width), 
                        Quaternion.identity, Map.transform);

                    // Initialize the map.
                    MapTile mapTile = mapTileObject.GetComponent<MapTile>();
                    mapTile.transform.name = string.Format("MapTile({0},{1})", x, y);
                    mapTile.Position = new Vector2(x, y);
                }
            }
        }
    }

    
}