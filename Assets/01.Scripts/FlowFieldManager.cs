using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Grid
{
    public int cost = int.MaxValue;

    public Vector2 direction;
    public readonly Vector2 position;

    public GameObject viewObject;
    public TMP_Text viewText;
    public Grid(Vector2 _position) => position = _position;
}

public class FlowFieldManager : MonoBehaviour
{
    public static FlowFieldManager Instance;

    [SerializeField, Header("맵 사이즈")] private Vector2 size;
    private Dictionary<Vector2, Grid> grid = new();

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        size = new Vector2((int)size.x, (int)size.y);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.transform.position, size);
    }
#endif

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            GridBake();
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }
    }

    private Grid SpawnView(Transform _parent, Vector2 _position, int _count)
    {
        var testGrid = new Grid(_position);

        var loadText = Resources.Load<GameObject>("GridText");
        var spawnText = MonoBehaviour.Instantiate(loadText);
        testGrid.viewText = spawnText.GetComponent<TMP_Text>();

        var loadObject = Resources.Load<GameObject>("GridArrow");
        var spawnObejct = MonoBehaviour.Instantiate(loadObject);
        testGrid.viewObject = spawnObejct;
        testGrid.viewObject.gameObject.name = $"{loadObject.name} {_count}";

        testGrid.viewObject.transform.position = _position;
        testGrid.viewObject.transform.SetParent(_parent.transform);
        spawnText.transform.position = testGrid.viewObject.transform.position;
        spawnText.transform.SetParent(testGrid.viewObject.transform);

        return testGrid;
    }

    private void GridBake()
    {   //test
        //var tempList = new List<Grid>();
        var spanwCount = 0;
        var parent = new GameObject("AllGrid");
        var mapLayer = LayerMask.GetMask("Map");

        var originPos = size * -0.5f;
        originPos.y += 1f;

        var spawnPos = originPos;
        var isBlock = false;

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                var gridPos = new Vector2(spawnPos.x, spawnPos.y);
                var rayPos = new Vector2(spawnPos.x + 0.5f, spawnPos.y - 0.5f);
                var isGrid = Physics2D.Raycast(rayPos, Vector2.up, 0.2f, mapLayer);

                if ((!isBlock && isGrid) || (isBlock && !isGrid))
                {
                    isBlock = !isBlock;
                    //grid.Add(gridPos, new Grid(gridPos));
                    spanwCount++;
                    var newGrid = SpawnView(parent.transform, gridPos, spanwCount);

                    grid.Add(gridPos, newGrid);
                    //tempList.Add(newGrid);
                }

                spawnPos.x++;
            }

            spawnPos.y++;
            spawnPos.x = originPos.x;
        }

        //test
        //testGrid = tempList.ToArray();
    }
}
