using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/Controller")]
public class Controller : MonoBehaviour
{
    public float normalSpeed = 10;
    private float _sprintSpeed; 
    public float rotationSpeed = 90;
    public float gravity = -9.80f;
    public float jumpSpeed = 10;
    public Transform camera;
    private bool isJumping = false;

    public MouseLook _MouseLook;
    private float defaultSensHor;
    private float defaultSensVert;
    public Animator horseAnimator;
 
    CharacterController _characterController;
    private Vector3 _moveVelocity;
    private Vector3 _turnVelocity;
    private Camera _camera;
 
    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _sprintSpeed = normalSpeed + 4;
        _camera = GetComponent<Camera>();
        _MouseLook = GetComponent<MouseLook>();
        defaultSensHor = _MouseLook.sensitivityHor;
        defaultSensVert = _MouseLook.sensitivityVert;

    }
 
    void Update()
    {
        var hInput = Input.GetAxis("Horizontal");
        var vInput = Input.GetAxis("Vertical");
        
        //set currenSpeed to sprintSpeed if leftShift is pressed
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? _sprintSpeed : normalSpeed;

        if(_characterController.isGrounded)
        {
            _moveVelocity = transform.forward * (currentSpeed * vInput);
            _turnVelocity = transform.up * (rotationSpeed * hInput);
            if (isJumping)
            {
                transform.Rotate(45,0,0);
                camera.transform.Rotate(-45,0,0);
                _MouseLook.sensitivityVert = defaultSensVert;
                _MouseLook.sensitivityHor = defaultSensHor;
                isJumping = false;
                Debug.Log("jump");
            }
            
            if(Input.GetKeyDown(KeyCode.Space))
            {
                
                isJumping = true;
                _moveVelocity.y = jumpSpeed;
                transform.Rotate(-45, 0, 0);
                camera.transform.Rotate(45,0,0);
                _MouseLook.sensitivityVert = 0;
                _MouseLook.sensitivityHor = 0;
            }
        }
        
        //Adding gravity
        _moveVelocity.y += gravity * Time.deltaTime;
        _characterController.Move(_moveVelocity * Time.deltaTime);
        transform.Rotate(_turnVelocity * Time.deltaTime);
    }
    
}