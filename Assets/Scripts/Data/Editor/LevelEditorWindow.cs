using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class LevelEditorWindow : EditorWindow
{
    [MenuItem("Tools/Level Editor")]
    private static void OpenWindow()
    {
        GetWindow<LevelEditorWindow>("Level Editor");
    }

    [System.Serializable]
    public class BlockData
    {
        public int id;
        public int row;
        public int col;
        public int width;
        public int height;
        public float rotation;
    }

    public int rows = 8;
    public int cols = 8;
    public float cellSize = 1f;

    private int blockID;
    private int blockWidth = 1;
    private int blockHeight = 1;
    private float blockRotation;
    private int blockRow;
    private int blockCol;

    private List<BlockData> blocks = new List<BlockData>();

    private void OnGUI()
    {
        EditorGUILayout.LabelField("🧱 GRID SETTINGS", EditorStyles.boldLabel);
        rows = EditorGUILayout.IntField("Rows", rows);
        cols = EditorGUILayout.IntField("Columns", cols);
        cellSize = EditorGUILayout.FloatField("Cell Size", cellSize);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("🧱 BLOCK SETTINGS", EditorStyles.boldLabel);
        blockID = EditorGUILayout.IntField("Block ID", blockID);
        blockWidth = EditorGUILayout.IntField("Width", blockWidth);
        blockHeight = EditorGUILayout.IntField("Height", blockHeight);
        blockRotation = EditorGUILayout.FloatField("Rotation", blockRotation);
        blockRow = EditorGUILayout.IntField("Row", blockRow);
        blockCol = EditorGUILayout.IntField("Col", blockCol);

        if (GUILayout.Button("+ Add Block"))
        {
            BlockData b = new BlockData
            {
                id = blockID,
                row = blockRow,
                col = blockCol,
                width = blockWidth,
                height = blockHeight,
                rotation = blockRotation
            };
            blocks.Add(b);
            Repaint();
            SceneView.RepaintAll();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("📋 Current Blocks: " + blocks.Count);

        if (blocks.Count > 0 && GUILayout.Button("🗑 Clear All"))
        {
            blocks.Clear();
            Repaint();
            SceneView.RepaintAll();
        }

        if (GUILayout.Button("💾 Save Level as JSON"))
        {
            string json = JsonUtility.ToJson(new BlockListWrapper { list = blocks }, true);
            string path = EditorUtility.SaveFilePanel("Save Level", "Assets", "level.json", "json");
            if (!string.IsNullOrEmpty(path))
            {
                System.IO.File.WriteAllText(path, json);
                AssetDatabase.Refresh();
            }
        }
    }

    private void OnFocus()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnLostFocus()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Handles.color = Color.gray;
        Vector3 origin = Vector3.zero;

        // Vẽ grid
        for (int r = 0; r <= rows; r++)
            Handles.DrawLine(origin + Vector3.forward * r * cellSize, origin + Vector3.right * cols * cellSize + Vector3.forward * r * cellSize);

        for (int c = 0; c <= cols; c++)
            Handles.DrawLine(origin + Vector3.right * c * cellSize, origin + Vector3.forward * rows * cellSize + Vector3.right * c * cellSize);

        // Vẽ block
        foreach (var b in blocks)
        {
            Vector3 pos = origin + new Vector3(b.col * cellSize, 0, b.row * cellSize);
            Vector3 size = new Vector3(b.width * cellSize, 0, b.height * cellSize);
            Vector3 center = pos + new Vector3(size.x / 2, 0, size.z / 2);

            Handles.color = Color.cyan;
            Handles.DrawSolidRectangleWithOutline(
                new Vector3[]
                {
                    center + new Vector3(-size.x/2, 0, -size.z/2),
                    center + new Vector3(-size.x/2, 0,  size.z/2),
                    center + new Vector3( size.x/2, 0,  size.z/2),
                    center + new Vector3( size.x/2, 0, -size.z/2)
                },
                new Color(0, 1, 1, 0.25f),
                Color.blue
            );

            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.black;
            style.alignment = TextAnchor.MiddleCenter;
            Handles.Label(center + Vector3.up * 0.1f, $"ID:{b.id}", style);
        }
    }

    [System.Serializable]
    public class BlockListWrapper
    {
        public List<BlockData> list;
    }
}
