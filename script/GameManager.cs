using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject knifePrefab;      // Reference to the knife prefab
    public Transform knifeSpawnPoint;   // Where the knife will appear
    public Transform target;            // The spinning target
    public Text resultText;             // UI text to show results
    private bool canThrow = true;       // Control when the player can throw

    void Start()
    {
        resultText.text = "Click to Throw!";
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canThrow)
        {
            ThrowKnife();
        }
    }

    void ThrowKnife()
    {
        canThrow = false; // Prevent multiple throws
        GameObject knife = Instantiate(knifePrefab, knifeSpawnPoint.position, Quaternion.identity);
        Rigidbody2D knifeRigidbody = knife.GetComponent<Rigidbody2D>();
        knifeRigidbody.linearVelocity = new Vector2(0, 10f); // Use linearVelocity instead of velocity
    }

    public void ShowResult(bool isHit)
    {
        if (isHit)
        {
            resultText.text = "Well Done!";
        }
        else
        {
            resultText.text = "Try Again!";
        }
        canThrow = true; // Allow another throw
    }
}

