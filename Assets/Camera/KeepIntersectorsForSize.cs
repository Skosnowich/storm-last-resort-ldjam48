using UnityEngine;

namespace Camera
{
    public class KeepIntersectorsForSize : MonoBehaviour
    {
        public Transform IntersectorTransform;
        
        private UnityEngine.Camera _camera;

        private void Start()
        {
            _camera = GetComponent<UnityEngine.Camera>();
        }

        private void LateUpdate()
        {
            const float referenceSize = 12F;
            var currentSize = _camera.orthographicSize;

            IntersectorTransform.localScale = Vector3.one * (currentSize / referenceSize);
        }
    }
}
