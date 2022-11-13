using UnityEngine;
using UnityEngine.UI;

public class clickAd : MonoBehaviour
{
    Button button;

    void Start(){
        button = GetComponent<Button>();
        button.onClick.AddListener(()=>{
            AdsManager.Instance.ShowInterstitialTimer(null);
        });
    }
}