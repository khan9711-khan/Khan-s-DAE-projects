using UnityEngine;

public class Knife : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager gameManager = FindAnyObjectByType<GameManager>();

        if (gameManager != null)
        {
            if (collision.gameObject.CompareTag("Target"))
            {
                gameManager.ShowResult(true); // If it hits the target
            }
            else
            {
                gameManager.ShowResult(false); // If it hits anything else
            }
        }

        Destroy(gameObject); // Remove the knife after the collision
    }
}

