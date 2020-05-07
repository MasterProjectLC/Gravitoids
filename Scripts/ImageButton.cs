using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageButton : MenuButton
{
    private bool chosen = false;

    public override void PointerDown()
    {
        transform.parent.GetComponent<ShipSelectOverseer>().UnchooseOtherButtons(this);
        chosen = true;
        base.PointerDown();
    }

    public override void PointerExit()
    {
        if (!chosen)
            base.PointerExit();
    }

    public void Unchoose()
    {
        chosen = false;
        PointerExit();
    }
}
