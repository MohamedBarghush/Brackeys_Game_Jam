using UnityEngine;

public class IPick : MonoBehaviour, IInteractable
{
    public enum PickupType {
        BOOK,
        BOTTLE
    }

    public PickupType pickupType;

    public bool picked = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (picked) { 
            if (Vector3.Distance(transform.position, Weapon.instance.transform.position) < 2f)
                Destroy(gameObject);
        }
    }

    public void Interact (bool grabbed = false) {
        Debug.Log("I (" + gameObject.name + ") " + "got picked up");

        if (pickupType == PickupType.BOOK)
            RoomOneManager.booksHeld++;
        else if (pickupType == PickupType.BOTTLE)
            RoomOneManager.bottlesHeld++;

        RoomOneManager.instance.UpdateUI();
        if (!grabbed)
            Destroy(gameObject);
        else 
            picked = true;
        // picked = true;
    }

    public void Check () {

    }
}
