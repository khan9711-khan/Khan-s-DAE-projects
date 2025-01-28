using UnityEngine;

public class Knife : MonoBehaviour
{
    // Public variable for knife speed (float type)
    public float knifeSpeed = 15f;

    // Private variable to track if the knife has hit the target (bool type)
    private bool hasHitTarget = false;

    // Public variable for knife's level or rank (integer type)
    public int knifeLevel = 1;

    // Start is called once before the first execution of Update
    void Start()
    {
        Debug.Log($"Knife Level {knifeLevel} is ready to be thrown!");
    }

    // Update is called once per frame
    void Update()
    {
        // Conditional statement: If the knife has not hit the target, it moves upward
        if (!hasHitTarget)
        {
            transform.Translate(Vector2.up * knifeSpeed * Time.deltaTime);
        }
    }

    // Triggered when the knife collides with another object
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            hasHitTarget = true;
            StickToTarget(collision.gameObject);
            Debug.Log($"Knife Level {knifeLevel} hit the target!");
        }
        else if (collision.gameObject.CompareTag("Knife"))
        {
            Debug.Log("Game Over! Knife hit another knife.");
            GameOver();
        }
    }

    // Private method to handle the knife sticking to the target
    private void StickToTarget(GameObject target)
    {
        transform.SetParent(target.transform); // Stick the knife to the target
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero; // Stop knife movement
        GetComponent<Rigidbody2D>().isKinematic = true; // Disable physics for the knife
    }

    // Private method to handle game over logic
    private void GameOver()
    {
        // Placeholder for Game Over logic (e.g., show Game Over UI, stop the game)
        Debug.Log("Game Over!");
    }
}
