using UnityEngine;

public class IPick : MonoBehaviour, IInteractable
{
    public enum PickupType {
        BOOK,
        BOTTLE,
        CHAIR,
        TABLE,
        BARREL
    }

    public PickupType pickupType;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact () {
        Debug.Log("I (" + gameObject.name + ") " + "got picked up");

        if (pickupType == PickupType.BOOK)
            RoomOneManager.booksHeld++;
        else if (pickupType == PickupType.BOTTLE)
            RoomOneManager.bottlesHeld++;

        RoomOneManager.instance.UpdateUI();
        Destroy(gameObject, 1.0f);
    }

    public void Check () {

    }
}
