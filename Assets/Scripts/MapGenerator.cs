using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public float baseHeight = 0.5f;
    public Vector2 startSize = Vector2.one * 10;
    public Vector2 safeZone = Vector2.one * 2;
    public float cellSize = 5;
    public float scaleRandomness = 2;
    public float MinimumScale = 1;

    public float startDensity;
    public float minDensity = 1;
    public float densityIncreaser;
    public float leftDensity;
    public float rightDensity;
    public float bottomDensity;

    public Transform root;

    [SerializeField] GameObject Background;
    [SerializeField] GameObject[] trashes;
    [SerializeField] GameObject water;
    [SerializeField] GameObject nutriment;

    Vector2 min = Vector2.zero;
    Vector2 max = Vector2.zero;

    Vector2Int size = Vector2Int.zero;

    // Start is called before the first frame update
    void Start()
    {
        Generate(Vector3.zero, startDensity, true);
        min.x = -startSize.x * cellSize / 2;
        min.y = -startSize.y * cellSize;
        max.x = startSize.x * cellSize / 2;
        max.y = 0;
        size = Vector2Int.one;
        leftDensity = startDensity + densityIncreaser;
        rightDensity = startDensity + densityIncreaser;
        bottomDensity = startDensity + densityIncreaser;
    }

    public void Generate(Vector3 position, float density, bool isSafeZone = false)
    {
        Transform bg = Instantiate(Background, position, Quaternion.identity).transform;
        float half = safeZone.x / 2;
        int countX = (int)(startSize.x * cellSize / density);
        int halfCountX = countX / 2;
        int countY = (int)(startSize.y * cellSize / density);
        for (int i = 0; i < countX; i++)
        {
            for (int j = 1; j <= countY; j++)
            {
                int rand = Random.Range(0, 2);
                GameObject toSpawn = rand == 0 ? water : nutriment;
                float scale = Random.Range(MinimumScale, MinimumScale + scaleRandomness);
                float range = Mathf.Max(0, density - scale);
                float halfRange = range / 2;
                Vector3 pos = position + new Vector3((i - halfCountX) * density + halfRange + Random.Range(0f, range), -baseHeight - j * density - halfRange + Random.Range(0f, range), 0);
                GameObject go = Instantiate(toSpawn, pos, Quaternion.identity, bg);
                go.GetComponent<Pickable>().SetSize(scale);

                pos = position + new Vector3((i - halfCountX) * density + Random.Range(0f, range), -baseHeight - j * density + Random.Range(0f, range), 0);
                if (isSafeZone && pos.y > -safeZone.y && pos.x < half && pos.x > -half)
                    continue;

                go = Instantiate(trashes[Random.Range(0, trashes.Length)], pos, Quaternion.Euler(0, 0, Random.Range(0, 360)), bg);
            }
        }
    }

    private void Update()
    {
        float d = cellSize * startSize.x / 2;
        if (root.position.x < min.x + d)
        {
            GenerateLeft();
        }

        if (root.position.x > max.x - d)
        {
            GenerateRight();
        }

        if (root.position.y < min.y + d)
        {
            GenerateBottom();
        }
    }

    void GenerateLeft()
    {
        Vector3 pos = Vector3.zero;
        pos.x = min.x - (cellSize * startSize.x / 2);
        for (int i = 0; i < size.y; i++)
        {
            pos.y = -i * cellSize * startSize.y;
            Generate(pos, leftDensity);
        }
        size.x++;
        leftDensity += densityIncreaser;
        if (leftDensity < minDensity)
            leftDensity = minDensity;
        min.x -= cellSize * startSize.x;
    }

    void GenerateRight()
    {
        Vector3 pos = Vector3.zero;
        pos.x = max.x + (cellSize * startSize.x / 2);
        for (int i = 0; i < size.y; i++)
        {
            pos.y = -i * cellSize * startSize.y;
            Generate(pos, rightDensity);
        }
        size.x++;
        rightDensity += densityIncreaser;
        if (rightDensity < minDensity)
            rightDensity = minDensity;
        max.x += cellSize * startSize.x;
    }

    void GenerateBottom()
    {
        Debug.Log("Generating bottom");
        Vector3 pos = Vector3.zero;
        pos.y = min.y;
        float start = min.x + cellSize * startSize.x / 2;
        for (int i = 0; i < size.x; i++)
        {
            pos.x = start + i * cellSize * startSize.x;
            Generate(pos, bottomDensity);
        }
        size.y++;
        bottomDensity += densityIncreaser;
        if (bottomDensity < minDensity)
            bottomDensity = minDensity;
        min.y -= cellSize * startSize.y;
    }
}
