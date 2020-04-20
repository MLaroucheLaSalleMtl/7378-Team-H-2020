﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : Grabbable
{
    [SerializeField] private AudioSource aSource;
    // Start is called before the first frame update
    new void Start() {
        base.Start();
        base.id = 3;
    }

    public override void DoInteraction() {
        aSource.Play();
        base.DoInteraction();
        ShowInHands();
    }
    protected override bool ShowInHands() {
        if (!base.ShowInHands()) return false;
        //Sets the object position relatable to the camera*/
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + 0.39f, gameObject.transform.localPosition.y - 0.09f, gameObject.transform.localPosition.z + 0.47f);
        gameObject.transform.localRotation = cam.transform.localRotation;
        gameObject.transform.localEulerAngles = new Vector3(-44, gameObject.transform.localEulerAngles.y + 56.72f, gameObject.transform.localEulerAngles.z + 0);
        return true;
    }
}