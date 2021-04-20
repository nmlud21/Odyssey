using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Random = System.Random;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    private float nextTimeToFire = 0f;
    public float recoilStrength = 5f;

    public int maxAmmo = 10;
    private int currentAmmo;
    public int maxReserves;
    public int currentReserves;
    public float reloadSpeed = 1f;
    private bool isReloading = false;
    private bool hasReserves = true;

    public Camera fpsCamera;
    public ParticleSystem muzzleFlash;
    public Animator animator;
    public Text magazine;
    public Text reserves;
    public CharacterController player;
    public GameObject weaponSwap;
    public GameObject crosshair;

    public LayerMask enemyMask;
    //public GameObject recoilCamera;


    void Start()
    {
        currentAmmo = maxAmmo;
        currentReserves = maxReserves;
        reserves.text = currentReserves.ToString();
        animator.keepAnimatorControllerStateOnDisable = true; //prevents weapon reload animation from messing up gun when switching weapons
    }

    private void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
        animator.SetBool("Shooting", false);
        reserves.text = currentReserves.ToString();
        if (weaponSwap.GetComponent<WeaponSwap>().selectedWeapon == 0)
        {
            animator.Play("WeaponIdle", 0, 0f);
        }
        else if (weaponSwap.GetComponent<WeaponSwap>().selectedWeapon == 1)
        {
            animator.Play("PistolIdle", 0, 0f);
        }
    }

    void Update()
    {

        if (PauseMenu.gameIsPaused == false || CrystalInteract.gameOver == false)
        {
            magazine.text = currentAmmo.ToString();

            if (isReloading)
            {
                return;
            }

            if ((currentAmmo <= 0 && hasReserves) || (Input.GetKeyDown(KeyCode.R) && currentAmmo != maxAmmo)) //Reload
            {
                if (player.GetComponent<PlayerMove>().isSprinting == false)
                {
                    if (currentReserves != 0)
                    {
                        StartCoroutine(Reload());
                        animator.SetBool("Shooting", false);
                        animator.SetBool("Walking", false);
                    }
                    else
                    { 
                        animator.SetBool("Shooting", false);
                        hasReserves = false;
                        return;
                    }
                }
            }

            if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire &&
                weaponSwap.GetComponent<WeaponSwap>().selectedWeapon == 0) //can use Fire1
            {
                if (player.GetComponent<PlayerMove>().isSprinting == false)
                {
                    //Sets fire rate to .25 sec intervals
                    nextTimeToFire = Time.time + 1f / fireRate;
                    Shoot();
                }
            }
            else if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire &&
                     weaponSwap.GetComponent<WeaponSwap>().selectedWeapon == 1) //Semi-auto pistol
            {
                if (player.GetComponent<PlayerMove>().isSprinting == false)
                {
                    nextTimeToFire = Time.time + 1f / fireRate;
                    Shoot();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                animator.SetBool("Shooting", false);
            }

            if (player.GetComponent<PlayerMove>().isWalking) //Walking
            {
                animator.SetBool("Walking", true);
            }
            else
            {
                animator.SetBool("Walking", false);
            }

            if (player.GetComponent<PlayerMove>().isSprinting)
            {
                animator.SetBool("Sprinting", true);
            }
            else
            {
                animator.SetBool("Sprinting", false);
            }

            if (Input.GetMouseButton(1)) //ADS
            {
                animator.SetBool("Aiming", true);
                crosshair.SetActive(false);
            }
            else
            {
                animator.SetBool("Aiming", false);
                crosshair.SetActive(true);
            }
        }
        
    }

    void Shoot()
    {
        if (currentAmmo != 0)
        {
            muzzleFlash.Play();
            currentAmmo--;
            magazine.text = currentAmmo.ToString();
            animator.SetBool("Shooting", true);
        }
        
        //Camera Recoil
        //recoilCamera.transform.Rotate(new Vector3(-recoilStrength, 0, 0));

        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range, enemyMask))
        {
            Debug.Log(hit.transform.name);

            EnemyController enemy = hit.transform.GetComponent<EnemyController>();
            enemy.TakeDamage(damage);

            if (enemy.isDead)
            {
                currentReserves += 15;
                reserves.text = currentReserves.ToString();
            }

            // EnemyController enemy = GetComponent<EnemyController>();
            // if (hit.collider.CompareTag("Enemy"))
            // {
            //     enemy.TakeDamage(damage);
            // }
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        //Debug.Log("Reloading...");

        animator.SetBool("Reloading", true);
        //amount of seconds to wait for reload
        yield return new WaitForSeconds(reloadSpeed - .25f);
        animator.SetBool("Reloading", false);
        animator.SetBool("Aiming", false);
        yield return new WaitForSeconds(.3f); //used so you cannot shoot before the reload animation is finished

        if (currentReserves > 0)
        {
            hasReserves = true;
        }
        else
        {
            hasReserves = false;
        }
        
        if (currentReserves >= maxAmmo)
        {
            currentReserves -= maxAmmo - currentAmmo;
            currentAmmo = maxAmmo;
        }
        else if(currentReserves < maxAmmo && currentReserves > 0)
        {
            currentAmmo += currentReserves;
            currentReserves = 0;
            if (currentAmmo > maxAmmo)
            {
                currentAmmo = maxAmmo;
            }
        }
        
        isReloading = false;
        magazine.text = currentAmmo.ToString();
        reserves.text = currentReserves.ToString();
    }
}