using UnityEngine;

namespace Camera
{
    public class AdaptCameraToVelocity : MonoBehaviour
    {
        public Rigidbody2D AttachedRigidbody;
        public float MaxLookAhead = 5F;
        public float MaxVelocity = 3F;

        private float _currentLookAhead;
        
        private void LateUpdate()
        {
            var currentVelocity = AttachedRigidbody.velocity.magnitude;

            _currentLookAhead = Mathf.Lerp(_currentLookAhead, currentVelocity / MaxVelocity * MaxLookAhead, Time.deltaTime);
            var position = transform.localPosition;
            transform.localPosition = new Vector3(position.x, _currentLookAhead, position.z);
        }
    }
}
