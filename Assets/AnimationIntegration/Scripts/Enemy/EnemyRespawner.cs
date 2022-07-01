using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyRespawner : MonoBehaviour
{
    [SerializeField] float EnemyRespawnTime;
    public UnityEvent<Enemy> OnEnemyDeath; //called from Enemy on death
    // Start is called before the first frame update
    void Start()
    {
        OnEnemyDeath = new UnityEvent<Enemy>();
        OnEnemyDeath.AddListener(RespawnEnemy);
    }

    void RespawnEnemy(Enemy en)
    {
        StartCoroutine(TimedRespawn(en));
    }

    IEnumerator TimedRespawn(Enemy en)
    {
        yield return new WaitForSeconds(EnemyRespawnTime);
        en.transform.position = new Vector3(Random.Range(-48, 48), 0, Random.Range(-48, 48));
        en.RespawnSequence();
    }
}
