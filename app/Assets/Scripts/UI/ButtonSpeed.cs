using UnityEngine;

public class ButtonSpeed : MonoBehaviour
{
    public GameObject ImgSpeed1;
    public GameObject ImgSpeed2;

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
        TimeScaleController.Instance.SetTimeScale(1.0f);
        TimeScaleController.Instance.UpdateAnimatorList();
        TimeScaleController.Instance.ApplySpeed();
    }

    private void OnClickSpeed2()
    {
        TimeScaleController.Instance.SetTimeScale(2.0f);
        TimeScaleController.Instance.UpdateAnimatorList();
        TimeScaleController.Instance.ApplySpeed();
    }
}
