using System;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Character character;
    
    [Header("Characteristics")] 
    public float speed = 5;
    public float runMultiplier;

    [Header("Physic Model")] 
    [SerializeField] private Rigidbody rb;

    private new Camera camera;
    private Plane plane;
    private Vector3 viewDirection;

    [Header("moveState")] 
    [SerializeField] private bool isMove;
    [SerializeField] private bool isRun;
    
    private void Start()
    {
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
        LookAtMousePosition();

    }
    
    private void Move()
    {
        var verticalMove = Input.GetAxisRaw("Vertical") * new Vector3(1, 0, 1);
        var horizontalMove = Input.GetAxisRaw("Horizontal") * new Vector3(1, 0, -1);
        var velocity = verticalMove + horizontalMove;

        isMove = velocity != Vector3.zero; 
        
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
