using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Characteristics")] 
    public float heathpoints;
    public float staminapoints;
    public float speed = 5;
    public float runMultiplier;

    [Header("Physic Model")] 
    [SerializeField] private Rigidbody rb;
    [SerializeField] private new CapsuleCollider collider;

    private new Camera camera;
    private Plane plane;
    private Vector3 viewDirection;
    public GameObject cub;

    private void Start()
    {
        camera = Camera.main;
        plane = new Plane(Vector3.up, Vector3.zero);
        
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
    }
    
    private void Update()
    {
        Move();
        LookAtMousePosition();
    }
    
    private void Move()
    {
        var verticalMove = Input.GetAxisRaw("Vertical") * new Vector3(1, 0, 1);
        var horizontalMove = Input.GetAxisRaw("Horizontal") * new Vector3(1, 0, -1);
        var velocity = verticalMove + horizontalMove;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            runMultiplier = 1.5f;
        }
        else
        {
            runMultiplier = 1;
        }
        
        rb.velocity = speed * runMultiplier * velocity.normalized;
        
        if (!Input.GetMouseButton(1))
        {
            if (rb.velocity != Vector3.zero)
            {
                float rotation = Mathf.Atan2(velocity.normalized.x, velocity.normalized.z) * Mathf.Rad2Deg;
                LerpRotation(rotation);
            }
        }
    }

    private void LookAtMousePosition()
    {
        if (Input.GetMouseButton(1))
        {
            var mousepointRay = camera.ScreenPointToRay(Input.mousePosition);
            plane.Raycast(mousepointRay, out var distance);
            var point = mousepointRay.GetPoint(distance);
            
            float rotation = Mathf.Atan2(point.x - transform.position.x, point.z - transform.position.z) * Mathf.Rad2Deg;
            LerpRotation(rotation);
        }
    }

    private void LerpRotation(float targetAngle)
    {
        var rot = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, 0.05f);
    }
}
