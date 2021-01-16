using UnityEngine;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;
using UnityEngine.Windows.Speech;
using System;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.IO;

public class SubtitleManager : MonoBehaviour
{
    public Text _statusText;

    public Text _subtitleText;

    public Text _tempText;

    public RawImage _statusImage;

    public Camera _mainCamera;

    DictationHandler _dictationHandler;

    DictationRecognizer _dictationRecognizer;



    // Start is called before the first frame update
    void Start()
    {
        CoreServices.DiagnosticsSystem.ShowDiagnostics = false;

        CoreServices.DiagnosticsSystem.ShowProfiler = false;

        PointerHandler pointerHandler = gameObject.AddComponent<PointerHandler>();

        CoreServices.InputSystem.RegisterHandler<IMixedRealityPointerHandler>(pointerHandler);

        pointerHandler.OnPointerDown.AddListener(_gestureRecognizer_TappedEvent);

        pointerHandler.OnPointerClicked.AddListener(_gestureRecognizer_TappedEvent);

        pointerHandler.OnPointerUp.AddListener(_gestureRecognizer_TappedEvent);

        //_mainCamera.clearFlags = CameraClearFlags.SolidColor;

        //_mainCamera.backgroundColor = Color.clear;

        //_mainCamera.nearClipPlane = (float)0.85;
    }


    private void Awake()
    {
        _dictationRecognizer = new DictationRecognizer();

        _dictationHandler = gameObject.AddComponent<DictationHandler>();

        CoreServices.InputSystem.RegisterHandler<IMixedRealityDictationHandler>(_dictationHandler);

        _dictationRecognizer.DictationHypothesis += _dictationRecognizer_DictationHypothesis;

        _dictationRecognizer.DictationResult += _dictationRecognizer_DictationResult;

        _dictationRecognizer.DictationComplete += _dictationRecognizer_DictationComplete;

        PhraseRecognitionSystem.Shutdown();

        _dictationRecognizer.Start();
    }

    private void _dictationRecognizer_DictationComplete(DictationCompletionCause cause)
    {
        Set_Listening();
    }

    private void _dictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        Set_Listening();

        _subtitleText.text = _tempText.text + " " + text;

        _tempText.text = _subtitleText.text;
    }

    private void _dictationRecognizer_DictationHypothesis(string text)
    {
        _subtitleText.text = _tempText.text + " " + text;

        Set_Thinking();

    }

    private void _gestureRecognizer_TappedEvent(MixedRealityPointerEventData evt)
    {
        //Debug.Log("Tap Detected");
        System.Diagnostics.Debug.WriteLine("Tap Detected");

        Set_Listening();
        
    }

    private void Set_Listening()
    {
        _dictationRecognizer.Start();

        //Debug.Log("Set_Listening");
        System.Diagnostics.Debug.WriteLine("Set_Listening");

        var listeningTexture = Resources.Load("Listening");

        _statusImage.texture = (Texture2D)listeningTexture;

        _statusText.text = "Listening";

    }

    private void Set_Thinking()
    {
        //Debug.Log("Set_Thinking");
        System.Diagnostics.Debug.WriteLine("Set_Thinking");

        var thinkingTexture = Resources.Load("Thinking");

        _statusImage.texture = (Texture2D)thinkingTexture;

        _statusText.text = "Thinking";
    }
}
