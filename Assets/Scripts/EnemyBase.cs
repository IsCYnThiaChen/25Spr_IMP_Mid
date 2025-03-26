using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    protected Transform target;
    protected float hp = 0;
    [SerializeField]
    protected float hpTotal = 100;
    private float timer = 0f;
    [SerializeField]
    private float timerTotal = 1f;
    [SerializeField]
    protected GameManager gameManager;

    protected NavMeshAgent nav;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        hp = hpTotal;
        GameObject targetObject = GameObject.FindWithTag("Player");
        target = targetObject.transform;
        nav = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        TimerTool();
    }

    private void TimerTool()
    {
        if (timer > timerTotal)
        {
            timer = 0;
            TimerContent();
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
    protected virtual void TimerContent()
    {
        print("Time's up");
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            int damage=0;
            Damaged(damage);
        }
    }

    public virtual void Damaged(int damage)
    {
        gameManager.score = Mathf.Max(0, gameManager.score - damage);
        if (hp == 0)
        {
            Death();
        }
    }
    protected virtual void Death()
    {
        SpawnerManager.instance.RemoveEnemy(this);
        Destroy(gameObject);
    }
}
