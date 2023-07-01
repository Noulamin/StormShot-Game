using UnityEngine;
using UnityEngine.UI;

public class clickAd : MonoBehaviour
{
    public UnityEngine.UI.Button.ButtonClickedEvent clickedEvent;
    private void OnEnable()
    {
        var b = GetComponent<UnityEngine.UI.Button>();
        b.onClick.AddListener(showAd);
    }
    private void OnDisable()
    {
        var b = GetComponent<UnityEngine.UI.Button>();
        b.onClick.RemoveListener(showAd);
    }

    void showAd()
    {
        AdsManager.Instance.ShowInterstitialTimer(() =>
        {
            clickedEvent?.Invoke();
        });
    }
}