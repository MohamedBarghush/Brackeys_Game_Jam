using UnityEngine;
using UnityEngine.UIElements;

public class UI_Control : MonoBehaviour
{
    public UIDocument uiDocument; // Assign in Inspector
    public string targetElementName = "BackgroundElement"; // Name of the existing element in UI Builder
    public Texture2D backgroundTexture; // Assign in Inspector

    private void Start()
    {
        disableUI();
    }
    public void ChangeQTEUI()
    {
        if (uiDocument == null)
        {
            Debug.LogError("UI Document is not assigned!");
            return;
        }

        // Get the root UI element
        VisualElement root = uiDocument.rootVisualElement;

        // Find the existing VisualElement by name
        VisualElement targetElement = root.Q<VisualElement>(targetElementName);
        if (targetElement == null)
        {
            Debug.LogError($"VisualElement '{targetElementName}' not found!");
            return;
        }

        // Apply the background image if assigned
        if (backgroundTexture != null)
        {
            targetElement.style.backgroundImage = new StyleBackground(backgroundTexture);
        }
        else
        {
            Debug.LogError("No background texture assigned in the Inspector!");
        }
    }

    public void enableUI()
    {

        // Get the root UI element
        VisualElement root = uiDocument.rootVisualElement;

        // Find the existing VisualElement by name
        VisualElement targetElement = root.Q<VisualElement>(targetElementName);
        targetElement.visible = true;

    }

    public void disableUI()
    {
        // Get the root UI element
        VisualElement root = uiDocument.rootVisualElement;

        // Find the existing VisualElement by name
        VisualElement targetElement = root.Q<VisualElement>(targetElementName);
        targetElement.visible = false;
    }
}
