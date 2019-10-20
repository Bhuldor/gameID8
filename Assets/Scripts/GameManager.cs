using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour{

    /*##privateSettings##*/
    //score
    public Text countText;

    //floors
    public GameObject floorsPrefab;
    public GameObject floorsFaixaPrefab;
    public float maxTime = 0.1f;

    //Obstacles Prefabs
    public GameObject bus;
    public GameObject car;
    public GameObject cavalete;
    public GameObject poste;

    //player
    public GameObject player;

    //background
    public GameObject backgroundPrefab;

    //audioSource
    public AudioSource audioSource;
    public AudioClip musicFast;
    public AudioClip musicMiddle;
    public AudioClip musicSlow;

    /*##privateSettings##*/
    //score
    private float score = 0f;
    private float nextActionTimeBackground = 0.0f;

    private float nextActionTimeObstacle = 0.0f;
    private float nextActionTime = 0.0f;
    private float period = 0.25f;
    private float periodBackground = 10f;
    private float periodObstacle = 10f;
    //Spawn
    private float x = -22f;
    private float xBackground = 500f;
    private float xObstacle = 0f;
    private float yObstacle = 1.2f;
    private float zObstacle = 0f;
    private int i;
    private int countWaitFaixa = 0;
    private bool faixaPut = false;
    
    private void Start(){
        audioSource.clip = musicSlow;
        //Starts Floors
        SpawnFloors(25);
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
    }

    public void SpawnBackground(){
        var instanceBackground = Instantiate(backgroundPrefab);
        instanceBackground.transform.position = new Vector3(xBackground, backgroundPrefab.transform.position.y, backgroundPrefab.transform.position.z);

        xBackground += 700f;
    }

    private void SpawnRandomObstacle(){
        int spawnRandom = Random.Range(0,2);
        Debug.Log("Random= " + spawnRandom);
        GameObject instanceObstacle = bus;
        xObstacle = player.transform.position.x + 30f;


        switch (spawnRandom){
            case 0:
                instanceObstacle = bus;
                yObstacle = -1.6f;
                break;
            case 1:
                instanceObstacle = car;
                yObstacle = 1.2f;
                break;
            /*case 2:
                instanceObstacle = cavalete;
                break;
            case 3:
                instanceObstacle = poste;
                break;*/
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
