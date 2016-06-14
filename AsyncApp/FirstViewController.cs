using System;

using UIKit;

namespace AsyncApp
{
	public partial class FirstViewController : UIViewController
	{
		public FirstViewController() : base("FirstViewController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			UIButton button = new UIButton(new CoreGraphics.CGRect(0, 0, 50, 50));
			button.SetTitle("Test", UIControlState.Normal);
			button.SetTitleColor(UIColor.Red, UIControlState.Normal);

			button.TouchUpInside += (sender, e) =>
			{
				NavigationController.PushViewController(new SecondViewController(), false);
			};

			View.AddSubview(button);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


