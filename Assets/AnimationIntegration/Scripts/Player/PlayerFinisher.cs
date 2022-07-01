using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class PlayerFinisher : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float FinisherDistance;
    [SerializeField] float FinisherStoppingDistance;
    [SerializeField] float FinisherEnemyCheckFrequency;
    [SerializeField] float FinisherTimeUntilEnemyRagdolls;
    [SerializeField] float FinisherUITextHeight;

    [Header("Weaponry")]
    [SerializeField] Transform gun;
    [SerializeField] Transform sword;

    [Header("References")]
    [SerializeField] RectTransform UIFinisherText;
    [SerializeField] PlayerMovement PlayerMovement;
    [SerializeField] Animator PlayerAnimator;

    Enemy _currentTarget;
    Collider[] tempTargets;
    Camera _cam;

    private void Start()
    {
        tempTargets = new Collider[5];
        StartCoroutine(EnemyCheck());
        _cam = Camera.main;
    }

    private void Update()
    {
        if (_currentTarget != null) //if we have enemy in range
        {
            UIFinisherText.gameObject.SetActive(true);
            //move text above enemy head
            UIFinisherText.position = _cam.WorldToScreenPoint(_currentTarget.transform.position + Vector3.up * _currentTarget.GetComponent<Collider>().bounds.max.y * FinisherUITextHeight);
            if(Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(FinisherSequence());
            }
        }
        else
        {
            UIFinisherText.gameObject.SetActive(false);
        }
    }

    IEnumerator FinisherSequence()
    {
        //ideally I'd replace this with NavMeshAgent and making it move the player, but whatever
        PlayerMovement.SwitchMovementMode(false);
        Vector3 directionTo = _currentTarget.transform.position - transform.position;
        PlayerMovement.MovementTarget = _currentTarget.transform.position - directionTo.normalized * FinisherStoppingDistance; //get to a position a few units away from the enemy 

        yield return new WaitUntil(() => PlayerMovement.HasReachedTarget); //we wait until we are in position
        PlayerMovement.ShouldMove = false; //stop rigidbody from moving player
        gun.gameObject.SetActive(false); //switch to sword
        sword.gameObject.SetActive(true);

        PlayerAnimator.SetTrigger("finishing"); //start finishing animation
        yield return new WaitUntil(() => PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Finishing")); //make sure we enter finisher animation

        yield return new WaitForSeconds(FinisherTimeUntilEnemyRagdolls); //as animation is read-only, we can't really add animation events to synchronize blade hitting and enemy ragdolling
                                                                         //so we add a little KOCTbI/\b
        _currentTarget.DeathSequence(); //ragdoll the enemy

        yield return new WaitUntil(() => !PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Finishing")); //finisher animation has finished

        gun.gameObject.SetActive(true); //switch back to gun
        sword.gameObject.SetActive(false);

        PlayerMovement.ShouldMove = true;
        PlayerMovement.SwitchMovementMode(true); //bring control back to player
    }

    //could be in Update, but using WaitForSeconds make it more performant without losing too much precision
    IEnumerator EnemyCheck()
    {
        while (true)
        {
            int targets = Physics.OverlapSphereNonAlloc(transform.position, FinisherDistance, tempTargets);
            if (targets > 0)
            {
                float distance = 9999;
                Collider closest = null;
                //get closest enemy
                for (int i = 0; i < targets; i++)
                {
                    if (tempTargets[i].tag == "Enemy")
                    {
                        float _distanceToEnemy = Vector3.Distance(transform.position, tempTargets[i].transform.position);
                        if (_distanceToEnemy < distance)
                        {
                            distance = _distanceToEnemy;
                            closest = tempTargets[i];
                        }
                    }
                    _currentTarget = closest == null ? null : closest.GetComponent<Enemy>();
                }
            }
            yield return new WaitForSeconds(FinisherEnemyCheckFrequency);
        }
    }
}
