using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs; // Prefabs for the 5 item types
    public Transform[] spawnPoints;  // Array of 15 spawn points
    public int itemsPerType = 3;    // Number of each item type to spawn

    private void Start()
    {
        SpawnItems(); // Call the method to spawn items when the game starts
    }

    // Method to spawn items
    void SpawnItems()
    {
        // Shuffle the spawn points array to distribute items randomly
        ShuffleArray(spawnPoints);

        // Create a list to keep track of used spawn points
        List<Transform> usedSpawnPoints = new List<Transform>();

        // Iterate through each item type
        for (int i = 0; i < itemPrefabs.Length; i++)
        {
            // Spawn the specified number of items for each type
            for (int j = 0; j < itemsPerType; j++)
            {
                // Choose a random spawn point that hasn't been used
                Transform spawnPoint = GetUnusedSpawnPoint(usedSpawnPoints);

                if (spawnPoint != null)
                {
                    // Instantiate the item at the spawn point
                    GameObject item = Instantiate(itemPrefabs[i], spawnPoint.position, Quaternion.identity);

                    // Make sure the spawned item has the correct tag
                    string itemType = item.tag;

                    // Add the used spawn point to the list
                    usedSpawnPoints.Add(spawnPoint);
                }
            }
        }
    }

    // Method to shuffle the elements of an array
    void ShuffleArray(Transform[] array)
    {
        int n = array.Length;
        for (int i = 0; i < n - 1; i++)
        {
            // Choose a random index within the array
            int j = Random.Range(i, n);
            
            // Swap the current element with a random element
            Transform temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    // Method to get an unused spawn point from the list of spawn points
    Transform GetUnusedSpawnPoint(List<Transform> usedSpawnPoints)
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            // Check if the spawn point has not been used
            if (!usedSpawnPoints.Contains(spawnPoint))
            {
                return spawnPoint; // Return the unused spawn point
            }
        }
        return null; // Return null if all spawn points have been used
    }
}
