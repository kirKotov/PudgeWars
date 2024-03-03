using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class TPSController : MonoBehaviour
{
    [SerializeField] private Transform _playerCameraParent;

    [SerializeField] private float _speed = 7.5f;
    [SerializeField] private float _gravity = 20.0f;

    [SerializeField] private float _lookSpeed = 2.0f;
    [SerializeField] private float _lookXLimit = 60.0f;

    private CharacterController _characterController;

    private Vector3 _moveDirection = Vector3.zero;
    private Vector2 _rotation = Vector2.zero;

    private bool _canMove = true;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _rotation.y = transform.eulerAngles.y;
    }

    private void Update()
    {
        if (_characterController.isGrounded)
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            float curSpeedX = _canMove ? _speed * Input.GetAxis("Vertical") : 0;
            float curSpeedY = _canMove ? _speed * Input.GetAxis("Horizontal") : 0;

            _moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        }

        _moveDirection.y -= _gravity * Time.deltaTime;

        _characterController.Move(_moveDirection * Time.deltaTime);

        if (_canMove)
        {
            _rotation.y += Input.GetAxis("Mouse X") * _lookSpeed;
            _rotation.x += -Input.GetAxis("Mouse Y") * _lookSpeed;

            _rotation.x = Mathf.Clamp(_rotation.x, -_lookXLimit, _lookXLimit);

            _playerCameraParent.localRotation = Quaternion.Euler(_rotation.x, 0, 0);

            transform.eulerAngles = new Vector2(0, _rotation.y);
        }
    }
}