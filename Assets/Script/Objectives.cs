using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Objectives : MonoBehaviour
{
    public static Objectives instance;

    [Header("Objectives")]
    public List<ObjectivesItem> listObjectives;

    [Header("Setup")]
    public ObjectivesListItem prefabList;
    public List<ObjectivesListItem> listItem;
    public Transform transformList;
    public Transform transformList2;
    ObjectivesListItem prevItem;
    public int limitList = 5;

    [Header("Finish Event")]
    public int jumlahFinish;
    public UnityEvent finishEvent;
    

    [Header("Timer")]
    public float elapsedTime;
    public TMP_Text timer;
    public bool timerStart = false;

    public int indexObjectives;


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        int jumlahItem = 1;
        indexObjectives = 0;
        jumlahFinish = 0;
        elapsedTime = 0;
        int nomor = 0;
        RoboticGrabObject rAnim;
        foreach (var item in listObjectives)
        {
            if (prevItem != null)
            {
                if(prevItem.label == item.label)
                {
                    prevItem.listObjectives.Add(item);
                    jumlahItem++;
                    prevItem.setCount(jumlahItem);
                    if (prevItem.robot)
                    {
                        rAnim = item.GetComponent<RoboticGrabObject>();
                        prevItem.GetComponent<RoboticAnimationSequance>().sequance.Add(rAnim);
                    }
                    continue;
                }
            }
            jumlahItem = 1;
            nomor++;
            ObjectivesListItem newItem = Instantiate(prefabList, transformParent());
            newItem.name = "List item ke-" + nomor;
            newItem.listObjectives = new List<ObjectivesItem>();
            prevItem = newItem;
            newItem.setNomor(nomor);
            newItem.setLabel(item.label);
            newItem.setCount(jumlahItem);
            newItem.robot = item.robot;
            if (newItem.robot)
            {
                rAnim = item.GetComponent<RoboticGrabObject>();
                newItem.GetComponent<RoboticAnimationSequance>().sequance.Clear();
                newItem.GetComponent<RoboticAnimationSequance>().sequance.Add(rAnim);
            }
            newItem.listObjectives.Add(item);
            listItem.Add(newItem);
        }
    }
    public Transform transformParent()
    {
        if (transformList.childCount < limitList)
        {
            return transformList;
        }
        else
        {
            return transformList2;
        }
    }
    private void Update()
    {
        if (timerStart)
        {
            elapsedTime += Time.deltaTime;

            int hours = (int)(elapsedTime / 3600);
            int minutes = (int)(elapsedTime / 60) % 60;
            int seconds = (int)(elapsedTime % 60);

            timer.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }
    }
    [ContextMenu("Mulai")]
    public void Mulai()
    {
        timerStart = true;
        CheckAnimRobot(0);
    }
    public void Check(ObjectivesItem itemCheck)
    {
        Debug.Log(itemCheck.name);
        int index = 0;
        bool isFinish = false;
        foreach (var item in listItem)
        {
            bool isMember = item.CheckIsMember(itemCheck);
            if (isMember)
            {
                indexObjectives = index;
                Debug.Log(itemCheck.name + " is member of " + item.gameObject.name);
                item.AddObjectCount();
                isFinish = item.isFinish;
                Debug.Log("is Finish");
                break;
            }
            else
            {
                Debug.Log(itemCheck.name + " is NOT member of " + item.gameObject.name);
            }
            index++;
        }
        if (isFinish)
        {
            Debug.Log("Finish 1 Step-" + indexObjectives);
            indexObjectives = indexObjectives + 1;
            jumlahFinish++;
            if (jumlahFinish == listItem.Count)
            {
                timerStart = false;
                finishEvent?.Invoke();
            }
        }
        CheckAnimRobot(indexObjectives);
    }
    public void CheckAnimRobot(int index)
    {
        Debug.Log("check index-" + index);
        if (index >= listItem.Count)
        {
            return;
        }
        if (listItem[index].robot)
        {
            int animIndexRobot = listItem[index].elapsedCount;
            RoboticAnimationSequance animRobot = listItem[index].GetComponent<RoboticAnimationSequance>();
            Debug.Log("Animate: " + listItem[index].name + " ke-" + animIndexRobot);
            animRobot.Animate(animIndexRobot);
        }
    }
}
