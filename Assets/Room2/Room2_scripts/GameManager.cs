using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float referenceYOffset = 50f; // Height above endpoint
    public float referenceScale = 0.8f; // Scale of reference shapes
    private GameObject[] referenceShapes = new GameObject[4];
    [Header("Input System")]
public InputActionAsset inputActions;
private InputAction changeShape0;
private InputAction changeShape1;
private InputAction changeShape2;
private InputAction changeShape3;
    public GameObject[] shapePrefabs;
    public Transform[] startPoints;
    public Transform[] endPoints;
    public GameObject effectPrefab;    // Assign your effect prefab in Inspector
    public GameObject skeletonPrefab;  // Assign your skeleton prefab in Inspector
    public float effectDuration = 1f;  // Time to show effect before skeleton

    private Dictionary<int, string> endPointRequirements = new Dictionary<int, string>();
    private GameObject[] activeShapes = new GameObject[4];
    private Coroutine[] movementCoroutines = new Coroutine[4];
    private Vector3[] checkpointPositions = new Vector3[4];
    private HashSet<int> lockedPaths = new HashSet<int>();
    private int[] currentShapeIndices = new int[4];
    private int currentLevel = 1;
    private float baseMoveSpeed;
    private float baseSpawnMaxDelay;

    public float floatHeight = 2f;
    public float moveSpeed = 2f;
    public float fallThreshold = -5f;
    public float spawnMinDelay = 1f;
    public float spawnMaxDelay = 3f;
    public float speedIncreasePerLevel = 0.5f;
    public float spawnDelayReduction = 0.3f;
    public int maxLevels = 3;

    private bool gameActive = true;
    public AudioClip changeSound;
public Camera mainCamera;
    public float correctEffectDuration = 0.5f;
    public float correctEffectIntensity = 0.1f;
    public float wrongEffectDuration = 0.3f;
    public float wrongEffectIntensity = 0.2f;
    public AudioClip correctSound;
    public AudioClip wrongSound;
    [Range(0, 1)] public float soundVolume = 0.5f;
    private Vector3 originalCameraPosition;
    private Coroutine cameraEffectCoroutine;
    private bool isAdvancing = false;
    public float rotationSpeed = 100f;
    public AudioSource audioSource;
    public AudioClip backgroundMusic;
    [Range(0, 1)] public float musicVolume = 0.5f;
    private void CreateReferenceShapes()
    {
        for(int i = 0; i < endPoints.Length; i++)
        {
            // Get correct prefab index from requirements
            int prefabIndex = System.Array.FindIndex(shapePrefabs, 
                prefab => prefab.name == endPointRequirements[i]);

            if(prefabIndex >= 0)
            {
                // Create reference shape
                Vector3 spawnPos = endPoints[i].position + Vector3.up * referenceYOffset;
                referenceShapes[i] = Instantiate(
    shapePrefabs[prefabIndex], 
    spawnPos, 
    Quaternion.Euler(0, 270, 0) // Rotates 270 degrees on Y-axis
);


                // Disable physics and make static
                Rigidbody rb = referenceShapes[i].GetComponent<Rigidbody>();
                if(rb != null)
                {
                    rb.isKinematic = true;
                    rb.detectCollisions = false;
                }

                // Scale and color adjustments
                referenceShapes[i].transform.localScale *= referenceScale;
                
                // Optional: Change material or add outline effect
                // Add your custom visual treatment here
            }
        }
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = soundVolume;
        }
        
        audioSource.clip = backgroundMusic;
        audioSource.volume = musicVolume;
        audioSource.loop = true;
        audioSource.Play();
        baseMoveSpeed = moveSpeed;
        baseSpawnMaxDelay = spawnMaxDelay;
      if(mainCamera == null) mainCamera = Camera.main;
        originalCameraPosition = mainCamera.transform.position;
        for(int i = 0; i < shapePrefabs.Length; i++)
        {
            endPointRequirements[i] = shapePrefabs[i].name;
            
        }
        CreateReferenceShapes();

        InitializeLevel();
    }

    public void InitializeLevel()
{
    // Stop all path coroutines
    for(int i = 0; i < movementCoroutines.Length; i++)
    {
        if(movementCoroutines[i] != null)
        {
            StopCoroutine(movementCoroutines[i]);
            movementCoroutines[i] = null;
        }
    }

    // Clear existing shapes
   for(int i = 0; i < activeShapes.Length; i++)
        {
            if(activeShapes[i] != null && activeShapes[i] != referenceShapes[i])
            {
                Destroy(activeShapes[i]);
                activeShapes[i] = null;
            }
        }

    lockedPaths.Clear();
    
    // Only start new paths if game is still active
    if(gameActive)
    {
        for(int i = 0; i < startPoints.Length; i++)
        {
            checkpointPositions[i] = startPoints[i].position;
            StartCoroutine(PathSpawningCoroutine(i));
        }
    }
}

    private IEnumerator PathSpawningCoroutine(int index)
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 2f));

        while(gameActive && !lockedPaths.Contains(index))
        {
            if(activeShapes[index] == null)
            {
                SpawnShape(index);
                yield return new WaitForSeconds(Random.Range(spawnMinDelay, spawnMaxDelay));
            }
            yield return null;
        }
    }

    private void SpawnShape(int index)
{
    if (lockedPaths.Contains(index)) return;

    // Create list of possible indices excluding the correct shape index
    List<int> possibleIndices = new List<int>();
    for (int i = 0; i < shapePrefabs.Length; i++)
    {
        if (i != index)  // Exclude the correct shape index
        {
            possibleIndices.Add(i);
        }
    }

    // Get random index from remaining possibilities
    int randomIndex = possibleIndices[Random.Range(0, possibleIndices.Count)];
    currentShapeIndices[index] = randomIndex;

    Vector3 spawnPos = new Vector3(
        startPoints[index].position.x,
        startPoints[index].position.y + floatHeight,
        startPoints[index].position.z
    );

    GameObject shape = Instantiate(shapePrefabs[randomIndex], spawnPos, Quaternion.identity);
    activeShapes[index] = shape;
    checkpointPositions[index] = shape.transform.position;

    if (movementCoroutines[index] != null)
        StopCoroutine(movementCoroutines[index]);

    movementCoroutines[index] = StartCoroutine(MoveShapeToEndPoint(shape, endPoints[index], index));
}

    private IEnumerator MoveShapeToEndPoint(GameObject shape, Transform endPoint, int index)
{
    Rigidbody shapeRb = shape.GetComponent<Rigidbody>();
    if(shapeRb != null) shapeRb.useGravity = false;

    Vector3 currentPos = shape.transform.position;
    Vector3 startPosition = new Vector3(
        currentPos.x,
        startPoints[index].position.y + floatHeight,
        currentPos.z
    );

    Vector3 targetPosition = new Vector3(
        endPoint.position.x,
        startPosition.y,
        endPoint.position.z
    );

    shape.transform.position = startPosition;
    float journeyLength = Vector3.Distance(startPosition, targetPosition);
    float startTime = Time.time;

    while(shape != null && Vector3.Distance(shape.transform.position, targetPosition) > 0.1f)
    {
        if(activeShapes[index] != shape || lockedPaths.Contains(index)) yield break;

        // Movement
        float distanceCovered = (Time.time - startTime) * moveSpeed;
        float fractionOfJourney = distanceCovered / journeyLength;
        shape.transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
        
        // Rotation - added this block
        shape.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        
        yield return null;
    }

    if(shape != null && !lockedPaths.Contains(index))
    {
        if(shapeRb != null)
        {
            shapeRb.useGravity = true;
            shapeRb.AddForce(Vector3.down * 10f, ForceMode.Acceleration);
        }
        yield return new WaitForSeconds(1f);
        if(shape != null) CheckShapeMatch(shape, endPoint, index);
    }
}

    private void CheckShapeMatch(GameObject shape, Transform endPoint, int index)
    {
        string shapeName = shape.name.Replace("(Clone)", "").Trim();

        if(shapeName == endPointRequirements[index])
        {
            if(!lockedPaths.Contains(index))
            {
                StartCoroutine(HandleCorrectShape(shape, endPoint.position, index));
            }
        }
        else
        {
            Debug.Log("Wrong shape! Restarting path...");
            RestartSinglePath(index);
        }
    }

private void StartCameraEffect(float duration, float intensity)
    {
        if(cameraEffectCoroutine != null) StopCoroutine(cameraEffectCoroutine);
        cameraEffectCoroutine = StartCoroutine(CameraEffect(duration, intensity));
    }

    private IEnumerator CameraEffect(float duration, float intensity)
    {
        float elapsed = 0f;
        Vector3 basePosition = mainCamera.transform.position;

        while(elapsed < duration)
        {
            // Shake effect with intensity curve
            float shakeScale = Mathf.Lerp(intensity, 0f, elapsed/duration);
            Vector3 randomOffset = new Vector3(
                Random.Range(-shakeScale, shakeScale),
                Random.Range(-shakeScale, shakeScale),
                0
            );

            mainCamera.transform.position = basePosition + randomOffset;
            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = originalCameraPosition;
        cameraEffectCoroutine = null;
    }
    private IEnumerator HandleCorrectShape(GameObject shape, Vector3 position, int index)
    {
        lockedPaths.Add(index);
        
        Vector3 finalPosition = shape.transform.position;
        Quaternion finalRotation = shape.transform.rotation;
        
        Destroy(shape);

        // Play correct sound with effect prefab
        GameObject effect = Instantiate(effectPrefab, finalPosition, effectPrefab.transform.rotation);
        effect.transform.localScale = effectPrefab.transform.localScale;
        
        // Add sound to effect prefab
        AudioSource effectAudio = effect.AddComponent<AudioSource>();
        effectAudio.playOnAwake = false;
        effectAudio.clip = correctSound;
        effectAudio.volume = soundVolume;
        effectAudio.Play();
        
        StartCameraEffect(correctEffectDuration, correctEffectIntensity);        
        yield return new WaitForSeconds(effectDuration);
        Destroy(effect);

    GameObject skeleton = Instantiate(
        skeletonPrefab,
        finalPosition,
        skeletonPrefab.transform.rotation
    );
    skeleton.transform.localScale = skeletonPrefab.transform.localScale;
    
    Rigidbody skeletonRb = skeleton.GetComponent<Rigidbody>();
    if(skeletonRb != null) skeletonRb.isKinematic = true;

    activeShapes[index] = skeleton;
    Debug.Log($"Path {index + 1} Locked! ({lockedPaths.Count}/4)");

    // Add the isAdvancing check here
    if(lockedPaths.Count == 4 && !isAdvancing && gameActive)
{
    isAdvancing = true;
    StartCoroutine(AdvanceLevel());
}
}

private IEnumerator AdvanceLevel()
{
    Debug.Log($"LEVEL {currentLevel} COMPLETE!");

    // Check BEFORE incrementing
    if (currentLevel >= maxLevels)
    {
        Debug.Log("YOU BEAT ALL 3 LEVELS! FINAL VICTORY!");
        // Load the next scene using the active scene's build index + 1
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
        gameActive = false;
        isAdvancing = false;
        yield break;
    }

    currentLevel++;  // Increment level for the next round

    // Update moveSpeed and spawn delay for the next level
    moveSpeed = baseMoveSpeed + (currentLevel * speedIncreasePerLevel);
    spawnMaxDelay = Mathf.Max(0.5f, baseSpawnMaxDelay - (currentLevel * spawnDelayReduction));
    
    yield return new WaitForSeconds(2f);
    InitializeLevel();
    isAdvancing = false;
}



    private void RestartSinglePath(int index)
    {
        if(lockedPaths.Contains(index)) return;

        // Play wrong sound
        if(audioSource != null && wrongSound != null)
        {
            audioSource.PlayOneShot(wrongSound, soundVolume);
        }
        
        StartCameraEffect(wrongEffectDuration, wrongEffectIntensity);
        if(activeShapes[index] != null)
        {
            Vector3 resetPos = new Vector3(
                checkpointPositions[index].x,
                startPoints[index].position.y + floatHeight,
                checkpointPositions[index].z
            );
            
            activeShapes[index].transform.position = resetPos;
            Rigidbody rb = activeShapes[index].GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            
            if(movementCoroutines[index] != null)
                StopCoroutine(movementCoroutines[index]);
            
            movementCoroutines[index] = StartCoroutine(MoveShapeToEndPoint(activeShapes[index], endPoints[index], index));
        }
    }

    private void OnEnable()
{
    // Initialize input actions
    var gameplayActionMap = inputActions.FindActionMap("Gameplay");
    
    changeShape0 = gameplayActionMap.FindAction("ChangeShape0");
    changeShape1 = gameplayActionMap.FindAction("ChangeShape1");
    changeShape2 = gameplayActionMap.FindAction("ChangeShape2");
    changeShape3 = gameplayActionMap.FindAction("ChangeShape3");

    changeShape0.performed += ctx => TryChangeShape(0);
    changeShape1.performed += ctx => TryChangeShape(1);
    changeShape2.performed += ctx => TryChangeShape(2);
    changeShape3.performed += ctx => TryChangeShape(3);

    inputActions.Enable();
}

private void OnDisable()
{
    changeShape0.performed -= ctx => TryChangeShape(0);
    changeShape1.performed -= ctx => TryChangeShape(1);
    changeShape2.performed -= ctx => TryChangeShape(2);
    changeShape3.performed -= ctx => TryChangeShape(3);

    inputActions.Disable();
}

// Remove the old Update() method completely

    private void TryChangeShape(int index)
    {
        if(lockedPaths.Contains(index)) return;

        if(activeShapes[index] != null)
        {
            Vector3 currentPosition = activeShapes[index].transform.position;
            currentPosition.y = startPoints[index].position.y + floatHeight;

            Destroy(activeShapes[index]);

            // Play shape change sound
            if(audioSource != null && changeSound != null)
            {
                audioSource.PlayOneShot(changeSound, soundVolume);
            }

            currentShapeIndices[index] = (currentShapeIndices[index] + 1) % shapePrefabs.Length;
            int newIndex = currentShapeIndices[index];

            activeShapes[index] = Instantiate(shapePrefabs[newIndex], currentPosition, Quaternion.identity);

            if(movementCoroutines[index] != null)
                StopCoroutine(movementCoroutines[index]);

            movementCoroutines[index] = StartCoroutine(MoveShapeToEndPoint(activeShapes[index], endPoints[index], index));
        }
    }
}