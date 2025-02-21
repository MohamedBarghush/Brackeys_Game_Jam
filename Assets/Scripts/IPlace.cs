using System.Collections.Generic;
using UnityEngine;

public class IPlace : MonoBehaviour, IInteractable
{
    public enum BookType {
        RED,
        BLUE,
        YELLOW,
        GREEN
    }
    [SerializeField] private BookType bookType;
    private int idx = 1;
    [SerializeField] private List<GameObject> bookMeshes; 

    public void Interact(bool grabbed = false)
    {
        Debug.Log("I reached this place");
        Interaction(ref RoomOneManager.booksHeldArray[(int)bookType]);
    }

    void Interaction (ref int bookCount) {
        if (bookCount > 0) {
            bookCount--;
            if (bookCount == 0)
                RoomOneManager.instance.DisableMesh((int)bookType, true);
            
            RoomOneManager.booksHeld--;
            RoomOneManager.booksReturned++;
            RoomOneManager.instance.UpdateUI();
            bookMeshes[idx].SetActive(true);
            idx++;
            RoomOneManager.TriggerCheck();
        }
    }
}
