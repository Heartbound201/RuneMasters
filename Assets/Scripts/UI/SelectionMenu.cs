using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class SelectionMenu : MonoBehaviour
    {
        public Text menuTitle;
        public List<SelectionMenuOption> menuOptions = new List<SelectionMenuOption>();
        public int selectionIndex;
        public int maxIndex;
        public GameObject menuOptionPrefab;

        public void Load(string text, List<string> options)
        {
            menuTitle.text = text;
            
            for (int i = 0; i < options.Count; ++i)
            {
                GameObject entry = Instantiate(menuOptionPrefab, this.transform);
                SelectionMenuOption selectionMenuOption = entry.GetComponent<SelectionMenuOption>();
                selectionMenuOption.label.text = options[i];
                menuOptions.Add(selectionMenuOption);
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Clear()
        {
            menuOptions.Clear();
        }

        private void BuildMenuOptions()
        {
            
        }
        
        public void LockMenuOption (int index, bool value)
        {
            if (index < 0 || index >= menuOptions.Count)
                return;
            // menuOptions[index].isLocked = value;
            if (value && selectionIndex == index)
                Next();
        }
        
        public void Next ()
        {
            for (int i = selectionIndex + 1; i < selectionIndex + menuOptions.Count; ++i)
            {
                int index = i % menuOptions.Count;
                if (SetSelection(index))
                    break;
            }
        }

        private bool SetSelection(int index)
        {
            throw new System.NotImplementedException();
        }

        public void Previous ()
        {
            for (int i = selectionIndex - 1 + menuOptions.Count; i > selectionIndex; --i)
            {
                int index = i % menuOptions.Count;
                if (SetSelection(index))
                    break;
            }
        }
    }
}