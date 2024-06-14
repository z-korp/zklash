using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxXP(int xp)
    {
        slider.maxValue = xp;
        //slider.value = xp;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetXP(int xp)
    {
        slider.value = xp;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetColor(Color color)
    {
        fill.color = color;
    }
}
