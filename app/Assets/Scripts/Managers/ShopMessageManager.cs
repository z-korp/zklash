using TMPro;
using UnityEngine;

public class ShopMessageManager : Singleton<ShopMessageManager>
{

    [SerializeField]
    private TextMeshProUGUI _shopText;
    private string _messageDefault = "Welcome to the shop\n\nHover Mob or Item to see prices";

    private void Start()
    {
        _shopText.text = _messageDefault;
    }


    public void ShowShopMessage(string message)
    {
        _shopText.text = message;
    }

    public void ResetShopMessage()
    {
        _shopText.text = _messageDefault;
    }

}