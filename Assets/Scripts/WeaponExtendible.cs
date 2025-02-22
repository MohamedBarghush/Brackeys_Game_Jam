using UnityEngine;

public class WeaponExtendible : MonoBehaviour
{
    ///////////////////////////////// Weapon Normal Collision logic is in WeaponExtraCollider.cs

    public Weapon daWeapon;

    [HideInInspector] public Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() => rb = GetComponent<Rigidbody>();

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IInteractable interactable))
        {
            // if (interactable.GetType() != typeof(IPick)) return;
            interactable.Interact(grabbed: true);
            if (interactable.GetType() == typeof(IPick) && RoomOneManager.booksHeld < 3) {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                transform.position = other.gameObject.transform.position;
                other.transform.SetParent(transform);
                GetComponent<BoxCollider>().enabled = false;
            }
            daWeapon.RetractImmediately(0.5f);
        }
    }
}
