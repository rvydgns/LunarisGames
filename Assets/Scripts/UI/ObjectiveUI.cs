using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveUI : MonoBehaviour
{
    [SerializeField] private Transform contentParent; // ObjectivesPanel/Content
    [SerializeField] private GameObject rowPrefab;    // ObjectiveRow prefab

    private readonly Dictionary<ObjectiveId, RowRefs> rows = new();

    private class RowRefs
    {
        public Image icon;
        public TMP_Text desc;
        public TMP_Text progress;
        public ObjectiveType type;
    }

    private void OnEnable()
    {
        ObjectiveTracker.Instance.OnObjectivesReloaded += Rebuild;
        ObjectiveTracker.Instance.OnObjectiveUpdated += Refresh;
    }

    private void OnDisable()
    {
        if (ObjectiveTracker.Instance == null) return;
        ObjectiveTracker.Instance.OnObjectivesReloaded -= Rebuild;
        ObjectiveTracker.Instance.OnObjectiveUpdated -= Refresh;
    }

    private void Start() => Rebuild();

    private void Rebuild()
    {
        foreach (Transform c in contentParent) Destroy(c.gameObject);
        rows.Clear();

        var data = ObjectiveTracker.Instance.GetLoaded();
        if (data == null) return;

        foreach (var o in data.objectives)
        {
            var go = Instantiate(rowPrefab, contentParent);

            var r = new RowRefs
            {
                icon = go.transform.Find("Icon").GetComponent<Image>(),
                desc = go.transform.Find("DescriptionText").GetComponent<TMP_Text>(),
                progress = go.transform.Find("ProgressText").GetComponent<TMP_Text>(),
                type = o.type
            };

            r.desc.text = o.description;

            if (o.type == ObjectiveType.Collect)
            {
                r.icon.gameObject.SetActive(true);
                r.progress.gameObject.SetActive(true);

                r.icon.sprite = o.icon;
                r.progress.text = $"0/{ObjectiveTracker.Instance.GetTarget(o.id)}";
            }
            else
            {
                // Text görevi: sadece açıklama görünsün
                r.icon.gameObject.SetActive(false);
                r.progress.gameObject.SetActive(false);
            }

            rows[o.id] = r;
            Refresh(o.id);
        }
    }

    private void Refresh(ObjectiveId id)
    {
        if (!rows.TryGetValue(id, out var r)) return;
        if (r.type != ObjectiveType.Collect) return;

        int cur = ObjectiveTracker.Instance.GetCurrent(id);
        int tar = ObjectiveTracker.Instance.GetTarget(id);
        r.progress.text = $"{cur}/{tar}";
    }
}
