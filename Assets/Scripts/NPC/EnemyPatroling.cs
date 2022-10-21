using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroling : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]private Transform[] patrollingLocation;
    private int pl;

    void Start()
    {
        pl = 0;
        //patrollingLocation = GetComponentsInChildren<Transform>();

        
    }

    public Vector3 whatsMyPatrolLocation(Vector3 enemyCurrentPostion)
    {
        foreach (Transform t in patrollingLocation)
        {
            if (t.position == enemyCurrentPostion)
            {
                pl++;

            }


        }
        pl %= patrollingLocation.Length;
        return patrollingLocation[pl].position;
    }

    
}
