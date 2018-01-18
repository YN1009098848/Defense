using System;

namespace CoreEngine.InputRef
{

	public class InputActionID {
		public const int IA_NONE = 0;
		public const int IA_ONEPOINT_CLICK = 1;
		public const int IA_ONEPOINT_DOUBLECLICK = 2;
		public const int IA_ONEPOINT_DRAG = 3;
		public const int IA_ONEPOINT_DRAG_EXIT = 4;
		public const int IA_ONEPOINT_PRESS = 5;
		public const int IA_ONEPOINT_EXIT = 6;
		public const int IA_TWOPOINT_CLICK = 10;
		public const int IA_TWOPOINT_DOUBLECLICK = 11;
		public const int IA_TWOPOINT_DRAG = 12;
		public const int IA_TWOPOINT_DRAG_EXIT = 13;
		public const int IA_TWOPOINT_EXIT = 14;
		public const int IA_THREE_CLICK = 20;
	}
	
	public enum InputActionPrior {
		INPUT_FIRST = 0,
		INPUT_UI = 1,
		INPUT_OP = 2,
		INPUT_UIAVI = 3,
    }
}
