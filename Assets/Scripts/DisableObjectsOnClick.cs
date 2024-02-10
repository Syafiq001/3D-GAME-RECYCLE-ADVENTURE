using UnityEngine;
using UnityEngine.UI;

public class DisableObjectsOnClick : MonoBehaviour
{
    public Button disableButton; // Reference to the UI Button

    public GameObject[] objectsToDisable; // Array of GameObjects to disable

    private void Start()
    {
        // Ensure the button is assigned and add a listener to handle clicks
        if (disableButton != null)
        {
            disableButton.onClick.AddListener(OnDisableButtonClick);
        }
        else
        {
            Debug.LogError("Button not assigned to DisableObjectsOnClick script.");
        }
    }

    private void OnDisableButtonClick()
    {
        // Iterate through the array and disable each GameObject
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
    }
}
