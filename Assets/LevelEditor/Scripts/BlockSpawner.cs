using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] Transform _blockParents;
    [SerializeField] BlockEditor _blockPrefab;
    [SerializeField] BlocksVisualSO data;

    public float spacing = 2;
    public int rows = 3;
    public int cols = 9;
    
    private void Start()
    {
        GenerateBlocks();
    }

    #region BLOCK GENERATE
    public void GenerateBlocks()
    {
        float spacing = 3f;

        for (int i = 0; i < data.shapes.Length; i++)
        {
            int row = i / cols;
            int col = i % cols;

            // Tính offset để căn giữa toàn bộ grid
            float xOffset = (cols - 1) * spacing * 0.5f;
            float zOffset = (rows - 1) * spacing * 0.5f;

            BlockEditor block = Instantiate(_blockPrefab, _blockParents);
            block.Initialize(data.shapes[i]);

            block.transform.localPosition = new Vector3(
                col * spacing - xOffset,
                0,
                -row * spacing + zOffset
            );
        }
    }
    #endregion
}
