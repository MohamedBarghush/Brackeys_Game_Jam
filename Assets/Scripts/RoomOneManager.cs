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
    public static int[] booksRequiredArray = new int[4] { 5, 2, 5, 4 };

    public static int BIG_PICKUP = 0;

    public static int booksReturned = 0;

    public static int correctlyOrdered = 0;
    public static int fixedCount = 0;
    public static int fixedCountMax = 10;

    public UIDocument uiDoc;
    private Label booksHeldText;
    private Label GATHERText;
    private Label ORDERText;
    private Label FIXText;
    // private Label bottlesHeldText;

    [SerializeField] private List<GameObject> booksIndicators;
    [SerializeField] private List<GameObject> bigPickupIndicators;

    void Awake()
    {
        instance = this;
    } 

    void Start()
    {
        AudioManager.Instance.PlayMusic(SoundType.BG);
        var root = uiDoc.rootVisualElement;
        booksHeldText = root.Q<Label>("booksHeld");
        GATHERText = root.Q<Label>("Gather");
        ORDERText = root.Q<Label>("Order");
        FIXText = root.Q<Label>("Fix");
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
            // Debug.Log("Not done yet");
            return;
        } else {
            Debug.Log("Books over");
            questsDone[0] = true;
            // Mark the fkin book quest as done
        }

        EndLevel ();
    }

    public static void TriggerFix () {
        if (fixedCount < fixedCountMax) {
            // Debug.Log("Not done yet");
            return;
        } else {
            Debug.Log("Fixed all");
            questsDone[1] = true;
            // Mark the fkin fix quest as done
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
        GATHERText.text = "Gather: " + booksReturned + "<size=30>/16</size>";
        ORDERText.text = "Order: " + correctlyOrdered + "<size=30>/4</size>";
        FIXText.text = "Fix: " + fixedCount + "<size=30>/" + fixedCountMax + "</size>";
        // bottlesHeldText.text = "x"+BIG_PICKUP.ToString();
    }
}
