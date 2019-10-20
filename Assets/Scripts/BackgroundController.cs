using UnityEngine;

public class BackgroundController : MonoBehaviour{
    
    public float speed = 2f;
    private Rigidbody rb;
    private Vector3 movement;

    void Start(){
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update(){
        movement = new Vector3(1f, 0.0f, 0.0f);
        
        transform.Translate(movement * speed * Time.deltaTime);
    }
}
