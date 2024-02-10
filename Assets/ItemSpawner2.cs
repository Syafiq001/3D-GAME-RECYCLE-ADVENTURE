using UnityEngine;

public class ItemSpawner2 : MonoBehaviour
{
    public GameObject[] itemPrefabs; // Array to hold different item prefabs
    public int maxItems = 10; // Maximum number of items to spawn
    public Vector3 minSpawnPosition = new Vector3(-10f, 0.5f, -10f); // Minimum spawn position
    public Vector3 maxSpawnPosition = new Vector3(10f, 0.5f, 10f); // Maximum spawn position
    public string[] itemTags = { "Item1", "Item2", "Item3", "Item4", "Item5" }; // Array of tags for items

    private void Start()
    {
        // Spawn initial items
        SpawnItems(maxItems);
    }

    private void Update()
    {
        // Check the number of items and respawn if necessary
        int currentItems = CountItems(); // Get the current count of items in the scene.
        int itemsToSpawn = maxItems - currentItems; // Calculate the number of items needed to reach the maximum.

        if (itemsToSpawn > 5)
        {
            SpawnItems(itemsToSpawn); // Spawn additional items if needed.
        }
    }

    private void SpawnItems(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Randomly choose an item prefab from the array
            GameObject randomItemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

            // Choose a random tag from the array
            string randomTag = itemTags[Random.Range(0, itemTags.Length)];

            // Generate random spawn position within the specified area
            Vector3 spawnPosition = new Vector3(
                Random.Range(minSpawnPosition.x, maxSpawnPosition.x),
                Random.Range(minSpawnPosition.y, maxSpawnPosition.y),
                Random.Range(minSpawnPosition.z, maxSpawnPosition.z)
            );

            // Instantiate the chosen item prefab at the random spawn position with the chosen tag
            GameObject spawnedItem = Instantiate(randomItemPrefab, spawnPosition, Quaternion.identity);
            
            // Set the tag of the spawned item
            //spawnedItem.tag = randomTag; // Commented out to avoid setting tags for now.
        }
    }

    private int CountItems()
    {
        int count = 0;
        foreach (var tag in itemTags)
        {
            // Count the number of items in the scene with each specified tag
            count += GameObject.FindGameObjectsWithTag(tag).Length;
        }
        return count;
    }
}
