using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] location;
    
    private int currentIndex = 1;
    private bool isMoving = true;
    [SerializeField] 
    private bool stayInPlace;

    [SerializeField] 
    private float speed = 2;
    [SerializeField] 
    private float rotSpeed = 2;

    [SerializeField] 
    private Transform player;
    

    void Update()
    {
        if (isMoving && !stayInPlace)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, location[currentIndex].transform.position, speed);
            
            var target = location[currentIndex].transform.position;
            var moveDirection = target - transform.position;

            if (transform.position != new Vector3(0,0,0))
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), rotSpeed * Time.deltaTime);
            }
        }
        else
        {
            var target = player.position;
            var moveDirection = target - transform.position;
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), rotSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isMoving = false;
        }
        else if (other.CompareTag("Finish"))
        {
            //Next in array
            for (int i = 0; i < 1; i++)
            {
                currentIndex += 1;
            }

            if (currentIndex >= location.Length)
            {
                currentIndex = 0;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isMoving = true;
        }
    }
}
