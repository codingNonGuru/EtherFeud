using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace EtherGame
{
	public class Arrow : MonoBehaviour 
	{
		Rigidbody rigidBody = null;

		float impulse = 0.0f;

		float lifeTime = 0.0f;

		public float Impulse 
		{
			set {impulse = value;}
		}

		void Start()
		{
			rigidBody = GetComponent<Rigidbody>();

			rigidBody.AddForce(transform.forward * impulse);
		}

		void Update()
		{
			lifeTime += Time.deltaTime;

			if(lifeTime > 5.0f)
			{
				Destroy(gameObject);
			}
		}
	}
}
