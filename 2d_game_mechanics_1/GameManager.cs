using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject knifePrefab;      // Reference to the knife prefab
    public Transform knifeSpawnPoint;   // Where the knife will appear
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

        if (knifeRigidbody != null)
        {
            knifeRigidbody.gravityScale = 0; // Prevent gravity from affecting the knife
            knifeRigidbody.linearVelocity = new Vector2(0, 10f); // Move the knife straight up
        }
    }

    public void ShowResult(bool isHit, string message = "")
    {
        if (isHit)
        {
            resultText.text = string.IsNullOrEmpty(message) ? "Well Done!" : message;
        }
        else
        {
            resultText.text = string.IsNullOrEmpty(message) ? "Try Again!" : message;
            canThrow = false; // Stop further throws when the game is over
        }
        
        if (!message.Contains("Game Over"))
        {
            canThrow = true; // Allow another throw if the game is not over
        }
    }
}

