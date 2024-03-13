using TMPro;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public static ObjectInteraction Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    [SerializeField] private Transform Camera;
    [SerializeField] private float InteractionDistance;

    private RaycastHit InteractionRaycast;

    [Header("Item Name")]
    [SerializeField] private GameObject ItemNameObject;
    [SerializeField] private TextMeshProUGUI ItemNameText;
    private void Update()
    {
        if (Physics.Raycast(Camera.position,Camera.forward,out InteractionRaycast, InteractionDistance))
        {
            if (InteractionRaycast.transform.GetComponent<IInteractible>()!=null)
            {
                DynamicCross.Instance.Available = false;             
                ItemNameText.text = InteractionRaycast.transform.GetComponent<IInteractible>().Name;
                ItemNameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    InteractionRaycast.transform.GetComponent<IInteractible>().Interact();
                }
            }
            else
            {
                DynamicCross.Instance.Available = true;
                ItemNameObject.SetActive(false);
            }
        }
        else
        {
            DynamicCross.Instance.Available = true;
            ItemNameObject.SetActive(false);
        }
    }
}
