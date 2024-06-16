using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Viguar.UserInterfaceSystem
{
    [RequireComponent(typeof(Button))]
    public class UiTabSwitcher : MonoBehaviour
    {
        [SerializeField] private GameObject m_ConnectedTab;

        private Button m_Button;
        private GameObject[] m_Tabs;
        private GameObject[] m_TabButtons;


        private void Start()
        {
            m_Button = GetComponent<Button>();
            m_TabButtons = GameObject.FindGameObjectsWithTag("uiTabButton");
            m_Tabs = GameObject.FindGameObjectsWithTag("uiTab");
            print(m_TabButtons.Length);
        }

        private void enableAllButtons()
        {
            foreach(GameObject m_TabButton in m_TabButtons)
            {
                m_TabButton.GetComponent<Button>().interactable = true;
            }
        }

        private void disableAllTabs()
        {
            foreach(GameObject m_Tab in m_Tabs)
            {
                m_Tab.SetActive(false);
            }
        }

        public void switchTab()
        {
            enableAllButtons();
            m_Button.interactable = false;
            disableAllTabs();
            m_ConnectedTab.SetActive(true);
        }
    }
}
