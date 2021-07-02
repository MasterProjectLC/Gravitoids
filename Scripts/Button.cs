using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class Button : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    bool mouseOver = false;
    float red = 0f;
    float blue = 0f;

    int originalSize;
    Vector2 originalSizeV;

    // Start is called before the first frame update
    public void Start()
    {
        if (GetComponent<Text>())
            originalSize = GetComponent<Text>().fontSize;
        else if (GetComponent<Image>())
            originalSizeV = GetComponent<Image>().rectTransform.sizeDelta;
    }

    // Update is called once per frame
    /*void Update()
    {
        // Mobile Controls
        /*if (isMobile && Input.touchCount > 0)
        {
            Touch theTouch = Input.GetTouch(0);
            if (theTouch.phase == TouchPhase.Ended || theTouch.phase == TouchPhase.Canceled)
            {
                PointerUp();
            } else if (InputControl.TouchRaycast())
            {
                if (InputControl.IsDoubleTap())
                    RightPointerDown();
                else
                    PointerDown();
            }
        }
    }*/

    // Getter
    public bool GetMouse()
    {
        return mouseOver;
    }


    // Setter/Changer
    public void ChangeTone(float color)
    {
        if (GetComponent<Image>() != null)
            GetComponent<Image>().color = new Color(color - blue, color - red - blue, color - red);
        if (GetComponent<Text>() != null)
            GetComponent<Text>().color = new Color(color - blue, color - red - blue, color - red);
    }

    public void ChangeTone(float color, float red)
    {
        this.red = red;
        ChangeTone(color);
    }

    public void ChangeTone(float color, float blue, bool isBlue)
    {
        this.blue = blue;
        ChangeTone(color);
    }

    public void ChangeScale(float scale)
    {
        if (GetComponent<Text>())
            GetComponent<Text>().fontSize = (int)(originalSize * scale);

        else if (GetComponent<Image>()) {
            Image image = GetComponent<Image>();
            Vector2 oldSize = image.rectTransform.sizeDelta;
            Vector2 updatedSize = originalSizeV * scale;

            float deltaX = (oldSize.x/2 - updatedSize.x/2)/2;
            float deltaY = (oldSize.y/2 - updatedSize.y/2)/2;
            image.rectTransform.sizeDelta = updatedSize;
            image.rectTransform.localPosition += new Vector3(deltaX, deltaY, 0);
        }
    }

    public void ChangeChildTone(int indexx, float color)
    {
        for (int i = 0; i < indexx; i++)
        {
            if (gameObject.transform.GetChild(i) != null)
            {
                if (gameObject.transform.GetChild(i).GetComponent<Text>() != null)
                    gameObject.transform.GetChild(i).GetComponent<Text>().color = new Color(color, color - red, color - red);

                else if (gameObject.transform.GetChild(i).GetComponent<Image>() != null)
                    gameObject.transform.GetChild(i).GetComponent<Image>().color = new Color(color, color - red, color - red);
            }
        }
    }

    public void ChangeChildTone(int indexx, float color, float red)
    {
        this.red = red;
        ChangeChildTone(indexx, color);
    }

    public abstract void PointerDown();

    public abstract void RightPointerDown();

    public abstract void PointerUp();

    public abstract void RightPointerUp();

    public abstract void PointerEnter();

    public abstract void PointerExit();

    // Pointer stuff
    public void OnPointerDown(PointerEventData eventData)
    {
        if (mouseOver)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                PointerDown();
            else if (eventData.button == PointerEventData.InputButton.Right)
                RightPointerDown();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (mouseOver)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                PointerUp();
            else if (eventData.button == PointerEventData.InputButton.Right)
                RightPointerUp();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        PointerEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        PointerExit();
    }

}