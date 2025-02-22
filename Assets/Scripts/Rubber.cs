using UnityEngine;
public enum rubberType
{
    Duck,
    Frog,
    Cat

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

        animator.SetTrigger("squeak");

    }
}
