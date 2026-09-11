using UnityEngine;

public class BulletsScript : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
<<<<<<< Updated upstream
    }
}
=======

        //despawnTimer+= Time.deltaTime;
        //if(despawnTimer >= 5f)
        //{
        //    Destroy(this.gameObject);
        //}       
    }             
                  
}                 
                  
>>>>>>> Stashed changes
