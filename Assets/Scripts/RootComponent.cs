using UnityEngine;

public class RootComponent : MonoBehaviour
{
    [SerializeField] private int WaterGain;
    [SerializeField] private int MineralGain;

    public float speed;
    public float rotationSpeed;
    public float steerInterval;
    public float steerAmount;
    public float savePositionInterval = 0.1f;
    public float RespawnTime = 1;
    public float TruncSize = 2;
    public LineRenderer line;
    public LineRenderer line2;
    public float currentSize = 1;

    public ParticleSystem particleLeft;
    public ParticleSystem particleRight;

    [SerializeField] GameObject rootVisual;

    private float currentSteer = 0;
    private float savePositionTime = 0;
    private float steerTime = 0;

    private static int orderInLayer = 0;

    private void Start()
    {
        RegrowRoot();
    }

    // Update is called once per frame
    void Update()
    {
        savePositionTime += Time.deltaTime;
        if (savePositionTime >= savePositionInterval)
        {
            SavePosition();
            savePositionTime = 0;
        }

        steerTime += Time.deltaTime;
        if (steerTime >= steerInterval)
        {
            currentSteer = Random.Range(-steerAmount, steerAmount);
            steerTime = 0;
        }

        transform.position += speed * Time.deltaTime * transform.forward;

        float axis = Input.GetAxisRaw("Horizontal") + currentSteer;
        if (axis != 0)
        {
            transform.Rotate(Vector3.up, axis * rotationSpeed * Time.deltaTime);
            if (transform.eulerAngles.x <= 0 || transform.eulerAngles.x >= 180)
            {
                transform.Rotate(Vector3.up, -axis * rotationSpeed * Time.deltaTime);
            }
        }

        int count = line.positionCount;
        if (count > 0)
        {
            Debug.Log(count);
            line.SetPosition(count - 1, transform.position);
            line2.SetPosition(count - 1, transform.position);
        }
    }

    private void SavePosition()
    {
        line.positionCount++;
        line2.positionCount++;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Obstacle") || collision.transform.CompareTag("Trash"))
        {
            enabled = false;
            if (!Resources.Instance.CheckConditions())
                Invoke(nameof(RegrowRoot), RespawnTime);
        }

        if (collision.transform.CompareTag("Water"))
        {
            int gain = (int)collision.transform.GetComponent<Pickable>().value;
            Resources.Instance.GiveWater(WaterGain);
        }

        if (collision.transform.CompareTag("Mineral"))
        {
            int gain = (int)collision.transform.GetComponent<Pickable>().value;
            Resources.Instance.GiveMineral(MineralGain);
        }

        if (collision.transform.CompareTag("Trash"))
        {
            float gain = collision.transform.GetComponent<Pickable>().value;
            Resources.Instance.IncreaseTrashMultiplier(gain);
        }
    }

    public void RegrowRoot()
    {
        Debug.Log("Regrow");
        GameObject visual = Instantiate(rootVisual);
        line = visual.transform.Find("FirstLine").GetComponent<LineRenderer>();
        line2 = visual.transform.Find("BackgroundLine").GetComponent<LineRenderer>();
        line2.widthMultiplier = currentSize + 0.2f;
        line2.sortingOrder = orderInLayer++;
        line.widthMultiplier = currentSize;
        line.sortingOrder = orderInLayer++;
        transform.SetPositionAndRotation(Vector3.right * (Random.Range(0, TruncSize - currentSize) - (TruncSize - currentSize) / 2f), Quaternion.Euler(45 + Random.Range(0, 90), -90, -90));
        enabled = true;
    }

    private void OnEnable()
    {
        particleLeft.Play();
        particleRight.Play();
    }

    private void OnDisable()
    {
        particleLeft.Stop();
        particleRight.Stop();
    }
}
