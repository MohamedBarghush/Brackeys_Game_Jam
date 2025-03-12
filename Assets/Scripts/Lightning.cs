using System.Collections;
using UnityEngine;
using StarterAssets;
using UnityEngine.Playables;

public class Lightning : MonoBehaviour
{
    [SerializeField] private Color targetColor;

    public GameObject cinematic;
    public GameObject UI;
    public PlayableDirector director;
    private bool skipped = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)&& !skipped)
        {
            
            director.time = 34.0;
            director.Evaluate();
            skipped = true;
            Destroy(gameObject, 6.2f);
            UI.SetActive(false);
        }
    }

    public void TurnLightsOff () {
        UI.SetActive(false);
        StartCoroutine(FadeToBlack());
    }

    IEnumerator FadeToBlack()
    {
        Color currentColor = RenderSettings.ambientLight;
        float duration = 0.1f; // Duration of the fade
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            RenderSettings.ambientLight = Color.Lerp(currentColor, targetColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        RenderSettings.ambientLight = targetColor; // Ensure the color is set to black at the end
    }

    public void EndCinematic () {
        cinematic.SetActive(false);
    }
}
