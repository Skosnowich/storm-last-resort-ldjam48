using UnityEngine;

namespace Ship
{
    public class CannonBall : MonoBehaviour
    {
        public float Distance;
        public Vector2 Speed;
        public ShipControl OriginShip;
        public float Damage;
        public float CrewDamage;

        private float _travelledDistance;
        private float _speed;

        private void Start()
        {
            var rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.velocity = Speed;
            _speed = Speed.magnitude;
        }

        private void FixedUpdate()
        {
            if (GlobalGameState.IsUnpaused())
            {
                _travelledDistance += _speed * Time.deltaTime;
                if (_travelledDistance >= Distance)
                {
                    Debug.Log("Platsch!");
                    Destroy(gameObject);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var otherShipControl = ShipControl.FindShipControlInParents(other.gameObject);
            if (otherShipControl != null && otherShipControl != OriginShip)
            {
                Debug.Log($"Hit ship {otherShipControl.gameObject.name}");
                Destroy(gameObject);
                otherShipControl.ChangeHullHealth(-Damage);
                otherShipControl.ChangeCrewHealth(-CrewDamage);
            }
        }
    }
}
