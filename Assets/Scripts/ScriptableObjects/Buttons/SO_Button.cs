using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Button", menuName = "Scriptable Objects/SO_Button")]

public class SO_Button : ScriptableObject
{
    public string text;
    public Color textColor;
    public bool isInteractable;
    public bool isActive;
    public E_ButtonActions actionToExecute;
}
