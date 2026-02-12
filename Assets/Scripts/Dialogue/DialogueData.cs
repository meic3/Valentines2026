using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;

    [TextArea(2, 5)]
    public string text;
}

[CreateAssetMenu(menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    public DialogueLine[] lines;

    public string dialogueID;
    public bool playOnce;

    [Header("Item System")]
    public bool unlockItemUI = false; // Set true to show item UI after this dialogue
    public bool collectItem = false; // Set true if this dialogue gives an item
    public int itemIndex = -1; // Which item to collect (0, 1, or 2). -1 means no item
}