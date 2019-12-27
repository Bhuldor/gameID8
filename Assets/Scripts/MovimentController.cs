using UnityEngine;

public class MovimentController : MonoBehaviour{
    
    public float speed = 2f;
    public GameManager gameManager;
    private Rigidbody rb;
    private Vector3 movement;

    void Start(){
        rb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update(){
        movement = new Vector3(1f, 0.0f, 0.0f);
        if(!gameManager.gameOver)
            transform.Translate(movement * speed * Time.deltaTime);
    }
}
