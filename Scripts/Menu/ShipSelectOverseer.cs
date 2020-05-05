using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSelectOverseer : MenuOverseer
{
    [SerializeField]
    private MainMenuOverseer menu;

    [SerializeField]
    private Space space;

    override public void Selected(int type)
    {
        if (type == 100)
        {
            menu.OpenMenu(this, false);
        } else
        {
            StartGame(type);
        }
    }

    void StartGame(int shipType)
    {
        AudioManager.AM.Stop("MainMenuTheme");
        space.gameObject.SetActive(true);
        space.SetBackgrounding(false);
        space.StartGame(shipType);
        gameObject.SetActive(false);
    }
}
