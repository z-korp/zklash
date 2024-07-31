using System;
using UnityEngine;
using GameCharacter = zKlash.Game.Character;

public class MouseHoverDetector : MonoBehaviour
{
    public GameObject canvas;

    private bool isDragging = false;

    private ShopMessageManager _shopMessageManager;

    private string _priceString = "It will cost you: \n\n";

    private int _price;


    void Start()
    {
        _shopMessageManager = ShopMessageManager.Instance;
        GameCharacter character = GetComponent<MobController>().Character;
        _price = character.Role.Cost(character.Level);
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
    }


    void OnMouseEnter()
    {
        if (canvas != null)
        {
            _shopMessageManager.ShowShopMessage(_priceString + _price.ToString());
            if (!isDragging)
                canvas.SetActive(true);

        }
    }

    void OnMouseExit()
    {
        if (canvas != null)
        {
            ShopMessageManager.Instance.ResetShopMessage();
            if (!isDragging)
                canvas.SetActive(false);
        }
    }

    public void OnMouseDownCanvas()
    {
        if (canvas != null)
        {
            isDragging = true;
            canvas.SetActive(false);
        }
    }

    public void OnMouseUpCanvas()
    {
        if (canvas != null)
        {
            isDragging = false;
            canvas.SetActive(true);
        }
    }

}
