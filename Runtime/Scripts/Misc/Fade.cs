using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class Fade : MonoBehaviour {

    public static Fade Instance;

    private IEnumerator _coRoutine;
    protected CanvasGroup _canvas;
    private Image[] _imgs;
    private AdvancedLerp _lerp;

    private GameObject _child = null;
    private CanvasGroup _canvasChild = null;

	[Header("-- Fading Parameters --")]
    [SerializeField]
    private Image _waitingImage;
    [SerializeField]
    private Sprite _loadingSprite;
    [SerializeField]
    private Sprite _whiteSprite;
    [SerializeField]
    private GameObject _coin;
    [SerializeField]
    private FadeOptions _optionsDisplay;
	[SerializeField]
    private FadeOptions _optionsClear;
    [SerializeField]
    private FadeOptions _optionsDefaultBackAndForth = new FadeOptions(null, 1.25f, new AnimationCurve(new Keyframe(0f,0f),new Keyframe(.3f,1f),new Keyframe(0.7f,1f), new Keyframe(1f,0)), Color.black, true);

    public FadeOptions OptionsDisplay
    {
        get { return _optionsDisplay; }
    }

	public FadeOptions OptionsClear
    {
        get { return _optionsClear; }
    }

    public bool IsFinish()
    {
        return _lerp == null || _lerp.IsFinish();
    }

    public float GetPourcentage ()
    {
        if (_lerp == null) return 1;
        return _lerp.GetPourcentage();
    }

	public float GetAlpha()
	{
		return _canvas.alpha;
	}

	private void OnDestroy()
	{
        StopAllCoroutines();
        CancelInvoke();
		if (Instance == this)
			Instance = null;
	}

	protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            if (gameObject.transform.parent == null)
                DontDestroyOnLoad(gameObject);
            
            _canvas = GetComponent<CanvasGroup>();
            _imgs = GetComponentsInChildren<Image>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void StartFadeInOut(Color color, System.Action callback = null, float duration = .5f, float waitDuration = 0.2f)
    {
        Debug.Log("StartFadeInOut");
        StartCoroutine(FadeInOut(color, callback, duration, waitDuration));
    }
    private IEnumerator FadeInOut(Color color, System.Action callback = null, float duration = .5f, float waitDuration = 0.2f)
    {
        ActiveCoin();

        FadeOptions options = new FadeOptions(null, duration, new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) }), color, true);
        FadeOptions clearOption = new FadeOptions(null, duration, new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) }), color, false);

        StartFade(options);
        do
        {
            yield return null;
        }
        while (!IsFinish()); // When Full black screen

        if (callback != null) callback.Invoke();

        yield return new WaitForSeconds(waitDuration);

        StartFade(clearOption);
        do
        {
            yield return null;
        }
        while (!IsFinish());

        yield return null;
    }

    public void StartBasicFadeToBlack()
	{
		FadeOptions options = new FadeOptions(null, .4f, new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) }), Color.black, true);
		StartFade(options);
	}

	public void StartBasicFadeToClear()
	{
        Debug.Log("StartBasicFadeToClear");

        FadeOptions options = new FadeOptions(null, .4f, new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) }), Color.black, false);
		StartFade(options);
	}

    public void StartFade(FadeOptions options)
    {
        if (_coRoutine != null)
        {
            StopCoroutine(_coRoutine);
        }
        _coRoutine = FadeCoRoutine(options);
        StartCoroutine(_coRoutine);
    }
    //Kilian : For animation or event calls that can only use argumentless methods or single arguments with base variable types
    /// <summary>
    /// Default duration is 1 second. Check current "Options Default Back And Forth" settings on the Fade component to verify.
    /// </summary>
    public void StartDefaultFade()
    {
        Debug.Log("Playing basic fade!");
        StartFade(_optionsDefaultBackAndForth);
    }

    public void SetWaitingImageBlack()
    {
        // We want loading screen with image only at first load of the game
        _waitingImage.color = Color.black;
        _waitingImage.sprite = _whiteSprite;
        if (_coin) _coin.SetActive(false);
    }

    IEnumerator FadeCoRoutine(FadeOptions options)
    {
        ActiveCoin();

        AdvancedLerp.LerpFloat param = new AdvancedLerp.LerpFloat()
        {
            during = (options.Duration > 0) ? options.Duration : 1f,
            useLerpCurve = true,
            lerpCurve = options.Curve,
            startFloat = _canvas.alpha,
            endFloat = options.Display ? 1f : 0f
        };

        _lerp = new AdvancedLerp(param, true);

        if (options.Child != null && _child == null /*&& _canvas.alpha == 0*/)
        {
            _child = Instantiate(options.Child, gameObject.transform) as GameObject;
            _child.transform.localPosition = Vector3.zero;
            _child.transform.localRotation = Quaternion.identity;
            _canvasChild = _child.GetComponent<CanvasGroup>();
        }

        if(_canvasChild != null)
        {
            _canvasChild.alpha = 1f;
        }

        foreach (Image _img in _imgs)
        {
            _img.color = options.Color;
        }

        Debug.Log("<color=darkblue><b>FADE</b></color> Start Fade from " + param.startFloat + " to " + param.endFloat + " in " + param.during + " seconds");

        do
        {
            yield return null;
            float alpha = _lerp.GetFloat();
            _canvas.alpha = alpha;
            if (_child != null && param.endFloat == 0)
            {
                _canvasChild.alpha = (alpha - 0.5f) * 2f;
            }

        } while (!_lerp.IsFinish()) ;

        if (_child != null && _canvas.alpha == 0)
        {
            Destroy(_child);
            _child = null;
            _canvasChild = null;
        }

        _lerp = null;
        _coRoutine = null;

        //if (options.Display) VoiceManager.StopAll(); //Stop all voice lines that would still be playing
    }


    void ActiveCoin()
    {
        _coin.SetActive(false);
    }
}

[System.Serializable]
public struct FadeOptions
{
    public GameObject Child;
    public float Duration;
    public AnimationCurve Curve;
    public Color Color;
    public bool Display;

	public FadeOptions(	GameObject child, 
						float duration, 
						AnimationCurve animationCurve, 
						Color color, 
						bool display)
	{
		Child = child;
		Duration = duration;
		Curve = animationCurve;
		Color = color;
		Display = display;
	}
}
