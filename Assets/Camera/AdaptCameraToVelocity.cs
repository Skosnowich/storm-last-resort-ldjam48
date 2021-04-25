using Ship;
using UnityEngine;

namespace Camera
{
    public class AdaptCameraToVelocity : MonoBehaviour
    {
        public ShipControl ShipControl;
        public float MaxLookAhead = 5F;
        public float MaxVelocity = 3F;

        private float _currentLookAhead;

        private void LateUpdate()
        {
            if (GlobalGameState.IsUnpaused())
            {
                var currentVelocity = ShipControl.GetCurrentVelocity();

                _currentLookAhead = Mathf.Lerp(_currentLookAhead, currentVelocity / MaxVelocity * MaxLookAhead, Time.deltaTime);
                var position = transform.localPosition;
                transform.localPosition = new Vector3(position.x, _currentLookAhead, position.z);
            }
        }
    }
}
