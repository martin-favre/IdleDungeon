using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreenComponent : MonoBehaviour
{
    private enum State
    {
        DoingNothing,
        FadingIn,
        FadingOut
    }
    private Image image;

    [SerializeField]
    private float fadeStep;

    [SerializeField]
    private State state = State.DoingNothing;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void Hide()
    {
        if (!image) return;
        var color = image.color;
        image.color = new Color(color.r, color.g, color.b, 0);
    }
    public void Show()
    {
        if (!image) return;
        var color = image.color;
        image.color = new Color(color.r, color.g, color.b, 1);
    }

    private void Update()
    {
        if (!image) return;

        switch (state)
        {
            case State.FadingIn:
                {

                    var color = image.color;
                    float newA = color.a - (fadeStep * SingletonProvider.MainTimeProvider.DeltaTime);
                    if (newA < 0) newA = 0;
                    image.color = new Color(color.r, color.g, color.b, newA);
                    if (newA == 0)
                    {
                        state = State.DoingNothing;
                    }
                }
                break;
            case State.FadingOut:
                {
                    var color = image.color;
                    float newA = color.a + (fadeStep * SingletonProvider.MainTimeProvider.DeltaTime);
                    if (newA > 1) newA = 1;
                    image.color = new Color(color.r, color.g, color.b, newA);
                    if (newA == 1)
                    {
                        state = State.DoingNothing;
                    }
                }

                break;
        }
    }



    public void FadeIn()
    {
        Show();
        state = State.FadingIn;
    }

    public void FadeOut()
    {
        Hide();
        state = State.FadingOut;
    }
}
