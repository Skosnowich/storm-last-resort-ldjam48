using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Ship
{
    [RequireComponent(typeof(SpriteShapeRenderer), typeof(PolygonCollider2D))]
    public class FiringArcControl : MonoBehaviour
    {
        public SpriteShapeRenderer ReadySpriteShapeRenderer;
        public Transform ReadySpriteMaskTransform;
        public float ReadyUpTime = 10f;
        public float MaxDistance = 18.25F;
        public GameObject CannonBallPrefab;
        public float CannonBallSpeed;
        public int CannonBallCount = 5;
        public float CannonBallLength = 1F;
        public float CannonBallDamage = 5F;
        public float CannonXOffset;

        private List<ShipControl> _shipsInRange;

        private SpriteShapeRenderer _spriteShapeRenderer;
        private PolygonCollider2D _collider;
        private ShipControl _ownShipControl;

        private float _remainingReadyUpTime;
        private float _spriteMaskStartPosition;

        private bool _visible;

        private void Start()
        {
            _spriteShapeRenderer = GetComponent<SpriteShapeRenderer>();
            _collider = GetComponent<PolygonCollider2D>();

            _spriteShapeRenderer.enabled = false;
            ReadySpriteShapeRenderer.enabled = false;

            _spriteMaskStartPosition = ReadySpriteMaskTransform.transform.localPosition.x;

            _shipsInRange = new List<ShipControl>();

            _ownShipControl = ShipControl.FindShipControlInParents(gameObject);
        }

        private void Update()
        {
            if (GlobalGameState.IsUnpaused())
            {
                _spriteShapeRenderer.enabled = _visible;
                ReadySpriteShapeRenderer.enabled = _visible;

                if (_remainingReadyUpTime > 0)
                {
                    _remainingReadyUpTime -= Time.deltaTime;
                }

                var maskPosition = ReadySpriteMaskTransform.localPosition;
                var maskPositionX = Mathf.Lerp(_spriteMaskStartPosition * -1, _spriteMaskStartPosition,
                    Mathf.Clamp(ReadyUpTime - _remainingReadyUpTime, 0, ReadyUpTime) / ReadyUpTime);
                ReadySpriteMaskTransform.localPosition = new Vector3(maskPositionX, maskPosition.y, maskPosition.z);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var otherShipControl = ShipControl.FindShipControlInParents(other.gameObject);
            if (otherShipControl != null && otherShipControl != _ownShipControl && !_shipsInRange.Contains(otherShipControl) && otherShipControl.Team != _ownShipControl.Team)
            {
                _shipsInRange.Add(otherShipControl);
                Debug.Log($"Added shipControl to inRange {otherShipControl.gameObject.name}");
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var otherShipControl = ShipControl.FindShipControlInParents(other.gameObject);
            if (otherShipControl != null && otherShipControl != _ownShipControl && otherShipControl.Team != _ownShipControl.Team)
            {
                Debug.Log($"Removed shipControl from inRange {otherShipControl.gameObject.name}");
                _shipsInRange.Remove(otherShipControl);
            }
        }

        public void Fire(Vector2 targetPosition)
        {
            if (IsReady())
            {
                if (_ownShipControl.Team == Team.Player)
                {
                    targetPosition = _collider.ClosestPoint(targetPosition);
                }

                Debug.Log($"FIRE at {targetPosition}!");
                var ownPosition = (Vector2) _ownShipControl.transform.position;

                var cannonDistance = CannonBallLength / CannonBallCount;

                for (int i = 0; i < CannonBallCount; i++)
                {
                    var y = -CannonBallLength / 2 + cannonDistance * i;
                    var offset = (Vector2) transform.right * -1 * CannonXOffset + (Vector2) transform.up * y;
                    var cannonPosition = ownPosition + offset;
                    
                    var direction = targetPosition - cannonPosition;
                    if (_ownShipControl.Team == Team.Player)
                    {
                        direction = direction.normalized * MaxDistance;
                    }
                    else
                    {
                        direction = direction * 1.1F;
                    }

                    direction = Vector2.ClampMagnitude(direction, MaxDistance);

                    var cannonBallGameObject = Instantiate(CannonBallPrefab, cannonPosition, Quaternion.identity);
                    var cannonBall = cannonBallGameObject.GetComponent<CannonBall>();
                    cannonBall.Distance = direction.magnitude;
                    cannonBall.Speed = direction.normalized * CannonBallSpeed;
                    cannonBall.OriginShip = _ownShipControl;
                    cannonBall.Damage = CannonBallDamage;
                }

                _remainingReadyUpTime += ReadyUpTime;
            }
        }

        public List<ShipControl> GetShipsInRange()
        {
            return _shipsInRange;
        }

        public bool IsReady()
        {
            return _remainingReadyUpTime <= 0;
        }

        public void Show(bool visible)
        {
            _visible = visible;
        }
    }
}
