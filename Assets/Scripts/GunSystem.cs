using UnityEngine;
using TMPro;

public class GunSystem : MonoBehaviour
{
    //Gun stats
    public int damage;
    public PlayerMotor Motor;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;
    public int ammoReserves = 150;
    public int ammoReservesModifier;
    public int shotsFired;
    public float sprayMulti;
    public float sprayTimer = 0f;
    public float recoilResetTime = 1f;


    public enum moveTypeData //enum to define move 
    {
        Walk,
        Run,
        Jump,
        Crouch
    };

    public moveTypeData moveType;

    //bools 
    public bool shooting, readyToShoot, reloading;

    //Reference
    public Camera playerCam;
    public Transform recoilPoint;
    public Transform attackPoint;
    public RaycastHit rayHit;
     
    
    // Fixed recoil pattern
    public Vector2[] recoilPattern;
    private int recoilIndex = 0;

    //Graphics
    public GameObject muzzleFlash, bulletHoleGraphic;

    public TextMeshProUGUI text;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();
       

        //(sprayTimer);

        // Counting time between last bullet shot
        if (sprayTimer < recoilResetTime && readyToShoot == true)
        {
            sprayTimer += Time.deltaTime;
        }
        else if(readyToShoot == false)
        {
            sprayTimer = 0f;
            
        }
        else
        {
            recoilIndex = 0; // Reset the recoil pattern after recoilResetTime seconds of not shooting
        }
        //Set ammo Counter
        text.SetText(bulletsLeft + " / " + ammoReserves);
    }

    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        shotsFired = magazineSize - bulletsLeft;

        // Get the next value in the recoil pattern
        Vector2 recoilOffset = recoilPoint.transform.position;
        print(recoilOffset);
        if (recoilPattern.Length > 0)
        {
            recoilOffset = recoilPattern[recoilIndex];
            recoilIndex = (recoilIndex + 1) % recoilPattern.Length;
        }

        // Calculate the shoot direction relative to the camera
        Vector3 cameraForward = playerCam.transform.forward;
        Vector3 cameraRight = playerCam.transform.right;
        Vector3 shootDirection = cameraForward + cameraRight * recoilOffset.x * sprayMulti + playerCam.transform.up * recoilOffset.y * sprayMulti;
        shootDirection.Normalize();
        playerCam.gameObject.transform.LookAt(playerCam.transform.position + shootDirection);

        Ray ray = new Ray(playerCam.transform.position, shootDirection);

        if (Physics.Raycast(ray, out rayHit, range))
        {
            //if (rayHit.collider.CompareTag("Enemy"))
            //{
            // Add damage to enemy here
            //}
        }

        //Graphics
        Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));


        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }


    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);

    }


    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
        ammoReserves = ammoReserves - shotsFired;
        shotsFired = 0;
    }
}