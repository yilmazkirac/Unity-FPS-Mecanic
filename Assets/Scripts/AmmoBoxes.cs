using UnityEngine;

public class AmmoBoxes : MonoBehaviour, IInteractible
{
    public string ItemName;
    public WeaponManager.AmmoTypes Type;
    public int Amount;

    public string Name { get => ItemName; set => ItemName = value; }

    public void Interact()
    {
        WeaponManager.Instance.AddAmmo(Type,Amount);
        Destroy(gameObject);
    }
}
