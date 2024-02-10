using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public string nextSceneName; // The name of the scene to load after the loading screen.

    public float loadingTime = 7.0f; // Duration of the loading screen in seconds.

    private float startTime;

    private void Start()
    {
        startTime = Time.time; // Record the time when the loading manager starts.
    }

    private void Update()
    {
        // Check if the elapsed time since the loading manager started exceeds the loading time.
        if (Time.time - startTime >= loadingTime)
        {
            LoadNextScene(); // If so, load the next scene.
        }
    }

    // Load the next scene specified by nextSceneName.
    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
