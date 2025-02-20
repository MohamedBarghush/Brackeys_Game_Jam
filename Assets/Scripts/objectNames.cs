using UnityEngine;
using System.Collections.Generic;

public class ObjectChecker : MonoBehaviour
{
    private List<string> objectNames = new List<string>();
    public string objectName;
    public int requiredCount = 3;

    void OnTriggerEnter(Collider other)
    {
        objectNames.Add(other.gameObject.name);
        Debug.Log(other.gameObject.name);
        CheckObjects();
    }

    void OnTriggerExit(Collider other)
    {
        objectNames.Remove(other.gameObject.name);
        CheckObjects();
    }

    void CheckObjects()
    {
        if (objectNames.Count == requiredCount) 
        {
            bool allSame = true;
            //string firstName = objectNames[0];

            foreach (string name in objectNames)
            {
                Debug.Log(name);
                if (name != objectName)
                {
                    allSame = false;
                    break;
                }
            }

            if (allSame)
            {
                Debug.Log("Correct");
            }
        }
    }
}
