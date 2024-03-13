using System.Collections;
using TMPro;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    [SerializeField] private Transform WeaponTransform;
    [SerializeField] private Transform CurrentWeaponParent;
    private Camera PlayerCamera;
    public bool Availability;


    [Header("Weapon Slots")]
    [SerializeField] private WeaponVariables WeaponSlot1;
    [SerializeField] private WeaponVariables WeaponSlot2;

    [Header("Animation")]
    [SerializeField] private AnimationController Animation;
    [SerializeField] private string Fire1_ID;
    [SerializeField] private string Fire2_ID;
    [SerializeField] private string Reload_ID;
    [SerializeField] private string WeaponDown_ID;
    [SerializeField] private string Aim_ID;

    [Header("Fire Variables")]
    [SerializeField] private bool Fire;
    [SerializeField] private int CurrentAmmo;
    [SerializeField] private float FireFreq;
    float FireCounter;

    RaycastHit FireRaycast;
    [SerializeField] private float FireRange;


    [Header("Reload Variables")]
    [SerializeField] private bool Reload;
    [SerializeField] private int MaxAmmo;
    [SerializeField] private int TotalAmmo;
    [SerializeField] private AmmoTypes Type;

    public enum AmmoTypes { _5_56, _7_62, _9mm, _45cal, _12gal }
    [Header("Ammo Type")]
    [SerializeField] int _5_56;
    [SerializeField] int _7_62;
    [SerializeField] int _9mm;
    [SerializeField] int _45cal;
    [SerializeField] int _12gal;


    [Header("Muzzle Flash")]
    [SerializeField] private Transform WeaponTip;
    [SerializeField] private GameObject MuzzleFlash;
    [SerializeField] private ParticleSystem BulletShells;

    [Header("Bullet Holes & Particles")]
    [SerializeField] private GameObject[] BulletHoles;
    [SerializeField] private GameObject[] Particles;

    [Header("Indicators")]
    [SerializeField] private TextMeshProUGUI CurrentAmmoText;
    [SerializeField] private TextMeshProUGUI TotalAmmoText;

    [Header("Aim")]
    public bool Aim;
    [SerializeField] private Vector3 OriginalPos;
    [SerializeField] private Vector3 AimPos;

    [SerializeField] private Quaternion AimRot;
    [SerializeField] private Quaternion OriginalRot;

    [SerializeField] private float AimSpeed;

    [SerializeField] private float OriginalFov;
    [SerializeField] private float AimFov;


    [Header("Bullet Scatter")]
    [SerializeField] private Quaternion MaxScatter;
    [SerializeField] private Quaternion MinScatter;
    [SerializeField] private Quaternion CurrentScatter;


    [Header("Recoil")]
    [SerializeField] private Vector2 MaxRecoil;
    [SerializeField] private Vector2 MinRecoil;
    [SerializeField] private Recoil CameraRecoil;
    [SerializeField] private Recoil WeaponRecoil;


    [Header("Change Weapon")]
    [SerializeField] private WeaponVariables[] AllWeapons;
    private void Start()
    {
        PlayerCamera = CameraController.Instance.Camera.GetComponent<Camera>();
    }
    private void Update()
    {
        Inputs();
        SetTotalAmmo();
        SetAim();
    }

    private void Inputs()
    {
        WeaponTransform.localRotation = MouseLook.Instance.CameraParent.localRotation;
        CurrentAmmoText.text = CurrentAmmo.ToString();
        TotalAmmoText.text = TotalAmmo.ToString();

        if (Input.GetMouseButton(0) && !Reload && CurrentAmmo > 0 && Time.time > FireCounter && Availability )
        {
            StartFire();
        }


        if ((Input.GetKeyDown(KeyCode.R) || CurrentAmmo <= 0) && TotalAmmo > 0 && CurrentAmmo != MaxAmmo && !Fire )
        {
            StartReload();
        }
        if (Input.GetMouseButtonDown(1))
        {
            SetAimBool();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && WeaponSlot1 != null)
        {
            ChangeWeapon(WeaponSlot1);
            EndFire();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && WeaponSlot2 != null)
        {
            ChangeWeapon(WeaponSlot2);
            EndFire();
        }
    }

    public void StartFire()
    {
        Fire = true;
        //  StartCoroutine(EndFireIE());
        if (CurrentAmmo <= 1)
        {
            Animation.SetBool(Fire2_ID, Fire);
        }
        else
        {
            Animation.SetBool(Fire1_ID, Fire);
        }

        CurrentAmmo -= 1;
 

        if (Physics.Raycast(CameraController.Instance.Camera.position, SetScatter() * CameraController.Instance.Camera.forward, out FireRaycast, FireRange))
        {
            if (FireRaycast.transform.GetComponent<Rigidbody>() != null)
            {
                FireRaycast.transform.GetComponent<Rigidbody>().AddForce(-FireRaycast.normal * 1000f);
            }

            GameObject CopyBulletHole = Instantiate(BulletHoles[Random.Range(0, BulletHoles.Length)], FireRaycast.point, Quaternion.LookRotation(FireRaycast.normal));
           // CopyBulletHole.transform.parent = FireRaycast.transform;
            Destroy(CopyBulletHole, 15f);

            for (int i = 0; i < Particles.Length; i++)
            {
                if (Particles[i].tag == FireRaycast.transform.tag)
                {
                    GameObject CopyParticle = Instantiate(Particles[i], FireRaycast.point, Quaternion.LookRotation(FireRaycast.normal));
                    Destroy(CopyParticle, 5f);
                }
            }
        }
        CreateMuzzleFlash();
        SetRecoil();
        CameraRecoil.SetTarget();
        WeaponRecoil.SetTarget();
        FireCounter = Time.time + FireFreq;
    }
    private void CreateMuzzleFlash()
    {
        GameObject muzzleFlashCopy = Instantiate(MuzzleFlash, WeaponTip.position, WeaponTip.rotation, WeaponTip);
        Destroy(muzzleFlashCopy, 5f);
        BulletShells.Play();
    }
    private void SetRecoil()
    {
        float X = Random.Range(MaxRecoil.x, MinRecoil.x);
        float Y = Random.Range(MaxRecoil.y, MinRecoil.y);
        MouseLook.Instance.AddRecoil(X, Y);
    }

    private Quaternion SetScatter()
    {
        if (PlayerMovement.Instance.IsWalking || PlayerMovement.Instance.IsRunning)
        {
            CurrentScatter = Quaternion.Euler(Random.Range(-MaxScatter.eulerAngles.x, MaxScatter.eulerAngles.x), Random.Range(-MaxScatter.eulerAngles.y, MaxScatter.eulerAngles.y), Random.Range(-MaxScatter.eulerAngles.z, MaxScatter.eulerAngles.z));
        }
        else if (Aim)
        {
            CurrentScatter = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            CurrentScatter = Quaternion.Euler(Random.Range(-MinScatter.eulerAngles.x, MinScatter.eulerAngles.x), Random.Range(-MinScatter.eulerAngles.y, MinScatter.eulerAngles.y), Random.Range(-MinScatter.eulerAngles.z, MinScatter.eulerAngles.z));
        }
        return CurrentScatter;
    }
      
    public void EndFire()
    {
        Fire = false;
        Animation.SetBool(Fire1_ID, Fire);
        Animation.SetBool(Fire2_ID, Fire);
    }
    public void StartReload()
    {
        Reload = true;
        Animation.SetBool(Reload_ID, Reload);
    }
    public void EndReload()
    {
        Reload = false;
        int Amount = SetReloadAmount(TotalAmmo);
        CurrentAmmo += Amount;

        if (Type == AmmoTypes._5_56)
            _5_56 -= Amount;

        else if (Type == AmmoTypes._7_62)
            _7_62 -= Amount;

        else if (Type == AmmoTypes._9mm)
            _9mm -= Amount;

        else if (Type == AmmoTypes._45cal)
            _45cal -= Amount;

        else if (Type == AmmoTypes._12gal)
            _12gal -= Amount;

        Animation.SetBool(Reload_ID, false);
    }
    private void SetTotalAmmo()
    {
        if (Type == AmmoTypes._5_56)
            TotalAmmo = _5_56;

        else if (Type == AmmoTypes._7_62)
            TotalAmmo = _7_62;

        else if (Type == AmmoTypes._9mm)
            TotalAmmo = _9mm;

        else if (Type == AmmoTypes._45cal)
            TotalAmmo = _45cal;

        else if (Type == AmmoTypes._12gal)
            TotalAmmo = _12gal;

    }

    private int SetReloadAmount(int InventoryAmount)
    {
        int AmountNeeded = MaxAmmo - CurrentAmmo;
        if (AmountNeeded < InventoryAmount)
            return AmountNeeded;
        else
            return InventoryAmount;
    }

    public void CloseWeapon()
    {

    }
    public void AddAmmo(WeaponManager.AmmoTypes Type, int Amount)
    {
        if (Type == AmmoTypes._5_56)
            _5_56 += Amount;

        else if (Type == AmmoTypes._7_62)
            _7_62 += Amount;

        else if (Type == AmmoTypes._9mm)
            _9mm += Amount;

        else if (Type == AmmoTypes._45cal)
            _45cal += Amount;

        else if (Type == AmmoTypes._12gal)
            _12gal += Amount;
    }

    private void SetAimBool()
    {
        Aim = !Aim;
    }
    private void SetAim()
    {
        if (Aim&&!Reload&&Availability)
        {
            CurrentWeaponParent.localPosition = Vector3.Lerp(CurrentWeaponParent.localPosition, AimPos, AimSpeed * Time.deltaTime);
            CurrentWeaponParent.localRotation = Quaternion.Lerp(CurrentWeaponParent.localRotation, AimRot, AimSpeed * Time.deltaTime);
            PlayerCamera.fieldOfView = Mathf.Lerp(PlayerCamera.fieldOfView, AimFov, AimSpeed * Time.deltaTime);
        }
        else
        {
            CurrentWeaponParent.localPosition = Vector3.Lerp(CurrentWeaponParent.localPosition, OriginalPos, AimSpeed * Time.deltaTime);
            CurrentWeaponParent.localRotation = Quaternion.Lerp(CurrentWeaponParent.localRotation, OriginalRot, AimSpeed * Time.deltaTime);
            PlayerCamera.fieldOfView = Mathf.Lerp(PlayerCamera.fieldOfView, OriginalFov, AimSpeed * Time.deltaTime);
        }
        Animation.SetBool(Aim_ID, Aim);
    }

    private void ChangeWeapon(WeaponVariables Weapon)
    {
        if (Weapon.WeaponParent != CurrentWeaponParent)
        {
            CurrentWeaponParent.gameObject.SetActive(false);
            Weapon.WeaponParent.gameObject.SetActive(true);
            CurrentWeaponParent.GetComponent<WeaponVariables>().CurrentAmmo = CurrentAmmo;
            CurrentWeaponParent = Weapon.WeaponParent;

            Animation = Weapon.Animation;
            CurrentAmmo = Weapon.CurrentAmmo;
            FireFreq = Weapon.FireFreq;
            FireRange = Weapon.FireRange;

            MaxAmmo = Weapon.MaxAmmo;
            Type = Weapon.Type;

            WeaponTip = Weapon.WeaponTip;
            MuzzleFlash = Weapon.MuzzleFlash;
            BulletShells = Weapon.BulletShells;

            OriginalPos = Weapon.OriginalPos;
            AimPos = Weapon.AimPos;

            OriginalRot = Weapon.OriginalRot;
            AimRot = Weapon.AimRot;

            AimSpeed = Weapon.AimSpeed;

            OriginalFov = Weapon.OriginalFov;
            AimFov = Weapon.AimFov;

            MaxScatter = Weapon.MaxScatter;
            MinScatter = Weapon.MinScatter;

            MaxRecoil = Weapon.MaxRecoil;
            MinRecoil = Weapon.MinRecoil;
            WeaponRecoil= Weapon.WeaponRecoil;
        }
    }
    public void PickWeapon(string WeaponName)
    {
        WeaponVariables SelectedWeapon = null;
        for (int i = 0; i < AllWeapons.Length; i++)
        {
            if (AllWeapons[i].Weapon_ID==WeaponName)
            {
                SelectedWeapon = AllWeapons[i];
            }
        }

        if (WeaponSlot2==null)
        {
            WeaponSlot2 = SelectedWeapon;
            ChangeWeapon(WeaponSlot2);
        }
        else
        {
            if (CurrentWeaponParent.GetComponent<WeaponVariables>().Weapon_ID==WeaponSlot1.Weapon_ID)
            {
                DropWeapon(WeaponSlot1);
                WeaponSlot1 = SelectedWeapon;
                ChangeWeapon(WeaponSlot1);
            }
           else if (CurrentWeaponParent.GetComponent<WeaponVariables>().Weapon_ID == WeaponSlot2.Weapon_ID)
            {
                DropWeapon(WeaponSlot2);
                WeaponSlot2 = SelectedWeapon;
                ChangeWeapon(WeaponSlot2);
            }
        }
    }

    private void DropWeapon(WeaponVariables weapon)
    {
        GameObject PickableWeapon = Instantiate(weapon.PickableWeapon, CurrentWeaponParent.position, CurrentWeaponParent.rotation);
        PickableWeapon.GetComponent<Rigidbody>().AddForce(PickableWeapon.transform.forward * 1000f + PickableWeapon.transform.up * 500f);
    }
}

