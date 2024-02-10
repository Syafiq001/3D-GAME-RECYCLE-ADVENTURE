using UnityEngine;

public class ChestInteraction : MonoBehaviour
{
    public QuizManager quizManager; // Reference to the QuizManager script.
    public GameObject proceedCanvas;
    public GameObject quizCanvas;
    public Animator chestAnimator; // Reference to the Animator component on the chest.

    private bool isInTrigger = false;
    private bool hasInteracted = false;
    private bool isInQuiz = false;

    private void Start()
    {
        proceedCanvas.SetActive(false);
        quizCanvas.SetActive(false);

        // Ensure you have the Animator component attached to the chest GameObject.
        // Assign it in the Inspector.
        if (chestAnimator == null)
        {
            Debug.LogError("Chest Animator is not assigned. Assign it in the Inspector.");
        }
    }

    private void Update()
    {
        if (isInTrigger && !hasInteracted)
        {
            if (Input.GetKeyDown(KeyCode.E) && !isInQuiz)
            {
                OpenProceedCanvas();
                PlayChestEnterAnimation();
            }
        }

        if (isInQuiz && Input.GetKeyDown(KeyCode.E))
        {
            CloseQuiz();
        }
    }

    private void OpenProceedCanvas()
    {
        proceedCanvas.SetActive(true);
        hasInteracted = true;
    }

    public void OnProceedButton()
    {
        proceedCanvas.SetActive(false);
        quizCanvas.SetActive(true); // Open the quiz canvas.
    }

    public void OnCancelButton()
    {
        proceedCanvas.SetActive(false);
        hasInteracted = false;
    }

    private void CloseQuiz()
    {
        isInQuiz = false;
        quizCanvas.SetActive(false);
        proceedCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = true;
            PlayChestEnterAnimation();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
            hasInteracted = false;
            PlayChestExitAnimation(); // Optionally, play an exit animation when the player leaves the trigger zone.
            CloseQuiz();
        }
    }

    private void PlayChestEnterAnimation()
    {
        // Trigger the "ChestEnter" animation in the Animator on the chest GameObject.
        chestAnimator.SetTrigger("PlayerEnter");
    }

    private void PlayChestExitAnimation()
    {
        // Trigger the "ChestExit" animation in the Animator on the chest GameObject.
        chestAnimator.SetTrigger("PlayerExit");
    }
}
