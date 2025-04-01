using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    // Public variable for knife speed (float type)
    public float knifeSpeed = 15f;

    // Private variable to track if the knife has hit the target (bool type)
    private bool hasHitTarget = false;

    // Public variable for knife's level or rank (integer type)
    public int knifeLevel = 1;

    // Static list to store all thrown knives
    public static List<GameObject> thrownKnives = new List<GameObject>();

    // Array to hold multiple knife prefabs for variety
    public GameObject[] knifePrefabs;

    // Queue for knife pooling (performance optimization)
    private static Queue<GameObject> knifePool = new Queue<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Knife Level {knifeLevel} is ready to be thrown!");
    }

    // Update is called once per frame
    void Update()
    {
        // Loop through all thrown knives and print their status
        foreach (GameObject knife in thrownKnives)
        {
            Debug.Log($"Tracking Knife: {knife.name}");
        }

        // Move the knife upwards only if it hasn't hit the target
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

    // Method to handle knife sticking to the target
    private void StickToTarget(GameObject target)
    {
        transform.SetParent(target.transform); // Stick the knife to the target
        GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Stop knife movement
        GetComponent<Rigidbody2D>().isKinematic = true; // Disable physics for the knife

        // Add the knife to the list of thrown knives
        thrownKnives.Add(gameObject);

        // Display all thrown knives using a loop
        for (int i = 0; i < thrownKnives.Count; i++)
        {
            Debug.Log($"Knife {i + 1}: Stuck on target");
        }
    }

    // Method to spawn multiple knives (using an array)
    public void SpawnKnives(int numberOfKnives)
    {
        for (int i = 0; i < numberOfKnives; i++)
        {
            int randomIndex = Random.Range(0, knifePrefabs.Length);
            GameObject newKnife = Instantiate(knifePrefabs[randomIndex], transform.position, Quaternion.identity);
            Debug.Log($"Spawned Knife {i + 1} of Type: {knifePrefabs[randomIndex].name}");
        }
    }

    // Method to handle game over logic
    private void GameOver()
    {
        Debug.Log("Game Over!");
    }

    // Method to reset knives after a level
    public void ResetKnives()
    {
        // Remove all knives using a while loop
        while (thrownKnives.Count > 0)
        {
            GameObject knife = thrownKnives[0];
            thrownKnives.RemoveAt(0);
            Destroy(knife);
        }
    }

    // Knife pooling system for better performance
    public static GameObject GetPooledKnife(GameObject knifePrefab)
    {
        if (knifePool.Count > 0)
        {
            GameObject pooledKnife = knifePool.Dequeue();
            pooledKnife.SetActive(true);
            return pooledKnife;
        }
        else
        {
            return Instantiate(knifePrefab);
        }
    }

    public static void ReturnKnifeToPool(GameObject knife)
    {
        knife.SetActive(false);
        knifePool.Enqueue(knife);
    }
}
