using UnityEngine;

public class Recoil : MonoBehaviour
{
    [SerializeField] private Transform Object;

    [Header("")]
    [SerializeField] private Vector3 MaxTargetPos;
    [SerializeField] private Vector3 MinTargetPos;

    [SerializeField] private Vector3 TargetPos;
    [SerializeField] private Vector3 OriginalPos;

    [Header("")]
    [SerializeField] private Vector3 SlideVector;

    [Header("")]
    [SerializeField] private float SlideSpeed;
    [SerializeField] private float LerpSpeed;

    [SerializeField] private bool Lerp;


    private void Start()
    {
        OriginalPos=Object.localPosition;
    }

    private void Update()
    {
        if (Lerp)
        {
            SlideVector = Vector3.MoveTowards(SlideVector, TargetPos, SlideSpeed * Time.deltaTime);
            if (SlideVector==TargetPos)
            {
                Lerp = false;
            }
        }
        else
        {
            SlideVector=Vector3.MoveTowards(SlideVector,OriginalPos, SlideSpeed * Time.deltaTime);
        }

        Object.localPosition = Vector3.Lerp(Object.localPosition,SlideVector,LerpSpeed * Time.deltaTime);
    }
    public void SetTarget()
    {
        TargetPos = OriginalPos+ new Vector3(Random.Range(MinTargetPos.x, MaxTargetPos.x), Random.Range(MinTargetPos.y, MaxTargetPos.y), Random.Range(MinTargetPos.z, MaxTargetPos.z));
        Lerp=true;
    }


}
