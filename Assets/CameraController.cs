using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject character;

    public float sensetivity = 1;
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, character.transform.position, 0.02f);
    }
}
