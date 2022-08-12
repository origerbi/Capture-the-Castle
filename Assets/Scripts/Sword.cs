using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    // Fields
    private float swishRate, nextTimeToSwish;
    private AudioSource drawSound, swishSound;

    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    // Methods
    public void Start()
    {
        AudioSource[] sounds = GetComponents<AudioSource>();
        drawSound = sounds[0];
        swishSound = sounds[1];

        swishRate = 50f;
        nextTimeToSwish = 0f;
    }

    public void Update()
    {
        bool RMB = Input.GetMouseButtonDown(1);
        if ((RMB) && (Time.time >= nextTimeToSwish))
        {
            nextTimeToSwish = Time.time + (1 / swishRate);
            if (!animator.GetBool("IsAttacking"))
                StartCoroutine(Attack());
        }
    }

    public void OnBecameVisible()
    {
        drawSound.Play();
    }

    IEnumerator Attack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, 2.5f, enemyLayers);

        animator.SetBool("IsAttacking", true);
        
        if (!swishSound.isPlaying)
            swishSound.Play();

        foreach (Collider enemy in hitEnemies)
        {
            //UnityEngine.Debug.Log("I am an enemy and I am dumb");
            enemy.GetComponent<NPC>().TakeDamage(10);
        }

        yield return new WaitForSeconds(0.2f);
        animator.SetBool("IsAttacking", false);
    }
}