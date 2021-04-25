using Ship;
using UnityEngine;

namespace Enemy
{
    public class BehaviourFireBroadsides : MonoBehaviour
    {
        public float KeepInDistance = 20;
        public float KeepInDistanceRaycast = 28;
        public float KeepInSidesAngle = 30;
        public LayerMask ShipLayerMask;
        public FiringArcControl LeftFiringArcControl;
        public FiringArcControl RightFiringArcControl;
        
        private Transform _playerShip;
        private ShipControl _shipControl;
        private Transform _transform;

        public float _steeringAim = 0;
        public int _sailAim = 0;

        private void Start()
        {
            _playerShip = GameObject.FindWithTag("PlayerShip").transform;
            _shipControl = GetComponent<ShipControl>();
            _transform = transform;

            _sailAim = _shipControl.SailsOpenMax;
        }

        private void Update()
        {
            if (GlobalGameState.IsUnpaused() && _playerShip != null)
            {
                var directionToPlayerShip = _playerShip.position - _transform.position;
                var distanceToPlayerShip = directionToPlayerShip.magnitude;
                var angleToPlayerShip = Vector2.SignedAngle(_transform.right, directionToPlayerShip);

                var isInDistance = distanceToPlayerShip < KeepInDistance;
                var isRight = false;
                var isLeft = false;
                var isInCorrectAngle = false;
                if (angleToPlayerShip > 90 || angleToPlayerShip < -90)
                {
                    isLeft = true;
                    isInCorrectAngle = (angleToPlayerShip < -180 + KeepInSidesAngle || angleToPlayerShip > 180 - KeepInSidesAngle) && LeftFiringArcControl.IsReady();
                }
                else
                {
                    isRight = true;
                    isInCorrectAngle = angleToPlayerShip < KeepInSidesAngle && angleToPlayerShip > -KeepInSidesAngle  && RightFiringArcControl.IsReady();
                }

                var isSteeringNeeded = true;
                var raycastHits = Physics2D.BoxCastAll(_transform.position, new Vector2(KeepInDistanceRaycast * 2, 1), Vector2.Angle(Vector2.up, _transform.up), _transform.up,
                    50, ShipLayerMask);
                foreach (var raycastHit in raycastHits)
                {
                    Debug.Log(raycastHit.transform.gameObject.name);
                    var foundShipControl = ShipControl.FindShipControlInParents(raycastHit.transform.gameObject);
                    if (foundShipControl != null && foundShipControl.gameObject.CompareTag("PlayerShip"))
                    {
                        isSteeringNeeded = false;
                    }
                }

                if ((!isInCorrectAngle || !isInDistance) && (isRight && RightFiringArcControl.IsReady() || !LeftFiringArcControl.IsReady()))
                {
                    _steeringAim = isSteeringNeeded || !LeftFiringArcControl.IsReady() ? _shipControl.RudderPositionMax : 0;
                    _sailAim = _shipControl.SailsOpenMax;
                } else if ((!isInCorrectAngle || !isInDistance) && (isLeft && LeftFiringArcControl.IsReady() || !RightFiringArcControl.IsReady()))
                {
                    _steeringAim = isSteeringNeeded || !RightFiringArcControl.IsReady() ? _shipControl.RudderPositionMin : 0;
                    _sailAim = _shipControl.SailsOpenMax;
                }
                else
                {
                    _steeringAim = 0;
                    _sailAim = _shipControl.SailsOpenMin;
                }

                SteerAndSailToAim();
            }
        }

        private void SteerAndSailToAim()
        {
            if (_sailAim > _shipControl.OpenedSails())
            {
                _shipControl.OpenSail();
            } else if (_sailAim < _shipControl.OpenedSails())
            {
                _shipControl.CloseSail();
            }

            if (Mathf.Abs(_steeringAim - _shipControl.RudderPosition()) > 0.5F)
            {
                if (_steeringAim > _shipControl.RudderPosition())
                {
                    _shipControl.SteerRight();
                }
                else if (_steeringAim < _shipControl.RudderPosition())
                {
                    _shipControl.SteerLeft();
                }
            }
        }
    }
}
