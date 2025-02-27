using UnityEngine;

public class SpinningTarget : MonoBehaviour
{
    public float rotationSpeed = 100f; // Speed of rotation

    void Update()
    {
        // Rotate around the Z-axis to spin like a Ferris wheel
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
