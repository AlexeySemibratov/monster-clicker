using UnityEngine;

public class LooseDialogPresenter : MonoBehaviour
{
    [SerializeField]
    private DialogPopup _dialogPopup;

    [SerializeField]
    private GameLoop _gameLoop;

    public void OnRestartClicked()
    {
        _gameLoop.Restart();
        _dialogPopup.HideDialog();
    }

    public void OnContinueClicked()
    {
        _gameLoop.Continue();
        _dialogPopup.HideDialog();
    }
}
