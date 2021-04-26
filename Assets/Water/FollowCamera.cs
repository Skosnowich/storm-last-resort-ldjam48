using UnityEngine;

namespace Water
{
    public class FollowCamera : MonoBehaviour
    {
        private Transform _cameraTransform;

        private void Start()
        {
            _cameraTransform = GameObject.FindWithTag("MainCamera").transform;
        }

        private void LateUpdate()
        {
            var cameraTransformPosition = _cameraTransform.position;
            transform.position = new Vector3(cameraTransformPosition.x, cameraTransformPosition.y, 0);
            transform.rotation = _cameraTransform.rotation;
        }
    }
}
