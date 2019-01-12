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

		float speed = 0.3f;

		float turnSpeed = 2.0f;

		float lastArrowTimer = 0.0f;

		float arrowShootInterval = 1.0f;
		
		void Start()
		{
			
		}

		void Update()
		{
			lastArrowTimer += Time.deltaTime;

			if(controller == null)
				return;

			controller.Refresh();

			if(controller.IsDoing(Actions.MOVE_FORWARD))
			{
				transform.position += transform.forward * speed;
			}
			else if(controller.IsDoing(Actions.MOVE_BACKWARDS))
			{
				transform.position -= transform.forward * speed;
			}

			if(controller.IsDoing(Actions.TURN_LEFTWARDS))
			{
				transform.Rotate(transform.up, -turnSpeed);
			}
			else if(controller.IsDoing(Actions.TURN_RIGHTWARDS))
			{
				transform.Rotate(transform.up, turnSpeed);
			}

			if(controller.IsDoing(Actions.SHOOT) && lastArrowTimer > arrowShootInterval)
			{
				Shoot();	

				/*if(arrowPrefab == null)
					return;

				var arrowPosition = transform.position + transform.up + transform.forward;
				var arrowInstance = Instantiate(arrowPrefab, arrowPosition, transform.rotation);*/
			}
		}

		public void Initialize(Controller newController)
		{
			controller = newController;
		}

		void Shoot()
		{
			if(arrowPrefab == null)
				return;

			var arrowPosition = transform.position + transform.up * 0.2f + transform.forward;
			var arrowInstance = Instantiate(arrowPrefab, arrowPosition, transform.rotation);
			if(arrowInstance == null)
				return;

			//arrowInstance.transform.Rotate(arrowInstance.transform.right, -50.0f);
			arrowInstance.transform.LookAt(transform);
			arrowInstance.transform.forward *= -1.0f;

			var arrow = arrowInstance.GetComponent<Arrow>();
			if(arrow == null)
				return;

			arrow.Impulse = 4000.0f;

			lastArrowTimer = 0.0f;
		}
	}
}
