using System.Collections.Generic;
using UnityEngine;

public class TimeScaleController : Singleton<TimeScaleController>
{
    private List<Animator> animators;
    public float speedGame = 1f;

    protected override void Init()
    {
        UpdateAnimatorList();
    }

    void Update()
    {

    }

    public void UpdateAnimatorList()
    {
        animators = new List<Animator>(FindObjectsOfType<Animator>());
    }

    public void AddAnimator(Animator newAnimator)
    {
        if (!animators.Contains(newAnimator))
        {
            animators.Add(newAnimator);
            animators[animators.Count - 1].speed = Time.timeScale * speedGame;
        }
    }

    public void SetTimeScale(float newTimeScale)
    {
        Time.timeScale = newTimeScale;
    }

    public void SetTimeScale2X()
    {
        Time.timeScale = 2f;
        ApplySpeed();
    }

    public void SetTimeScale1X()
    {
        Time.timeScale = 1f;
        ApplySpeed();
    }

    public void ResetTimeScale()
    {
        Time.timeScale = 1f;
        ApplySpeed();
    }

    public void ApplySpeed()
    {
        foreach (var animator in animators)
        {
            animator.speed = Time.timeScale * speedGame;
        }
    }

    public void PlayNormalSpeed()
    {
        foreach (var animator in animators)
        {
            animator.speed = Time.timeScale * 1f;
        }

    }

}
