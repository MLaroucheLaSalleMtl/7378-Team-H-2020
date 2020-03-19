using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatlhGiver : BoxAnimation
{
    [SerializeField] private int healPoints;
    new void Start() {
        base.Start();
    }
    
    public override void DoInteraction() {
        code = GameManager.instance;
        if (!isInteractable) return;
        base.DoInteraction();
        code.GiveHealth(healPoints);
    }
}
