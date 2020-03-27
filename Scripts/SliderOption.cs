using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderOption : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isClicking = false;

    [SerializeField]
    private float centerRatio;
    [SerializeField]
    private float radius;
    [SerializeField]
    private string option;
    [SerializeField]
    private GameObject line;

    float value = 1f;
    float center;
    float oldWidth;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateToScreenWidth(value);
    }

    // Update is called once per frame
    void Update()
    {
        // Update to screen width
        if (Screen.width != oldWidth)
        {
            UpdateToScreenWidth(value);
        }

        // If holding slider
        if (isClicking)
        {
            float newX = Mathf.Clamp(Input.mousePosition.x, center - radius, center + radius);

            GetComponent<RectTransform>().position = new Vector3(newX, GetComponent<RectTransform>().position.y);
            float effectValue = (newX - center + radius) / (radius * 2);
            Effect(effectValue);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isClicking = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClicking = false;
    }

    void UpdateToScreenWidth(float value)
    {
        center = Screen.width * centerRatio;
        line.GetComponent<RectTransform>().position = new Vector3(center, line.GetComponent<RectTransform>().position.y);
        float newX = (value * (radius * 2)) - radius + center;
        GetComponent<RectTransform>().position = new Vector3(newX, GetComponent<RectTransform>().position.y);
        oldWidth = Screen.width;
    }

    void Effect(float value)
    {
        this.value = value;
        SendMessageUpwards("GetSliderValue", this);
    }

    // GETTER / SETTER
    public string GetOption()
    {
        return option;
    }

    public float GetRadius()
    {
        return radius;
    }

    public float GetCenter()
    {
        return center;
    }

    public float GetValue()
    {
        return value;
    }
}
