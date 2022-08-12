using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Knight : MonoBehaviour
{
    // Constants & Enums
    public enum ROLE { ATTACK, DEFENSE };

    // Fields
    protected int health, maxHealth;
    protected float speed, angularSpeed, rotationAboutX, rotationAboutY;
    protected Vector3 lastPosition;
    protected AudioSource chargeSound;

    public bool isDead;
    public ROLE team;
    public List<Knight> allies, enemies;
    public Slider healthBar;

    // Methods
    // Start is called before the first frame update
    public void Start()
    {
        AudioSource[] sounds = GetComponents<AudioSource>();

        chargeSound = new List<AudioSource>(sounds).Find(sound => sound.clip.name == "Charge");

        health = maxHealth = 100;
        angularSpeed = 300f;
        rotationAboutX = rotationAboutY = 0f;
        lastPosition = Vector3.zero;

        isDead = false;
    }

    // Update is called once per frame
    public void Update()
    {
        if (transform.position == lastPosition)
            chargeSound.Stop();
        else
        {
            lastPosition = transform.position;
            if (!chargeSound.isPlaying)
                chargeSound.Play();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.value = health < 0 ? 0 : health;

        isDead = health <= 0;
    }
}