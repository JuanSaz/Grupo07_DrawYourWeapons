using System.Collections.Generic;
using UnityEngine;

public class ChangeCanvasUI: MonoBehaviour
{
    [SerializeField] private List<GameObject> ActivateList = new List<GameObject>();
    [SerializeField] private List<GameObject> DeactivateList = new List<GameObject>();

    public void ChangeCanvas()
    {
        foreach (GameObject canvas in ActivateList)
        {
            canvas.SetActive(true);
        }

        foreach (GameObject canvas in DeactivateList)
        {
            canvas.SetActive(false);
        }
    }
}
