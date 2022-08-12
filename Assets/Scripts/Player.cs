using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class Player : Knight
{
    // Fields
    public CharacterController controller;
    public Selector selector;
    public Stopwatch damageCooldown;

    public GameObject playerCamera, heart;

    // Methods
    // Start is called before the first frame update
    public new void Start()
    {
        base.Start();

        speed = 7.5f;

        controller = GetComponent<CharacterController>();
        damageCooldown = new Stopwatch();
        damageCooldown.Start();
    }

    // Update is called once per frame
    public new void Update()
    {
        float dx = 0, dy = -1, dz = 0;
        Vector3 motion;

        base.Update();

        // Sets the rotation angles
        rotationAboutY += Input.GetAxis("Mouse X") * angularSpeed * Time.deltaTime;
        transform.localEulerAngles = new Vector3(0, rotationAboutY, 0); // sets the rotation angles of THIS
        rotationAboutX -= Input.GetAxis("Mouse Y") * angularSpeed * Time.deltaTime;
        if (rotationAboutX >= 80)
            rotationAboutX = 80;
        else if (rotationAboutX <= -60)
            rotationAboutX = -60;
        else
            playerCamera.transform.localEulerAngles = new Vector3(rotationAboutX, 0, 0);

        dx = Input.GetAxis("Horizontal");
        dx *= speed * Time.deltaTime;
        dz = Input.GetAxis("Vertical");
        dz *= speed * Time.deltaTime;

        // Adds motion using Character Controller
        motion = transform.TransformDirection(new Vector3(dx, dy, dz));
        controller.Move(motion);
    }

    public void PlayerDamage(int damage)
    {
        if (damageCooldown.Elapsed.TotalSeconds >= 1)
        {
            base.TakeDamage(damage);
            damageCooldown.Restart();
        }
        //if (health > 0f)
        //{
        //    if (health >= (maxHealth * 0.75f))
        //        heart.GetComponent<Animator>().SetInteger("Heartbeat", 1);
        //    else
        //        heart.GetComponent<Animator>().SetInteger("Heartbeat", health >= (maxHealth * 0.4f) ? 2 : 3);
        //}
    }
}