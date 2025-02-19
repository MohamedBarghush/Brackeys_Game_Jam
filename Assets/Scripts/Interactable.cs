using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        Debug.Log("I'm " + gameObject.name);
    }
    public void Rotate()
    {
        transform.rotation *= Quaternion.Euler(30, 0, 0);
        //if (transform.rotation.x < 0)
        //{
        //    transform.rotation *= Quaternion.Euler(30, 0, 0);
        //}
        //else 
        //{
            
        //}
        
    }
}
