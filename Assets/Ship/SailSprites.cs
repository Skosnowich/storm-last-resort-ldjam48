using UnityEngine;

namespace Ship
{
    public class SailSprites : MonoBehaviour
    {
        public SpriteRenderer FrontSail;
        public SpriteRenderer BackSail;
        public SpriteRenderer MidSail;
        public Transform RudderHolder;
        public float RudderModifier = 1;
        
        private ShipControl _shipControl;

        private void Start()
        {
            _shipControl = ShipControl.FindShipControlInParents(gameObject);
        }

        private void LateUpdate()
        {
            var openedSails = _shipControl.OpenedSails();
            MidSail.enabled = openedSails > 0;
            FrontSail.enabled = openedSails > 1;
            BackSail.enabled = openedSails > 2;
            RudderHolder.localEulerAngles = new Vector3(0, 0, _shipControl.RudderPosition() * RudderModifier);
        }
    }
}
