using UnityEngine;

namespace Ship
{
    public class CannonControlInput : MonoBehaviour
    {
        public FiringArcControl RightFiringArc;
        public FiringArcControl LeftFiringArc;

        private UnityEngine.Camera _camera;
        private FiringArcControl _activeFiringArc;


        private void Start()
        {
            _camera = GameObject.FindWithTag("MainCamera").GetComponent<UnityEngine.Camera>();
        }

        private void Update()
        {
            if (GlobalGameState.IsUnpaused())
            {
                ChooseActiveFiringArc();

                if (Input.GetMouseButtonDown(0) && _activeFiringArc != null && _activeFiringArc.IsReady())
                {
                    _activeFiringArc.Fire(_camera.ScreenToWorldPoint(Input.mousePosition));
                }
            }
            else
            {
                _activeFiringArc = null;
            }

            HideShowFiringArcs();
        }

        private void ChooseActiveFiringArc()
        {
            var mousePosition = Input.mousePosition;

            var mousePositionScreen = _camera.ScreenToViewportPoint(mousePosition);

            if (mousePositionScreen.x < 0.49F)
            {
                _activeFiringArc = LeftFiringArc;
            }
            else if (mousePositionScreen.x > 0.51F)
            {
                _activeFiringArc = RightFiringArc;
            }
            else
            {
                _activeFiringArc = null;
            }
        }

        private void HideShowFiringArcs()
        {
            RightFiringArc.Show(_activeFiringArc == RightFiringArc);
            LeftFiringArc.Show(_activeFiringArc == LeftFiringArc);
        }
    }
}
