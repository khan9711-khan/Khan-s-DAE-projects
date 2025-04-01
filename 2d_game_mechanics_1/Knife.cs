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
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;

            Vector3 directionToCenter = (transform.position - collision.transform.position).normalized;
            float radius = collision.collider.bounds.extents.x;
            transform.position = collision.transform.position + directionToCenter * (radius - 0.1f);
            transform.SetParent(collision.transform);

            if (gameManager != null)
            {
                gameManager.ShowResult(true, "Well Done!");
                gameManager.RegisterKnifeHit();
            }
        }
        else if (collision.gameObject.CompareTag("Apple"))
        {
            if (gameManager != null)
            {
                gameManager.ShowResult(false, "You hit the apple! Game Resetting...");
                gameManager.RestartGame();
            }
        }
        else if (collision.gameObject.CompareTag("Knife"))
        {
            if (gameManager != null)
            {
                gameManager.ShowResult(false, "Game Over! You hit another knife.");
            }
            Destroy(gameObject);
        }
        else
        {
            if (gameManager != null)
            {
                gameManager.ShowResult(false, "Try Again!");
            }
            Destroy(gameObject);
        }
    }
}







