using UnityEngine;
using System.Collections;

public class IRotate : MonoBehaviour, IInteractable
{
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private Vector3 initialRotation;
    [SerializeField] private float moveSpeed = 2f; 
    [SerializeField] private float rotateSpeed = 2f;
    private bool done = false;

    public enum PickupType
    {
        CHAIR,
        BOTTLE
    }
    public PickupType pickupType;
    public void Interact(bool grabbed = false, Transform hook = null)
    {
        // Debug.Log($"I ({gameObject.name}) will be rotated");
        if(!done && !grabbed)
        {
            RoomOneManager.fixedCount++;
            done = true;
            StartCoroutine(SmoothMove());
        }
        // else
        // {
        //     transform.rotation = Quaternion.Euler(initialRotation);
        //     transform.position = initialPosition;
        // }
        
    }

    private IEnumerator SmoothMove()
    {
        Vector3 startPosition = transform.localPosition;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(initialRotation);

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * moveSpeed;

            if(pickupType == PickupType.CHAIR)
                transform.localPosition = Vector3.Lerp(startPosition, initialPosition, elapsedTime);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime * rotateSpeed);

            yield return null;
        }

        if(pickupType == PickupType.CHAIR)
            transform.localPosition = initialPosition;
        transform.rotation = targetRotation;

        RoomOneManager.instance.UpdateUI();
    }

    public void Check()
    {
        // Placeholder for Check function
    }
}
