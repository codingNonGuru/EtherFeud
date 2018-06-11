using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace EtherGame
{
    public class BattleState : MonoBehaviour
    {
        [SerializeField]
		Transform floor = null;

        [SerializeField]
        Transform fighter = null;

        Transform mainCamera = null;

        float azimuth = 150.0f;

        float inclination = 40.0f; 

        float distance = 10.0f;

        void Start()
        {
            mainCamera = Camera.main.transform;
        }

        void Update()
        {
            RefreshCamera();
        }

        void RefreshCamera()
        {
            /*var direction = new Vector3(
                Mathf.Cos(azimuth) * Mathf.Sin(inclination),
                Mathf.Cos(inclination), 
                Mathf.Sin(azimuth) * Mathf.Sin(inclination));

            mainCamera.position = -direction * distance;

            mainCamera.LookAt(fighter);*/

            var direction = -fighter.forward * 3.0f;
            direction += floor.up * 1.0f;

            direction.Normalize();

            mainCamera.position = fighter.position + direction * distance;

            mainCamera.LookAt(fighter);
        }
    }
}