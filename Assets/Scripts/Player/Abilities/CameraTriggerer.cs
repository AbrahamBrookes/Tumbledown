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

		ResetCamera();
        
    }

	// when we enter a trigger, crunch the enum and route the call
	private void OnTriggerEnter(Collider other)
	{
		// see if the other collider has a tag
		if (other.tag == null)
		{
			return;
		}
		
		// see if the other collider's tag is in our enum
		if (!System.Enum.IsDefined(typeof(CameraTriggerType), other.tag))
		{
			return;
		}

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
		// if we don't have a virtual camera, don't do anything
		if (_virtualCamera == null)
		{
			return;
		}

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
		// if we don't have a virtual camera, don't do anything
		if (_virtualCamera == null)
		{
			return;
		}

		// reset the virtual camera's 3rd person follow Camera Distance setting to its original value
		DOTween.To(() => _virtualCamera.GetCinemachineComponent<Cinemachine.Cinemachine3rdPersonFollow>().CameraDistance, x => _virtualCamera.GetCinemachineComponent<Cinemachine.Cinemachine3rdPersonFollow>().CameraDistance = x, _originalCameraDistance, 1f);
	}
}
