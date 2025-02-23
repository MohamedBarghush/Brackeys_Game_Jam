using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Reset : MonoBehaviour
{
    public AudioSource resetSound; // Assign in the Inspector

    public void ResetScene()
    {
        if (resetSound != null)
        {
            StartCoroutine(PlaySoundThenReset());
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private IEnumerator PlaySoundThenReset()
    {
        resetSound.Play(); // Play the sound
        yield return new WaitForSeconds(resetSound.clip.length); // Wait for the sound to finish
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reset the scene
    }
}
