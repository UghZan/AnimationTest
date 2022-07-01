using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    Rigidbody[] ragdollRigidbodies;
    Joint[] ragdollJoints;
    Collider[] ragdollColliders;

    public void Init()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        ragdollJoints = GetComponentsInChildren<Joint>();
        ragdollColliders = GetComponentsInChildren<Collider>();
    }

    //so that no ragdoll parts influence enemy movement
    //also reduces performance hit from ragdoll-related components, which is nice too
    public void Unragdoll()
    {
        foreach (Rigidbody r in ragdollRigidbodies)
        {
            r.isKinematic = true;
            r.detectCollisions = false;
        }
        foreach(Collider c in ragdollColliders)
        {
            c.enabled = false;
        }
        foreach(Joint j in ragdollJoints)
        {
            j.enableCollision = false;
        }
    }

    //enable everything ragdoll related
    public void Ragdoll()
    {
        foreach (Rigidbody r in ragdollRigidbodies)
        {
            r.isKinematic = false;
            r.detectCollisions = true;
            r.velocity = Vector3.zero;
        }
        foreach (Collider c in ragdollColliders)
        {
            c.enabled = true;
        }
        foreach (Joint j in ragdollJoints)
        {
            j.enableCollision = true;
        }
    }
}
