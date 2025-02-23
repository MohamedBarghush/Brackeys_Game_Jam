using UnityEngine;
using UnityEngine.UIElements;

public class SmoothRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f;
    private Animator animator;
    [SerializeField] private UIDocument uiDocument;
    private Button duckyButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        duckyButton = uiDocument.rootVisualElement.Q<Button>("Ducky");
        duckyButton.clicked += () => AnimateAnim();
        InvokeRepeating("AnimateAnim", 20f, 20f);
    }

    // Update is called once per frame
    void Update()
    {
        float xRotation = Random.Range(0f, 0.5f);
        float yRotation = Random.Range(0.5f, 1f);
        float zRotation = Random.Range(0f, 0.5f);
        Vector3 randomRotation = new Vector3(xRotation, yRotation, zRotation);
        transform.Rotate(randomRotation * Time.deltaTime * rotationSpeed);
    }

    void AnimateAnim()
    {
        AudioManager.Instance.PlaySound(SoundType.Duck);
        animator.SetTrigger("squeak");
    }
}
