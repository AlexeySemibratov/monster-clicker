using UniRx;
using UnityEngine;

public class LooseDialogPresenter : MonoBehaviour
{
    [SerializeField]
    private DialogPopup _dialogPopup;

    [SerializeField]
    private GameLoop _gameLoop;

    [SerializeField]
    private RewarderAdShower _rewardedAdShower;

    private CompositeDisposable _adDisposable = new();

    public void OnRestartClicked()
    {
        _gameLoop.Restart();
        _dialogPopup.HideDialog();
    }

    public void OnContinueClicked()
    {
        _rewardedAdShower.RequestAdShowing()
            .Subscribe(result => HandleRewardedAdResult(result))
            .AddTo(_adDisposable);
    }

    private void HandleRewardedAdResult(RewardedAdResult result)
    {
        _adDisposable.Clear();
        Debug.Log($"Handle ad result {result}");

        if (result == RewardedAdResult.UserRewarded)
        {
            OnAdRewarded();
        }
    }

    private void OnAdRewarded()
    {
        _gameLoop.Continue();
        _dialogPopup.HideDialog();
    }

    private void OnDestroy()
    {
        Debug.Log("Dispose ad request");
        _adDisposable.Dispose();
    }
}
