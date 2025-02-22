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

        UpdateIndicators();

        CheckForWin();
    }

    private IEnumerator SmoothSwap(GameObject jar1, GameObject jar2)
    {
        float duration = 0.5f; // Duration of the swap
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

    public void UpdateIndicators () {
        foreach (GameObject jar in jars)
        {
            jar.GetComponent<ISwap>().UpdateIndicator();
        }
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
        RoomOneManager.correctlyOrdered = 0;
        for (int i = 0; i < jars.Count; i++)
        {
            ISwap jar1 = jars[i].GetComponent<ISwap>();
            if (jar1.correctIdx != jar1.index)
            {
                isOrdered = false;
            } else {
                RoomOneManager.correctlyOrdered++;
            }
        }

        RoomOneManager.instance.UpdateUI();

        if (isOrdered)
        {
            RoomOneManager.questsDone[2] = true;
            RoomOneManager.EndLevel();
            foreach (GameObject jar in jars)
            {
                StartCoroutine(DisableIndicators());
                jar.GetComponent<ISwap>().enabled = false;
            }
        }
    }

    private IEnumerator DisableIndicators()
    {
        yield return new WaitForSeconds(3.0f);
        foreach (GameObject jar in jars)
        {
            jar.transform.GetChild(0).gameObject.SetActive(false);
            jar.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
