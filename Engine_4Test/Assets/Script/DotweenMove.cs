using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DotweenMove : MonoBehaviour
{
    void Start()
    {
        transform.DOMoveY(1.5f, 2.5f).SetLoops(-1,LoopType.Yoyo);
    }
}
