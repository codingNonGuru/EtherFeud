using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace EtherGame
{
	public class Fighter : MonoBehaviour 
	{
		[SerializeField]
		GameObject arrowPrefab = null;

		Controller controller = null;

		float speed = 0.1f;

		float turnSpeed = 0.5f;

		float lastArrowTimer = 0.0f;

		float arrowShootInterval = 1.0f;
		
		void Start()
		{
			controller = new Controller();
		}

		void Update()
		{
			lastArrowTimer += Time.deltaTime;

			controller.Refresh();

			if(controller.IsDoing(FighterActions.MOVE_FORWARD))
			{
				transform.position += transform.forward * speed;
			}

			if(controller.IsDoing(FighterActions.TURN_LEFTWARDS))
			{
				transform.Rotate(transform.up, -turnSpeed);
			}
			else if(controller.IsDoing(FighterActions.TURN_RIGHTWARDS))
			{
				transform.Rotate(transform.up, turnSpeed);
			}

			if(controller.IsDoing(FighterActions.SHOOT) && lastArrowTimer > arrowShootInterval)
			{
				Shoot();	

				/*if(arrowPrefab == null)
					return;

				var arrowPosition = transform.position + transform.up + transform.forward;
				var arrowInstance = Instantiate(arrowPrefab, arrowPosition, transform.rotation);*/
			}
		}

		void Shoot()
		{
			if(arrowPrefab == null)
				return;

			var arrowPosition = transform.position + transform.up * 0.3f + transform.forward;
			var arrowInstance = Instantiate(arrowPrefab, arrowPosition, transform.rotation);
			if(arrowInstance == null)
				return;

			//arrowInstance.transform.Rotate(arrowInstance.transform.right, -50.0f);
			arrowInstance.transform.LookAt(transform);
			arrowInstance.transform.forward *= -1.0f;

			var arrow = arrowInstance.GetComponent<Arrow>();
			if(arrow == null)
				return;

			arrow.Impulse = 2000.0f;

			lastArrowTimer = 0.0f;
		}
	}
}
