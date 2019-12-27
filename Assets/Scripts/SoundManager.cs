using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip perigoAoLado;
    public AudioClip perigoAFrente;
    public GameObject player;
    public GameManager gameManager;
    
    private AudioSource audioSource;
    private bool starting = true;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(Wait());
    }
    private void Update()
    {
        if (gameManager.gameIsPaused)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.UnPause();
        }
    }

    private IEnumerator Wait()
    {
        while (starting)
        {
            yield return new WaitForSeconds(4f);
            if (!gameManager.gameOver)
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
