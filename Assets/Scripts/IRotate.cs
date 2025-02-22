using UnityEngine;
using System.Collections;

public class IRotate : MonoBehaviour, IInteractable
{
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private Vector3 initialRotation;
    [SerializeField] private float moveSpeed = 2f; 
    [SerializeField] private float rotateSpeed = 2f;

    private Coroutine movementCoroutine;
    public enum PickupType
    {
        CHAIR,
        BOTTLE
    }
    public PickupType pickupType;
    public void Interact(bool grabbed = false)
    {
        Debug.Log($"I ({gameObject.name}) will be rotated");
        if(pickupType == PickupType.CHAIR)
        {
            if (movementCoroutine != null)
                StopCoroutine(movementCoroutine);

            movementCoroutine = StartCoroutine(SmoothMove());
        }
        else
        {
            transform.rotation = Quaternion.Euler(initialRotation);
            transform.position = initialPosition;
        }
        
    }

    private IEnumerator SmoothMove()
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(initialRotation);

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * moveSpeed;

            transform.position = Vector3.Lerp(startPosition, initialPosition, elapsedTime);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime * rotateSpeed);

            yield return null;
        }

        transform.position = initialPosition;
        transform.rotation = targetRotation;
    }

    public void Check()
    {
        // Placeholder for Check function
    }
}
