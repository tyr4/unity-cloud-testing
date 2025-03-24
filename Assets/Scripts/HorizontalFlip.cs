using UnityEngine;

public class HorizontalFlip : MonoBehaviour
{
    public void Flip(float moveX, Transform gameObj)
    {
        if (moveX < 0)
        {
            // Debug.Log("GameObject Position: " + gameObj.localRotation + ", GameObject: " + gameObj);
            gameObj.localRotation = Quaternion.Euler(0, 180f, 0);
        }
        else if (moveX > 0)
        {
            gameObj.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
