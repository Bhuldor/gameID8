using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{

    /*##privateSettings##*/
    //score
    public Text countText;
    public Text countTextFinal;
    //floors
    public GameObject floorsPrefab;
    public GameObject floorsFaixaPrefab;
    public float maxTime = 0.1f;

    //Obstacles Prefabs
    public GameObject bus;
    public GameObject car;
    public GameObject cavalete;
    public GameObject trash;
    public GameObject glasses;

    //player
    public GameObject player;

    //background
    public GameObject backgroundPrefab;

    //audioSource
    public AudioSource audioSource;
    public AudioClip musicFast;
    public AudioClip musicMiddle;
    public AudioClip musicSlow;
    public AudioClip perigoFrente;
    public AudioClip perigoLado;

    /*##privateSettings##*/
    //score
    public float score = 0f;
    private float nextActionTimeBackground = 0.0f;
    private float nextActionTimeObstacle = 0.0f;
    private float nextActionTime = 0.0f;
    private float period = 0.25f;
    private float periodBackground = 10f;
    private float periodObstacle = 12f;
    //Spawn
    private float x = -22f;
    private float xBackground = 500f;
    private float xObstacle = 0f;
    private float yObstacle = 1.2f;
    private float zObstacle = 0f;
    private int i;
    private int countWaitFaixa = 0;
    private bool faixaPut = false;
    //general
    private int wichMusic = 0;
    
    private void Start(){
        Time.timeScale = 1f;
        //Starts Floors
        SpawnFloors(35);
    }

    public void RestartGame(){
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    void Update () {
        if (Time.time > nextActionTime ) {
            nextActionTime += period;
            SetCountText ();
            SpawnFloors(1);
        }

        if (Time.time > nextActionTimeBackground ) {
            nextActionTimeBackground += periodBackground;
            SpawnBackground();
        }

        if (Time.time > nextActionTimeObstacle ) {
            nextActionTimeObstacle += periodObstacle;
            SpawnRandomObstacle();
        }
    }

    void SetCountText (){
        score++;
        countText.text = "Score: " + score.ToString ();
        countTextFinal.text = "Final Score: " + score.ToString ();

        //Difficult increase
        //Music Change
        if(score > 150){
            if (wichMusic == 0){
                wichMusic = 1;
                audioSource.clip = musicMiddle;
                audioSource.Play();
                audioSource.loop = true;
                periodObstacle = 6f;
            }
        }
        if(score > 300){
            if (wichMusic == 1){
                wichMusic = 2;
                audioSource.clip = musicFast;
                audioSource.Play();
                audioSource.loop = true;
                periodObstacle = 3f;
            }
        }
    }

    public void SpawnBackground(){
        var instanceBackground = Instantiate(backgroundPrefab);
        instanceBackground.transform.position = new Vector3(xBackground, backgroundPrefab.transform.position.y, backgroundPrefab.transform.position.z);

        xBackground += 700f;
    }

    private void SpawnRandomObstacle(){
        int spawnRandom = Random.Range(10,13);
        int spawnRandomZ = Random.Range(0,2);
        AudioClip soundEffect = perigoFrente;
        GameObject instanceObstacle = bus;
        xObstacle = player.transform.position.x + 50f;
        zObstacle = 0f;
        yObstacle = 1.2f;

        if (player.transform.transform.position.z > 0){
            if (spawnRandomZ == 1){
                soundEffect = perigoFrente;
            }else{
                soundEffect = perigoLado;
            }
        }else{
            if (spawnRandomZ == 0){
                soundEffect = perigoFrente;
            }else{
                soundEffect = perigoLado;
            }
        }

        switch (spawnRandom){
            case 0:
            case 1:
            case 2:
                instanceObstacle = bus;
                yObstacle = -1.6f;
                zObstacle = 0f;
                break;
            case 3:
            case 4:
            case 5:
                instanceObstacle = car;
                zObstacle = 0f;
                break;
            case 6:
            case 7:
            case 8:
            case 9:
                instanceObstacle = cavalete;
                xObstacle = player.transform.position.x + 35f;
                instanceObstacle.GetComponent<AudioSource>().clip = soundEffect;
                yObstacle = 1f;
                if (spawnRandomZ == 0){zObstacle = -3f;}
                else{zObstacle = 3f;}
                break;
            case 10:
            case 11:
            case 12:
            case 13:
                instanceObstacle = trash;
                xObstacle = player.transform.position.x + 35f;
                instanceObstacle.GetComponent<AudioSource>().clip = soundEffect;
                yObstacle = 1f;
                if (spawnRandomZ == 0){zObstacle = -3f;}
                else{zObstacle = 3f;}
                break;
            case 14:
            case 15:
            case 16:
                instanceObstacle = glasses;
                yObstacle = -2.6f;
                if (spawnRandomZ == 0){zObstacle = -3.5f;}
                else{zObstacle = 2f;}
                break;
        }

        var instance = Instantiate(instanceObstacle);
        instance.transform.position = new Vector3(xObstacle, yObstacle, zObstacle);
    }

    public void SpawnFloors(int count){
        i = Random.Range(0,10);
        GameObject instanceObject = floorsPrefab;

        for (int c = 0; c < count; c++){
            x += 2f;

            if (i < 2 && !faixaPut){
                instanceObject = floorsFaixaPrefab;
                faixaPut = true;
            }else{
                instanceObject = floorsPrefab;
            }

            if (faixaPut){
                countWaitFaixa++;

                if (countWaitFaixa > 125){
                    faixaPut = false;
                }
            }

            var instance = Instantiate(instanceObject);
            instance.transform.position = new Vector3(x, 0f, 0f);
        }
    }
}
