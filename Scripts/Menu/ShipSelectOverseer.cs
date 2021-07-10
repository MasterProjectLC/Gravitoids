using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipSelectOverseer : MenuOverseer
{
    [SerializeField]
    private MainMenuOverseer menu;

    [SerializeField]
    private Space space;

    [SerializeField]
    private GameObject[] shipButtons;

    [SerializeField]
    private Text shipTitle;

    [SerializeField]
    private GameObject[] shipDescriptions;

    [SerializeField]
    private GameObject[] factionButtons;

    [SerializeField]
    private Text factionTitle;

    [SerializeField]
    private GameObject[] factionDescriptions;

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


    private void ChangeShip(int modifier)
    {
        shipButtons[ship].SetActive(false);
        shipDescriptions[ship].SetActive(false);

        ship = (ship + modifier + shipButtons.Length) % shipButtons.Length;
        shipTitle.text = shipButtons[ship].name;

        shipButtons[ship].SetActive(true);
        shipDescriptions[ship].SetActive(true);
    }

    private void ChangeFaction(int modifier)
    {
        factionButtons[faction].SetActive(false);
        factionDescriptions[faction].SetActive(false);

        faction = (faction + modifier + factionButtons.Length) % factionButtons.Length;
        factionTitle.text = factionButtons[faction].name;

        factionDescriptions[faction].SetActive(true);
        factionButtons[faction].SetActive(true);
    }
}
