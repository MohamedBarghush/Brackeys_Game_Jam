using UnityEngine;

public class WeaponExtendible : MonoBehaviour
{

    public Weapon daWeapon;

    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent(out IInteractable interactable);
        if (interactable != null)
        {
            interactable.Interact();
            rb.linearVelocity = Vector3.zero;
            transform.position = other.gameObject.transform.position;
            GetComponent<BoxCollider>().enabled = false;
            daWeapon.RetractImmediately();
        } else {
            rb.linearVelocity = Vector3.zero;
            GetComponent<BoxCollider>().enabled = false;
            daWeapon.RetractImmediately(0);
        }
    }
}
