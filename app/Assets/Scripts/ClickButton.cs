using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image _imageBtn;
    public Sprite _default, _pressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        _imageBtn.sprite = _pressed;
        //audioSource.PlayOneShot(compressedClip);
        ContractActions.instance.TriggerStartBattle();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _imageBtn.sprite = _default;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}
