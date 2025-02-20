using UnityEngine;
using StarterAssets;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public static Weapon instance;
    private StarterAssetsInputs _inputs;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _weapon;
    [SerializeField] private Transform _extendible;
    private Rigidbody _extendibleRb;
    [SerializeField] private float launchForce = 1000;
    [SerializeField] private float retractSpeed = 5;
    [SerializeField] private bool extended = false;
    private Vector3 _initialLocalPosition;
    private Vector3 targetPos;

    void Awake()
    {
        instance = this;
    } 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputs = GetComponent<StarterAssetsInputs>();
        _initialLocalPosition = _extendible.localPosition;
        _extendibleRb = _extendible.GetComponent<Rigidbody>();
        // _extendibleRb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputs.shoot && !extended) {
            Shoot ();
            _inputs.shoot = false;
        }
        if (extended) {
            _lineRenderer.SetPosition(0, _weapon.position + _weapon.forward * 0.1f);
            _lineRenderer.SetPosition(1, _extendible.position);
        }
    }

    void Shoot () {
        if (!extended) {
            extended = true;
            _lineRenderer.enabled = true;
            // _extendibleRb.isKinematic = false;
            _extendible.parent = null;
            _extendibleRb.AddForce(Camera.main.transform.forward.normalized * launchForce);
            StartCoroutine(Retract());
        }
    }

    public void RetractImmediately (float timer = 1.0f) {
        StopAllCoroutines();
        StartCoroutine(Retract(timer));
    }

    IEnumerator Retract (float timer = 2f) {
        yield return new WaitForSeconds(timer);
        _extendible.GetComponent<BoxCollider>().enabled = true;
        // _extendibleRb.isKinematic = true;
        _extendible.parent = _weapon.parent;
        targetPos = _initialLocalPosition;
        while (Vector3.Distance(_extendible.localPosition, targetPos) > 0.5f) {
            _extendible.localPosition = Vector3.LerpUnclamped(_extendible.localPosition, targetPos, Time.deltaTime * retractSpeed);
            yield return null;
        }
        _lineRenderer.enabled = false;
        _extendible.localPosition = _initialLocalPosition;
        _extendible.localRotation = Quaternion.identity;
        extended = false;

    }
}
