using UnityEngine;

public class WeaponExtraCollider : MonoBehaviour
{
    WeaponExtendible _weaponExtendible;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _weaponExtendible = GetComponentInParent<WeaponExtendible>();
    }

    void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent(out IInteractable interactable);
        if (interactable == null)
        {
            _weaponExtendible.rb.linearVelocity = Vector3.zero;
            _weaponExtendible.GetComponent<BoxCollider>().enabled = false;
            // GetComponent<BoxCollider>().enabled = false;
            _weaponExtendible.daWeapon.RetractImmediately(0.1f);
            // Invoke("EnableCollider", 0.5f);
        }
    }

    // void EnableCollider()
    // {
    //     // GetComponent<BoxCollider>().enabled = true;
    // }

}
