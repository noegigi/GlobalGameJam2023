using UnityEngine;

public class Pickable : MonoBehaviour
{
    public float value = 10;

    [SerializeField] float shrinkSpeed = 2;

    bool shrink = false;

    public void SetSize(float scale)
    {
        transform.localScale *= scale;
        value = Mathf.RoundToInt(value * scale);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Root"))
        {
            shrink = true;
        }
    }

    private void Update()
    {
        if (shrink)
        {
            transform.localScale -= shrinkSpeed * Time.deltaTime * Vector3.one;
            if (transform.localScale.x <= 0)
                Destroy(gameObject);
        }
    }
}
