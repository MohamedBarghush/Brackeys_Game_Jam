using UnityEngine;
public enum rubberType
{
    Duck,
    Frog,
    Cat,
    Bear

}
public class Rubber : MonoBehaviour, IInteractable
{
    [SerializeField]Animator animator;
    [SerializeField]rubberType rubbertype;

    public void Interact(bool grabbed = false, Transform hook = null)
    {
        if(rubbertype == rubberType.Duck) AudioManager.Instance.PlaySound(SoundType.Duck);
        if(rubbertype == rubberType.Frog) AudioManager.Instance.PlaySound(SoundType.Frog);
        if(rubbertype == rubberType.Cat) AudioManager.Instance.PlaySound(SoundType.Cat);
        if(rubbertype == rubberType.Bear) AudioManager.Instance.PlaySound(SoundType.Bear);

        animator.SetTrigger("squeak");

    }
}
