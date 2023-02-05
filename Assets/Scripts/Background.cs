using UnityEngine;

public class Background : MonoBehaviour
{
    public float size;
    public float objectDensity;
    public Sprite[] details;

    public Sprite[] objects;

    private static int order = 0;
    private static int objectOrder = 0;

    // Start is called before the first frame update
    void Start()
    {
        float offset = details[0].textureRect.width / details[0].pixelsPerUnit / 2;
        offset = Mathf.Sqrt(offset * offset + offset * offset);
        for (int i = 0; i < 3; i++)
        {
            GameObject sprite = new();
            sprite.transform.parent = transform;
            SpriteRenderer rend = sprite.AddComponent<SpriteRenderer>();
            rend.sortingLayerName = "Background";
            rend.sortingOrder = order++;
            rend.sprite = details[Random.Range(0, details.Length)];
            rend.transform.localPosition = new(Random.Range(0, size) - size / 2, -Random.Range(offset, size), 0);
            rend.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        }

        for (int i = 0; i < 10; i++)
        {
            Sprite s = objects[Random.Range(0, objects.Length)];
            offset = s.textureRect.width / s.pixelsPerUnit / 2;
            offset = Mathf.Sqrt(offset * offset + offset * offset);
            GameObject sprite = new();
            sprite.transform.parent = transform;
            SpriteRenderer rend = sprite.AddComponent<SpriteRenderer>();
            rend.sortingLayerName = "BackgroundObjects";
            rend.sortingOrder = objectOrder++;
            rend.sprite = s;
            rend.transform.localPosition = new(Random.Range(0, size) - size / 2, -Random.Range(offset, size), 0);
            rend.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
            Color col = Color.grey;
            col.a = 0.5f;
            rend.color = col;
        }
    }
}
