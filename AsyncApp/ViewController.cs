using System;
using UIKit;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using AVFoundation;

namespace AsyncApp
{
	public class ViewController : UITableViewController
	{
		Thread uiThread;

		HttpClient client = new HttpClient();
		List<Song> Songs = new List<Song>();

		static int refreshCount = 0;
		Task refreshTask;

		public ViewController ()
		{
			uiThread = Thread.CurrentThread;
			WhatThreadAmI ();
		}

		public override async void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			await Refresh ();
		}


		async Task Refresh ()
		{
			refreshCount++;

			WhatThreadAmI ();

			Songs = await DownloadSongs ();

			WhatThreadAmI ();

			this.TableView.ReloadData ();

			refreshCount--;
		}


		async Task<List<Song>> DownloadSongs ()
		{
			var json = await client.GetStringAsync("https://dl.dropboxusercontent.com/s/cv75h76pv9su7l4/songs-small.json"); //ca

			WhatThreadAmI ();

			return await ParseJson(json);

			//return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Song>> (json);
		}

		async Task<List<Song>> ParseJson (string json)
		{
			WhatThreadAmI ();

			return await Task.Run (() => 
			{
				WhatThreadAmI ();
				return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Song>> (json);
			});
		}


		void WhatThreadAmI ([CallerMemberName] string method = "",[CallerLineNumber] int line = 0)
		{
			Console.WriteLine ($"{method} - RefreshCount!!! {refreshCount} - On Main Thread = {IsMainThread}");
		}

		public bool IsMainThread => uiThread == Thread.CurrentThread;

		public override nint RowsInSection (UITableView tableView, nint section)
		{
			return Songs?.Count ?? 0;
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell ("Cell") ??
								new UITableViewCell (UITableViewCellStyle.Subtitle, "Cell");
			
			var song = Songs [indexPath.Row];
			cell.TextLabel.Text = song.Title;
			cell.DetailTextLabel.Text = song.Artist;

			return cell;
		}
	}

	public class Song
	{
		public string Artist { get; set; }
		public DateTime Timestamp { get; set; }
		public string TrackId { get; set; }
		public string Title { get; set; }
	}
}

