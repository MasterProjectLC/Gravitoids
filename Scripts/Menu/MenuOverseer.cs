using UnityEngine.SceneManagement;
using UnityEngine;

abstract public class MenuOverseer : MonoBehaviour
{

    abstract public void Selected(int type);

    public void ButtonHover(MenuButton button)
    {
        button.ChangeScale(1.1f);
    }

    public void ButtonStopHover(MenuButton button)
    {
        button.ChangeScale(1f);
    }
}
