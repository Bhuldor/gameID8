using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour{

    /*##privateSettings##*/
    //score
    public Text countText;

    //floors
    public GameObject floorsPrefab;
    public float maxTime = 0.1f;

    //background
    public GameObject backgroundPrefab;

    /*##privateSettings##*/
    //score
    private float score = 0f;
    private float nextActionTimeBackground = 0.0f;
    private float nextActionTime = 0.0f;
    private float period = 0.25f;
    private float periodBackground = 10f;
    //Spawn
    private float x = -14f;
    private float xBackground = 500f;
    
    private void Start(){
        //Starts Floors
        SpawnFloors(20);
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

    public void SpawnFloors(int count){
        for (int c = 0; c < count; c++){
            x += 2f;

            var instance = Instantiate(floorsPrefab);
            instance.transform.position = new Vector3(x, 0f, 0f);
        }
    }
}
