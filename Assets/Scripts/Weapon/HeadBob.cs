using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [SerializeField] private Transform Head;
    [SerializeField] private Transform HeadParent;

    [Header("Variables")]

    [SerializeField] private float BobFreq;
    [SerializeField] private float HorizontalMagnitude;
    [SerializeField] private float VerticalMagnitude;
    [SerializeField] private float LerpSpeed;

    private float WalkingTime;
    private Vector3 TargetVector;

    private void Update()
    {
        SetHeadBob();
    }
    private void SetHeadBob()
    {
        if (!PlayerMovement.Instance.IsWalking&& !PlayerMovement.Instance.IsRunning)
        {
            WalkingTime = 0;
        }
        else
        {
            WalkingTime += Time.deltaTime;
        }

        TargetVector = HeadParent.position + SetOffSet(WalkingTime);
        Head.position = Vector3.Lerp(Head.position, TargetVector,LerpSpeed*Time.deltaTime);
        if((Head.position - TargetVector).magnitude<=0.001f)Head.position = TargetVector;
    }
    private Vector3 SetOffSet(float Time)
    {
        float HorizontalOffSet = 0f;
        float VerticalOffSet = 0f;
        Vector3 Offset= Vector3.zero;

        if (Time>0 )
        {
            HorizontalOffSet=Mathf.Cos(Time*BobFreq* PlayerMovement.Instance.TotalSpeed()) *HorizontalMagnitude;
            VerticalOffSet=Mathf.Sin(Time*BobFreq*2f* PlayerMovement.Instance.TotalSpeed()) *VerticalMagnitude;

            Offset=HeadParent.right*HorizontalOffSet+HeadParent.up*VerticalOffSet;
        }
        return Offset;
    }
}
