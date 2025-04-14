using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    #region RIGHT SIDE PANELS

    private readonly Vector3[] _corners = new Vector3[4];
    private bool _isOpenRightSideMenu;
    
    private RectTransform _openRightSideMenuRect;

    private const float OpenSpeed = 2f;
    private float _menuWidth;

    #endregion

    public void OpenCloseRightSidePanel(RectTransform panel)
    {
        var menuPos = panel.Find("Panel UI") as RectTransform;
        var panelPos = panel.position;
        // menuPos.gameObject.SetActive(true);

        if (_corners[0] == Vector3.zero)
        {
            menuPos.GetWorldCorners(_corners); 
            _menuWidth = Vector2.Distance(_corners[0], _corners[3]);
        }
        
        Debug.Log(panel.position.x + " " + _menuWidth + " " + menuPos.anchoredPosition);
        if (!_isOpenRightSideMenu)
        {
            if (_openRightSideMenuRect)
            {
                var openMenuPos = _openRightSideMenuRect.position;
                _openRightSideMenuRect.position = new Vector2(openMenuPos.x + _menuWidth, openMenuPos.y);
            }
            _openRightSideMenuRect = panel;
            
            panel.position = new Vector2(panelPos.x - _menuWidth, panelPos.y);
            _isOpenRightSideMenu = true;
        }
        else
        {
            panel.position = new Vector2(panelPos.x + _menuWidth, panelPos.y);
            // menuPos.gameObject.SetActive(false);
            _isOpenRightSideMenu = false;
            _openRightSideMenuRect = null;
        }
    }
}