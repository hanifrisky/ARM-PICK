using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectivesListItem : MonoBehaviour
{
    public int count;
    public int elapsedCount;
    public string label;
    public bool isFinish = false;
    public GameObject centang;
    public TMP_Text textLabel;
    public bool robot = false;
    public List<ObjectivesItem> listObjectives;

    public void setLabel(string labelText)
    {
        label = labelText;
        if (count>1)
        {
            textLabel.text = label + "(" + elapsedCount + "/" + count + ")";
        }
        else
        {
            textLabel.text = label;
        }
    }
    public void setCount(int count)
    {
        this.count = count;
        elapsedCount = 0;
        if (count > 1)
        {
            textLabel.text = label + " (" + elapsedCount + "/" + count + ")";
        }
        else
        {
            textLabel.text = label;
        }
    }
    public void AddObjectCount()
    {
        elapsedCount++;
        if (count > 1)
        {
            textLabel.text = label + " (" + elapsedCount + "/" + count + ")";
        }
        if (elapsedCount == count)
        {
            setFinish(true);
        }
    }
    public bool CheckIsMember(ObjectivesItem itemCheck)
    {
        foreach (var item in listObjectives)
        {
            if (item.gameObject == itemCheck.gameObject)
            {
                return true;
            }
        }
        return false;
    }
    public void setFinish(bool isFinish)
    {
        this.isFinish = isFinish;
        centang.SetActive(isFinish);
    }
}
