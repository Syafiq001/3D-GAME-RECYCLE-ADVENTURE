using UnityEngine;

public class ActiveObjectManager : MonoBehaviour
{
    public static ActiveObjectManager instance; // Singleton instance

    public GameObject[] targetObjects; // Array of target objects to control

    private void Awake()
    {
        // Ensure only one instance of the manager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Activate a random target object when the scene starts
        ActivateRandomObject();
    }

    private void ActivateRandomObject()
    {
        // Deactivate all target objects
        DeactivateAllObjects();

        // Choose a random index from the array
        int randomIndex = Random.Range(0, targetObjects.Length);

        // Activate the target object at the chosen index
        targetObjects[randomIndex].SetActive(true);
    }

    private void DeactivateAllObjects()
    {
        // Deactivate all target objects
        foreach (GameObject obj in targetObjects)
        {
            obj.SetActive(false);
        }
    }
}
