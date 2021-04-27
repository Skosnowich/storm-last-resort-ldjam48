using Ship;
using UnityEngine;

public class BattleSceneLoader : MonoBehaviour
{
    public bool StageOverride;
    public Stage StartStage;
    
    private void Start()
    {
        if (!StageOverride)
        {
            StartStage = GlobalGameState.Stage;
        }
        
        var stageLoaders = FindObjectsOfType<StageLoader>(true);
        foreach (var stageLoader in stageLoaders)
        {
            if (stageLoader.Stage != StartStage)
            {
                Destroy(stageLoader.gameObject);
            }
        }
        foreach (var stageLoader in stageLoaders)
        {
            if (stageLoader != null && stageLoader.Stage == StartStage)
            {
                stageLoader.gameObject.SetActive(true);
            }
        }
    }
}
