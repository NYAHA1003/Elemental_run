using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DotweenObs : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        UpDownMove();
    }

    public void UpDownMove()
    {
        transform.DOMoveY(-1, 1f);
        transform.DOMoveY(1, 1f);
    }
}
