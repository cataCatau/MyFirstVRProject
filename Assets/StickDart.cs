using UnityEngine;

public class StickDart : MonoBehaviour
{
    private Rigidbody rb;
    private bool isStuck = false;

    [Header("Calibrare Inele")]
    public float bullseyeRadius = 0.02f;
    public float outerBullseyeRadius = 0.05f;
    public float trebleInnerRadius = 0.20f;
    public float trebleOuterRadius = 0.23f;
    public float doubleInnerRadius = 0.38f;
    public float doubleOuterRadius = 0.42f; 
    
    [Header("Calibrare Rotire")]
    public float rotationLogic = 0f;

    private readonly int[] boardScores = { 20, 1, 18, 4, 13, 6, 10, 15, 2, 17, 3, 19, 7, 16, 8, 11, 14, 9, 12, 5 };

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isStuck) return;

        if (collision.gameObject.CompareTag("Tinta"))
        {
            isStuck = true;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;

            // Stuck the dart and put it in the right position 
            transform.SetParent(collision.transform);
            transform.right = collision.contacts[0].normal;

            // 1. Calculate where the dart hit compared to the center
            Vector3 hitPoint = collision.contacts[0].point;
            Vector3 localHit = collision.transform.InverseTransformPoint(hitPoint);

            // 2. Calculate the distance from the center
            float distanceFromCenter = new Vector2(localHit.x, localHit.y).magnitude;

            // 3.Calculate the angle 
            float angle = Mathf.Atan2(-localHit.x, localHit.y) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360f;

            // align the slices and add the calibre variable
            float boardAngle = (angle + 9f +rotationLogic) % 360f;
            if (boardAngle < 0) boardAngle += 360f;

            int sliceIndex = Mathf.FloorToInt(boardAngle / 18f);
            if(sliceIndex < 0 || sliceIndex > 19) sliceIndex = 0; 
            
            int rawScore = boardScores[sliceIndex];
            int finalScore = 0;
            string region = "Single";

            // 4. Verify the zone 
            if (distanceFromCenter > doubleOuterRadius) {
                finalScore = 0;
                region = "Rata (In afara tablei)";
            } else if (distanceFromCenter > doubleInnerRadius) {
                finalScore = rawScore * 2;
                region = "Double";
            } else if (distanceFromCenter > trebleOuterRadius) {
                finalScore = rawScore;
                region = "Single (Exterior)";
            } else if (distanceFromCenter > trebleInnerRadius) {
                finalScore = rawScore * 3;
                region = "Treble";
            } else if (distanceFromCenter > outerBullseyeRadius) {
                finalScore = rawScore;
                region = "Single (Interior)";
            } else if (distanceFromCenter > bullseyeRadius) {
                finalScore = 25;
                region = "Outer Bullseye";
            } else {
                finalScore = 50;
                region = "Inner Bullseye";
            }

            Debug.Log($"SCOR: {finalScore} puncte! | Zona: {region} la numărul {rawScore} | Distanța exactă: {distanceFromCenter}");
        }
    }
}