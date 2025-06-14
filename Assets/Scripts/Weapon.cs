using UnityEngine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    #region Variables
    public static Weapon instance;

    private StarterAssetsInputs _inputs;
    private bool paused = false;

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
    #endregion

    //////////////////////////////////// Weapon Collision In WeaponExtendible.cs

    void Awake() => instance = this;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get inputs, position before shooting the weapon, and the rigidbody of the extendible part
        _inputs = GetComponent<StarterAssetsInputs>();
        _initialLocalPosition = _extendible.localPosition;
        _initialLocalRotation = _extendible.localRotation;
        _extendibleRb = _extendible.GetComponent<Rigidbody>();
        paused = false;
        
        // RoomOneManager.instance.PauseGame(paused);
    }

    void Update()
    {
        // Update Rope Visuals
        if (extended) {
            _lineRenderer.SetPosition(0, _weapon.position + _weapon.forward * 0.1f + _weapon.up * 0.05f);
            _lineRenderer.SetPosition(1, _extendible.position);
        }

        if (_inputs.pause) {
            // Pause Menu
            // PauseMenu.instance.TogglePauseMenu();
            paused = !paused;
            RoomOneManager.instance.PauseGame(paused);
            _inputs.pause = false;
        }
    } 

    // Update is called once per frame
    void FixedUpdate()
    {
        // Weapon Shooting
        if (_inputs.shoot && !extended) {
            AudioManager.Instance.PlaySound(SoundType.Shoot);
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
            _extendibleRb.isKinematic = false;
            _extendibleRb.AddForce(Camera.main.transform.forward.normalized * launchForce);
            _extendible.GetComponent<BoxCollider>().enabled = true;
            StartCoroutine(Retract()); // retraction to original position

        }
    }

    // handling when picking an object
    public void RetractImmediately (float timer = 1.0f) {
        StopAllCoroutines();
        if (_extendibleRb.isKinematic == false) {
            _extendibleRb.linearVelocity = Vector3.zero;
            _extendibleRb.angularVelocity = Vector3.zero;
        }
        // _extendibleRb.isKinematic = true;
        StartCoroutine(Retract(timer));
    }

    // retraction coroutine
    IEnumerator Retract (float timer = 1f) {
        yield return new WaitForSeconds(timer);
        _extendibleRb.isKinematic = true;
        _extendible.parent = _weapon.parent;
        _extendible.GetComponent<BoxCollider>().enabled = false;

        float t = 0;
        Vector3 startPos = _extendible.localPosition;

        // Retract smoothly
        while (t < 1f) {
            t += Time.deltaTime * retractSpeed;
            _extendible.localPosition = Vector3.Lerp(startPos, _initialLocalPosition, t);
            _extendible.localRotation = Quaternion.Lerp(_extendible.localRotation, _initialLocalRotation, t);
            yield return null;
        }

        _extendible.localPosition = _initialLocalPosition;
        _extendible.localRotation = _initialLocalRotation;
        _lineRenderer.enabled = false;
        extended = false;
        _extendibleRb.isKinematic = false;
        _extendible.GetComponent<BoxCollider>().enabled = false;
    }
}
