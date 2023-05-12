using System.Collections;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletSpeed = 100f;
    public float fireRate = 0.1f;
    public float sprayAngle = 5f;
    public float adsSpeed = 10f;
    public Vector3 recoilKick = new Vector3(0f, 0.2f, -0.5f);
    public float recoilDuration = 0.1f;
    public float ADSMulti = 2f;

    private bool isFiring = false;
    private bool isADS = false;
    private float fireTimer = 0f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private void Start()
    {
        // Store initial position and rotation for smooth ADS transition
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartFiring();
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            StopFiring();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            ToggleADS();
        }

        if (isFiring)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireRate)
            {
                Fire();
                fireTimer = 0f;
            }
        }

        // Smoothly transition between ADS and non-ADS states
        if (isADS)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * adsSpeed);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * adsSpeed);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime * adsSpeed);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, initialRotation, Time.deltaTime * adsSpeed);
        }
    }

    private void StartFiring()
    {
        isFiring = true;
    }

    private void StopFiring()
    {
        isFiring = false;
    }

    private void ToggleADS()
    {
        isADS = !isADS;

        // Adjust gun properties based on ADS state
        if (isADS)
        {
            // Example: Reduce spray angle when ADS
            sprayAngle /= ADSMulti;

            // Set target position and rotation for ADS
            targetPosition = new Vector3(-0.251f, 0.007f, -0.1f);
            targetRotation = Quaternion.Euler(0f, 0f, 1f);
        }
        else
        {
            // Example: Reset spray angle when not ADS
            sprayAngle *= ADSMulti;

            // Reset target position and rotation to initial values
            targetPosition = initialPosition;
            targetRotation = initialRotation;
        }
    }

    private void Fire()
    {
        // Calculate the randomized spray angle
        Quaternion sprayRotation = Quaternion.Euler(Random.Range(-sprayAngle, sprayAngle), Random.Range(-sprayAngle, sprayAngle), 0f);

        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation * sprayRotation);

        // Apply force to the bullet
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = bullet.transform.forward * bulletSpeed;

        // Apply visual recoil kick
        StartCoroutine(RecoilKick());
    }

    private IEnumerator RecoilKick()
    {
        // Save the initial position and rotation
        Vector3 initialGunPosition = transform.localPosition;
        Quaternion initialGunRotation = transform.localRotation;

        // Apply recoil kick
        transform.localPosition += recoilKick;
        yield return new WaitForSeconds(recoilDuration);

        // Reset to initial position and rotation
        transform.localPosition = initialGunPosition;
        transform.localRotation = initialGunRotation;
    }
}



