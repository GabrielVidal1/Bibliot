using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Joystick joystick;
    public float speed;
    private CharacterController characterController;
    public GameObject mesh;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = Vector3.zero;

        Vector3 forward = Vector3.Cross(Vector3.up, Camera.main.transform.forward);;
        Vector3 left = Vector3.Cross(Vector3.up, forward);

        move = - left * joystick.Vertical + forward * joystick.Horizontal;

        if (move.sqrMagnitude > 0.001)
            mesh.transform.LookAt(mesh.transform.position + move);
        
        move.y = -10f;

        characterController.Move(move * Time.deltaTime * speed);
    }
}
