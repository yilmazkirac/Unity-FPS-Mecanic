using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private float ForceMagnitude;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody RB=hit.collider.attachedRigidbody;
        if (RB != null )
        {
            Vector3 ForceDirection=hit.transform.position=this.transform.position;
            ForceDirection.y=0f;
            ForceDirection.Normalize();

            RB.AddForceAtPosition(ForceDirection * ForceMagnitude, this.transform.position, ForceMode.Impulse);
        }
    }
}
