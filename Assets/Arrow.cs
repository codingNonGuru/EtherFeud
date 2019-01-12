using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace EtherGame
{
	public class Arrow : MonoBehaviour 
	{
		[SerializeField]
		GameObject bloodSprayPrefab = null;
		
		Rigidbody rigidBody = null;

		float impulse = 0.0f;

		float lifeTime = 0.0f;

		bool isFlying = false;

		public float Impulse 
		{
			set {impulse = value;}
		}

		void Start()
		{
			rigidBody = GetComponent<Rigidbody>();

			rigidBody.AddForce(transform.forward * impulse);

			isFlying = true;
		}

		void Update()
		{
			lifeTime += Time.deltaTime;

			if(lifeTime > 3.0f && isFlying)
			{
				Destroy(gameObject);
			}
		}

		void OnCollisionEnter(Collision collision)
		{
			var fighter = collision.gameObject.GetComponent<Fighter>();
			if(fighter == null)
				return;

			var bloodSprayInstance = Instantiate(bloodSprayPrefab, transform.position, Quaternion.identity);
			
			var bloodSpray = bloodSprayInstance.GetComponent<ParticleSystem>();
			if(bloodSpray == null)
				return;

			isFlying = false;

			rigidBody.isKinematic = true;

			transform.SetParent(collision.transform);

			foreach(var contactPoint in collision.contacts)
			{
				bloodSpray.transform.forward = contactPoint.normal;
				break;
			}
		}
	}
}
