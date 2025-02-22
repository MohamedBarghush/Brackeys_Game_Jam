using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swapper : MonoBehaviour
{
    public static Swapper instance;
    [SerializeField] private List<GameObject> jars;

    [SerializeField] private int index1 = -1;
    [SerializeField] private int index2 = -1;

    public void SwapAndResetJars()
    {
        // Swap positions
        StartCoroutine(SmoothSwap(jars[index1], jars[index2]));

        // Swap them in the list
        GameObject tempJar = jars[index1];
        jars[index1] = jars[index2];
        jars[index2] = tempJar;

        jars[index1].GetComponent<ISwap>().index = index1;
        jars[index2].GetComponent<ISwap>().index = index2;

        // Set jars to null
        index1 = -1;
        index2 = -1;

        CheckForWin();
    }

    private IEnumerator SmoothSwap(GameObject jar1, GameObject jar2)
    {
        float duration = 1.0f; // Duration of the swap
        float elapsedTime = 0.0f;

        Vector3 startPos1 = jar1.transform.position;
        Vector3 startPos2 = jar2.transform.position;

        while (elapsedTime < duration)
        {
            jar1.transform.position = Vector3.Lerp(startPos1, startPos2, elapsedTime / duration);
            jar2.transform.position = Vector3.Lerp(startPos2, startPos1, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        jar1.transform.position = startPos2;
        jar2.transform.position = startPos1;
    }

    public void SetIndex(int index)
    {
        if (index1 == -1)
            index1 = index;
        else if (index2 == -1) {
            index2 = index;
            SwapAndResetJars();
        }
    }

    private void Awake() => instance = this;

    void CheckForWin()
    {
        bool isOrdered = true;
        for (int i = 0; i < jars.Count - 1; i++)
        {
            ISwap jar1 = jars[i].GetComponent<ISwap>();
            if (jar1.index != i)
            {
                isOrdered = false;
                break;
            }
        }

        if (isOrdered)
        {
            RoomOneManager.questsDone[2] = true;
            RoomOneManager.EndLevel();
            foreach (GameObject jar in jars)
            {
                jar.GetComponent<ISwap>().enabled = false;
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
