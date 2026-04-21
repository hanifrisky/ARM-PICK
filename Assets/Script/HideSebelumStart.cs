using System.Collections.Generic;
using UnityEngine;

public class HideSebelumStart : MonoBehaviour
{
    public List<GameObject> listObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideObject();
    }
    public void HideObject()
    {
        foreach (GameObject obj in listObject)
        {
            obj.SetActive(false);
        }
    }

    public void ShowObject()
    {
        foreach (GameObject obj in listObject)
        {
            obj.SetActive(true);
        }
    }

}
