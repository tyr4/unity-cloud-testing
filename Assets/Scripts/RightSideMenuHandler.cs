using UnityEngine;
using UnityEngine.EventSystems;

public class RightSideMenuHandler : MonoBehaviour, IButtonActionHandler
{
    private readonly Vector3[] _corners = new Vector3[4];
    private GameObject _lastPanelPressed;

    [SerializeField] private GameObject outline;
    [SerializeField] private GameObject menu;

    private const float OpenSpeed = 2f;
    private float _menuWidth;
    private bool _isOpenRightSideMenu;
    
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
    
    public void OnButtonClick(PointerEventData eventData)
    {
        var panel = eventData.pointerEnter.gameObject.transform as RectTransform;
        var panelMenu = panel.Find("Panel UI");
        panelMenu?.gameObject.SetActive(true);
        
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
    }
}