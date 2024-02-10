using UnityEngine;

// This class manages freezing and unfreezing the camera based on the activity of specified canvases.
public class CameraFreezeManager : MonoBehaviour
{
    public ThirdPersonOrbitCamBasic orbitCam; // Reference to your ThirdPersonOrbitCamBasic script
    public Canvas[] targetCanvases; // Array of Canvases to monitor

    // Update is called once per frame
    private void Update()
    {
        // Check if any of the target canvases are active
        if (AreAnyCanvasesActive())
        {
            FreezeCamera(); // Freeze the camera if any canvases are active
        }
        else
        {
            UnfreezeCamera(); // Unfreeze the camera if no canvases are active
        }
    }

    // Check if any of the target canvases are active
    private bool AreAnyCanvasesActive()
    {
        foreach (Canvas canvas in targetCanvases)
        {
            if (canvas != null && canvas.isActiveAndEnabled)
            {
                return true; // Return true if any canvas is active
            }
        }
        return false; // Return false if no canvas is active
    }

    // Disable the ThirdPersonOrbitCamBasic script to freeze the camera
    private void FreezeCamera()
    {
        if (orbitCam != null)
        {
            orbitCam.enabled = false; // Disable the camera script
        }
    }

    // Enable the ThirdPersonOrbitCamBasic script to unfreeze the camera
    private void UnfreezeCamera()
    {
        if (orbitCam != null)
        {
            orbitCam.enabled = true; // Enable the camera script
        }
    }
}
