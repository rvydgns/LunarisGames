using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Level Objectives")]
public class LevelObjectives : ScriptableObject
{
    public List<ObjectiveDef> objectives = new();

    [Serializable]
    public class ObjectiveDef
    {
        public ObjectiveId id;
        public ObjectiveType type;

        [TextArea] public string description;

        // Sadece Collect için:
        public Sprite icon;
        public int targetCount = 1;
    }
}
