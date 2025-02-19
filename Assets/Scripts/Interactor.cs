using UnityEngine;
using StarterAssets;

interface IInteractable {
    public void Interact();
    public void Rotate();
}

public class Interactor : MonoBehaviour
{
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private float rayDistance = 10f;

    private StarterAssetsInputs _inputs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputs = GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
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
        if (_inputs.rotate == true)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance, interactionLayer))
            {
                if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
                {
                    interactable.Rotate();
                }
            }
            _inputs.rotate = false;  
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * rayDistance);
    }
}
