using System.Collections.Generic;
using UnityEngine;

public class IAcquire : MonoBehaviour, IInteractable
{
    public List<GameObject> weaponObjects;
    public Weapon weapon;
    public void Interact(bool grabbed = false, Transform hook = null)
    {
        AudioManager.Instance.PlaySound(SoundType.AcquireWeapon);
        foreach (var weaponObject in weaponObjects)
        {
            weaponObject.SetActive(true);
        }
        weapon.enabled = true;
        Destroy(gameObject);
    }
}
