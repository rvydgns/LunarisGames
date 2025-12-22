using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveTracker : MonoBehaviour
{
    public static ObjectiveTracker Instance { get; private set; }

    public event Action OnObjectivesReloaded;
    public event Action<ObjectiveId> OnObjectiveUpdated;

    private LevelObjectives loaded;

    private readonly Dictionary<ObjectiveId, ObjectiveType> typeById = new();
    private readonly Dictionary<ObjectiveId, int> current = new();
    private readonly Dictionary<ObjectiveId, int> target = new();

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Load(LevelObjectives objectives)
    {
        loaded = objectives;

        typeById.Clear();
        current.Clear();
        target.Clear();

        if (loaded != null)
        {
            foreach (var o in loaded.objectives)
            {
                typeById[o.id] = o.type;

                if (o.type == ObjectiveType.Collect)
                {
                    current[o.id] = 0;
                    target[o.id] = Mathf.Max(1, o.targetCount);
                }
            }
        }

        OnObjectivesReloaded?.Invoke();
    }

    public LevelObjectives GetLoaded() => loaded;

    public ObjectiveType GetType(ObjectiveId id) =>
        typeById.TryGetValue(id, out var t) ? t : ObjectiveType.Text;

    public int GetCurrent(ObjectiveId id) => current.TryGetValue(id, out var v) ? v : 0;
    public int GetTarget(ObjectiveId id) => target.TryGetValue(id, out var v) ? v : 0;

    public void Add(ObjectiveId id, int amount = 1)
    {
        if (GetType(id) != ObjectiveType.Collect) return;
        if (!current.ContainsKey(id)) return;

        current[id] = Mathf.Clamp(current[id] + amount, 0, target[id]);
        OnObjectiveUpdated?.Invoke(id);
    }
}
