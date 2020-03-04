﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : Grabbable
{
    new void Start() {
        base.Start();
    }
    public override void DoInteraction() {
        base.DoInteraction();
        ShowInHands();
    }
    protected override void ShowInHands() {

        base.ShowInHands();
        //Sets the object position relatable to the camera*/
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + 0.46f, gameObject.transform.localPosition.y - 0.75f, gameObject.transform.localPosition.z + 0.05f);
        gameObject.transform.localRotation = cam.transform.localRotation;
        gameObject.transform.localEulerAngles = new Vector3(-142.58f, gameObject.transform.localEulerAngles.y + 35f, gameObject.transform.localEulerAngles.z + 70f);
    }
}
