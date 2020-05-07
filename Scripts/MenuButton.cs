using UnityEngine;

public class MenuButton : Button
{
    [SerializeField]
    private MenuOverseer menuOverseer;
    [SerializeField]
    private int type = 0;

    public override void PointerDown()
    {
        menuOverseer.Selected(type);
        Debug.Log("selected");
    }

    public override void RightPointerDown() { }

    public override void PointerUp()
    {
    }

    public override void RightPointerUp()
    {
    }

    public override void PointerEnter()
    {
        ChangeScale(1.1f);
    }

    public override void PointerExit()
    {
        ChangeScale(1f);
    }
}
