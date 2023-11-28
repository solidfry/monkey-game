using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NotificationData : ScriptableObject, IComparable<NotificationData>
{
    [field: SerializeField] public string Title { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public Color IconColor { get; private set; }

    public void SetTitle(string title)
    {
        Title = title;
    }

    public void SetDescription(string description)
    {
        Description = description;
    }

    public void SetIcon(Sprite icon)
    {
        Icon = icon;
    }

    public int CompareTo(NotificationData other)
    {
        return Title.CompareTo(other.Title);
    }

    public void Init(string title, string description, Sprite icon = null)
    {
        this.Title = title;
        this.Description = description;
        this.Icon = icon;
    }

    public static NotificationData CreateInstance(string title, string description = "", Sprite icon = null)
    {
        var data = ScriptableObject.CreateInstance<NotificationData>();
        data.Init(title, description, icon);
        return data;
    }
}
