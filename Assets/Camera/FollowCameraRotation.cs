using UnityEngine;

namespace Camera
{
    public class FollowCameraRotation : MonoBehaviour
    {
        private Transform _camera;
        private Transform _transform;

        private void Start()
        {
            _camera = GameObject.FindWithTag("MainCamera").transform;
            _transform = transform;
        }

        private void LateUpdate()
        {
            if (_camera != null)
            {
                _transform.rotation = _camera.rotation;
            }
        }
    }
}
