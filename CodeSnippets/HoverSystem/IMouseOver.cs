using UnityEngine;

namespace HoverSystem
{
	/// <summary>
	/// Can be implemented on a <see cref="MonoBehaviour"/> to make use of <see cref="HoverOverCamera"/>.
	/// </summary>
	public interface IMouseOver
	{
		/// <summary>
		/// Is the mouse currently hovering over the <see cref="Collider"/>
		/// </summary>
		public bool IsHovering { get; set; }
		
		/// <summary>
		/// Called when the mouse starts hovering over the <see cref="Collider"/>
		/// </summary>
		public void OnStartHover();
		
		/// <summary>
		/// Called when the mouse stops hovering over the <see cref="Collider"/>
		/// </summary>
		public void OnEndHover();
	}
}