using UnityEngine;
using UnityEditor;

public class MapTileGeneratorEditorWindow : EditorWindow
{
    public int Width;
    public int Height;
    public GameObject Parent;
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

        Width = EditorGUILayout.IntField("Width", Width);
        Height = EditorGUILayout.IntField("Height", Height);
        Parent = (GameObject)EditorGUILayout.ObjectField("Parent", Parent, typeof(GameObject), allowSceneObjects: true);
        MapTilePrefab = (GameObject)EditorGUILayout.ObjectField("Map Tile", MapTilePrefab, typeof(GameObject), allowSceneObjects: false);

        // Generate all of the map tiles.
        if (GUILayout.Button("Generate"))
        {
            for(int x = 0; x < Width; x++)
            {
                for(int y = 0; y < Height; y++)
                {
                    GameObject.Instantiate(
                        MapTilePrefab, 
                        new Vector3(
                            x * MapTile.Width, 
                            0,
                            y * MapTile.Width), 
                        Quaternion.identity, Parent.transform);
                }
            }
        }
    }
}