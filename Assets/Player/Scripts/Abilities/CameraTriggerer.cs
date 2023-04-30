using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/**
 * This script is assigned to the Protagonist, and handles moving the camera when the player passes
  * through a trigger that is tagged with an enum value from: PeekCameraTrigger, ZoomCameraTrigger
  *
  * We'll check the tage against our enum and then call the appropriate method from here.
  */

public class CameraTriggerer : MonoBehaviour
{
	// the camera we will control - a cinemachine virtual camera
	[SerializeField] private Cinemachine.CinemachineVirtualCamera _virtualCamera = default;

	// cache the camera's original settings
	[SerializeField] private float _originalCameraDistance;

	// our enum
	public enum CameraTriggerType
	{
		PeekCameraTrigger,
		ZoomCameraTrigger
	}

    // Start is called before the first frame update
    void Start()
    {
		// if we don't have a virtual camera, find one
		if (_virtualCamera == null)
		{
			_virtualCamera = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
		}
		
		// if we still don't have one, log an error
		if (_virtualCamera == null)
		{
			Debug.LogError("No virtual camera found in scene - please add one and assign it to the CameraTriggerer script on the Protagonist.");
		}

		ResetCamera();
        
    }

	// when we enter a trigger, crunch the enum and route the call
	private void OnTriggerEnter(Collider other)
	{
		// get the trigger type from the tag
		CameraTriggerType triggerType = (CameraTriggerType)System.Enum.Parse(typeof(CameraTriggerType), other.tag);

		// route the call
		switch (triggerType)
		{
			case CameraTriggerType.PeekCameraTrigger:
				PeekCameraTrigger(other);
				break;
			case CameraTriggerType.ZoomCameraTrigger:
				ZoomCameraTrigger(other);
				break;
			default:
				break;
		}
	}

	// when we exit a trigger, reset the camera
	private void OnTriggerExit(Collider other)
	{
		ResetCamera();
	}

	// this method will be called when we enter a PeekCameraTrigger
	private void PeekCameraTrigger(Collider other)
	{
		// lerp the virtual camera's 3rd person follow Camera Distance setting to -1 using dotween
		DOTween.To(() => _virtualCamera.GetCinemachineComponent<Cinemachine.Cinemachine3rdPersonFollow>().CameraDistance, x => _virtualCamera.GetCinemachineComponent<Cinemachine.Cinemachine3rdPersonFollow>().CameraDistance = x, -1, 1f);
		
	}

	// this method will be called when we enter a ZoomCameraTrigger
	private void ZoomCameraTrigger(Collider other)
	{

	}

	// this method will be called when we exit any trigger
	private void ResetCamera()
	{
		// reset the virtual camera's 3rd person follow Camera Distance setting to its original value
		DOTween.To(() => _virtualCamera.GetCinemachineComponent<Cinemachine.Cinemachine3rdPersonFollow>().CameraDistance, x => _virtualCamera.GetCinemachineComponent<Cinemachine.Cinemachine3rdPersonFollow>().CameraDistance = x, _originalCameraDistance, 1f);
	}
}
