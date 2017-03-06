// Adapted from : https://github.com/android/platform_development/tree/master/samples/devbytes/animation/PictureViewer

// Watch here: https://www.youtube.com/watch?v=9XbKMUtVnJA

using Android.App;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Android.Graphics.Drawables;
using Java.Lang;
using System.Threading;
using System;

namespace PictureViewerWithFade
{
	[Activity(Label = "PictureViewerWithFade", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		private Timer timer;

		int _currentDrawableNo = 0;
		int[] drawableIDs = {
			Resource.Drawable.p1,
			Resource.Drawable.p2,
			Resource.Drawable.p3,
		};

		ImageView prevImageView;
		ImageView nextImageView;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			prevImageView = (ImageView)FindViewById(Resource.Id.prevImageView);
			nextImageView = (ImageView)FindViewById(Resource.Id.nextImageView);

			prevImageView.Animate().SetDuration(200);
			prevImageView.Animate().SetDuration(200);

			var drawables = new BitmapDrawable[drawableIDs.Length];

			for (int i = 0; i < drawableIDs.Length; ++i)
			{
				var bitmap = BitmapFactory.DecodeResource(Resources, drawableIDs[i]);
				drawables[i] = new BitmapDrawable(Resources, bitmap);
			}

			prevImageView.SetImageDrawable(drawables[0]);
			nextImageView.SetImageDrawable(drawables[1]);

			timer = new Timer(x => UpdateView(drawables), null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
		}

		private void UpdateView(BitmapDrawable[] drawables)
		{
			this.RunOnUiThread(() =>
			{
				prevImageView.Animate().Alpha(0).WithLayer();
				nextImageView.Animate().Alpha(1).WithLayer().WithEndAction(new Runnable(() =>
				 {
					 // When the animation ends, set up references to change the prev/next
					 // associations
					 _currentDrawableNo = (_currentDrawableNo + 1) % drawables.Length;
					 int nextDrawableIndex = (_currentDrawableNo + 1) % drawables.Length;
					 prevImageView.SetImageDrawable(drawables[_currentDrawableNo]);
					 nextImageView.SetImageDrawable(drawables[nextDrawableIndex]);
					 nextImageView.Alpha = 0f;
					 prevImageView.Alpha = 1f;
				 }));
			});
		}

	}
}

