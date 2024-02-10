using UnityEngine;

public class GameObjectDestroyer : MonoBehaviour
{
    public Canvas associatedCanvas; // Assign the canvas in the Inspector that triggers destruction.

    private void OnEnable()
    {
        if (associatedCanvas != null)
        {
            associatedCanvas.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        if (associatedCanvas != null)
        {
            associatedCanvas.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
