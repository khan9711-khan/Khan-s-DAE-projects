using UnityEngine;
using System.Collections;

public class SpinningTarget : MonoBehaviour
{
    public float rotationSpeed = 100f;
    private bool isSpinningLeft = false; // ✅ Track when to spin left

    void Update()
    {
        if (!isSpinningLeft)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime); // Normal spinning
        }
        else
        {
            transform.Rotate(0, 0, -rotationSpeed * 2 * Time.deltaTime); // ✅ Spin left faster
        }
    }

    public IEnumerator SpinLeftAndDisappear()
    {
        isSpinningLeft = true; // ✅ Start spinning left
        yield return new WaitForSeconds(2); // ✅ Wait for 2 seconds
        Destroy(gameObject); // ✅ Target disappears
    }
}
