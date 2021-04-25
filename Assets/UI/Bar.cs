using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Bar : MonoBehaviour
    {
        public float Value = 1;

        public Transform Foreground;
        public Color ForegroundColor;
        
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = Foreground.GetComponent<SpriteRenderer>();
            _spriteRenderer.color = ForegroundColor;
        }

        private void Update()
        {
            Foreground.localScale = new Vector3(Value, 1, 1);
            Foreground.localPosition = new Vector3((1-Value) * -0.5F, 0, 0);
        }

        private void OnDrawGizmos()
        {
            Foreground.GetComponent<SpriteRenderer>().color = ForegroundColor;
        }
    }
}
