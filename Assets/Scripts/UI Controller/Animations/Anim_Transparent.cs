using UnityEngine;
using DG.Tweening;
public class Anim_Transparent : UI_SectionAnimate
{
    public CanvasGroup canvasGroup;
    Sequence anim;
    protected override void TurnOn()
    {
        base.TurnOn();
        canvasGroup.alpha = 0;
        anim.Kill();
        anim = DOTween.Sequence();
        anim.Append(canvasGroup.DOFade(1, speedTurnOn));
    }

    protected override void TurnOff()
    {
        base.TurnOff();
        anim.Kill();
        anim = DOTween.Sequence();
        anim.Append(canvasGroup.DOFade(0, speedTurnOff));
    }
}
