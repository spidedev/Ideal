using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OWN_GUI : MonoBehaviour
{
    public ItemObject item;
    public Image o, t;

    private void Start()
    {
        o.sprite = item.sprite;
        t.sprite = item.sprite;
    }
}
