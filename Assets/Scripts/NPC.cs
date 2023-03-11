using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NPC : Knight
{
    public const int ENEMYDAMAGE = 20, PLAYERDAMAGE = 7;

    // Fields
    private Knight nearestEnemy;
    private Stopwatch damageStopwatch;
    private AudioSource fleeSound, attackSound, painSound, deathSound;

    public Animator animator;
    public NavMeshAgent agent;
    public Transform fleeingPoint;
    public LayerMask enemyLayers;

    // Methods
    // Start is called before the first frame update
    public new void Start()
    {
        List<AudioSource> sounds = new List<AudioSource>(GetComponentsInChildren<AudioSource>());
        string attackSoundName = name.Contains("Guard") ? "Punch" : "SwordSwish";

        base.Start();

        damageStopwatch = new Stopwatch();

        fleeSound = sounds.Find(sound => sound.clip.name == "Flee");
        attackSound = sounds.Find(sound => sound.clip.name == attackSoundName);
        painSound = sounds.Find(sound => sound.clip.name == "Pain");
        deathSound = sounds.Find(sound => sound.clip.name == "Death");

        speed = 3f;

        damageStopwatch.Start();
    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();

        // Validations
        if (isDead)
            return;

        nearestEnemy = GetNearestEnemy();
        if (nearestEnemy == null)
            return;

        if (agent.isOnNavMesh)
            if (animator.GetInteger("State") == 0) // charging
                agent.SetDestination(nearestEnemy.transform.position);

        float enemyDistance = Vector3.Distance(transform.position, nearestEnemy.transform.position);
        if (enemyDistance <= 2.5f)
        {
            if ((animator.GetInteger("State") < 1) || (animator.GetInteger("State") > 2))
            {
                if (!isDead)
                    StartCoroutine(Attack());
            }
            enemyDistance = Vector3.Distance(transform.position, nearestEnemy.transform.position);
            if (enemyDistance <= 3f)
            {
                if (nearestEnemy == GameObject.Find("Player").GetComponent<Knight>())
                    GameObject.Find("Player").GetComponent<Player>().PlayerDamage(PLAYERDAMAGE);
                else
                    nearestEnemy.TakeDamage(ENEMYDAMAGE);
            }

            //{
            //    //if (nearestEnemy.gameObject == GameObject.Find("Player"))
            //    //{
            //    //    GameObject.Find("Player").GetComponent<Player>().TakeDamage(10);
            //    //}
            //    //else nearestEnemy.GetComponent<NPC>().TakeDamage(10);
            //}
            //TO DO: do damage to nearest enemy
        }
        //else if (Time.time >= nextTimeToHit)
        //{
        //    nextTimeToHit = Time.time + (1 / hitRate);
        //}
    }


    public Knight GetNearestEnemy()
    {
        Knight nearestEnemy = null;
        float minDistance = float.MaxValue, currentDistance;

        if ((enemies == null) || (enemies.Count == 0) || (isDead))
            return null;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
                continue;

            currentDistance = Vector3.Distance(transform.position, enemies[i].transform.position);
            if (currentDistance < minDistance)
            {
                if (!enemies[i].isDead)
                {
                    minDistance = currentDistance;
                    nearestEnemy = enemies[i];
                }
            }
        }

        return nearestEnemy;
    }

    public void Charge()
    {
        if (!isDead)
        {
            Knight nearestEnemy = GetNearestEnemy();

            animator.SetInteger("State", 0);

            agent.enabled = true;
            chargeSound.Play();

            if (nearestEnemy != null)
                agent.SetDestination(nearestEnemy.transform.position);
        }
        else
            StartCoroutine(Die());
    }

    public void Halt()
    {
        if (!isDead)
        {
            animator.SetInteger("State", 100);

            agent.enabled = false;
            chargeSound.Stop();
            fleeSound.Stop();
        }
        else
            StartCoroutine(Die());
    }
    public void Flee()
    {
        if (!isDead)
        {
            animator.SetInteger("State", 4);

            agent.enabled = true;
            fleeSound.Play();

            agent.SetDestination(fleeingPoint.position);
        }
        else
            StartCoroutine(Die());
    }
    public IEnumerator Attack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 2.5f, enemyLayers);
        if (!isDead)
        {
            animator.SetInteger("State", 1);

            agent.enabled = false;
            if (!attackSound.isPlaying)
                attackSound.Play();

            foreach (Collider enemy in hitEnemies)
            {
                enemy.GetComponent<Knight>().TakeDamage(10);
            }

            yield return new WaitForSeconds(2f);
            Charge();
        }
        else
            StartCoroutine(Die());
    }

    public override void TakeDamage(int damage)
    {
        if (damageStopwatch.Elapsed.TotalSeconds >= 1)
        {
            base.TakeDamage(damage);
            damageStopwatch.Restart();
        }

        if (!isDead)
        {
            animator.SetInteger("State",  3 );
            if (!painSound.isPlaying)
                painSound.Play();
        }
        else
            StartCoroutine(Die());
    }

    public IEnumerator Die()
    {
        animator.SetInteger("State", -1);

        damageStopwatch.Stop();

        agent.enabled = false;
        deathSound.Play();

        if (team != Spawner.playerTeam)
            GameManager.enemiesCount--;

        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }
}