using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyRespawner ParentRespawner;
    [SerializeField] Animator EnemyAnimator;
    [SerializeField] RagdollController EnemyRagdollController;
    [SerializeField] Collider EnemyCollider; //enemy collision box, as it is not directly related to ragdoll, we will keep it here


    private void Start()
    {
        EnemyRagdollController.Init();
        EnemyRagdollController.Unragdoll();
    }

    public void DeathSequence()
    {
        EnemyAnimator.enabled = false;
        EnemyCollider.enabled = false;
        EnemyRagdollController.Ragdoll();
        ParentRespawner.OnEnemyDeath.Invoke(this);
    }

    public void RespawnSequence()
    {
        EnemyRagdollController.Unragdoll();
        EnemyAnimator.enabled = true;
        EnemyCollider.enabled = true;
    }
}
