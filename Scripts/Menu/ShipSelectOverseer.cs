using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSelectOverseer : MenuOverseer
{
    [SerializeField]
    private MainMenuOverseer menu;

    [SerializeField]
    private Space space;

    [SerializeField]
    private ImageButton[] shipButtons;

    [SerializeField]
    private ImageButton[] factionButtons;

    private int shipType = 0;
    private int faction = 0;

    override public void Selected(int type)
    {
        if (type == 100)
            menu.OpenMenu(this, false);

        else if (type == 200)
            StartGame();

        else if (type >= 10)
            faction = type - 10;

        else
            shipType = type;
        
    }

    void StartGame()
    {
        AudioManager.AM.Stop("MainMenuTheme");
        space.gameObject.SetActive(true);
        space.SetBackgrounding(false);
        space.StartGame(shipType, faction);
        gameObject.SetActive(false);
    }

    public void UnchooseOtherButtons(ImageButton requester)
    {
        bool isShip = false;
        for (int i = 0; i < shipButtons.Length; i++)
        {
            if (shipButtons[i] == requester)
                isShip = true;
        }

        if (isShip)
        {
            for (int i = 0; i < shipButtons.Length; i++)
                if (shipButtons[i] != requester)
                    shipButtons[i].GetComponent<ImageButton>().Unchoose();
        }
        else
            for (int j = 0; j < factionButtons.Length; j++)
                if (factionButtons[j] != requester)
                    factionButtons[j].GetComponent<ImageButton>().Unchoose();
    }
}
