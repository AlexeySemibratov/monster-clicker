using UnityEngine;

public class DialogPopup : MonoBehaviour
{
    public void ShowDialog()
    {
        gameObject.SetActive(true);
    }

    public void HideDialog()
    {
        gameObject.SetActive(false);
    }
}
