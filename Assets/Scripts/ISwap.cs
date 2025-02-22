using UnityEngine;

public class ISwap : MonoBehaviour, IInteractable
{
    [Range(0,3)] public int correctIdx = 0;
    [Range(0,3)] public int index = 0;

    void Start()
    {
        UpdateIndicator();
    }

    public void Interact(bool grabbed = false, Transform hook = null)
    {
        AudioManager.Instance.PlaySound(SoundType.Barrel);
        Swapper.instance.SetIndex(index);
    }

    public void UpdateIndicator () {
        if (index == correctIdx) {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
        } else {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}
