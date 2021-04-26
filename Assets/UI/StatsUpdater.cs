using UI.Components;
using UnityEngine;

namespace UI
{
    public class StatsUpdater : MonoBehaviour
    {

        public UIBar HullHealthBar;
        public UIBar CrewHealthBar;
        public LabelValue CannonLabelValue;
        public LabelValue GoldLabelValue;

        private void Update()
        {
            HullHealthBar.Value = GlobalGameState.CurrentHullHealth;
            HullHealthBar.MaxValue = GlobalGameState.MaxHullHealth;
            
            CrewHealthBar.Value = GlobalGameState.CurrentCrewHealth;
            CrewHealthBar.MaxValue = GlobalGameState.MaxCrewHealth;
            
            CannonLabelValue.Value = GlobalGameState.CannonCount;

            GoldLabelValue.Value = GlobalGameState.Gold;
        }
    }
}
