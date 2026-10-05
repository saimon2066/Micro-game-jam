using System;
using Game.Input;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Velocity & Acceleration")]
        [SerializeField] private float _acceleration;
        [SerializeField] private float _maxAcceleration;
        [SerializeField] private float _maxVelocity;
        
        [Header("Jumping")]
        [SerializeField] private float _jumpForce;
        
        [Header("Other")]
        [SerializeField] private float _maxAngle;
        
        [Header("References")]
        [SerializeField] private PlayerController _playerController;
        
        private Rigidbody _rigidbody;
        private CapsuleCollider _collider;

        private bool _jump;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<CapsuleCollider>();
        }

        private void OnEnable()
        {
            InputManager.Instance.Inputs.Player.Jump.performed += OnJumpPerformed;
        }
        
        private void OnDisable()
        {
            InputManager.Instance.Inputs.Player.Jump.performed -= OnJumpPerformed;
        }

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void FixedUpdate()
        {
            bool isGrounded = IsGrounded();

            if (isGrounded && _jump)
            {
                Vector3 jumpForce = Vector3.up * _jumpForce;
                _rigidbody.AddForce(jumpForce, ForceMode.Impulse);
                
                _jump = false;
            }
            
            Vector2 moveInput = InputManager.Instance.Inputs.Player.Move.ReadValue<Vector2>();
            Vector3 moveVector = moveInput.x * transform.right + moveInput.y * transform.forward;

            Vector3 targetVelocity = moveVector * _maxVelocity;
            Vector3 horizontalVelocity = new Vector3(_rigidbody.linearVelocity.x, 0f, _rigidbody.linearVelocity.z);
            
            Vector3 velocityDifference = targetVelocity - horizontalVelocity;
            Vector3 accelerationForce = Vector3.ClampMagnitude(velocityDifference * _acceleration, _maxAcceleration);

            _rigidbody.AddForce(accelerationForce, ForceMode.Acceleration);

            Quaternion rotation = Quaternion.Euler(0f, _playerController.PlayerCinemachine.transform.localEulerAngles.y, 0f);
            _rigidbody.MoveRotation(rotation);
        }

        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            if (!IsGrounded())
            {
                return;
            }
            
            _jump = true;
        }

        private bool IsGrounded()
        {
            Vector3 origin = transform.position + _collider.center;
            Vector3 direction = Vector3.down;

            float radius = _collider.radius * 0.95f;
            float distance = _collider.height * 0.5f + 0.05f;
            
            Ray ray = new Ray(origin, direction);
            if (Physics.SphereCast(ray, radius, out RaycastHit hit, distance))
            {
                float angle = Vector3.Angle(hit.normal, Vector3.up);
                return angle <= _maxAngle;
            }

            return false;
        }
    }
}
