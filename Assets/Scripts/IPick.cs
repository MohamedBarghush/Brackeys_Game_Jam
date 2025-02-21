using UnityEngine;

public class IPick : MonoBehaviour, IInteractable
{
    public enum PickupType {
        SMALL_PICKUP,
        BIG_PICKUP
    }

    public enum BookType {
        RED,
        BLUE,
        YELLOW,
        GREEN
    }

    public PickupType pickupType;
    public BookType bookType;

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
        if (pickupType == PickupType.SMALL_PICKUP) {
            if (RoomOneManager.booksHeld == 3) return;
            RoomOneManager.booksHeldArray[(int)bookType]++;
            RoomOneManager.instance.EnableMesh((int)bookType, true);
            RoomOneManager.booksHeld++;
        }
        else if (pickupType == PickupType.BIG_PICKUP)
            RoomOneManager.BIG_PICKUP++;

        RoomOneManager.instance.UpdateUI(); // Update the UI
        if (!grabbed)
            Destroy(gameObject);
        else {
            if (TryGetComponent(out Rigidbody rb)) rb.isKinematic = true;
            GetComponent<Collider>().enabled = false;
            picked = true;
        }
    }

    public void Check () {

    }
}
