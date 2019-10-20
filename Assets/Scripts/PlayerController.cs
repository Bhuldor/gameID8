using UnityEngine;

public class PlayerController : MonoBehaviour{
    
	[Header("Movimations Settings")]
    public float speed = 5;
	public float BottonLane = -3f;
    public float MoveLaneSpeed = 0.6f;
    public float UpLane = 3f;
    public GameManager gameManager;
    
    [Header("General")]
    public GameObject mainCamera;
    public GameObject destroyPanel;

    [Header("Spawn Settings")]
    public GameObject spawnFloor;
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
	
	void Update(){
        //move lateral 
        Move();
		//update camera position to follow player
        mainCamera.transform.position = new Vector3(transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
        //update destroyPanel position to follow player
        destroyPanel.transform.position = new Vector3(transform.position.x-300f, destroyPanel.transform.position.y, destroyPanel.transform.position.z);
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
                if (transform.position.z <= 1)
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
                }
            }
        }
        else // position is BottonLane
        {
            if (!isMidLane) // position is start position
            {

                movement = new Vector3(1.0f, 0.0f, MoveLaneSpeed);
                if (transform.position.z >= 1)
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
                }
            }
        }
    }
}

