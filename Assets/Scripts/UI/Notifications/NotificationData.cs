using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NotificationData : ScriptableObject, IComparable<NotificationData>
{
    [field: SerializeField] public string Title { get; private set; }
    [field: SerializeField] public string Description { get; private set; }

    public void SetTitle(string title)
    {
        Title = title;
    }

    public void SetDescription(string description)
    {
        Description = description;
    }

    public int CompareTo(NotificationData other)
    {
        return Title.CompareTo(other.Title);
    }

    public void Init(string title, string description)
    {
        this.Title = title;
        this.Description = description;
    }

    public static NotificationData CreateInstance(string title, string description = "")
    {
        var data = ScriptableObject.CreateInstance<NotificationData>();
        data.Init(title, description);
        return data;
    }
}
