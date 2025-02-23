using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private UIDocument uiDocument; // Assign your UIDocument in the Inspector

    [Header("Game References")]
    [SerializeField] private GameManager gameManager;

    private Button playButton;

    private void Awake()
    {
        // Pause the game at start
        Time.timeScale = 0f;
        Debug.Log("Game paused (Time.timeScale = 0)");

        // Query only the Play button (its name in UI Builder must be "Play")
        playButton = uiDocument.rootVisualElement.Q<Button>("Play");
        if (playButton == null)
        {
            Debug.LogError("Play button not found! Ensure its name is 'Play' in UI Builder.");
            return;
        }

        // Add the click event listener
        playButton.clicked += OnPlayClicked;
        Debug.Log("Play button listener added.");
    }

    private void OnPlayClicked()
    {
        Debug.Log("Play button clicked!");

        // Disable the entire UI by deactivating the UIDocument GameObject
        uiDocument.gameObject.SetActive(false);
        Debug.Log("UIDocument deactivated.");

        // Resume the game
        Time.timeScale = 1f;
        Debug.Log("Game resumed (Time.timeScale = 1)");

        // Call the game manager to initialize the level
        if (gameManager != null)
        {
            gameManager.InitializeLevel();
            Debug.Log("GameManager.InitializeLevel() called.");
        }
        else
        {
            Debug.LogError("GameManager reference is not assigned!");
        }
    }

    private void OnDestroy()
    {
        // Clean up the listener
        if (playButton != null)
        {
            playButton.clicked -= OnPlayClicked;
        }
    }
}
