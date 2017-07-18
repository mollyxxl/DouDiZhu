using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SmartCard:MonoBehaviour
{
    protected GameObject aiNotice;
    void Start()
    {
        aiNotice = transform.Find("Notice").gameObject;
    }
}
