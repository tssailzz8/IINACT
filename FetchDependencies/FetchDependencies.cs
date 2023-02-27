using System.IO.Compression;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace FetchDependencies {
    public class FetchDependencies {
        private const string UserAgent =
            "IINACT";

        public string DependenciesDir { get; }

        public FetchDependencies() {
            var assemblyDir = AppDomain.CurrentDomain.BaseDirectory;
            DependenciesDir = Path.Combine(assemblyDir, "external_dependencies");
        }

        public async Task GetFfxivPlugin() {
			Directory.CreateDirectory(DependenciesDir);
			var pluginZipPath = Path.Combine(DependenciesDir, "FFXIV_ACT_Plugin.zip");
			var pluginPath = Path.Combine(DependenciesDir, "FFXIV_ACT_Plugin.dll");

			if (!await NeedsUpdate(pluginPath))
				return;

			if (!File.Exists(pluginPath))
				await DownloadPlugin(pluginPath);

			// ZipFile.ExtractToDirectory(pluginZipPath, DependenciesDir, overwriteFiles: true);
			//File.Delete(pluginPath);

			var patcher = new Patcher(DependenciesDir);
			patcher.MainPlugin();
			patcher.LogFilePlugin();
			patcher.MemoryPlugin();
		}

        private static async Task<bool> NeedsUpdate(string dllPath) {
			return !File.Exists(dllPath);

		}

        private static async Task DownloadPlugin(string path) {
			var client = new HttpClient();
			client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

			client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");//Set the User Agent to "request"
			client.GetStringAsync("https://api.github.com/repos/TundraWork/FFXIV_ACT_Plugin_CN/releases")
				.ContinueWith(ExtractOpCode).Wait();
			//         string url = "";
			//HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://github.com/TundraWork/FFXIV_ACT_Plugin_CN/releases/latest");
			//req.Method = "HEAD";
			//req.AllowAutoRedirect = false;
			//HttpWebResponse myResp = (HttpWebResponse)req.GetResponse();
			//if (myResp.StatusCode == HttpStatusCode.Redirect)
			//	url = myResp.GetResponseHeader("Location");
			//var bcd = url.Split(@"/")[7];
			var httpClient = new HttpClient();
			await using var downloadStream = await httpClient.GetStreamAsync(downURL);
			//await using var downloadStream = await httpClient.GetStreamAsync($"https://github.com/TundraWork/FFXIV_ACT_Plugin_CN/releases/download/{bcd}/FFXIV_ACT_Plugin.dll");
			await using var zipFileStream = new FileStream(path, FileMode.Create);
			await downloadStream.CopyToAsync(zipFileStream);
			zipFileStream.Close();
		}
		public static string downURL;
		private static void ExtractOpCode(Task<string> task)
		{
			try
			{
				var get = JsonConvert.DeserializeObject<List<FFXIV_ACT_PluginJson>>(task.Result);
				get.OrderBy(i => i.published_at);
				downURL = get[0].assets[0].browser_download_url;

			}
			catch (Exception)
			{

				throw;
			}
		}

	}
	public class FFXIV_ACT_PluginJson
	{
		public string? tag_name { get; set; }
		public DateTime? published_at { get; set; }
		public List<Assets> assets { get; set; }
	}
	public class Assets
	{
		public string? browser_download_url { get; set; }
	}
}
