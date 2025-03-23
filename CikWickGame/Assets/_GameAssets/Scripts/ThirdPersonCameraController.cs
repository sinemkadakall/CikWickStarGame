using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{

    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _orientationTransform;
    [SerializeField] private Transform _playerVisualTransform;
    [SerializeField] private float _rotationSpeed;


    private void Update()
    {
        Vector3 viewDirection = _playerTransform.position - new Vector3(transform.position.x,_playerTransform.position.y,transform.position.z);

        _orientationTransform.forward= viewDirection.normalized;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 inputDirecton = _orientationTransform.forward*verticalInput + _orientationTransform.right * horizontalInput ;

        if (inputDirecton != Vector3.zero)
        {
            _playerVisualTransform.forward = Vector3.Slerp(_playerVisualTransform.forward, inputDirecton.normalized, Time.deltaTime * _rotationSpeed);

        }


    }



}
