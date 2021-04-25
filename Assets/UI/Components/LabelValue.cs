using UnityEngine;
using UnityEngine.UI;

namespace UI.Components
{
    public class LabelValue : MonoBehaviour
    {
        public Text Label;
        public string Suffix;
        public int Value;

        private void Update()
        {
            Label.text = $"{Value}{Suffix}";
        }
    }
}
