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
    private float x = -22f;
    private float xBackground = 500f;
    private int i;
    private int countWaitFaixa = 0;
    private bool faixaPut = false;
    
    private void Start(){
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

                if (countWaitFaixa > 200){
                    faixaPut = false;
                }
            }

            var instance = Instantiate(instanceObject);
            instance.transform.position = new Vector3(x, 0f, 0f);
        }
    }
}
