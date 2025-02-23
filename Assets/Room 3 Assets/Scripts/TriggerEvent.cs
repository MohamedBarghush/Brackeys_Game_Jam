using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player"; // Player's tag
    [SerializeField] private QTEManager qteManager; // Reference to QTEManager
    [SerializeField] private QTEEvent qteEvent; // The QTE event to start
    [SerializeField] private UI_Control uI_Control; // The QTE event to start
    private BoxCollider box;
    private void Awake()
    {
        box = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && qteManager != null && qteEvent != null)
        {
            qteManager.startEvent(qteEvent); // Start the QTE event
            uI_Control.ChangeQTEUI();
            box.enabled = false;
        }

    }


    public void PrintStart()
    {
        Debug.Log("Start");
    }

    public void PrintSuccess()
    {
        Debug.Log("Success");
    }

    public void PrintFail()
    {
        Debug.Log("Fail");
    }

    public void PrintEnd()
    {
        Debug.Log("End");
    }
}
