using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] Transform _blockParents;
    [SerializeField] BlockEditor _blockPrefab;
    [SerializeField] ShapeData[] _shapeDatas;

    public float spacing = 2;
    
    private void Start()
    {
        GenerateBlocks();
    }

    #region BLOCK GENERATE
    public void GenerateBlocks()
    {
        int rows = 2;
        int cols = 7;
        float spacing = 3f;

        for (int i = 0; i < _shapeDatas.Length; i++)
        {
            int row = i / cols;
            int col = i % cols;

            // Tính offset để căn giữa toàn bộ grid
            float xOffset = (cols - 1) * spacing * 0.5f;
            float zOffset = (rows - 1) * spacing * 0.5f;

            BlockEditor block = Instantiate(_blockPrefab, _blockParents);
            block.Initialize(_shapeDatas[i]);

            block.transform.localPosition = new Vector3(
                col * spacing - xOffset,
                0,
                -row * spacing + zOffset
            );
        }
    }
    #endregion
}
