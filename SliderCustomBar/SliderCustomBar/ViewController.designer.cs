// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace SliderCustomBar
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UIButton btnSelectedValue { get; set; }

		[Outlet]
		UIKit.UISegmentedControl segCustom { get; set; }

		[Outlet]
		UIKit.UISlider slider { get; set; }

		[Outlet]
		UIKit.UISlider sliderSecond { get; set; }

		[Outlet]
		UIKit.UISlider slidetCustom { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnSelectedValue != null) {
				btnSelectedValue.Dispose ();
				btnSelectedValue = null;
			}

			if (segCustom != null) {
				segCustom.Dispose ();
				segCustom = null;
			}

			if (slider != null) {
				slider.Dispose ();
				slider = null;
			}

			if (slidetCustom != null) {
				slidetCustom.Dispose ();
				slidetCustom = null;
			}

			if (sliderSecond != null) {
				sliderSecond.Dispose ();
				sliderSecond = null;
			}
		}
	}
}
