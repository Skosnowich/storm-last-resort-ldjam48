using System;
using UI;
using UnityEngine;

namespace Ship
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ShipControl : MonoBehaviour
    {
        public bool Invincible;
        public Team Team = Team.Enemy;
        public int SailsOpenMin = 0;
        public int SailsOpenMax = 3;
        public float SpeedUpTime = 1;
        public float BreakTime = 1;
        public float RudderPositionMin = -30;
        public float RudderPositionMax = 30;
        public float SteeringSpeed = 30;
        public float RudderDeadZone = 2.5F;
        public float MaxHullHealth = 100;
        public float MaxCrewHealth = 50;
        public float SpeedModifier = 1F;

        private float _rudderPosition;
        private int _sailsOpen;
        private Rigidbody2D _rigidbody;

        private float _currentVelocity;
        private Bars _bars;

        private float _currentHullHealth;
        private float _currentCrewHealth;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _bars = GetComponent<Bars>();

            _currentHullHealth = MaxHullHealth;
            _currentCrewHealth = MaxCrewHealth;
        }

        private void Update()
        {
            if (GlobalGameState.IsUnpaused())
            {
                if (_currentVelocity < _sailsOpen)
                {
                    _currentVelocity = Mathf.Min(_currentVelocity + Time.deltaTime * SpeedUpTime * _sailsOpen, _sailsOpen);
                }
                else
                {
                    _currentVelocity = Mathf.Max(_currentVelocity - Time.deltaTime * BreakTime, _sailsOpen);
                }

                _rigidbody.velocity = transform.up * _currentVelocity * SpeedModifier;

                if (_currentVelocity > 0.1f)
                {
                    var rudderPositionWithAppliedDeadZone = RudderPosition();
                    _rigidbody.angularVelocity = -rudderPositionWithAppliedDeadZone / SailsOpenMax * _currentVelocity * SpeedModifier;
                }
                else
                {
                    _rigidbody.angularVelocity = 0;
                }
            }
        }

        public void ChangeHullHealth(float healthChange)
        {
            _currentHullHealth = Mathf.Clamp(_currentHullHealth + healthChange, 0, MaxHullHealth);

            var died = false;
            if (Math.Abs(_currentHullHealth) < 0.01f)
            {
                died = true;
                _currentHullHealth = 0;
            }

            _bars.HullBar.Value = _currentHullHealth / MaxHullHealth;

            if (died)
            {
                Die();
            }
        }

        private void Die()
        {
            if (!Invincible)
            {
                Destroy(gameObject);
            }
        }

        public void SteerRight()
        {
            if (ChangeRudderPosition(1))
            {
//                Debug.Log($"Steered right, now at {_rudderPosition}.");
            }
        }

        public void SteerLeft()
        {
            if (ChangeRudderPosition(-1))
            {
//                Debug.Log($"Steered left, now at {_rudderPosition}.");
            }
        }

        public void OpenSail()
        {
            if (ChangeOpenSailCount(1))
            {
//                Debug.Log($"Opened a sail, now having {_sailsOpen} opened sails.");
            }
        }

        public void CloseSail()
        {
            if (ChangeOpenSailCount(-1))
            {
//                Debug.Log($"Closed a sail, now having {_sailsOpen} opened sails.");
            }
        }

        public float GetCurrentVelocity()
        {
            return _currentVelocity * SpeedModifier;
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

        public static ShipControl FindShipControlInParents(GameObject otherGameObject)
        {
            var shipControl = otherGameObject.GetComponent<ShipControl>();
            if (shipControl != null)
            {
                return shipControl;
            }

            if (otherGameObject.transform.parent != null)
            {
                return FindShipControlInParents(otherGameObject.transform.parent.gameObject);
            }

            return null;
        }

        public int OpenedSails()
        {
            return _sailsOpen;
        }

        public float RudderPosition()
        {
            return Mathf.Abs(_rudderPosition) < RudderDeadZone ? 0 : _rudderPosition;
        }

        public void UpdateFromGlobalGameState()
        {
            MaxCrewHealth = GlobalGameState.MaxCrewHealth;
            MaxHullHealth = GlobalGameState.MaxHullHealth;
            _currentCrewHealth = GlobalGameState.CurrentCrewHealth;
            _currentHullHealth = GlobalGameState.CurrentHullHealth;
            var firingArcControls = GetComponentsInChildren<FiringArcControl>();
            foreach (var firingArcControl in firingArcControls)
            {
                firingArcControl.CannonBallCount = GlobalGameState.CannonCount;
                firingArcControl.ReadyUpTime = GlobalGameState.ReadyUpTime;
            }
        }

        public void UpdateToGlobalGameState()
        {
            GlobalGameState.MaxCrewHealth = Mathf.RoundToInt(MaxCrewHealth);
            GlobalGameState.MaxHullHealth = Mathf.RoundToInt(MaxHullHealth);
            GlobalGameState.CurrentCrewHealth = Mathf.RoundToInt(_currentCrewHealth);
            GlobalGameState.CurrentHullHealth = Mathf.RoundToInt(_currentHullHealth);
        }
    }
}
