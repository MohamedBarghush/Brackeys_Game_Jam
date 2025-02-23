using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWin : MonoBehaviour
{
    public GameObject winScreen; // Assign in Inspector
    public string playerTag = "Player"; // Tag for detecting the player
    public AudioSource audioSource; // The AudioSource playing the background music
    public AudioClip winSound; // Assign a victory sound in the Inspector

    void Start()
    {
        if (winScreen != null)
        {
            winScreen.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            TriggerWin();
        }
    }

    public void TriggerWin()
    {
        if (winScreen != null)
        {
            winScreen.SetActive(true);
        }

        // Stop the current music
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        // Play the win sound
        if (winSound != null && audioSource != null)
        {
            audioSource.clip = winSound;
            audioSource.Play();
        }

        Time.timeScale = 0f; // Stop movement
    }
}
