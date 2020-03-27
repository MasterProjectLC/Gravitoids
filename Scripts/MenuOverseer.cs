using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuOverseer : MonoBehaviour
{
    [SerializeField]
    private GameObject title;

    [SerializeField]
    private OptionOverseer options;

    [SerializeField]
    private Space space;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.AM.StopAll();
        AudioManager.AM.Play("MainMenuTheme");
        space.SetBackgrounding(true);
        space.StartGame();

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

    public void Selected(int type)
    {
        switch(type)
        {
            case 0:
                AudioManager.AM.Stop("MainMenuTheme");
                title.SetActive(false);
                space.gameObject.SetActive(true);
                space.SetBackgrounding(false);
                space.StartGame();
                break;

            case 1:
                options.gameObject.SetActive(true);
                title.gameObject.SetActive(false);
                break;

            case 3:
                Application.Quit();
                break;

            case 4:
                title.gameObject.SetActive(true);
                options.gameObject.SetActive(false);
                break;
        }
    }

    public void ButtonHover(MenuButton button)
    {
        button.ChangeScale(1.1f);
    }

    public void ButtonStopHover(MenuButton button)
    {
        button.ChangeScale(1f);
    }
}
