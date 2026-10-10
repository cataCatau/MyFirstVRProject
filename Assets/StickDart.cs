using UnityEngine;

public class StickDart : MonoBehaviour
{
    private Rigidbody rb;
    private bool isStuck = false;

    [Header("Table")]
    public Transform boardCenter;          // a empty object in the middle of the target
    public float boardRadius = 0.5f;       // the outer radius of the double ring 
    public bool flipX = false;             
    public float rotationOffset = 0f;      

    // proportions: 6.35 / 15.9 / 99 / 107 / 162 / 170
    const float BULL = 6.35f / 170f;
    const float OUTER_BULL = 15.9f / 170f;
    const float TREBLE_IN = 99f / 170f;
    const float TREBLE_OUT = 107f / 170f;
    const float DOUBLE_IN = 162f / 170f;

    private readonly int[] boardScores = { 20, 1, 18, 4, 13, 6, 10, 15, 2, 17, 3, 19, 7, 16, 8, 11, 14, 9, 12, 5 };

    void Start() => rb = GetComponent<Rigidbody>();

    void OnCollisionEnter(Collision collision)
    {
        if (isStuck || !collision.gameObject.CompareTag("Tinta")) return;
        isStuck = true;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        transform.SetParent(collision.transform);
        transform.right = collision.contacts[0].normal;

        Transform c = boardCenter != null ? boardCenter : collision.transform;
        Vector3 offset = collision.contacts[0].point - c.position;

        float x = Vector3.Dot(offset, c.right) * (flipX ? -1f : 1f);
        float y = Vector3.Dot(offset, c.up);

        float dist = new Vector2(x, y).magnitude;
        float n = dist / boardRadius;              

        float angle = Mathf.Atan2(x, y) * Mathf.Rad2Deg;
        float boardAngle = (angle + 9f + rotationOffset + 360f) % 360f;
        int rawScore = boardScores[Mathf.FloorToInt(boardAngle / 18f) % 20];

        int finalScore; string region;
        if (n > 1f)               { finalScore = 0;            region = "Rata"; }
        else if (n > DOUBLE_IN)   { finalScore = rawScore * 2; region = "Double"; }
        else if (n > TREBLE_OUT)  { finalScore = rawScore;     region = "Single (exterior)"; }
        else if (n > TREBLE_IN)   { finalScore = rawScore * 3; region = "Treble"; }
        else if (n > OUTER_BULL)  { finalScore = rawScore;     region = "Single (interior)"; }
        else if (n > BULL)        { finalScore = 25;           region = "Outer Bullseye"; }
        else                      { finalScore = 50;           region = "Inner Bullseye"; }

        ScoreManager manager = FindObjectOfType<ScoreManager>();
            
            if (manager != null)
            {
                manager.AddScore(finalScore);
            }
            else
            {
                Debug.LogWarning("Failed to find the manager");
            }
    }
}