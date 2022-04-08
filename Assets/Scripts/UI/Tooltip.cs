    using System;
    using TMPro;
    using UnityEngine;

    public class Tooltip : MonoBehaviour
    {
        private static Tooltip _instance;
        
        private TMP_Text tooltipText;
        private RectTransform backgroundRect;
        private RectTransform rect;
        [SerializeField] private Vector2 padding;
        [SerializeField] private RectTransform canvasRect;

        private void Awake()
        {
            if(_instance == null)
            {
                _instance = this;
            }
            backgroundRect = transform.Find("background").GetComponent<RectTransform>();
            rect = transform.GetComponent<RectTransform>();
            tooltipText = transform.Find("text").GetComponent<TMP_Text>();
            
            HideTooltip();
            
            
        }

        private void Update()
        {
            Vector2 anchoredPosition = Input.mousePosition / canvasRect.localScale.x;

            anchoredPosition.x = Mathf.Clamp(anchoredPosition.x, 0, canvasRect.rect.width - backgroundRect.rect.width);
            anchoredPosition.y = Mathf.Clamp(anchoredPosition.y, 0, canvasRect.rect.height - backgroundRect.rect.height);
            rect.anchoredPosition = anchoredPosition;
        }

        private void SetText(String text)
        {
            tooltipText.SetText(text);
            tooltipText.ForceMeshUpdate();
            Vector2 textSize = tooltipText.GetRenderedValues(false);
            Vector2 paddingSize = padding;
            backgroundRect.sizeDelta = textSize + paddingSize;
        }
        private void Show(string text)
        {
            gameObject.SetActive(true);
            SetText(text);
        }
        private void Hide()
        {
            gameObject.SetActive(false);
        }
        public static void ShowTooltip(string text)
        {
            if (_instance == null)
            {
                Debug.LogError("There's not tooltip object in the scene");
                return;
            }
            _instance.Show(text);
        }
        public static void HideTooltip()
        {
            if (_instance == null)
            {
                Debug.LogError("There's not tooltip object in the scene");
                return;
            }
            _instance.Hide();
        }
    }