using UnityEngine;
using System;

namespace CoreEngine.InputRef
{

	/** this is the "one point press" action
*/
	class IA_OnePointPress : InputAction {
		/** the screen pos pressed */
		public Vector2 pos;
		
		public IA_OnePointPress() {
			base.id = InputActionID.IA_ONEPOINT_PRESS;
		}  
	}
	
	/** this is the "one point click" action
*/
	class IA_OnePointClick : InputAction {
		/** the screen pos clicked */
		public Vector2 pos;
		
		public IA_OnePointClick() {
			base.id = InputActionID.IA_ONEPOINT_CLICK;
		}
	}


	class IA_OnePointExit : InputAction {
		public Vector2 pos;
		public IA_OnePointExit()
		{
			base.id = InputActionID.IA_ONEPOINT_EXIT;
		}
	}
	
	/** this is the "one point double click" action
*/
	class IA_OnePointDoubleClick : InputAction {
		/** the screen pos clicked */
		public Vector2 pos;
		
		public IA_OnePointDoubleClick() {
			base.id = InputActionID.IA_ONEPOINT_DOUBLECLICK;
		}
	}
	
	/** this is the "one point drag" action
*/
	class IA_OnePointDrag : InputAction {
		/** the mouse pos in screen space */
		public Vector2 pos;
		
		/** the delta pos in screen space */
		public Vector2 deltaPos;
		
		/** the delta time dragging */
		public float deltaTime;
		
		public IA_OnePointDrag() {
			base.id = InputActionID.IA_ONEPOINT_DRAG;
		}
	}
	
	/** this is the "one point drag exit" action
*/
	class IA_OnePointDragExit : InputAction {
		/** the mouse pos in screen space */
		public Vector2 pos;
		
		public IA_OnePointDragExit() {
			base.id = InputActionID.IA_ONEPOINT_DRAG_EXIT;
		}
	}
	
	/** this is the "two point click" action
*/
	class IA_TwoPointClick : InputAction {
		/** the 1st pos in screen space */
		public Vector2 pos0;
		
		/** the 2nd pos in screen space */
		public Vector2 pos1;
		
		public IA_TwoPointClick() {
			base.id = InputActionID.IA_TWOPOINT_CLICK;
		}
	}
	
	/** this is the "two point double click" action
*/
	class IA_TwoPointDoubleClick : InputAction {
		/** the 1st pos in screen space */
		public Vector2 pos0;
		
		/** the 2nd pos in screen space */
		public Vector2 pos1;
		
		public IA_TwoPointDoubleClick() {
			base.id = InputActionID.IA_TWOPOINT_DOUBLECLICK;
		}
	}

	class IA_TwoPointExit : InputAction {
		public Vector2 pos0;
		public Vector2 pos1;
		public IA_TwoPointExit()
		{
			base.id = InputActionID.IA_TWOPOINT_EXIT;
		}
	}

	
	/** this is the "two point drag" action
*/
	class IA_TwoPointDrag : InputAction {
		/** the 1st screen pos */
		public Vector2 pos0;
		
		/** the 2nd screen pos */
		public Vector2 pos1;
		
		/** the 1st delta screen pos */
		public Vector2 deltaPos0;
		
		/** the 2nd delta screen pos */
		public Vector2 deltaPos1;
		
		/** the 1st delta time */
		public float deltaTime0;
		
		/** the 2nd delta time */
		public float deltaTime1;
		
		public IA_TwoPointDrag() {
			base.id = InputActionID.IA_TWOPOINT_DRAG;
		}
	}
	
	/** the "two point drag exit" action
*/
	class IA_TwoPointDragExit : InputAction {
		/** the 1st screen pos */
		public Vector2 pos0;
		
		/** the 2nd screen pos */
		public Vector2 pos1;
		
		public IA_TwoPointDragExit() {
			base.id = InputActionID.IA_TWOPOINT_DRAG_EXIT;
		}
	}
	
	class IA_ShowDebugTool : InputAction {
		public IA_ShowDebugTool() {
			base.id = InputActionID.IA_THREE_CLICK;
		}
	}
}
