using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HoverSystem
{
	/// <summary>
	/// Script that allows for hovering over a <see cref="GameObject"/> with a <see cref="MonoBehaviour"/> that implements <see cref="IMouseOver"/>.
	/// <remarks>
	/// Needs to be attached to a <see cref="Camera"/>.
	/// </remarks>
	/// </summary>
	[RequireComponent(typeof(Camera))]
	public class HoverOverCamera : MonoBehaviour
	{
		private Camera mainCamera;
		private Transform hoveringOver;
		private List<IMouseOver> mouseOvers = new();

		private void Awake()
		{
			mainCamera = GetComponent<Camera>();

			if (!mainCamera)
			{
				Debug.LogWarning($"{typeof(HoverOverCamera)} should be attached to a {typeof(Camera)}!");
			}
		}

		private void FixedUpdate()
		{
			Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
			
			if (Physics.Raycast (ray, out RaycastHit hit, 100))
			{
				if (hit.transform == hoveringOver) return;

				hoveringOver = hit.transform;
				mouseOvers.ForEach(pOver =>
				{
					pOver.IsHovering = false;
					pOver.OnEndHover();
				});
				
				mouseOvers = hit.transform.GetComponents<IMouseOver>().ToList();
				
				mouseOvers.ForEach(pOver =>
				{
					pOver.IsHovering = true;
					pOver.OnStartHover();
				});
			}
		}
	}
}