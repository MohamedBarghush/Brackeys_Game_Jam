using UnityEngine;
using StarterAssets;
using System.Collections.Generic;

// Interface for interactable objects
interface IInteractable {
    public void Interact(bool grabbed = false, Transform hook = null);
    // public void Check();
}

public class Interactor : MonoBehaviour
{
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private float rayDistance = 10f;

    private StarterAssetsInputs _inputs;

    public bool weaponAcquired = false;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputs = GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        // Raycast for interaction
        RaycastHit hit;
        if (_inputs.interact == true)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance, interactionLayer))
            {
                if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
                {
                    interactable.Interact();
                }
            }
            _inputs.interact = false;
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * rayDistance);
    }
}
