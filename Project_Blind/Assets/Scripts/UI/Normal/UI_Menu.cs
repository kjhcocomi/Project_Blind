﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_Menu : UI_Base
    {
        [SerializeField] private GameObject _settingUI;
        [SerializeField] private GameObject _clueUI;
        private GameObject _currActiveUI = null;
        #region Enums
        enum Images
        {
            Image_Bag,
            Image_Talisman,
            Image_Clue,
            Image_Setting,
            

            Image_Close,
            Image_Cursor,
        }
        #endregion
        const int MENU_SIZE = (int)Images.Image_Setting + 1;
        private Action[] _actions = new Action[MENU_SIZE];

        private int _currCursor;


        TransitionPoint _transition;
        public override void Init()
        {
            Bind<Image>(typeof(Images));

            UIManager.Instance.KeyInputEvents -= HandleUIKeyInput;
            UIManager.Instance.KeyInputEvents += HandleUIKeyInput;

            InitEvents();
            Time.timeScale = 0;

            _transition = FindObjectOfType<TransitionPoint>();

            _settingUI.SetActive(false);
            _clueUI.SetActive(false);
        }
        private void InitEvents()
        {
            Get<Image>((int)Images.Image_Bag).gameObject.BindEvent(PushBagButton, Define.UIEvent.Click);
            Get<Image>((int)Images.Image_Talisman).gameObject.BindEvent(PushTalismanButton, Define.UIEvent.Click);
            Get<Image>((int)Images.Image_Clue).gameObject.BindEvent(PushClueButton, Define.UIEvent.Click);
            Get<Image>((int)Images.Image_Setting).gameObject.BindEvent(PushSettingButton, Define.UIEvent.Click);
            Get<Image>((int)Images.Image_Close).gameObject.BindEvent(PushCloseButton, Define.UIEvent.Click);

            _actions[(int)Images.Image_Bag] += PushBagButton;
            _actions[(int)Images.Image_Talisman] += PushTalismanButton;
            _actions[(int)Images.Image_Clue] += PushClueButton;
            _actions[(int)Images.Image_Setting] += PushSettingButton;
            

            _currCursor = (int)Images.Image_Bag;
            Get<Image>((int)Images.Image_Cursor).transform.position = Get<Image>(_currCursor).transform.position;
        }
        private void PushButton(int num)
        {
            DataManager.Instance.SaveGameData();
            _currCursor = num;
            Get<Image>((int)Images.Image_Cursor).transform.position = Get<Image>(_currCursor).transform.position;
            ClearCurrActiveSetting();
        }
        private void PushBagButton()
        {
            PushButton((int)Images.Image_Bag);
            Debug.Log("PushBagButton");
        }
        private void PushTalismanButton()
        {
            PushButton((int)Images.Image_Talisman);
            Debug.Log("PushTalismanButton");
        }
        private void PushClueButton()
        {
            //UIManager.Instance.ShowNormalUI<UI_Clue>();
            PushButton((int)Images.Image_Clue);
            _clueUI.SetActive(true);
            _currActiveUI = _clueUI;
        }
        private void PushSettingButton()
        {
            //UIManager.Instance.ShowNormalUI<UI_Setting>();
            PushButton((int)Images.Image_Setting);
            _settingUI.SetActive(true);
            _currActiveUI = _settingUI;
        }
        private void PushCloseButton()
        {
            DataManager.Instance.SaveGameData();
            Time.timeScale = 1;
            UIManager.Instance.KeyInputEvents -= HandleUIKeyInput;
            UIManager.Instance.CloseNormalUI(this);
        }
        #region Update
        private void HandleUIKeyInput()
        {
            if (!Input.anyKey)
                return;

            if (_uiNum != UIManager.Instance.UINum)
                return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PushCloseButton();
                return;
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                _actions[_currCursor].Invoke();
                return;
            }

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                _currCursor = (_currCursor + 1) % MENU_SIZE;
                Get<Image>((int)Images.Image_Cursor).transform.position = Get<Image>(_currCursor).transform.position;
                return;
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _currCursor = (_currCursor - 1 + MENU_SIZE) % MENU_SIZE;
                Get<Image>((int)Images.Image_Cursor).transform.position = Get<Image>(_currCursor).transform.position;
                return;
            }
        }
        #endregion
        private void ClearCurrActiveSetting()
        {
            if (_currActiveUI != null)
            {
                if (_currActiveUI == _settingUI)
                {
                    _currActiveUI.GetComponent<UI_Setting>().CloseUI();
                }
                _currActiveUI.SetActive(false);
                _currActiveUI = null;
            }
        }
    }
}
