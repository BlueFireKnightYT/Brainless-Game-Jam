using UnityEngine;
using UnityEngine.EventSystems;

public class BuilderUI : MonoBehaviour
{
    public BlockScriptableObjects selectedBlock;

    public void ChooseGrid()
    {
        if (selectedBlock != null)
        { 
            GameObject selectedButton = EventSystem.current.currentSelectedGameObject;
            SOExtractor soExtractor = selectedButton.GetComponent<SOExtractor>();
            soExtractor.blockSO = selectedBlock;
            soExtractor.wasExtracted = false;
        }
    }
}
