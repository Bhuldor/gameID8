using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip perigoAoLado;
    public AudioClip perigoAFrente;
    public GameObject player;
    
    private AudioSource audioSource;
    private bool starting = true;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.Find("Player");
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        while (starting)
        {
            yield return new WaitForSeconds(6f);
            goSound();
        }
    }

    private void goSound()
    {
        if ((player.transform.position.z > 0 && transform.position.z < 0) || (player.transform.position.z < 0 && transform.position.z > 0))
        {
            audioSource.clip = perigoAoLado;
        }
        else
        {
            audioSource.clip = perigoAFrente;
        }
        audioSource.Play();
        starting = false;
    }
}
