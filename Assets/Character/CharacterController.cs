using System;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Character character;
    
    [Header("Characteristics")] 
    public float speed = 3;
    public float runMultiplier;

    [Header("Physic Model")] 
    [SerializeField] private Rigidbody rb;

    public GameObject cameraAnchor;
    private new Camera camera;
    private Plane plane;
    private Vector3 viewDirection;
    private float scrollSensetivity = 3;

    [Header("Move State")] 
    [SerializeField] private bool isMove;
    [SerializeField] private bool isRun;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        character = new Character();
        
        camera = Camera.main;
        plane = new Plane(Vector3.up, Vector3.zero);
        
        rb = GetComponent<Rigidbody>();
        Debug.Log($"level: {character.health.statLevel.level}, exp: {character.health.statLevel.exp}, exp to next level: {character.health.statLevel.expToNextLevel}");
        Debug.Log($"HP: {character.health.statValue.currentValue}");
    }
    
    private void Update()
    {
        Move();
        //LookAtMousePosition();

    }
    
    private void Move()
    {
        var moveInputForward = Input.GetAxisRaw("Vertical") * rb.transform.forward;
        var moveInputSide = Input.GetAxisRaw("Horizontal") * rb.transform.right;
        var angularInputVertical = Input.GetAxisRaw("Mouse Y");
        var angularInputHorizontal = Input.GetAxisRaw("Mouse X");
        var scrollInput = Input.GetAxisRaw("Mouse ScrollWheel");
        
        //Camera movement
        transform.Rotate(0, angularInputHorizontal, 0);
        cameraAnchor.transform.Rotate(-angularInputVertical, 0, 0);

        var eulerAngles = cameraAnchor.transform.localEulerAngles;
        
        if (eulerAngles.y != 0 || eulerAngles.z != 0)
        {
            cameraAnchor.transform.Rotate(angularInputVertical, 0, 0);
        }
        
        camera.transform.Translate(0,0, scrollInput * scrollSensetivity, Space.Self);

        if (camera.transform.localPosition.z is > -1 or < -8)
        {
            camera.transform.Translate(0, 0, -scrollInput*scrollSensetivity, Space.Self);
        }
        
        //Character Movement
        if (Input.GetKey(KeyCode.LeftShift))
        {
            runMultiplier = 1.5f;
            isRun = true;
        }
        else
        {
            runMultiplier = 1;
            isRun = false;
        }

        var velocity = Vector3.Normalize(moveInputForward + moveInputSide);
        rb.velocity = speed * runMultiplier * velocity;
    }

    private void LookAtMousePosition()
    {
        var position = transform.position;
        if (Input.GetMouseButton(1) && !isRun)
        {
            var mousepointRay = camera.ScreenPointToRay(Input.mousePosition);
            plane.Raycast(mousepointRay, out var distance);
            var point = mousepointRay.GetPoint(distance);
            
            var rotation = Mathf.Atan2(point.x - position.x, point.z - position.z) * Mathf.Rad2Deg;
            LerpRotation(rotation);
        }
    }

    private void LerpRotation(float targetAngle)
    {
        var rot = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, 0.1f);
    }
}
