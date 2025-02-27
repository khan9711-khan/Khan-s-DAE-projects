using UnityEngine;

public class Knife : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager gameManager = FindAnyObjectByType<GameManager>();

        if (collision.gameObject.CompareTag("Target"))
        {
            // Stop knife movement
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;

            // Position the knife on the edge of the target
            Vector3 directionToCenter = (transform.position - collision.transform.position).normalized;
            float radius = collision.collider.bounds.extents.x; // Assuming the target is circular
            transform.position = collision.transform.position + directionToCenter * (radius - 0.1f);

            // Make the knife a child of the target to rotate with it
            transform.SetParent(collision.transform);

            if (gameManager != null)
            {
                gameManager.ShowResult(true, "Well Done!");
            }
        }
        else if (collision.gameObject.CompareTag("Apple"))
        {
            // Handle hitting an apple
            if (gameManager != null)
            {
                gameManager.ShowResult(true, "You hit an apple! Well Done!");
            }
            Destroy(collision.gameObject); // Remove the apple
        }
        else if (collision.gameObject.CompareTag("Knife"))
        {
            // Game over if the knife hits another knife
            if (gameManager != null)
            {
                gameManager.ShowResult(false, "Game Over! You hit another knife.");
            }
        }
        else
        {
            if (gameManager != null)
            {
                gameManager.ShowResult(false, "Try Again!");
            }
            Destroy(gameObject); // Remove the knife if it misses everything
        }
    }
}
