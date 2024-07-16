using UnityEngine;

public class ButtonSpeed : MonoBehaviour
{
    public GameObject ImgSpeed1;
    public GameObject ImgSpeed2;

    private TimeScaleController _timeScaleController;

    private void Start()
    {
        _timeScaleController = TimeScaleController.Instance;
    }

    public void OnClickToggleSpeed()
    {
        ImgSpeed1.SetActive(!ImgSpeed1.activeSelf);
        ImgSpeed2.SetActive(!ImgSpeed2.activeSelf);

        if (ImgSpeed1.activeSelf)
        {
            OnClickSpeed1();
        }
        else
        {
            OnClickSpeed2();
        }
    }

    private void OnClickSpeed1()
    {
        _timeScaleController.SetTimeScale(1.0f);
        _timeScaleController.UpdateAnimatorList();
        _timeScaleController.ApplySpeed();
    }

    private void OnClickSpeed2()
    {
        _timeScaleController.SetTimeScale(2.0f);
        _timeScaleController.UpdateAnimatorList();
        _timeScaleController.ApplySpeed();
    }
}
