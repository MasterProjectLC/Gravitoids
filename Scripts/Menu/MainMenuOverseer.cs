using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuOverseer : MenuOverseer
{
    [SerializeField]
    private GameObject title;

    [SerializeField]
    private OptionOverseer options;

    [SerializeField]
    private ShipSelectOverseer shipSelect;

    [SerializeField]
    private Space space;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.AM.StopAll();
        AudioManager.AM.Play("MainMenuTheme");
        space.SetBackgrounding(true);
        space.StartGame(0, 0);

        if (Messenger.MS.resetting)
        {
            Messenger.MS.resetting = false;
            Selected(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // R - Reset
        if (Input.GetKeyDown(KeyCode.R) && space.gameObject.activeInHierarchy)
        {
            space.ResetGame();
        }

        // Esc - Menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (title.activeInHierarchy)
                Selected(3);
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    override public void Selected(int type)
    {
        switch (type)
        {
            case 0:
                OpenMenu(shipSelect, true);
                break;

            case 1:
                OpenMenu(options, true);
                break;

            case 3:
                Application.Quit();
                break;
        }
    }

    public void OpenMenu(MenuOverseer otherMenu, bool activate)
    {
        otherMenu.gameObject.SetActive(activate);
        title.gameObject.SetActive(!activate);
    }
}

