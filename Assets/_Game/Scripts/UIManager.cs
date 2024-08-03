using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private void Awake() {
        instance = this;
    }

    [SerializeField] private TMP_Text coinText;

    public void SetCoin(int coin) {
        coinText.text = coin.ToString();
    }

}
