using System;
using UnityEditor;
using UnityEngine;

namespace Ship
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ShipControl : MonoBehaviour
    {
        public int SailsOpenMin = 0;
        public int SailsOpenMax = 3;
        public float SpeedUpTime = 1;
        public float BreakTime = 1;
        public float RudderPositionMin = -30;
        public float RudderPositionMax = 30;
        public float SteeringSpeed = 30;
        public float RudderDeadZone = 2.5F;

        private float _rudderPosition;
        private int _sailsOpen;
        private Rigidbody2D _rigidbody;

        public float _currentVelocity;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (GlobalGameState.IsUnpaused())
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    OpenSail();
                }

                if (Input.GetKeyDown(KeyCode.S))
                {
                    CloseSail();
                }

                if (Input.GetKey(KeyCode.A))
                {
                    SteerLeft();
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    SteerRight();
                }

                if (_currentVelocity < _sailsOpen)
                {
                    _currentVelocity = Mathf.Min(_currentVelocity + Time.deltaTime * SpeedUpTime * _sailsOpen, _sailsOpen);
                }
                else
                {
                    _currentVelocity = Mathf.Max(_currentVelocity - Time.deltaTime * BreakTime, _sailsOpen);
                }

                _rigidbody.velocity = transform.up * _currentVelocity;

                var currentVelocity = _rigidbody.velocity.magnitude;
                if (currentVelocity > 0.1f)
                {
                    var rudderPositionWithAppliedDeadZone = Mathf.Abs(_rudderPosition) < RudderDeadZone ? 0 : _rudderPosition;
                    _rigidbody.angularVelocity = -rudderPositionWithAppliedDeadZone / SailsOpenMax * currentVelocity;
                }
                else
                {
                    _rigidbody.angularVelocity = 0;
                }
            }
        }

        private void SteerRight()
        {
            if (ChangeRudderPosition(1))
            {
                Debug.Log($"Steered right, now at {_rudderPosition}.");
            }
        }

        private void SteerLeft()
        {
            if (ChangeRudderPosition(-1))
            {
                Debug.Log($"Steered left, now at {_rudderPosition}.");
            }
        }

        private void OpenSail()
        {
            if (ChangeOpenSailCount(1))
            {
                Debug.Log($"Opened a sail, now having {_sailsOpen} opened sails.");
            }
        }

        private void CloseSail()
        {
            if (ChangeOpenSailCount(-1))
            {
                Debug.Log($"Closed a sail, now having {_sailsOpen} opened sails.");
            }
        }

        private bool ChangeOpenSailCount(int sailCountChange)
        {
            var newSailCount = _sailsOpen + sailCountChange;
            if (newSailCount < SailsOpenMin || newSailCount > SailsOpenMax)
            {
                return false;
            }

            _sailsOpen += sailCountChange;
            return true;
        }

        private bool ChangeRudderPosition(int direction)
        {
            var newRudderPosition = _rudderPosition + direction * SteeringSpeed * Time.deltaTime;
            if (newRudderPosition < RudderPositionMin || newRudderPosition > RudderPositionMax)
            {
                return false;
            }

            _rudderPosition = Mathf.Clamp(newRudderPosition, RudderPositionMin, RudderPositionMax);
            return true;
        }
    }
}
