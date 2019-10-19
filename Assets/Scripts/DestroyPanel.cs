using UnityEngine;

public class DestroyPanel : MonoBehaviour{
    void OnCollisionEnter(Collision collision){
        Destroy(collision.collider.gameObject);
    }
}
