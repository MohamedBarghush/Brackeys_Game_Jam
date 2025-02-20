using UnityEngine;

public class IRotate : MonoBehaviour, IInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(bool grabbed = false)
    {
        Debug.Log("I (" + gameObject.name + ") " + "will be rotated");
        transform.rotation *= Quaternion.Euler(30, 0, 0);
    }
    public void Check()
    {
        
    }
}
