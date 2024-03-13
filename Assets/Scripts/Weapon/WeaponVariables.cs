using UnityEngine;
using static WeaponManager;

public class WeaponVariables : MonoBehaviour
{
    public string Weapon_ID;
    public Transform WeaponParent;

    [Header("Animation")]
    public AnimationController Animation;

    [Header("Fire Variables")]
    public int CurrentAmmo;
    public float FireFreq;
    public float FireRange;

    [Header("Reload Variables")]

    public int MaxAmmo;

    public WeaponManager.AmmoTypes Type;


    [Header("Muzzle Flash")]
    public Transform WeaponTip;
    public GameObject MuzzleFlash;
    public ParticleSystem BulletShells;


    [Header("Aim")]
    public Vector3 OriginalPos;
    public Vector3 AimPos;

    public Quaternion AimRot;
    public Quaternion OriginalRot;

    public float AimSpeed;

    public float OriginalFov;
    public float AimFov;

    [Header("Bullet Scatter")]
    public Quaternion MaxScatter;
    public Quaternion MinScatter;


    [Header("Recoil")]
    public Vector2 MaxRecoil;
    public Vector2 MinRecoil;
    public Recoil WeaponRecoil;

    [Header("DropWeapon")]
    public GameObject PickableWeapon;
}
