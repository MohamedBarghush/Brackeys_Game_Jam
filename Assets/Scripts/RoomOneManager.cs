using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RoomOneManager : MonoBehaviour
{
    public static bool[] questsDone = new bool[3] { false, false, false };

    public static RoomOneManager instance;

    public static int booksHeld = 0;

    // 0: red, 1: blue, 2: yellow, 3: green
    public static int[] booksHeldArray = new int[4] { 0, 0, 0, 0 };
    public static int[] booksReturnedArray = new int[4] { 0, 0, 0, 0 };
    public static int[] booksRequiredArray = new int[4] { 5, 2, 5, 3 };

    public static int BIG_PICKUP = 0;

    public static int booksReturned = 0;

    public UIDocument uiDoc;
    private Label booksHeldText;
    // private Label bottlesHeldText;

    [SerializeField] private List<GameObject> booksIndicators;
    [SerializeField] private List<GameObject> bigPickupIndicators;

    void Awake()
    {
        instance = this;
    } 

    void Start()
    {
        var root = uiDoc.rootVisualElement;
        booksHeldText = root.Q<Label>("booksHeld");
        // bottlesHeldText = root.Q<Label>("bottlesHeld");

        UpdateUI();
    }

    public static void TriggerCheck () {
        bool flagNotDone = false;
        for (int i = 0; i < 4; i++) {
            if (booksReturnedArray[i] < booksRequiredArray[i]) {
                flagNotDone = true;
                break;
            }
        }

        // Debug.Log("Red: " + booksReturnedArray[0] + " Blue: " + booksReturnedArray[1] + " Yellow: " + booksReturnedArray[2] + " Green: " + booksReturnedArray[3]);

        if (flagNotDone) {
            Debug.Log("Not done yet");
            return;
        } else {
            Debug.Log("Books over");
            questsDone[0] = true;
            // Mark the fkin book quest as done
        }

        EndLevel ();
    }

    public static void EndLevel () {
        foreach (bool end in questsDone) {
            if (!end) {
                Debug.Log("Not done yet");
                return;
            }
        }
        Debug.Log("Done here");

        // End the fkin room at last
    }

    public void EnableMesh (int index, bool book = false) {
        if (book)
            booksIndicators[index].SetActive(true);
        else
            bigPickupIndicators[index].SetActive(true);
    }
    public void DisableMesh (int index, bool book = false) {
        if (book)
            booksIndicators[index].SetActive(false);
        else
            bigPickupIndicators[index].SetActive(false);
    }

    public void UpdateUI () {
        booksHeldText.text = booksHeld.ToString()+"<size=30>/3</size>";
        // bottlesHeldText.text = "x"+BIG_PICKUP.ToString();
    }
}
