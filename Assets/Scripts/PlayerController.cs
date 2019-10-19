using UnityEngine;

public class PlayerController : MonoBehaviour{
    
	[Header("Movimations Settings")]
    public float speed = 2.5f;
	public float moveHorizontal = 1f;
    public float BottonLane = -3f;
    public float MoveLaneSpeed = 0.6f;
    public float UpLane = 3f;
    public GameObject destroyPanel;
    
    [Header("General")]
    public GameObject mainCamera;
    public GameManager gameManager;

    /*##privateSettings##*/
    //directionControllers
    private Rigidbody rb;
    private Vector3 movement;
    private bool isUplane = true;
    private bool isMidLane = false;
    public bool moveDown = false;
    public bool moveUp = false;
    public bool dodge = false;
    public bool stop = false;
	
	void Start(){
        rb = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate(){
        //move lateral 
        Move();
		//update camera position to follow player
        mainCamera.transform.position = new Vector3(transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
        //update destroyPanel position to follow player
        destroyPanel.transform.position = new Vector3(transform.position.x-100f, destroyPanel.transform.position.y, destroyPanel.transform.position.z);
	}


    private void Move(){
        gameManager.SpawnFloors(1);

       if(moveDown)
        {
            // Move to BottonLane position at MoveLaneSpeed
            movement = new Vector3(0.0f, 0.0f, (MoveLaneSpeed * -1));
            rb.AddForce(movement * speed);
            if(transform.position.z <= BottonLane )
            {
                rb.velocity = new Vector3(rb.velocity.x, 0.0f, 0.0f);
                rb.angularVelocity = Vector3.zero;
                moveDown = false;
                isUplane = false;
            }
       }
       else if(moveUp)
        {
            // Move to UpLane position at MoveLaneSpeed
            movement = new Vector3(0.0f, 0.0f, MoveLaneSpeed);
            rb.AddForce(movement * speed);
            if (transform.position.z >= UpLane)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0.0f, 0.0f);
                rb.angularVelocity = Vector3.zero;
                moveUp = false;
                isUplane = true;
            }
        }
       else if(dodge)
        {
            Dodge();
        }
       if(stop)
        {
            // Slowing speed before stoping
            if (rb.velocity.x > 0)
            {
                movement = new Vector3(((speed*1.5f) * -1), 0.0f, 0.0f);
            }
            else
            {
                rb.velocity = Vector3.zero;
                movement = new Vector3(0.0f, 0.0f, 0.0f);
            }
            
        }
        else
        {
            movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
        }
       rb.AddForce(movement * speed);
    }

    void Dodge()
    {
        // Move to MidLane and back to start position

        if (isUplane) // position is UpLane
        {
            if (!isMidLane) // position is start position
            {
                movement = new Vector3(0.0f, 0.0f, (MoveLaneSpeed * -1));
                rb.AddForce(movement * speed);
                if (transform.position.z <= 1)
                {
                    rb.velocity = new Vector3(rb.velocity.x, 0.0f, 0.0f);
                    isMidLane = true;
                }
            }
            else // position is MidLane
            {
                movement = new Vector3(0.0f, 0.0f, MoveLaneSpeed);
                rb.AddForce(movement * speed);
                if (transform.position.z >= UpLane)
                {
                    rb.velocity = new Vector3(rb.velocity.x, 0.0f, 0.0f);
                    dodge = false;
                    isMidLane = false;
                }
            }
        }
        else // position is BottonLane
        {
            if (!isMidLane) // position is start position
            {

                movement = new Vector3(0.0f, 0.0f, MoveLaneSpeed);
                rb.AddForce(movement * speed);
                if (transform.position.z >= 1)
                {
                    rb.velocity = new Vector3(rb.velocity.x, 0.0f, 0.0f);
                    isMidLane = true;
                }
            }
            else // position is MidLane
            {
                movement = new Vector3(0.0f, 0.0f, (MoveLaneSpeed * -1));
                rb.AddForce(movement * speed);
                if (transform.position.z <= BottonLane)
                {
                    rb.velocity = new Vector3(rb.velocity.x, 0.0f, 0.0f);
                    dodge = false;
                    isMidLane = false;
                }
            }
        }
    }
}

