using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    // Fields
    private float blockRate, nextTimeToBlock;
    private AudioSource blockSound;

    public Animator animator;
    public Transform blockPoint;
    public LayerMask enemyLayers;

    // Methods
    public void Start()
    {
        blockSound = GetComponent<AudioSource>();

        blockRate = 20f;
        nextTimeToBlock = 0f;
    }

    public void Update()
    {
        if ((Input.GetMouseButton(0)) && (Time.time >= nextTimeToBlock))
        {
            nextTimeToBlock = Time.time + (1 / blockRate);
            if (!animator.GetBool("IsBlocking"))
                StartCoroutine(Block());
        }
    }

    IEnumerator Block()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(blockPoint.position, 1.5f, enemyLayers);
        
        animator.SetBool("IsBlocking", true);
        blockSound.Play();

        foreach (Collider enemy in hitEnemies)
            enemy.GetComponent<NPC>().TakeDamage(5);

        yield return new WaitForSeconds(0.2f);
        animator.SetBool("IsBlocking", false);
    }
}