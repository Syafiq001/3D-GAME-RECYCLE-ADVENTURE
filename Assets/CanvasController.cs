using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public Canvas[] canvases; // Reference to your five canvases
    private int currentCanvasIndex;
    private float elapsedTime;
    private bool isRotating;

    public float loadingDuration = 1.0f; // Duration of the loading screen in seconds
    public float canvasDisplayDuration = 0.5f; // Duration to display each canvas

    void Start()
    {
        // Initialize variables
        currentCanvasIndex = Random.Range(0, canvases.Length);
        isRotating = true;

        // Disable all canvases except the initial one
        for (int i = 0; i < canvases.Length; i++)
        {
            if (i != currentCanvasIndex)
            {
                canvases[i].gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (isRotating)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= loadingDuration)
            {
                elapsedTime = 0f;
                isRotating = false;

                // Disable the currently displayed canvas
                canvases[currentCanvasIndex].gameObject.SetActive(false);

                // Randomly choose the next canvas
                int nextCanvasIndex = GetRandomCanvasIndex();

                // Enable the new canvas
                canvases[nextCanvasIndex].gameObject.SetActive(true);
                currentCanvasIndex = nextCanvasIndex;
            }
        }
        else
        {
            // Display the current canvas for a fixed duration
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= canvasDisplayDuration)
            {
                elapsedTime = 0f;
                isRotating = true;
            }
        }
    }

    int GetRandomCanvasIndex()
    {
        int nextCanvasIndex = Random.Range(0, canvases.Length);

        // Make sure the next canvas is different from the current one
        while (nextCanvasIndex == currentCanvasIndex)
        {
            nextCanvasIndex = Random.Range(0, canvases.Length);
        }

        return nextCanvasIndex;
    }
}
