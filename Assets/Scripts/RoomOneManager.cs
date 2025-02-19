using UnityEngine;
using UnityEngine.UIElements;

public class RoomOneManager : MonoBehaviour
{
    public static RoomOneManager instance;

    public static int booksHeld = 0;
    public static int bottlesHeld = 0;

    public static int booksReturned = 0;
    public static int bottlesReturned = 0;

    public UIDocument uiDoc;
    private Label booksHeldText;
    private Label bottlesHeldText;

    void Awake()
    {
        instance = this;
    } 

    void Start()
    {
        var root = uiDoc.rootVisualElement;
        booksHeldText = root.Q<Label>("booksHeld");
        bottlesHeldText = root.Q<Label>("bottlesHeld");

        UpdateUI();
    }

    public void UpdateUI () {
        booksHeldText.text = "x"+booksHeld.ToString();
        bottlesHeldText.text = "x"+bottlesHeld.ToString();
    }
}
