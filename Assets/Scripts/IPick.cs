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

    private bool picked = false;

    // Update is called once per frame
    void Update()
    {
        if (picked) { 
            if (Vector3.Distance(transform.position, Weapon.instance.transform.position) < 2f)
                Destroy(gameObject);
        }
    }

    public void Interact (bool grabbed = false) {
        // Debugging
        // Debug.Log("I (" + gameObject.name + ") " + "got picked up");

        // Determine the pickup type
        if (pickupType == PickupType.BOOK)
            RoomOneManager.booksHeld++;
        else if (pickupType == PickupType.BOTTLE)
            RoomOneManager.bottlesHeld++;

        RoomOneManager.instance.UpdateUI(); // Update the UI
        if (!grabbed)
            Destroy(gameObject);
        else 
            picked = true;
    }

    public void Check () {

    }
}
