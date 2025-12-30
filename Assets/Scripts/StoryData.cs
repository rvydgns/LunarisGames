using UnityEngine;

[CreateAssetMenu(fileName = "StoryData", menuName = "Echoes/Story Data")]
public class StoryData : ScriptableObject
{
    [TextArea(4, 12)]
    public string[] pages;
}

