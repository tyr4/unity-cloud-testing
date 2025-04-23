using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;



public class InventoryFilterHandler : MonoBehaviour, IButtonActionHandler
{
    public void OnButtonClick(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerEnter);
    }
}
