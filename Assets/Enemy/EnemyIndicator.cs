using UnityEngine;

namespace Enemy
{
    public class EnemyIndicator : MonoBehaviour
    {
        public LayerMask IntersectorLineLayerMask;
        public Vector3 NormalSize;

        private Transform _mainCameraTransform;
        private UnityEngine.Camera _camera;
        private Transform _parentTransform;
        private Transform _transform;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _mainCameraTransform = GameObject.FindWithTag("MainCamera").transform;
            _camera = _mainCameraTransform.GetComponent<UnityEngine.Camera>();
            _transform = transform;
            _parentTransform = _transform.parent.transform;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void LateUpdate()
        {
            var viewportPoint = _camera.WorldToViewportPoint(_parentTransform.position);
            if (viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1)
            {
                _spriteRenderer.enabled = false;
                return;
            }

            _spriteRenderer.enabled = true;

            var direction = (_parentTransform.position - _mainCameraTransform.position).normalized;

            var raycastHit2D = Physics2D.Raycast(_mainCameraTransform.position, direction, 100, IntersectorLineLayerMask);
            if (raycastHit2D)
            {
                _transform.position = raycastHit2D.point;
            }

            const int referenceCameraSize = 12;
            var currentSize = _camera.orthographicSize;

            _transform.localScale = NormalSize * (currentSize / referenceCameraSize);
        }
    }
}
