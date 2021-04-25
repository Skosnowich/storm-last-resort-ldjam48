using UnityEngine;

namespace Ship
{
    [RequireComponent(typeof(ShipControl))]
    public class ShipControlInput : MonoBehaviour
    {
        private ShipControl _shipControl;

        private void Start()
        {
            _shipControl = GetComponent<ShipControl>();
        }

        private void Update()
        {
            if (GlobalGameState.IsUnpaused())
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    _shipControl.OpenSail();
                }

                if (Input.GetKeyDown(KeyCode.S))
                {
                    _shipControl.CloseSail();
                }

                if (Input.GetKey(KeyCode.A))
                {
                    _shipControl.SteerLeft();
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    _shipControl.SteerRight();
                }
            }
        }
    }
}
