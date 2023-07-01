using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MTT
{
    internal enum UIStartButtonMode
    {
        NoName,
        Warming,
        Warming2,
        Join,
        Create,
        Loading
    }


    [RequireComponent(typeof(EventTrigger))]
    internal class UIStartButton : MonoBehaviour, IPointerClickHandler
    {
        public delegate void ClickHandler();
        public event ClickHandler Click;

        [SerializeField] private string _noNameText;
        [SerializeField] private string _warmingText;
        [SerializeField] private string _warming2Text;
        [SerializeField] private string _joinText;
        [SerializeField] private string _createText;
        [Space]
        [SerializeField] private Text _text;
        [SerializeField] private Image _loading;

        public void ChangeText(UIStartButtonMode mode)
        {
            if (_loading.gameObject.activeSelf && mode != UIStartButtonMode.Loading)
            {
                _text.gameObject.SetActive(true);
                _loading.gameObject.SetActive(false);
            }
            else if (mode == UIStartButtonMode.Loading)
            {
                _text.gameObject.SetActive(false);
                _loading.gameObject.SetActive(true);
            }
            switch (mode)
            {
                case UIStartButtonMode.NoName:
                    {
                        _text.text = _noNameText;
                        break;
                    }
                case UIStartButtonMode.Join:
                    {
                        _text.text = _joinText;
                        break;
                    }
                case UIStartButtonMode.Create:
                    {
                        _text.text = _createText;
                        break;
                    }
                case UIStartButtonMode.Warming:
                    {
                        _text.text = _warmingText;
                        break;
                    }
                case UIStartButtonMode.Warming2:
                    {
                        _text.text = _warming2Text;
                        break;
                    }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Click?.Invoke();
        }
    }
}