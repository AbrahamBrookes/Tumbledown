using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tumbledown {

	/**
	 * The PlayerCamera script is responsible for the in-game camera. The camera is a top down
	 * rpg style camera that follows the player around. If there is no camera on scene load, this
	 * script will create one.
	 * 
	 * The camera is a child of the player, and is offset by a distance and height. The camera
	 * does not rotate - the player controller will rotate and walk around but the camera will
	 * always be looking down at the player.
	 */
	
	public class PlayerCamera : MonoBehaviour
	{
		[SerializeField] private Transform _target = default;

		[SerializeField] private float _distance = 4.5f;

		[SerializeField] private float _height = 5f;

		[SerializeField] private float _damping = 5f;

		[SerializeField] private float _lookDownAngle = 45f;

		[SerializeField] private Camera _camera = default;

		void Start()
		{
			// if we don't have a camera, create one and make it the scene camera
			if (_camera == null)
			{
				_camera = new GameObject("Player Camera").AddComponent<Camera>();
				_camera.tag = "MainCamera";

				// set the camera to be a child of the player
				_camera.transform.parent = transform;

				// set the camera position to be offset from the player
				_camera.transform.localPosition = new Vector3(0f, _height, -_distance);

				// look the camera down
				_camera.transform.localRotation = Quaternion.Euler(_lookDownAngle, 0f, 0f);
			}			
		}
	}
}
