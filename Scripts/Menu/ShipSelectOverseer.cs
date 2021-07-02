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
    private GameObject[] shipButtons;

    [SerializeField]
    private GameObject[] factionButtons;

    private int ship = 0;
    private int faction = 0;

    override public void Selected(int type)
    {
        if (type == 100)
            menu.OpenMenu(this, false);

        else if (type == 200)
            StartGame();

        else
            switch(type)
            {
                case 1:
                    ChangeShip(-1);
                    break;
                case 2:
                    ChangeShip(1);
                    break;

                case 3:
                    ChangeFaction(-1);
                    break;
                case 4:
                    ChangeFaction(1);
                    break;
            }
        
    }

    void StartGame()
    {
        AudioManager.AM.Stop("MainMenuTheme");
        space.gameObject.SetActive(true);
        space.SetBackgrounding(false);
        space.StartGame(ship, faction);
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


    private void ChangeShip(int modifier)
    {
        shipButtons[ship].SetActive(false);
        ship = (ship + modifier + shipButtons.Length) % shipButtons.Length;
        shipButtons[ship].SetActive(true);
    }

    private void ChangeFaction(int modifier)
    {
        factionButtons[faction].SetActive(false);
        faction = (faction + modifier + factionButtons.Length) % factionButtons.Length;
        factionButtons[faction].SetActive(true);
    }
}
