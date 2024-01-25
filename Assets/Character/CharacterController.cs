using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Character character;
    
    [Header("Characteristics")] 
    public float speed = 3;
    public float jumpHeight = 10;
    public float speedMultiplier;

    [Header("Physic Model")] 
    [SerializeField] private Rigidbody rb;
    private float distToGround;

    [SerializeField] private Transform cameraAnchorH;
    [SerializeField] private Transform cameraAnchorV;
    [SerializeField] private Transform cameraTransform;
    public Transform characterAnchor;
    
    [SerializeField] private float scrollSensetivity = 3;

    [Header("Move State")] 
    [SerializeField] private bool isMove;
    [SerializeField] private bool isRun;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isStandby;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        character = new Character();

        cameraTransform = Camera.main!.transform;
        cameraAnchorV = cameraTransform.transform.parent;
        cameraAnchorH = cameraAnchorV.transform.parent;
        
        rb = GetComponent<Rigidbody>();
        
        isGrounded = true;
        
        Debug.Log($"level: {character.health.statLevel.level}, exp: {character.health.statLevel.exp}, exp to next level: {character.health.statLevel.expToNextLevel}");
        Debug.Log($"HP: {character.health.statValue.currentValue}");
    }

    private void Update()
    {
        CameraRotate();
        Move();
    }

    private void CameraRotate()
    {
        var angularInputVertical = Input.GetAxisRaw("Mouse Y");
        var angularInputHorizontal = Input.GetAxisRaw("Mouse X");
        var scrollInput = Input.GetAxisRaw("Mouse ScrollWheel");

        //Camera movement
        cameraAnchorH.Rotate(0, angularInputHorizontal, 0);
        cameraAnchorV.Rotate(-angularInputVertical, 0, 0);

        if (cameraAnchorV.localEulerAngles.y != 0)
        {
            cameraAnchorV.Rotate(angularInputVertical, 0, 0);
        }

        cameraTransform.Translate(0, 0, scrollInput * scrollSensetivity, Space.Self);

        if (cameraTransform.localPosition.z is > -1 or < -8)
        {
            cameraTransform.transform.Translate(0, 0, -scrollInput * scrollSensetivity, Space.Self);
        }
    }

    private void Move()
    {
        var moveInputForward = Input.GetAxisRaw("Vertical");
        var moveInputSide = Input.GetAxisRaw("Horizontal");
        
        
        //MoveState Check
        if (Input.GetKey(KeyCode.LeftShift) && !isStandby)
        {
            speedMultiplier = 1.5f;
            isRun = true;
        }
        else
        {
            speedMultiplier = 1;
            isRun = false;
        }

        if (Input.GetMouseButton(1))
        {
            isStandby = true;
            speedMultiplier = 0.5f;
        }
        else
        {
            isStandby = false;
        }
        //Character Movement
        var velocity = Vector3.Normalize(cameraAnchorH.forward * moveInputForward + cameraAnchorH.right * moveInputSide);
        rb.velocity = speed * speedMultiplier * velocity + Vector3.up * rb.velocity.y;
        
        if (isStandby)
        {
            LerpRotateTo(cameraAnchorH.forward);
        }
        else
        {
            if (velocity.x != 0 && velocity.z != 0)
            {
                LerpRotateTo(velocity);
            }
        }
        
        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isGrounded = true;
    }
    private void OnTriggerExit(Collider other)
    {
        isGrounded = false;
    }

    private void LerpRotateTo(Vector3 direction)
    {
        var target = Quaternion.LookRotation(direction);
        characterAnchor.rotation = Quaternion.RotateTowards(characterAnchor.rotation, target, 360 * Time.deltaTime);
    }
}