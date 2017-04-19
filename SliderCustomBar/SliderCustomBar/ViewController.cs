using System;

using UIKit;

namespace SliderCustomBar
{
	public partial class ViewController : UIViewController
	{

		SliderCustom sCustom;

		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			string[] imagesArray = new string[] {"sliderthumb1.png","sliderthumb2.png",
										"sliderthumb3.png","sliderthumb4.png","sliderthumb5.png",
										"sliderthumb6.png","sliderthumb7.png" };

			sCustom = new SliderCustom(slider, sliderSecond, imagesArray, imagesArray, UIColor.Green, 18, 15);
			sCustom.AddEvents();

			btnSelectedValue.TouchUpInside += BtnSelectedValue_TouchUpInside;
		}

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);
			sCustom.RemoveEvents();
		}


		private void BtnSelectedValue_TouchUpInside(object sender, EventArgs e)
		{
			btnSelectedValue.SetTitle(sCustom.SelectedValue.ToString(), UIControlState.Normal);
			sCustom.Disable();
		}
	}
}
