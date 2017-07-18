using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSmartCard : SmartCard
{
    void Start()
    {
        aiNotice = transform.Find("Notice").gameObject;
    }
    protected virtual void AutoDiscardCard(bool isNone)
    {
       HandCards t=gameObject.GetComponent<HandCards>();
        if(OrderController.Instance.Type== t.cType)
        {
          StartCoroutine(DelayDiscardCard(isNone));
        }
    }
    public virtual IEnumerator DelayDiscardCard(bool isNone)
    {
      yield return new WaitForSeconds(1.0f);
    }
}

