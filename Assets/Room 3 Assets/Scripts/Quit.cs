using UnityEngine;
using StarterAssets;

public class Quit : MonoBehaviour
{
    [SerializeField] private StarterAssetsInputs inputs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inputs.pause) {
            Application.Quit();
        }
    }
}
