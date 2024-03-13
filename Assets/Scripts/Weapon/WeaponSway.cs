using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] private Transform Weapon;
    [SerializeField] private float SlerpSpeed;
    [SerializeField] private float Intensity;
    [SerializeField] private float AimIntensity;

    private void Update()
    {
        Sway();
    }
    private void Sway()
    {
        float X= Input.GetAxis("Mouse X")* TotalIntensity();
        float Y= Input.GetAxis("Mouse Y")* TotalIntensity();

        Quaternion XRot = Quaternion.AngleAxis(-Y,Vector3.right);
        Quaternion YRot = Quaternion.AngleAxis(-X,Vector3.up);

        Quaternion Rot=XRot*YRot;

        Weapon.localRotation = Quaternion.Slerp(Weapon.localRotation,Rot,SlerpSpeed*Time.deltaTime);
    }
    private float TotalIntensity()
    {
        if (WeaponManager.Instance.Aim)
        {
            return AimIntensity;
        }
        else
        {
            return Intensity;
        }
    }
}
