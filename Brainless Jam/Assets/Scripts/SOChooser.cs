using UnityEngine;

public class SOChooser : MonoBehaviour
{
    public BlockScriptableObjects so;
    public BuilderUI builderUI;

    public void getSO()
    {
        builderUI.selectedBlock = so;
    }
}
