using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    public string speakerName;

    [TextArea(2, 5)]
    public string[] lines;
}
