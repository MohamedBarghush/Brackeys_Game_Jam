using UnityEngine;
using System.Collections;

public class AutoRun : MonoBehaviour
{
    public float speed = 5f;
    public float sideMoveDistance = 2f; // How far left/right the player moves
    public float sideMoveDuration = 0.5f; // Time taken to move left/right

    private bool isRunning = true;
    private Rigidbody rb;
    private Vector3 originalPosition; // Stores the original Z-axis position

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        originalPosition = transform.position; // Save initial position
    }

    void Update()
    {
        if (isRunning)
        {
            transform.position += Vector3.right * speed * Time.deltaTime; // Move forward on X-axis
        }
    }

    public void StopRunning()
    {
        isRunning = false;
    }

    public void ResumeRunning()
    {
        isRunning = true;
    }

    public void MoveLeftThenReset()
    {
        StartCoroutine(MoveSideThenReset(sideMoveDistance)); // Move left
    }

    public void MoveRightThenReset()
    {
        StartCoroutine(MoveSideThenReset(-sideMoveDistance)); // Move right
    }

    private IEnumerator MoveSideThenReset(float moveAmount)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + new Vector3(0, 0, moveAmount);

        float elapsedTime = 0f;

        while (elapsedTime < sideMoveDuration)
        {
            float forwardMovement = speed * Time.deltaTime; // Keep moving forward
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / sideMoveDuration);
            transform.position += new Vector3(forwardMovement, 0, 0); // Maintain forward movement
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition + new Vector3(speed * Time.deltaTime, 0, 0); // Ensure exact position

        // Wait before resetting
        yield return new WaitForSeconds(3.5f);

        // Move back to original Z position while moving forward
        elapsedTime = 0f;
        Vector3 resetPosition = new Vector3(transform.position.x, transform.position.y, originalPosition.z);

        while (elapsedTime < sideMoveDuration)
        {
            float forwardMovement = speed * Time.deltaTime; // Keep moving forward
            transform.position = Vector3.Lerp(transform.position, resetPosition, elapsedTime / sideMoveDuration);
            transform.position += new Vector3(forwardMovement, 0, 0); // Maintain forward movement
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = resetPosition + new Vector3(speed * Time.deltaTime, 0, 0); // Ensure reset position
    }


    public void SpeedUp()
    {
        speed *= 2;
    }
}
