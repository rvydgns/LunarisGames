using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speaker;

    [TextArea(2, 4)]
    public string text;
}
