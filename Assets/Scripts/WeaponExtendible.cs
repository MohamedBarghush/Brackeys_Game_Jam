using UnityEngine;

public class WeaponExtendible : MonoBehaviour
{

    public Weapon daWeapon;

    [HideInInspector] public Rigidbody rb;
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
            interactable.Interact(grabbed: true);
            rb.linearVelocity = Vector3.zero;
            transform.position = other.gameObject.transform.position;
            other.transform.parent = transform;
            GetComponent<BoxCollider>().enabled = false;
            daWeapon.RetractImmediately(0.5f);
        }
    }
}
