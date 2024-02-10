using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This class manages the pause menu functionality.
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseCanvas; // Reference to the pause menu canvas
    public GameObject[] otherCanvases; // Array to hold other canvases you want to disable
    public Canvas[] preventResumeCanvases; // Array of canvases that prevent resuming

    public bool paused; // Flag to track whether the game is paused

    // Start is called before the first frame update
    void Start()
    {
        paused = false;
        Time.timeScale = 1; // Set the time scale to normal (unpaused) initially
        SceneManager.activeSceneChanged += ChangeActiveScene; // Subscribe to the activeSceneChanged event
    }

    // Event handler for the activeSceneChanged event
    void ChangeActiveScene(Scene current, Scene next)
    {
        Time.timeScale = 1; // Ensure time scale is set to normal when scene changes
    }

    // Update is called once per frame
    void Update()
    {
        // Check for input to toggle pause state
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
                Resume(); // If paused, resume the game
            else
                Pause(); // If not paused, pause the game
        }        
    }

    // Method to pause the game
    public void Pause()
    {
        Time.timeScale = 0; // Set the time scale to 0 (pause)
        pauseCanvas.SetActive(true); // Activate the pause menu canvas
        paused = true; // Update the paused flag

        // Disable other canvases
        foreach (GameObject canvas in otherCanvases)
        {
            canvas.SetActive(false);
        }
    }

    // Method to resume the game
    public void Resume()
    {
        // Check if resuming is allowed based on preventResumeCanvases
        if (!ShouldPreventResume())
        {
            Time.timeScale = 1; // Set the time scale to normal (unpause)
            pauseCanvas.SetActive(false); // Deactivate the pause menu canvas
            paused = false; // Update the paused flag

            // Enable other canvases
            foreach (GameObject canvas in otherCanvases)
            {
                canvas.SetActive(true);
            }
        }
    }

    // Method to check if resuming is prevented by any active preventResumeCanvases
    private bool ShouldPreventResume()
    {
        // Check if any of the prevent resume canvases are active
        foreach (Canvas canvas in preventResumeCanvases)
        {
            if (canvas != null && canvas.isActiveAndEnabled)
            {
                // Check if the canvas is not the pauseCanvas
                if (canvas != pauseCanvas)
                {
                    return true; // If active and not the pauseCanvas, prevent resume
                }
            }
        }
        return false; // If none of the canvases prevent resume, allow resume
    }
}
