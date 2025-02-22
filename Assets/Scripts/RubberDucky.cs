using UnityEngine;

public class RubberDucky : MonoBehaviour, IInteractable
{
    [SerializeField]Animator animator;
    public void Interact(bool grabbed = false, Transform hook = null)
    {
        AudioManager.Instance.PlaySound(SoundType.Duck);
        animator.SetTrigger("squeak");

    }
}
