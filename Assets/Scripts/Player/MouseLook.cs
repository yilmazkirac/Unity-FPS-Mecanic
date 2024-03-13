using UnityEngine;
public class MouseLook : MonoBehaviour
{
    public static MouseLook Instance;
    private void Awake()
    {
        if (Instance != null&& Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    [SerializeField] private Transform CharacterBody;
    public Transform CameraParent;

    [Header("")]


    [SerializeField] private float Sensitivity;

    float X;
    float Y;


    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState= CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        MouseControl();
    }

    private void MouseControl()
    {
      X +=Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime;
      Y +=Input.GetAxis("Mouse Y") * Sensitivity * Time.deltaTime;
        Y=Mathf.Clamp(Y, -80, 80);

        CameraParent.localRotation=Quaternion.Euler(-Y,0f,0f);

        CharacterBody.localRotation=Quaternion.Euler(0f, X, 0f);
    }

    public void AddRecoil(float x,float y)
    {
        X += x;
        Y += y;
    }
}
