using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip moveSound;
    static AudioSource audioSrc;
    // Start is called before the first frame update
    public SoundManagerScript()
    {
     //   moveSound = Resources.Load<AudioClip>("moving");

     //   audioSrc = GetComponent<AudioSource>();
    }
   
    void Start ()
    {
        moveSound = Resources.Load<AudioClip>("moving");

        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound (string clip)
    {
        if (clip == "move")
            audioSrc.PlayOneShot(moveSound);

        /* switch (clip)
         {
             case "move":
                 audioSrc.PlayOneShot(moveSound);
                 break;
         }*/
    }

}
