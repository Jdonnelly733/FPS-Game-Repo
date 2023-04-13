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
    public float jumpSprayMulti = 2f;
    public float crouchSprayMulti = 0.5f;
    public float walkSprayMulti = 1f;
    public float sprayMulti;
    public AnimationCurve sprayOverTime;
    public float sprayTimer = 0f;
 

    public enum moveTypeData
    {
        Walk,
        Run,
        Jump,
        Crouch
    };

    public moveTypeData moveType;

    //bools 
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    //public LayerMask whatIsEnemy;

    //Graphics
    public GameObject muzzleFlash, bulletHoleGraphic;

    public TextMeshProUGUI text;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        //Motor = GetComponent<PlayerMotor>();

    }
    private void Update()
    {

        // Counting time between last bullet shot
        if (sprayTimer < 2f)
        {
            sprayTimer += Time.deltaTime;
            
        }
        else
        {
            sprayTimer = 0f;
        }


        MyInput();

        sprayTimer += Time.deltaTime;

        print(sprayTimer);


        shotsFired = magazineSize - bulletsLeft;
        

        //SetText
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

        int totalShotsFired = magazineSize - bulletsLeft;
       
        






        //float x = Random.Range(-spread * Motor.velocityMagnitude * sprayMulti, spread * Motor.velocityMagnitude * sprayMulti);
        //float y = Random.Range(-spread * Motor.velocityMagnitude * sprayMulti, spread * Motor.velocityMagnitude * sprayMulti);

        
        //Vector3 direction = fpsCam.transform.forward + new Vector3(recoil.x, recoil.y, 0);

        //RayCast
        //if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range))
        //{
           //print(rayHit.collider.name);
           

            //if (rayHit.collider.CompareTag("Enemy"))
                //// ADD DAMAGE HERE WHEN OTHER PLAYERS ARE ADDED
        //}

        

        //Graphics
        Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
        //Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

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