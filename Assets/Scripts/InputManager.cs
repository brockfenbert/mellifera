﻿using UnityEngine;

namespace Player
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] public float mouseSensitivity = 1;
        [Space(20)]
        [SerializeField] public string speedUpAxis = "Fire1";
        [SerializeField] public string slowDownAxis = "Fire2";
        [SerializeField] public KeyCode vortexKey = KeyCode.E;
        [SerializeField] public KeyCode danceKey = KeyCode.W;
        [SerializeField] public KeyCode stingKey = KeyCode.Q;
        [SerializeField] public KeyCode landFlyKey = KeyCode.Space;
        [SerializeField] public KeyCode talkKey = KeyCode.R;

        [Header("Non-configurable")] 
        [SerializeField] public KeyCode pauseKey = KeyCode.P;

        public bool GetSpeedUpBtnClicked() => Input.GetButtonDown(speedUpAxis);

        public bool GetSlowDownBtnClicked() => Input.GetButtonDown(slowDownAxis);

        public bool GetVortexKeyClicked() => Input.GetKeyDown(vortexKey);
        
        public bool GetDanceKeyClicked() => Input.GetKeyDown(danceKey);

        public bool GetStingKeyClicked() => Input.GetKeyDown(stingKey);
        
        public bool GetLandFlyKeyClicked() => Input.GetKeyDown(landFlyKey);

        public bool GetPauseBtnClicked() => Input.GetKeyDown(pauseKey);
        
        public bool GetNPCTalkBtnClicked() => Input.GetKeyDown(talkKey);

        

        public Vector2 GetMouseAxes() => new Vector2(Input.GetAxis("Mouse X") * mouseSensitivity, 
            Input.GetAxis("Mouse Y") * mouseSensitivity);
        
    }
}