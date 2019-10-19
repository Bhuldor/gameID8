using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour{

    /*##privateSettings##*/
    public Text countText;

    /*##privateSettings##*/
    //score
    private float score = 0f;
    private float timer = 0f;
    private float nextActionTime = 0.0f;
    public float period = 0.25f;


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
}
