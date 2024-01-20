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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
    }
    
    private void Update()
    {
        Move();
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
    }
}
