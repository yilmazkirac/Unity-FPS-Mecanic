using UnityEngine;

public class DynamicCross : MonoBehaviour
{
    public static DynamicCross Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    [HideInInspector] public bool Available;
    [SerializeField] private RectTransform Crosshair;

    [Header("")]
    [SerializeField] private float MaxSize;
    [SerializeField] private float MinSize;
    [SerializeField] private float CurrentSize;
    [Header("")]
    [SerializeField] private float Speed;

    private void Update()
    {
        Inputs();
        SetSize();
    }
    private void SetSize()
    {
        Crosshair.sizeDelta = new Vector2(CurrentSize, CurrentSize);
    }
    private void Inputs()
    {
        if (!PlayerMovement.Instance.IsWalking&&!PlayerMovement.Instance.IsRunning)
        {
            SetMin();
        }
        else if (PlayerMovement.Instance.IsWalking)
        {
            SetMax();
        }

        if (PlayerMovement.Instance.IsRunning || !Available || WeaponManager.Instance.Aim)
        {
            SetDeactive();
        }
        else
        {
            SetActive();
        }
    }

    private void SetMin()
    {
        CurrentSize=Mathf.Lerp(CurrentSize, MinSize, Speed*Time.deltaTime);
    }
    private void SetMax()
    {
        CurrentSize = Mathf.Lerp(CurrentSize, MaxSize, Speed * Time.deltaTime);
    }
    private void SetActive()
    {
        Crosshair.gameObject.SetActive(true);
    }
    private void SetDeactive()
    {
        Crosshair.gameObject.SetActive(false);
    }
}
