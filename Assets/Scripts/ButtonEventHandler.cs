using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public interface IButtonActionHandler
{
    void OnButtonClick(PointerEventData eventData);
}

public class ButtonEventHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GraphicRaycaster raycaster;
    [SerializeField] private EventSystem eventSystem;
    // enough space for 1 button, can allocate more in the inspector
    [SerializeField] private Sprite[] sprites = new Sprite[3];
    // very important for intended behaviour
    [SerializeField] private GameObject buttonParent;
    // custom function to call when the button is clicked
    [SerializeField] private MonoBehaviour actionScript; 

    private IButtonActionHandler _handler;
    private GameObject _currentHovered;
    private GameObject _lastButtonClicked;
    private Image _currentHoveredImage;
    private int _currentId;

    private void Start()
    {
        _handler = actionScript as IButtonActionHandler;
        if (_handler == null)
        {
            Debug.LogError("Assigned script does not implement IButtonActionHandler");
        }
    }

    private void Update()
    {
        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        GameObject newHovered = null;

        foreach (var result in results)
        {
            if (result.gameObject.transform.parent?.transform == buttonParent.transform)
            {
                newHovered = result.gameObject;
                break;
            }
        }

        // 2nd condition makes sure that the image doesnt change if the button
        // is already selected
        if (newHovered != _currentHovered && (newHovered != _lastButtonClicked || (!newHovered && !_lastButtonClicked))) 
        {
            // Exit old
            if (_currentHoveredImage && _lastButtonClicked != _currentHovered)
            {
                _currentHoveredImage.sprite = sprites[_currentId * 3];
            }

            // Enter new
            if (newHovered)
            {
                _currentHovered = newHovered;
                _currentHoveredImage = newHovered.GetComponent<Image>();
                _currentId = _currentHovered.name[^1] - '0';
                _currentHoveredImage.sprite = sprites[_currentId * 3 + 1];
            }
            else
            {
                _currentHovered = null;
                _currentHoveredImage = null;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var button = eventData.pointerEnter;
        if (button.transform.parent != buttonParent.transform)
        {
            return;
        }
        _handler?.OnButtonClick(eventData);
        
        var buttonId = button.name[^1] - '0';
        var image = button.GetComponent<Image>();
        
        if (button != _lastButtonClicked)
        {
            image.sprite = sprites[buttonId * 3 + 2];
            
            if (_lastButtonClicked)
            {
                var lastImage = _lastButtonClicked.GetComponent<Image>();
                var lastId = lastImage.name[^1] - '0';
                lastImage.sprite = sprites[lastId * 3];
            }
            
        }
        else
        {
            image.sprite = sprites[buttonId * 3];
            _lastButtonClicked = null;
            return;
        }
        _lastButtonClicked = button;
    }
}
