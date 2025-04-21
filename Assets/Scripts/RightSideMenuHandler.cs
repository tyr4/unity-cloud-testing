using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RightSideMenuHandler : MonoBehaviour, IPointerClickHandler
{
    private readonly Vector3[] _corners = new Vector3[4];
    private GameObject _lastPanelPressed;
    
    
    [SerializeField] private GraphicRaycaster raycaster;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject outline;
    [SerializeField] private GameObject menu;
    // normal - hover - selected * 4 buttons
    [SerializeField] private Sprite[] sprites = new Sprite[12];

    private const float OpenSpeed = 2f;
    private float _menuWidth;
    private bool _isOpenRightSideMenu;

    private GameObject _currentHovered;
    private GameObject _lastButtonClicked;
    private Image _currentHoveredImage;
    private int _currentId;
    
    private void Start()
    {
        // set the menus inactive
        if (outline != null)
        {
            outline.SetActive(false);    
        }
        
        for (int i = 0; i < menu.transform.childCount; i++)
        {
            Transform child = menu.transform.GetChild(i);
            for (int j = 0; j < child.childCount; j++)
            {
                Transform child2 = child.GetChild(j);
                if (child2.name.EndsWith("Panel UI"))
                {
                    var panelUI = child2;
                    if (panelUI != null)
                    {   
                        panelUI.gameObject.SetActive(false);
                    }
                }
            }
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
            if (result.gameObject.transform.parent?.name == "Side Bar")
            {
                newHovered = result.gameObject;
                break;
            }
        }

        // 2nd condition makes sure that the image doesnt change if the button
        // is already selected
        Debug.Log(newHovered);
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
        var buttonId = button.name[^1] - '0';
        var image = button.GetComponent<Image>();
        OpenCloseRightSidePanel(button.transform as RectTransform);
        
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

    private void OpenCloseRightSidePanel(RectTransform panel)
    {
        var panelMenu = panel.Find("Panel UI");
        panelMenu.gameObject.SetActive(true);
        
        var menuPos = panelMenu as RectTransform;
        var parentPos = panel.parent.position;

        // get the distance to move the buttons by
        if (_corners[0] == Vector3.zero)
        {
            menuPos.GetWorldCorners(_corners); 
            _menuWidth = Vector2.Distance(_corners[0], _corners[3]);
        }

        // open for the first time
        if (!_isOpenRightSideMenu)
        {
            panel.parent.position = new Vector2(parentPos.x - _menuWidth, parentPos.y);
            outline.transform.position = new Vector2(outline.transform.position.x - _menuWidth, panel.position.y);
            
            outline.SetActive(true);
            _isOpenRightSideMenu = true;
        }
        // close if the same tab is pressed twice
        else if (_lastPanelPressed == panel.gameObject)
        {
            panel.parent.position = new Vector2(parentPos.x + _menuWidth, parentPos.y);
            outline.transform.position = new Vector2(outline.transform.position.x + _menuWidth, panel.position.y); 
            
            panelMenu.gameObject.SetActive(false);
            outline.SetActive(false);
            _isOpenRightSideMenu = false;
        }
        else
        {
            outline.transform.position = new Vector2(outline.transform.position.x, panel.position.y);
            var lastMenu = _lastPanelPressed.transform.Find("Panel UI");
            lastMenu.gameObject.SetActive(false);
        }
        
        _lastPanelPressed = panel.gameObject;
        EventSystem.current.SetSelectedGameObject(null);
    }
}