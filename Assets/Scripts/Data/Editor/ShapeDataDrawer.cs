using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShapeData), false)]
[CanEditMultipleObjects]
[System.Serializable]
public class ShapeDataDrawer : Editor
{
    private ShapeData ShapeDataInstance => target as ShapeData;

    #region PREVIEW MESH
    // Biến phục vụ preview
    private PreviewRenderUtility previewRenderUtility;
    private Vector2 previewDir = new Vector2(120, -20);
    private Mesh previewMesh;
    private Material previewMaterial;

    private void OnEnable()
    {
        // Khởi tạo preview utility
        previewRenderUtility = new PreviewRenderUtility();
        previewRenderUtility.cameraFieldOfView = 30f;
        previewMaterial = new Material(Shader.Find("Standard"));
    }

    private void OnDisable()
    {
        previewRenderUtility?.Cleanup();
    }
    #endregion

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawShapePrefab();
        EditorGUILayout.Space();

        // --- Phần Mesh + Rotation ---
        DrawMeshSettings();
        EditorGUILayout.Space();

        ClearBoardButton();
        EditorGUILayout.Space();

        DrawColumnsInputFields();
        EditorGUILayout.Space();

        if (ShapeDataInstance.board != null && ShapeDataInstance.columns > 0 && ShapeDataInstance.rows > 0)
        {
            DrawBoardTable();
        }

        // Hiển thị preview nếu có mesh
        if (ShapeDataInstance.meshData != null)
        {
            DrawMeshPreview();
            EditorGUILayout.Space();
        }

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(ShapeDataInstance);
        }
    }

    private void ClearBoardButton()
    {
        if (GUILayout.Button("Clear Board"))
        {
            ShapeDataInstance.Clear();
        }
    }

    private void DrawShapePrefab()
    {
        EditorGUILayout.LabelField("Shape Prefabs", EditorStyles.boldLabel);
        ShapeDataInstance.shapePrefab = (Shape)EditorGUILayout.ObjectField("Shape Prefab", ShapeDataInstance.shapePrefab, typeof(Shape), false);
    }

    private void DrawMeshSettings()
    {
        EditorGUILayout.LabelField("Mesh Settings", EditorStyles.boldLabel);
        ShapeDataInstance.meshData = (Mesh)EditorGUILayout.ObjectField("Mesh Data", ShapeDataInstance.meshData, typeof(Mesh), false);
        ShapeDataInstance.rotation = EditorGUILayout.Vector3Field("Rotation", ShapeDataInstance.rotation);
    }

    private void DrawMeshPreview()
    {
        EditorGUILayout.LabelField("Mesh Preview", EditorStyles.boldLabel);

        Rect previewRect = GUILayoutUtility.GetRect(150, 150, GUIStyle.none);
        HandlePreviewEvents(previewRect);

        if (Event.current.type == EventType.Repaint)
        {
            previewRenderUtility.BeginPreview(previewRect, GUIStyle.none);

            var mesh = ShapeDataInstance.meshData;
            var bounds = mesh.bounds;

            // Tính toán kích thước và khoảng cách camera phù hợp
            float maxExtent = Mathf.Max(bounds.extents.x, bounds.extents.y, bounds.extents.z);
            float distance = maxExtent * 3f; // càng lớn càng zoom out xa hơn

            // Thiết lập camera
            previewRenderUtility.camera.transform.position = new Vector3(0, 0, -distance);
            previewRenderUtility.camera.transform.rotation = Quaternion.identity;
            previewRenderUtility.camera.nearClipPlane = 0.01f;
            previewRenderUtility.camera.farClipPlane = distance * 5f;

            // Ánh sáng
            previewRenderUtility.lights[0].intensity = 1f;
            previewRenderUtility.lights[0].transform.rotation = Quaternion.Euler(40, 40, 0);
            previewRenderUtility.lights[1].intensity = 1f;

            // Tính rotation hiển thị
            Quaternion rot = Quaternion.Euler(new Vector3(previewDir.y, -previewDir.x, 0));
            Matrix4x4 matrix = Matrix4x4.TRS(-bounds.center, rot, Vector3.one);

            // Vẽ mesh
            previewRenderUtility.DrawMesh(mesh, matrix, previewMaterial, 0);

            previewRenderUtility.camera.Render();
            Texture result = previewRenderUtility.EndPreview();
            GUI.DrawTexture(previewRect, result, ScaleMode.StretchToFill, false);
        }
    }

    private void HandlePreviewEvents(Rect rect)
    {
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        Event e = Event.current;

        if (e.type == EventType.MouseDrag && rect.Contains(e.mousePosition) && e.button == 0)
        {
            previewDir -= e.delta * 0.5f;
            e.Use();
            Repaint();
        }
    }

    private void DrawColumnsInputFields()
    {
        var columnsTemp = ShapeDataInstance.columns;
        var rowsTemp = ShapeDataInstance.rows;

        ShapeDataInstance.columns = EditorGUILayout.IntField("Columns", ShapeDataInstance.columns);
        ShapeDataInstance.rows = EditorGUILayout.IntField("Rows", ShapeDataInstance.rows);

        if ((ShapeDataInstance.columns != columnsTemp || ShapeDataInstance.rows != rowsTemp) &&
            ShapeDataInstance.columns > 0 && ShapeDataInstance.rows > 0)
        {
            ShapeDataInstance.CreateNewBoard();
        }
    }

    private void DrawBoardTable()
    {
        var tableStyle = new GUIStyle("box");
        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 32;

        var headerColumnStyle = new GUIStyle();
        headerColumnStyle.fixedWidth = 25;
        headerColumnStyle.alignment = TextAnchor.MiddleCenter;

        var rowStyle = new GUIStyle();
        rowStyle.fixedHeight = 25;
        rowStyle.alignment = TextAnchor.MiddleCenter;

        var dataFieldStyle = new GUIStyle(EditorStyles.miniButtonMid);
        dataFieldStyle.normal.background = Texture2D.grayTexture;
        dataFieldStyle.onNormal.background = Texture2D.whiteTexture;

        for(var row = 0; row < ShapeDataInstance.rows; row++)
        {
            EditorGUILayout.BeginHorizontal(headerColumnStyle);

            for(var column = 0; column < ShapeDataInstance.columns; column++)
            {
                EditorGUILayout.BeginHorizontal(rowStyle);
                var data = EditorGUILayout.Toggle(ShapeDataInstance.board[row].column[column], dataFieldStyle);
                ShapeDataInstance.board[row].column[column] = data;

                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
