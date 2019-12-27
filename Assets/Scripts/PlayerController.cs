using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour{
    
	[Header("Movimations Settings")]
    public float speed = 5;
	public float BottonLane = -3f;
    public float MoveLaneSpeed = 0.6f;
    public float UpLane = 3f;
    public GameManager gameManager;
    
    [Header("Sounds")]
    public AudioClip gameOverSound;
    public AudioClip score0;
    public AudioClip score1;
    public AudioClip score2;
    public AudioClip score3;
    public AudioClip score4;
    public AudioClip score5;
    public AudioClip score6;
    public AudioClip score7;
    public AudioClip score8;
    public AudioClip score9;
    public AudioClip pontuacao;
    public AudioClip recomecar;

    [Header("General")]
    public GameObject mainCamera;
    public GameObject destroyPanel;
    public GameObject gameOverPanel;
    public GameObject blindFullEffect;
    public GameObject blindHalfEffect;
    public GameObject blindStartEffect;

    [Header("Spawn Settings")]
    public GameObject spawnFloor;
    /*##privateSettings##*/
    //directionControllers
    private Rigidbody rb;
    private Vector3 movement;
    private bool isUplane = true;
    private bool isMidLane = false;
    private bool canPress = true;
    public bool moveDown = false;
    public bool moveUp = false;
    public bool dodge = false;
    public bool stop = false;
	
    private float nextActionTime = 0.0f;
    private float period = 15f;
    private bool slowed = false;
    private float originalSpeed;

    private BlindFloorSpawnManager bfSpawnManager;
    //private GameManager gameManager;
    
    private AudioSource source {get {return GetComponent<AudioSource>();}}

	void Start(){
        rb = GetComponent<Rigidbody>();
        originalSpeed = speed;
        bfSpawnManager = spawnFloor.GetComponent<BlindFloorSpawnManager>();
    }
	
	void Update(){
        if (!gameManager.gameOver)
        {
        KeyPress();
        //move lateral 
        Move();
		//update camera position to follow player
        mainCamera.transform.position = new Vector3(transform.position.x+7.5f, mainCamera.transform.position.y, mainCamera.transform.position.z);
        //update destroyPanel position to follow player
        destroyPanel.transform.position = new Vector3(transform.position.x-300f, destroyPanel.transform.position.y, destroyPanel.transform.position.z);

        //RestoreRegularSpeed
        if (slowed){
            if (Time.time > nextActionTime ) {
                nextActionTime += period;
            }
            slowed = false;
        }
            if (gameManager.gameIsPaused)
            {
                source.Pause();
            }
            else
            {
                source.UnPause();
            }
        }
    }

    public void ExitGame(){
		Application.Quit();
	}

    IEnumerator WaitTime(float time){
        yield return new WaitForSeconds(time);
        stop = false;
        yield return new WaitForSeconds(0.5f);
        canPress = true;
    }
    private void Move()
    {
       if(moveDown)
        {
            // Move to BottonLane position at MoveLaneSpeed
            movement = new Vector3(1.0f, 0.0f, (MoveLaneSpeed * -1));
            if(transform.position.z <= BottonLane )
            {
                moveDown = false;
                isUplane = false;
                canPress = true;
            }
       }
       else if(moveUp)
        {
            // Move to UpLane position at MoveLaneSpeed
            movement = new Vector3(1.0f, 0.0f, MoveLaneSpeed);
            if (transform.position.z >= UpLane)
            {
                moveUp = false;
                isUplane = true;
                canPress = true;
            }
        }
       else if(dodge)
        {
            Dodge();
        }
       else if(stop)
        {
            // Slowing speed before stoping
            if (rb.velocity.x > 0)
            {
                movement = new Vector3(((speed / 2) * -1), 0.0f, 0.0f);
            }
            else
            {
                movement = new Vector3(0.0f, 0.0f, 0.0f);
            }
            
        }
        else
        {
            movement = new Vector3(1f, 0.0f, 0.0f);
        }
        transform.Translate(movement * speed * Time.deltaTime);
    }

    void Dodge()
    {
        // Move to MidLane and back to start position

        if (isUplane) // position is UpLane
        {
            if (!isMidLane) // position is start position
            {
                movement = new Vector3(1.0f, 0.0f, (MoveLaneSpeed * -1));
                if (transform.position.z <= 0)
                {
                    isMidLane = true;
                }
            }
            else // position is MidLane
            {
                movement = new Vector3(1.0f, 0.0f, MoveLaneSpeed);
                if (transform.position.z >= UpLane)
                {
                    dodge = false;
                    isMidLane = false;
                    canPress = true;
                }
            }
        }
        else // position is BottonLane
        {
            if (!isMidLane) // position is start position
            {

                movement = new Vector3(1.0f, 0.0f, MoveLaneSpeed);
                if (transform.position.z >= 0)
                {
                    isMidLane = true;
                }
            }
            else // position is MidLane
            {
                movement = new Vector3(1.0f, 0.0f, (MoveLaneSpeed * -1));
                if (transform.position.z <= BottonLane)
                {
                    dodge = false;
                    isMidLane = false;
                    canPress = true;
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision){
        if (collision.collider.tag == "Obstacle"){
            GameOver();
        }
        if (collision.collider.tag == "Glasses"){
            Destroy(collision.gameObject);
            StartCoroutine(GettingDark(1.5f));
            gameManager.GetComponent<AudioSource>().volume = 0.12f;
        }
    }

    private void GameOver(){
        blindStartEffect.SetActive(false);
        blindHalfEffect.SetActive(false);
        blindFullEffect.SetActive(false);

        //Game Over
        gameManager.GetComponent<AudioSource>().Stop();
        transform.GetComponent<AudioSource>().Stop();
        //StartCoroutine(PlayGameOverSounds());
        StartCoroutine(ScoreToSound());
        gameOverPanel.SetActive(true);
        //Time.timeScale = 0f;
        gameManager.gameOver = true;
    }

    IEnumerator PlayGameOverSounds(){
        source.volume = 1;
        source.priority = 256;
        source.PlayOneShot(gameOverSound);

        float pauseEndTime = Time.realtimeSinceStartup + 2.5f;
        while (Time.realtimeSinceStartup < pauseEndTime){
            yield return 0;
        }
        //source.PlayOneShot(menuButtons);
    }

    IEnumerator GettingDark(float delayForGettingDark){
        yield return new WaitForSeconds(delayForGettingDark);
        blindStartEffect.SetActive(true);
        yield return new WaitForSeconds(delayForGettingDark);
        blindHalfEffect.SetActive(true);
        yield return new WaitForSeconds(delayForGettingDark);
        blindFullEffect.SetActive(true);
    }

    private IEnumerator ScoreToSound()
    {
        source.volume = 1;
        source.priority = 256;
        int[] score_un = GetIntArray(gameManager.score);
        source.clip = pontuacao;
        source.Play();
        yield return new WaitForSeconds(1f);
        foreach (int a in score_un) { 
            yield return new WaitForSeconds(1.5f);
            playScore(a);   
        }
        yield return new WaitForSeconds(1.5f);
        source.clip = recomecar;
        source.Play();
        yield return new WaitForSeconds(6f);
        source.Stop();
    }

    private int[] GetIntArray(int num)
    {
        List<int> list = new List<int>();
        while(num > 0)
        {
            list.Add(num % 10);
            num = num / 10;
        }
        list.Reverse();
        return list.ToArray();
    }
    private void playScore(int score)
    {
        AudioClip playclip = score0;
        switch (score)
        {
            case 1:
                playclip = score1;
                break;
            case 2:
                playclip = score2;
                break;
            case 3:
                playclip = score3;
                break;
            case 4:
                playclip = score4;
                break;
            case 5:
                playclip = score5;
                break;
            case 6:
                playclip = score6;
                break;
            case 7:
                playclip = score7;
                break;
            case 8:
                playclip = score8;
                break;
            case 9:
                playclip = score9;
                break;
        }
        source.clip = playclip;
        source.loop = false;
        source.Play();
    }

    void KeyPress()
    {
        if (canPress && Input.GetKeyDown(KeyCode.A))
        {
            if(gameManager.tutorialOn && gameManager.tutorialA)
            {
                return;
            }
            canPress = false;
            bfSpawnManager.SpawnFloor(this.gameObject, 2, isUplane);  //DODGE
            
        }
        else if (canPress && Input.GetKeyDown(KeyCode.D))
        {
            if (gameManager.tutorialOn && !gameManager.tutorialS)
            {
                return;
            }
            canPress = false;
            if (isUplane)
            {
                bfSpawnManager.SpawnFloor(this.gameObject, 1,isUplane);  //MOVE DOWN
            }
            else
            {
                bfSpawnManager.SpawnFloor(this.gameObject, 0, isUplane); //MOVE UP
            }
        }
        else if (canPress && Input.GetKeyDown(KeyCode.S) && !stop)
        {
            if(gameManager.tutorialOn && (!gameManager.tutorialA || gameManager.tutorialS))
            {
                return;
            }
            canPress = false;
            bfSpawnManager.SpawnFloor(this.gameObject, 3, isUplane); //STOP
        }
        
        // else if() //REST
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "FloorMoveUp")
        {
            moveUp = true;
        }
        else if(other.tag == "FloorMoveDown")
        {
            moveDown = true;
        }
        else if(other.tag == "FloorDodge")
        {
            dodge = true;
        }
        else if(other.tag == "FloorStop")
        {
            stop = true;
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(WaitTime(1.5f));
        }
        else if(other.tag == "FloorRest")
        {

        }
    }
}

