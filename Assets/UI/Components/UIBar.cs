using UnityEngine;
using UnityEngine.UI;

namespace UI.Components
{
    public class UIBar : MonoBehaviour
    {
        public bool UpdateEditorView;
        
        public RectTransform ForegroundTransform;
        public Image ForegroundSpriteRenderer;
        public Image BackgroundSpriteRenderer;
        public Text Label;
        public Text DescriptionLabel;
        public Color ForegroundColor;
        public Color BackgroundColor;
        public Color TextColor;

        public string Description;
        public int MaxValue = 100;
        private int _value = 100;

        public int Value
        {
            get => _value;
            set => _value = Mathf.Clamp(value, 0, MaxValue);
        }

        private void Start()
        {
            _value = MaxValue;
        }

        private void Update()
        {
            UpdateThis();
        }

        private void UpdateThis()
        {
            ForegroundSpriteRenderer.color = ForegroundColor;
            BackgroundSpriteRenderer.color = BackgroundColor;
            Label.color = TextColor;
            DescriptionLabel.text = $"{Description} ";

            ForegroundTransform.localScale = new Vector3((float) Value / MaxValue, 1, 1);
            Label.text = $"{Value} / {MaxValue}";
        }

        private void OnDrawGizmos()
        {
            if (UpdateEditorView)
            {
                _value = MaxValue / 2;
                UpdateEditorView = false;
                UpdateThis();
            }
        }
    }
}
