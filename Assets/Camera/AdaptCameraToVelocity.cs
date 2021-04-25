using System;
using Ship;
using UnityEngine;

namespace Camera
{
    public class AdaptCameraToVelocity : MonoBehaviour
    {
        public ShipControl ShipControl;
        public float MaxLookAhead = 5F;
        public float MaxVelocity = 3F;
        public float MinSize = 12;
        public float MaxSize = 24;

        private float _currentLookAhead;
        private UnityEngine.Camera _camera;

        private void Start()
        {
            _camera = GetComponent<UnityEngine.Camera>();
        }

        private void Update()
        {
            if (GlobalGameState.IsUnpaused())
            {
                var zoomAxis = Input.GetAxisRaw("Zoom");
                zoomAxis = zoomAxis > 0 ? 1 : (zoomAxis < 0 ? -1 : 0);
                if (Math.Abs(zoomAxis) > 0.05)
                {
                    _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - zoomAxis, MinSize, MaxSize);
                }
            }
        }

        private void LateUpdate()
        {
            if (GlobalGameState.IsUnpaused() && ShipControl != null)
            {
                var currentVelocity = ShipControl.GetCurrentVelocity();
                var shipControlTransform = ShipControl.transform;
                var shipsPosition = shipControlTransform.position;

                _currentLookAhead = Mathf.Lerp(_currentLookAhead, currentVelocity / MaxVelocity * MaxLookAhead, Time.deltaTime);
                var position = transform.localPosition;
                transform.localPosition = new Vector3(shipsPosition.x, shipsPosition.y, position.z) + shipControlTransform.up * _currentLookAhead;
                transform.rotation = shipControlTransform.rotation;
            }
        }
    }
}
