using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour{

    /*##privateSettings##*/
    //score
    public Text countText;

    //floors
    public GameObject floorsPrefab;
    public float maxTime = 0.1f;

    /*##privateSettings##*/
    //score
    private float score = 0f;
    private float nextActionTime = 0.0f;
    private float period = 0.25f;
    //Spawn
    private float x = -14f;
    
    private void Start(){
        //Starts Floors
        SpawnFloors(20);
    }

    void Update () {
        if (Time.time > nextActionTime ) {
            nextActionTime += period;
            SetCountText ();
        }
    }

    void SetCountText (){
        score++;
        countText.text = "Score: " + score.ToString ();
    }

    public void SpawnFloors(int count){
        for (int c = 0; c < count; c++){
            x += 2f;

            var instance = Instantiate(floorsPrefab);
            instance.transform.position = new Vector3(x, 0f, 0f);
        }
    }
}
