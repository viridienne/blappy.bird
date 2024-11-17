using System.Collections.Generic;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;

[CreateAssetMenu(fileName = "UIDatabase", menuName = "Flappy/UI/UIDatabase")]
public class UIDatabase : SerializedScriptableObject
{
    public Dictionary<UIKey, BaseUI> UDictionary = new();
}

public enum UIKey
{
    Full_MainScreen = 1,
    Popup_GameOver = 2,
}