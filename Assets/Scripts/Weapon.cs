using UnityEngine;
using StarterAssets;
using System.Collections;

public class Weapon : MonoBehaviour
{
    #region Variables
    public static Weapon instance;

    private StarterAssetsInputs _inputs;

    [Header("Weapon Settings")]
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _weapon;
    [SerializeField] private Transform _extendible;
    private Rigidbody _extendibleRb;
    [SerializeField] private float launchForce = 1000;
    [SerializeField] private float retractSpeed = 5;
    [SerializeField] private bool extended = false;
    private Vector3 _initialLocalPosition;
    private Quaternion _initialLocalRotation;
    private Vector3 targetPos;
    #endregion

    //////////////////////////////////// Weapon Collision In WeaponExtendible.cs

    void Awake()
    {
        instance = this;
    } 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get inputs, position before shooting the weapon, and the rigidbody of the extendible part
        _inputs = GetComponent<StarterAssetsInputs>();
        _initialLocalPosition = _extendible.localPosition;
        _initialLocalRotation = _extendible.localRotation;
        _extendibleRb = _extendible.GetComponent<Rigidbody>();
        // _extendibleRb.isKinematic = true;
    }

    void Update()
    {
        // Update Rope Visuals
        if (extended) {
            _lineRenderer.SetPosition(0, _weapon.position + _weapon.forward * 0.1f);
            _lineRenderer.SetPosition(1, _extendible.position);
        }
    } 

    // Update is called once per frame
    void FixedUpdate()
    {
        // Weapon Shooting
        if (_inputs.shoot && !extended) {
            Shoot ();
            _inputs.shoot = false;
        }
    }

    // Shooting logic
    void Shoot () { 
        if (!extended) {
            extended = true;
            _lineRenderer.enabled = true;
            _extendible.parent = null;
            _extendibleRb.AddForce(Camera.main.transform.forward.normalized * launchForce);
            StartCoroutine(Retract()); // retraction to original position
        }
    }

    // handling when picking an object
    public void RetractImmediately (float timer = 1.0f) {
        StopAllCoroutines();
        StartCoroutine(Retract(timer));
    }

    // retraction coroutine
    IEnumerator Retract (float timer = 2f) {
        yield return new WaitForSeconds(timer);
        _extendible.GetComponent<BoxCollider>().enabled = true;
        _extendible.parent = _weapon.parent;
        targetPos = _initialLocalPosition;

        // Retract smoothly
        while (Vector3.Distance(_extendible.localPosition, targetPos) > 0.5f) {
            _extendible.localPosition = Vector3.LerpUnclamped(_extendible.localPosition, targetPos, Time.deltaTime * retractSpeed);
            yield return null;
        }

        _lineRenderer.enabled = false;
        _extendible.localPosition = _initialLocalPosition;
        _extendible.localRotation = _initialLocalRotation;
        extended = false;
    }
}
