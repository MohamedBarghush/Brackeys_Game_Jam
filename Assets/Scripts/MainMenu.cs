using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    private UIDocument uiDoc;
    private Button startButton;
    private Button quitButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiDoc = GetComponent<UIDocument>();
        startButton = uiDoc.rootVisualElement.Q<Button>("StartBTN");
        quitButton = uiDoc.rootVisualElement.Q<Button>("ExitBTN");

        startButton.clicked += () => UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        quitButton.clicked += () => Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
