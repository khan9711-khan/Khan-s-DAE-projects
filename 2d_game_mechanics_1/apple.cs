using UnityEngine;
using System.Collections; // ✅ Fix: Added System.Collections for IEnumerator

public class Apple : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Knife"))
        {
            GameManager.instance.ShowResult(false, "You hit the apple! Game Resetting...");
            StartCoroutine(ResetGameAfterDelay());
        }
    }

    private IEnumerator ResetGameAfterDelay()
    {
        yield return new WaitForSeconds(2); // ✅ Wait 2 seconds before resetting
        GameManager.instance.RestartGame(); // ✅ Now works because RestartGame() is public
    }
}

