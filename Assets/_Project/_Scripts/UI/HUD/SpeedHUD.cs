using System.Collections;
using TMPro;
using Unity.Assertions;
using UnityEngine;

namespace Assets._Project._Scripts.UI.HUD
{
    public class SpeedHUD : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TMP_Text _text;

        [SerializeField, Range(0.001f, 0.1f)] private float _fadingSpeed = 0.01f;
        [SerializeField, Range(1f, 5f)] private float _showingTimeSeconds = 3f;

        private Coroutine _coroutine;

        private void Start()
        {
            Assert.IsNotNull(_canvas);
            Assert.IsNotNull(_text);

            _text.alpha = 0;
        }

        public void ShowSpeed(float speed)
        {
            if (_coroutine != null) 
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(ShowSpeedInternal(speed));
        }

        private IEnumerator ShowSpeedInternal(float speed)
        {
            _text.text = $"Current speed: {speed} km/h";

            for (float alpha = _text.alpha; alpha < 1; alpha += _fadingSpeed)
            {
                _text.alpha = alpha;
                yield return null;
            }
            _text.alpha = 1;

            yield return new WaitForSeconds(_showingTimeSeconds);
            
            for (float alpha = 1f; alpha > 0; alpha -= _fadingSpeed)
            {
                _text.alpha = alpha;
                yield return null;
            }
            _text.alpha = 0;
        }
    }
}
