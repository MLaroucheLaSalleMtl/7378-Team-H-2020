using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrench : Grabbable
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }
    public override void DoInteraction() {        
        base.DoInteraction();
        ShowInHands();
    }
    protected override void ShowInHands() {

        base.ShowInHands();
        //Sets the object position relatable to the camera*/
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + 0.39f, gameObject.transform.localPosition.y - 0.24f, gameObject.transform.localPosition.z + 0.47f);
        gameObject.transform.localRotation = cam.transform.localRotation;
        gameObject.transform.localEulerAngles = new Vector3(-2.58f, gameObject.transform.localEulerAngles.y + 0f, gameObject.transform.localEulerAngles.z + 20f);
    }
}
