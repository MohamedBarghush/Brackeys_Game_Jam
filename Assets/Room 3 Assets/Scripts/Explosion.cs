using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject explosionEffectPrefab; // Assign in Inspector (Particle Effect)
    public GameObject spawnedObjectPrefab; // Assign in Inspector (Another Prefab)

    public GameObject targetObject; // The transform where prefabs should be instantiated

    public void Explode()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target transform is not assigned!");
            return;
        }

        Transform transform = targetObject.transform;
        Destroy(targetObject);

        // Instantiate the explosion effect
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, targetObject.transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Explosion effect prefab is not assigned!");
        }

        // Instantiate the additional object
        if (spawnedObjectPrefab != null)
        {
            Instantiate(spawnedObjectPrefab, targetObject.transform.position, targetObject.transform.rotation);
        }
        else
        {
            Debug.LogError("Spawned object prefab is not assigned!");
        }

        

        
    }
}
