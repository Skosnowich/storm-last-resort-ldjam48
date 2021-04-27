using Ship;
using UnityEngine;

namespace Enemy
{
    public class FireIfInRange : MonoBehaviour
    {
        public FiringArcControl RightFiringArc;
        public FiringArcControl LeftFiringArc;

        private ShipControl _shipControl;

        private void Start()
        {
            _shipControl = GetComponent<ShipControl>();
        }

        private void Update()
        {
            if (GlobalGameState.IsUnpaused() && (RightFiringArc.IsReady() || LeftFiringArc.IsReady()))
            {
                FireIfPossible(RightFiringArc);
                FireIfPossible(LeftFiringArc);
            }
        }

        private void FireIfPossible(FiringArcControl firingArc)
        {
            var shipsInRange = firingArc.GetShipsInRange();
            if (shipsInRange != null)
            {
                foreach (var shipControl in shipsInRange)
                {
                    if (shipControl != null)
                    {
                        var currentVelocity = shipControl.GetCurrentVelocity();
                        var shipControlTransform = shipControl.transform;
                        var distance = (shipControlTransform.position - _shipControl.transform.position).magnitude;
                        var travelTime = distance / firingArc.CannonBallSpeed;
                        var targetPosition = (Vector2) shipControlTransform.position + (Vector2) shipControlTransform.up * currentVelocity * travelTime;
                        firingArc.Fire(targetPosition);
                        return;
                    }
                }
            }
        }
    }
}
