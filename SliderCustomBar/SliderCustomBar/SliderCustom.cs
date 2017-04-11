using System;
using System.Drawing;
using System.Linq;
using CoreGraphics;
using UIKit;

namespace SliderCustomBar
{
	public class SliderCustom
	{
		private UISlider slider;
		private UISlider sliderBackground;
		private bool lockslider = false;

		public int SelectedValue { get; private set; }

		public string[] ItemsSource { get; private set; }

		public int BlockedItem { get; private set; }

		public SliderCustom(UISlider slider, UISlider sliderBackground, string[] itemsSource, int blockedItem = 0)
		{
			this.slider = slider;
			this.slider.MinValue = 0;
			this.slider.MaxValue = itemsSource.Count() - 1;
			this.ItemsSource = itemsSource;
			this.BlockedItem = blockedItem;
			this.slider.Value = blockedItem;
			this.slider.MinimumTrackTintColor = UIColor.LightGray;
			this.slider.MaximumTrackTintColor = UIColor.LightGray;

			this.sliderBackground = sliderBackground;
			this.sliderBackground = sliderBackground;
			this.sliderBackground.MinValue = slider.MinValue;
			this.sliderBackground.MaxValue = slider.MaxValue;
			this.sliderBackground.Value = slider.Value;
			this.sliderBackground.TintColor = UIColor.Clear;
			this.sliderBackground.MinimumTrackTintColor = UIColor.Clear;
			this.sliderBackground.MaximumTrackTintColor = UIColor.Clear;
			sliderBackground.Hidden = true;


			Slider_ValueChanged(null, null);
		}

		public void AddEvents()
		{
			slider.ValueChanged += Slider_ValueChanged;
			slider.TouchDown += Slider_TouchDragEnter;
			slider.TouchUpInside += Slider_TouchDragExit;
		}

		public void RemoveEvents()
		{
			slider.ValueChanged -= Slider_ValueChanged;
		}

		public void Disable()
		{
			slider.Enabled = false;
			ChangeSliderThumb(SelectedValue, false);
		}

		public void Enable()
		{
			slider.Enabled = true;
			ChangeSliderThumb(SelectedValue);
		}

		private void Slider_TouchDragEnter(object sender, EventArgs e)
		{
			sliderBackground.Alpha = 0;
			sliderBackground.Hidden = false;
			UIView.Animate(0.2, () => sliderBackground.Alpha = 1, null);
		}

		private void Slider_TouchDragExit(object sender, EventArgs e)
		{
			UIView.Animate(0.2, () => { sliderBackground.Alpha = 0; }, () => sliderBackground.Hidden = true);
		}

		private void Slider_ValueChanged(object sender, EventArgs e)
		{
			if (!lockslider) {
				lockslider = true;
				var x = slider.Value;

				if (x <= BlockedItem) {
					slider.Value = BlockedItem;
				} else {
					for (int i = 0; i < ItemsSource.Count(); i++) {
						if (x > i - 0.5 && x < i + 0.5) {
							slider.Value = i;
						}
					}
				}

				ChangeSliderThumb((int)slider.Value);
				SelectedValue = (int)slider.Value;
				lockslider = false;
			}

			sliderBackground.Value = slider.Value;
			ChangeSliderBackgroundThumb((int)slider.Value);
		}

		private void ChangeSliderThumb(int position, bool IsEnable = true)
		{
			var img = UIImage.FromFile(ItemsSource[position]);
			if (!IsEnable) {
				img = ConvertGrayScale(img);
			}
			slider.SetThumbImage(img, UIControlState.Normal);
		}

		private void ChangeSliderBackgroundThumb(int position)
		{
			var img = UIImage.FromFile(ItemsSource[position]);
			sliderBackground.SetThumbImage(img, UIControlState.Normal);
		}

		private UIImage ConvertGrayScale(UIImage image)
		{
			RectangleF imageRect = new RectangleF(PointF.Empty, new SizeF((float)image.Size.Width, (float)image.Size.Height));
			using (var colorSpace = CGColorSpace.CreateDeviceGray())
			using (var context = new CGBitmapContext(IntPtr.Zero, (int)imageRect.Width, (int)imageRect.Height, 8, 0, colorSpace, CGImageAlphaInfo.None)) {
				context.DrawImage(imageRect, image.CGImage);
				using (var imageRef = context.ToImage())
					return new UIImage(imageRef);
			}
		}



	}
}
