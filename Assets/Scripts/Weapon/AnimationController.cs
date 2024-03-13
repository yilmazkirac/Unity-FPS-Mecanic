using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator Animation;

    private void Start()
    {
        Animation = GetComponent<Animator>();
    }
    public void SetCrossFade(string AnimID)
    {
        Animation.CrossFade(AnimID,1f);
    }
    public void Play(string AnimID)
    {
        Animation.Play(AnimID);
    }
    public void SetBool(string AnimID,bool Animbool)
    {
        Animation.SetBool(AnimID,Animbool);
    }
    public void EndFire()
    {
        WeaponManager.Instance.EndFire();
    }
 
    public void EndReload()
    {
        WeaponManager.Instance.EndReload();
    }
    public void WeaponDown()
    {
        WeaponManager.Instance.CloseWeapon();
    }
    public void SetAvailability(int Index)
    {
        WeaponManager.Instance.Availability = Index == 0 ? false : true;
    }
}
