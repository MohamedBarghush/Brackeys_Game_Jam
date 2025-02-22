using UnityEngine;

public class ISwap : MonoBehaviour, IInteractable
{
    [Range(0,3)] public int correctIdx = 0;
    [Range(0,3)] public int index = 0;

    public void Interact(bool grabbed = false)
    {
        Swapper.instance.SetIndex(index);
    }
}
