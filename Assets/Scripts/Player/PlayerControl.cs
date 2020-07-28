﻿using System;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Player
{
    /// <summary>
    /// Control the Movement of the Player.
    /// Current Control Scheme:
    ///     Mouse controls pitch and Yaw
    ///     Fire1 (left btn) to go faster
    ///     Fire2 (right btn) to go slower
    ///     Space to Land (and take off again)
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerControl : MonoBehaviour
    {

        [Space(10)]
        [SerializeField] private Vector3 controllerOffset = new Vector3(0.0f, 0.06f, 1.77f);

        [Header("Rotation Settings")] 
        [SerializeField] private Vector2 rotSpeedIncr;
        [SerializeField] private Vector2 maxRotSpeed;
        [SerializeField] private Vector2 minRotSpeed;
        public Vector2 currRotSpeed;
        [Header("Movement Settings")]
        [SerializeField] private float speedIncr = 0.5f;
        [SerializeField] private float maxSpeed = 15.0f;
        [SerializeField] private float minSpeed = 5.0f;
        public float currSpeed;


        public PlayerFlightState currState;

        private CharacterController controller;
        private PlayerPowerupBehavior powerup;
        private Vector3 move;

        void Start()
        {
            controller = GetComponent<CharacterController>();
            powerup = GetComponent<PlayerPowerupBehavior>();
            Cursor.lockState = CursorLockMode.Locked;

            currState = PlayerFlightState.Flying;
        }

        void Update()
        {
            if (!LevelManager.gamePaused)
            {
                Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

                switch (currState)
                {
                    case PlayerFlightState.Flying:
                        FlyingControl(mouseInput);
                        break;
                    case PlayerFlightState.Landed:
                        LandedControl(mouseInput);
                        break;
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    switch (currState)
                    {
                        case PlayerFlightState.Flying:
                            currState = PlayerFlightState.Landed;
                            break;
                        case PlayerFlightState.Landed:
                            currState = PlayerFlightState.Flying;
                            break;
                    }
                    // TODO: implement fighting movement control

                }

                if (Input.GetButtonDown("Powerup"))
                {
                    powerup.Activate();
                }
            }
        }

        private void FlyingControl(Vector2 input)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                currSpeed += speedIncr;
                currRotSpeed += rotSpeedIncr;
            }

            if (Input.GetButtonDown("Fire2"))
            {
                currSpeed -= speedIncr;
                currRotSpeed -= rotSpeedIncr;
            }
            currSpeed = Mathf.Clamp(currSpeed, minSpeed, maxSpeed);

            float boostedSpeed = currSpeed;
            if (powerup.GetActiveCurrentPowerup() == PlayerPowerup.Vortex)
            {
                boostedSpeed += PlayerPowerupBehavior.speedBoost;
            }
            move = transform.forward * boostedSpeed;
            
            currRotSpeed.x = Mathf.Clamp(currRotSpeed.x, minRotSpeed.x, maxRotSpeed.x);
            currRotSpeed.y = Mathf.Clamp(currRotSpeed.y, minRotSpeed.y, maxRotSpeed.y);
            
            Vector3 yaw = transform.right * (input.x * currRotSpeed.x * Time.deltaTime);
            Vector3 pitch = transform.up * (input.y * currRotSpeed.y * Time.deltaTime);
            Vector3 dir = yaw + pitch;

            if (Math.Abs(dir.magnitude) > Mathf.Epsilon)
            {
                float maxX = Quaternion.LookRotation(dir).eulerAngles.x;
                
                // limit rotation to avoid going getting stuck in a loop
                bool enteringLoop = (maxX < 90 && maxX > 70 || maxX > 270 && maxX < 290);
                
                if (!enteringLoop)
                {
                    move += dir;
                    transform.rotation = Quaternion.LookRotation(move);
                }
            }

            
            controller.Move((move + controllerOffset) * Time.deltaTime);
        }

        private void LandedControl(Vector2 input)
        {
            move = Physics.gravity;
            float moveX = Input.GetAxis("Mouse X") * currRotSpeed.x * Time.deltaTime;
            float moveY = -Input.GetAxis("Mouse Y") * currRotSpeed.y * Time.deltaTime;
            
            transform.Rotate(Vector3.up * moveX);
            transform.Rotate(Vector3.right * moveY);
            
            controller.Move(move * Time.deltaTime);
        }
        

    }
    
    public enum PlayerFlightState
    {
        Flying = 0, Landed = 1, Fighting = 2
    }
}
