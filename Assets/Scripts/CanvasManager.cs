using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public Canvas welcomeCanvas; // Reference to your first welcome canvas
    public Canvas welcomeCanvas2; // Reference to your second welcome canvas
    public Canvas welcomeCanvas3; // Reference to your third welcome canvas
    public Canvas[] otherCanvases; // Array of other canvases to hide

    private bool isGamePaused = false; // Flag to track whether the game is paused

    private void Start()
    {
        // Show only the first welcome canvas
        ShowCanvas(welcomeCanvas);
    }

    // Call this method to hide a canvas
    public void HideCanvas(Canvas canvas)
    {
        canvas.gameObject.SetActive(false);
    }

    // Call this method to show a canvas
    public void ShowCanvas(Canvas canvas)
    {
        // Hide all other canvases first
        foreach (Canvas otherCanvas in otherCanvases)
        {
            HideCanvas(otherCanvas);
        }

        // Show the desired canvas
        canvas.gameObject.SetActive(true);

        // Pause the game when a welcome canvas is shown
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    // Call this method to close all welcome canvases and resume the game
    public void CloseAllWelcomeCanvases()
    {
        HideCanvas(welcomeCanvas);
        HideCanvas(welcomeCanvas2);
        HideCanvas(welcomeCanvas3);

        // Show all other canvases
        foreach (Canvas otherCanvas in otherCanvases)
        {
            ShowCanvas(otherCanvas);
        }

        // Resume the game
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    // Check if the game is paused
    public bool IsGamePaused()
    {
        return isGamePaused;
    }
}
