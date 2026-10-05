using System;
using Game.Input;
using UnityEngine;

namespace Player
{
    public class PlayerGrappling : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private float _strength;
        [SerializeField] private float _damper;
        [SerializeField] private float _maxGrappleDistance;
        
        [Header("References")]
        [SerializeField] private PlayerController _playerController;
        
        private LineRenderer _lineRenderer;
        
        private SpringJoint _grappleJoint;
        private Vector3 _grapplePoint;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            Ray ray = new Ray(_playerController.PlayerCinemachine.transform.position,
                _playerController.PlayerCinemachine.transform.forward);

            if (InputManager.Instance.Inputs.Player.Grap.WasPressedThisFrame() &&
                Physics.Raycast(ray, out RaycastHit hit, _maxGrappleDistance))
            {
                _grapplePoint = hit.point;

                _grappleJoint = gameObject.AddComponent<SpringJoint>();
                _grappleJoint.autoConfigureConnectedAnchor = false;
                _grappleJoint.connectedAnchor = _grapplePoint;
                _grappleJoint.spring = _strength;
                _grappleJoint.damper = _damper;
                _grappleJoint.maxDistance = 0f;
                _grappleJoint.minDistance = 0f;

                _lineRenderer.enabled = true;
            }
            else if (InputManager.Instance.Inputs.Player.Grap.WasReleasedThisFrame())
            {
                Destroy(_grappleJoint);
                _lineRenderer.enabled = false;
            }
            
            if (_grappleJoint != null)
            {
                _lineRenderer.SetPositions(new[]
                {
                    transform.position, _grapplePoint
                });
            }
        }
    }
}
