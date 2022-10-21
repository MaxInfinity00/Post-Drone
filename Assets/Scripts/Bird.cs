using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] private float _alertRadius = 15f;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _minPos = 2f;
    [SerializeField] private float _maxPos = 5f;
    private float _minX, _minZ, _minY;
    private float _maxX, _maxZ, _maxY;
    private SphereCollider _sphereCollider;
    private Transform _target;
    private Vector3 _moveSpot;
    private float _turnSpeed = 5f;

    [SerializeField] private float width = 3.5f;
    [SerializeField] private float timeCounter = 1f;
    [SerializeField] private bool _patrolInCircle = true;

    void Awake()
    {
        SetPatrolArea();
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.radius = _alertRadius;

        if (!_patrolInCircle)
        {
            _moveSpot = GetNewPatrolPos();
            _speed = 2f;
        }
            
    }

    void Update()
    {
        RotateFaceForward();
        
        if (_patrolInCircle)
            PatrolInCircle();
        else
            MoveTowardsPosition();

        // TODO: If player is spotted, move towards player.
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other) {return;}

        // TODO change _movespot to player position and update it continously
        // TODO change movespeed to a higher degree
        // TODO IF you catch up to the player, drop its package
    }

    private void SetPatrolArea()
    {
        _minX = transform.position.x + _minPos;
        _minY = transform.position.y + _minPos;
        _minZ = transform.position.z + _minPos;
        _maxX = transform.position.x + _maxPos;
        _maxY = transform.position.y + _maxPos;
        _maxZ = transform.position.z + _maxPos;
    }

    private Vector3 GetNewPatrolPos()
    {
        float randomX = Random.Range(_minX, _maxX);
        float randomY = Random.Range(_minY, _maxY);
        float randomZ = Random.Range(_minZ, _maxZ);
        Vector3 newPosition = new Vector3(randomX, randomY, randomZ);
        return newPosition;
    }

    private void PatrolInCircle() // if _patrolInCircle = true
    {        
        timeCounter += Time.deltaTime * _speed;
        float x = Mathf.Cos (timeCounter) * width;
        float z = Mathf.Sin (timeCounter) * width;
        float y = 0;

        _moveSpot = transform.position = new Vector3 (x,y + 90f,z);
    }

    private void MoveTowardsPosition() // if _patrolInCircle = false
    {
        Vector3 distance = _moveSpot - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, _moveSpot, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _moveSpot) <= 0.2f)
        {
            _moveSpot = GetNewPatrolPos();
        }
    }

    void RotateFaceForward()
    {
        Vector3 targetDirection = _moveSpot - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, _turnSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
        // Quaternion toRotation = Quaternion.LookRotation(_moveSpot, Vector3.forward);
        // transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _turnSpeed * Time.deltaTime);
    }
}
