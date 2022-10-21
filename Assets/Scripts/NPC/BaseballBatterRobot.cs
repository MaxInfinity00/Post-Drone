using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BaseballBatterRobot : MonoBehaviour
{

    private GameObject player;
    private bool playerHaveEnterdArea;
    private Vector3 returnPoint;
    private float timerReturn;
    [SerializeField] private float timerReturnMax = 2f;

    private bool imAtOriginalPosition;

    private Animator animator;


    [SerializeField] private float speedChasing = 10f;
    [SerializeField] private float speedPatroling = 5f;
    [SerializeField] private LayerMask mask;
    [SerializeField] float throwForce = 500;

    [SerializeField] private Material glowOrange;
    [SerializeField] private Material glowRed;
    [SerializeField] SkinnedMeshRenderer[] skinnedMeshRenderes;

    private EnemyPatroling fathersEnemyPatrolling;
    private bool iHaveAFather;
    private bool iHaveRecentlyHit;
    private float timerBetweenAttacks;
    [SerializeField] private float timerBetweenAttacksMax = 5f;
    [SerializeField]
    private float _humLoopTime = 6.8f;

    private float timeStartAttack = 0.5f;

    private List<Sound> _activeSounds = new List<Sound>();
    private AudioManager _audioManager;
    [Header("Sounds")]
    [SerializeField]
    private string _badHum;
    [SerializeField]
    private string _badHit;

    [SerializeField]private LayerMask mask2;
    private RaycastHit hit;
    private RaycastHit hit2;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        returnPoint = transform.position;
        timerReturn = timerReturnMax;
        skinnedMeshRenderes = new SkinnedMeshRenderer[] { transform.GetChild(0).transform.GetChild(1).GetComponent<SkinnedMeshRenderer>(), transform.GetChild(0).transform.GetChild(2).GetComponent<SkinnedMeshRenderer>() };

        _audioManager = FindObjectOfType<AudioManager>();
        if (!string.IsNullOrEmpty(_badHum))
        {
            StartCoroutine(HumLoop());
        }

        fathersEnemyPatrolling = transform.parent.GetComponent<EnemyPatroling>();
        if (fathersEnemyPatrolling != null)
        {
            iHaveAFather = true;
        }
        else
        {
            iHaveAFather = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHaveEnterdArea)
        {
            transform.LookAt(player.transform);
            foreach (SkinnedMeshRenderer smr in skinnedMeshRenderes)
            {
                smr.materials = new Material[] { smr.materials[0], glowRed, smr.materials[2] };
            }

            if (iHaveRecentlyHit)
            {
                timerBetweenAttacks -= Time.deltaTime;
                if (timerBetweenAttacks < 0)
                {
                    iHaveRecentlyHit = false;
                }
            }
            else if(!Physics.Raycast(transform.position, transform.forward, out hit2, 25f, mask2))
            {
                timerReturn = timerReturnMax;
                timerBetweenAttacks = timerBetweenAttacksMax;
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speedChasing * Time.deltaTime);

                RaycastHit hit;

                if (Physics.Raycast(transform.position, transform.forward, out hit, 5f, mask))
                {
                    Debug.Log("ImNear");
                    //player.transform.Translate((player.transform.position - transform.position).normalized * Time.deltaTime * throwForce);
                    DroneMovement droneMovement = player.GetComponent<DroneMovement>();
                    if (droneMovement == null)
                    {
                        Debug.Log($"Drone Movement Not Found");
                        return;
                    }
                    animator.SetTrigger("Attack");
                    droneMovement.YeetThePlayer(transform, throwForce);
                    iHaveRecentlyHit = true;
                    if (!string.IsNullOrEmpty(_badHit))
                    {
                        _audioManager.PlaySound(_badHit);
                    }
                }
                Debug.DrawRay(transform.position, transform.forward, Color.yellow);
            }
        }
        else
        {
            timerReturn -= Time.deltaTime;

            foreach (SkinnedMeshRenderer smr in skinnedMeshRenderes)
            {
                smr.materials = new Material[] { smr.materials[0], glowOrange, smr.materials[2] };
            }

            if (timerReturn < 0 && !imAtOriginalPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, returnPoint, speedPatroling * Time.deltaTime);
                transform.LookAt(returnPoint);
            }
            if (transform.position == returnPoint)
            {
                if (iHaveAFather && fathersEnemyPatrolling != null)
                {
                    returnPoint = fathersEnemyPatrolling.whatsMyPatrolLocation(transform.position);
                }
                else
                {
                    imAtOriginalPosition = true;
                    //looks att same position as before kinda
                    transform.LookAt(returnPoint + Vector3.forward);
                }
            }
            else
            {
                imAtOriginalPosition = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        DroneMovement dm = other.GetComponent<DroneMovement>();
        if (dm != null)
        {
            playerHaveEnterdArea = true;

            player = other.gameObject;
            timerBetweenAttacks = timeStartAttack;
            iHaveRecentlyHit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        DroneMovement dm = other.GetComponent<DroneMovement>();

        if (dm != null)
        {
            playerHaveEnterdArea = false;
            player = null;
            iHaveRecentlyHit = false;
        }
    }


    IEnumerator HumLoop()
    {
        _audioManager.PlaySoundAtSpot(_badHum, transform.position, 1f);
        yield return new WaitForSeconds(_humLoopTime);
        StartCoroutine(HumLoop());
    }
}
