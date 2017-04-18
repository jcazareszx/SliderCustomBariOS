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
			var img1 = UIImage.FromFile(ItemsSource[position]);
			var img = DrawText(img1, "Text" + position.ToString(), UIColor.Blue, 18);
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

		private UIImage DrawText(UIImage uiImage, string sText, UIColor textColor, int iFontSize)
		{
			nfloat fWidth = uiImage.Size.Width;
			nfloat fHeight = uiImage.Size.Height;

			uiImage = GetColoredImage(uiImage, UIColor.White);

			CGColorSpace colorSpace = CGColorSpace.CreateDeviceRGB();

			using (CGBitmapContext ctx = new CGBitmapContext(IntPtr.Zero, (nint)fWidth, (nint)fHeight, 8, 4 * (nint)fWidth, CGColorSpace.CreateDeviceRGB(), CGImageAlphaInfo.PremultipliedFirst)) {
				ctx.DrawImage(new CGRect(0, 0, (double)fWidth, (double)fHeight), uiImage.CGImage);

				ctx.SelectFont("Helvetica", iFontSize, CGTextEncoding.MacRoman);

				//Measure the text's width - This involves drawing an invisible string to calculate the X position difference
				float start, end, textWidth;

				//Get the texts current position
				start = (float)ctx.TextPosition.X;
				//Set the drawing mode to invisible
				ctx.SetTextDrawingMode(CGTextDrawingMode.Invisible);
				//Draw the text at the current position
				ctx.ShowText(sText);
				//Get the end position
				end = (float)ctx.TextPosition.X;
				//Subtract start from end to get the text's width
				textWidth = end - start;

				nfloat fRed;
				nfloat fGreen;
				nfloat fBlue;
				nfloat fAlpha;
				//Set the fill color to black. This is the text color.
				textColor.GetRGBA(out fRed, out fGreen, out fBlue, out fAlpha);
				ctx.SetFillColor(fRed, fGreen, fBlue, fAlpha);

				//Set the drawing mode back to something that will actually draw Fill for example
				ctx.SetTextDrawingMode(CGTextDrawingMode.Fill);

				//Draw the text at given coords.
				ctx.ShowTextAtPoint((int)(((double)fWidth / 2) - (textWidth / 2)), 17, sText);

				return UIImage.FromImage(ctx.ToImage());
			}
		}

		private UIImage GetColoredImage(UIImage image, UIColor color)
		{
			UIImage coloredImage = null;

			UIGraphics.BeginImageContext(image.Size);
			using (CGContext context = UIGraphics.GetCurrentContext()) {

				context.TranslateCTM(0, image.Size.Height);
				context.ScaleCTM(1.0f, -1.0f);

				var rect = new RectangleF(0, 0, (float)image.Size.Width, (float)image.Size.Height);

				// draw image, (to get transparancy mask)
				context.SetBlendMode(CGBlendMode.Normal);
				context.DrawImage(rect, image.CGImage);

				// draw the color using the sourcein blend mode so its only draw on the non-transparent pixels
				context.SetBlendMode(CGBlendMode.SourceIn);
				context.SetFillColor(color.CGColor);
				context.FillRect(rect);

				coloredImage = UIGraphics.GetImageFromCurrentImageContext();
				UIGraphics.EndImageContext();
			}
			return coloredImage;
		}

	}
}
