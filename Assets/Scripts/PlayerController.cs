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
    private bool canPress = true;
    public bool moveDown = false;
    public bool moveUp = false;
    public bool dodge = false;
    public bool stop = false;
	
	void Start(){
        rb = GetComponent<Rigidbody>();
	}
	
	void Update(){
        KeyPress();
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
                canPress = true;
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
                canPress = true;
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
                if (transform.position.z <= 0)
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
                    canPress = true;
                }
            }
        }
        else // position is BottonLane
        {
            if (!isMidLane) // position is start position
            {

                movement = new Vector3(1.0f, 0.0f, MoveLaneSpeed);
                if (transform.position.z >= 0)
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
                    canPress = true;
                }
            }
        }
    }

    void KeyPress()
    {
        if (canPress && Input.GetKeyDown(KeyCode.A))
        {
            canPress = false;
            spawnFloor.GetComponent<BlindFloorSpawnManager>().SpawnFloor(this.gameObject, 2, isUplane);  //DODGE
            
        }
        else if (canPress && Input.GetKeyDown(KeyCode.D))
        {
            canPress = false;
            if (isUplane)
            {
                spawnFloor.GetComponent<BlindFloorSpawnManager>().SpawnFloor(this.gameObject, 1,isUplane);  //MOVE DOWN
            }
            else
            {
                spawnFloor.GetComponent<BlindFloorSpawnManager>().SpawnFloor(this.gameObject, 0, isUplane); //MOVE UP
            }
        }
        else if (canPress && Input.GetKeyDown(KeyCode.S) && !stop)
        {
            canPress = false;
            spawnFloor.GetComponent<BlindFloorSpawnManager>().SpawnFloor(this.gameObject, 3, isUplane); //STOP
        }
        else if (stop && Input.GetKeyDown(KeyCode.S))
        {
            stop = false;
            canPress = true;
        }
        // else if() //REST
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "FloorMoveUp")
        {
            moveUp = true;
        }
        else if(other.tag == "FloorMoveDown")
        {
            moveDown = true;
        }
        else if(other.tag == "FloorDodge")
        {
            dodge = true;
        }
        else if(other.tag == "FloorStop")
        {
            stop = true;
        }
        else if(other.tag == "FloorRest")
        {

        }
    }
}

