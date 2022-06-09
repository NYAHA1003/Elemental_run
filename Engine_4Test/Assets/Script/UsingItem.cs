using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingItem : MonoBehaviour
{
    private Ray ray;
    private float chargeGauge = 10f;
    private int cntItem = 0;
    private int maxItem = 3;

    private void Update()
    {
        StartCoroutine(GetItem());
    }

    IEnumerator GetItem()
    {
        cntItem++;
        yield return new WaitForSeconds(chargeGauge);
    }
}
