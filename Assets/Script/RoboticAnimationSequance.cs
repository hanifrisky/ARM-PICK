using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoboticAnimationSequance : MonoBehaviour
{
    public List<RoboticJoin> listRoboticJoin;
    public List<RoboticGrabObject> sequance;
    public int index = 0;
    public bool mulai = false;
    public bool finishAnimateGrab;
    public bool finishAnimateJoin = false;

    private void Start()
    {
        finishAnimateGrab = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!mulai)
        {
            return;
        }
        if (!finishAnimateGrab)
        {
            return;
        }
        if(index > sequance.Count-1)
        {
            return;
        }
        StartCoroutine(prosesAmbil());
    }
    void SetAnimateJoin(bool val)
    {
        finishAnimateJoin = val;
    }
    IEnumerator prosesAmbil(int indexAnim = 0)
    {
        index = indexAnim;
        if (index > sequance.Count - 1)
        {
            yield break;
        }
        finishAnimateGrab = false;
        RoboticGrabObject objectGrab = sequance[index];

        //Rest Position
        AnimateRest();
        while (!finishAnimateJoin) yield return null;

        //----------- Ambil Object
        Debug.Log("Ambil:" + index);
        AnimateList(objectGrab.ambil);
        while (!finishAnimateJoin) yield return null;

        //----------- Grab Object
        Debug.Log("grab:" + index);
        AnimateList(objectGrab.grab);
        while (!finishAnimateJoin) yield return null;

        //folow target dan matikan simulasi
        objectGrab.FollowTarget();
        objectGrab.Simulasi(false);

        //----------- Transisi Object
        Debug.Log("transisi:" + index);
        AnimateList(objectGrab.transisi);
        while (!finishAnimateJoin) yield return null;

        //----------- Taruh Object
        Debug.Log("taruh:" + index);
        AnimateList(objectGrab.taruh);
        while (!finishAnimateJoin) yield return null;

        //----------- Lepas Object
        Debug.Log("lepas:" + index);
        AnimateList(objectGrab.lepas, .3f);
        while (!finishAnimateJoin) yield return null;

        //folow target dan matikan simulasi
        objectGrab.FollowTarget(false);
        objectGrab.Simulasi(true);

        //Rest Position
        AnimateRest();
        while (!finishAnimateJoin) yield return null;

        

        finishAnimateGrab = true;
    }
    public void Animate(int index)
    {
        StartCoroutine(prosesAmbil(index));
    }
    void AnimateList(List<RoboticAnim> list, float waktuAnimasi = 1f)
    {
        StartCoroutine(proses());
        IEnumerator proses()
        {
            SetAnimateJoin(false);
            int index = 0;
            bool isFinish = false;
            foreach (var anim in list)
            {
                if (index == 0)
                {
                    anim.join.AnimateTo(anim.angle, waktuAnimasi, (val) => {
                        Debug.Log("Aanimation join selesai:" + val);
                        isFinish = val;

                    });
                }
                else
                {
                    anim.join.AnimateTo(anim.angle, waktuAnimasi, (val) => {});
                }
                
                index++;
            }
            while (!isFinish)
            {
                yield return null;
            }
            //yield return new WaitForSeconds(waktuAnimasi);
            SetAnimateJoin(true);
        }
    }

    public void AnimateRest()
    {
        StartCoroutine(proses());
        IEnumerator proses()
        {
            Debug.Log("Rest:" + index);
            float waktuAnimasi = 1f;
            SetAnimateJoin(false);
            foreach (var join in listRoboticJoin)
            {
                join.AnimateTo(join.restAxis, waktuAnimasi, (val) => { });
            }
            yield return new WaitForSeconds(waktuAnimasi);
            SetAnimateJoin(true);
        }
    }
}
