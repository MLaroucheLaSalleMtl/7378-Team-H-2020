using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Grabbable { 
    new void Start()
    {
        base.Start();
        base.id = 0;
    }
    public override void DoInteraction()
    {
        base.DoInteraction();
        ShowInHands();
    }
    protected override bool ShowInHands()
    {
        if (!base.ShowInHands()) return false;
        //Sets the object position relatable to the camera*/
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + 0.46f, gameObject.transform.localPosition.y - 0.75f, gameObject.transform.localPosition.z + 0.05f);
        gameObject.transform.localRotation = cam.transform.localRotation;
        gameObject.transform.localEulerAngles = new Vector3(-142.58f, gameObject.transform.localEulerAngles.y + 35f, gameObject.transform.localEulerAngles.z + 70f);
        return true;
    }
}
