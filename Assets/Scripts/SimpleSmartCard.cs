using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSmartCard : SmartCard
{
    void Start()
    {
        aiNotice = transform.Find("Notice").gameObject;
        OrderController.Instance.smartCard += AutoDiscardCard;
    }

    protected virtual void AutoDiscardCard(bool isNone)
    {
       HandCards t=gameObject.GetComponent<HandCards>();
        if(OrderController.Instance.Type== t.cType)
        {
          StartCoroutine(DelayDiscardCard(isNone));
        }
    }
    public override IEnumerator DelayDiscardCard(bool isNone)
    {
        return base.DelayDiscardCard(isNone);
    }
    public override List<Card> FirstCard()
    {
        List<Card> ret = new List<Card>();
        for (int i = 12; i >= 5; i--)
        {
            ret = FindStraight(GetAllCards(), (int)Weight.Three, i, true);
            if (ret.Count != 0)
                break;
        }
        if (ret.Count == 0)
        {
            for (int i = 0; i < 36; i += 3)
            {
                ret = FindThreeAndTwo(GetAllCards(), i, true);
                if (ret.Count != 0)
                    break;
            }
        }

        if (ret.Count == 0)
        {
            for (int i = 0; i < 36; i += 3)
            {
                ret = FindThreeAndOne(GetAllCards(), i, true);
                if (ret.Count != 0)
                    break;
            }
        }

        if (ret.Count == 0)
        {
            for (int i = 0; i < 36; i += 3)
            {
                ret = FindOnlyThree(GetAllCards(), i, true);
                if (ret.Count != 0)
                    break;
            }
        }

        if (ret.Count == 0)
        {
            for (int i = 0; i < 24; i += 2)
            {
                ret = FindDouble(GetAllCards(), i, true);
                if (ret.Count != 0)
                    break;
            }
        }

        if (ret.Count == 0)
        {
            ret = FindSingle(GetAllCards(), (int)Weight.Three, true);
        }

        return ret;
    }
}

