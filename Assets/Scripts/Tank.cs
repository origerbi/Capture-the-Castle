using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Tank : MonoBehaviour
{
    // Fields
    private Stopwatch aimStopwatch, fireStopwatch;
    private AudioSource aimSound, fireSound;

    // Methods
    // Start is called before the first frame update
    public void Start()
    {
        List<AudioSource> sounds = new List<AudioSource>(GetComponents<AudioSource>());

        aimStopwatch = new Stopwatch();
        fireStopwatch = new Stopwatch();

        aimSound = sounds.Find(sound => sound.clip.name == "TankAim");
        fireSound = sounds.Find(sound => sound.clip.name == "TankFire");

        aimStopwatch.Start();
    }

    // Update is called once per frame
    public void Update()
    {
        if ((aimStopwatch.Elapsed.TotalSeconds >= 10f) && (!aimSound.isPlaying))
        {
            aimStopwatch.Reset();
            fireStopwatch.Start();

            aimSound.Play();
        }

        if ((fireStopwatch.Elapsed.TotalSeconds >= 3f) && (!fireSound.isPlaying))
        {
            aimStopwatch.Start();
            fireStopwatch.Reset();

            fireSound.Play();
        }
    }
}
