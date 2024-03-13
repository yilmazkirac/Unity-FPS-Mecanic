using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour, IInteractible
{
    public string WeaponName;
    public string Name { get => WeaponName; set => WeaponName=value; }

    public void Interact()
    {
        WeaponManager.Instance.PickWeapon(WeaponName);
        Destroy(gameObject);
    }
}
