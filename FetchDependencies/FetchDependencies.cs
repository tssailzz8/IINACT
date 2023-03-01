using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
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

			if (!await NeedsUpdate(DependenciesDir))
				return;

			if (!File.Exists(pluginPath))
				await DownloadPlugin(DependenciesDir);

			// ZipFile.ExtractToDirectory(pluginZipPath, DependenciesDir, overwriteFiles: true);
			//File.Delete(pluginPath);

			var patcher = new Patcher(DependenciesDir);
			patcher.MainPlugin();
			patcher.LogFilePlugin();
			patcher.MemoryPlugin();
		}

        private static async Task<bool> NeedsUpdate(string dllPath) {
			var httpClient = new HttpClient();
			var txtPath = Path.Combine(dllPath, "版本.txt");
			if (File.Exists(txtPath))
			{
				using var txt = new StreamReader(txtPath);
				var nowVerson = new Version(txt.ReadToEnd());
				var textStream = await httpClient.GetStringAsync("https://cninact.diemoe.net/CN解析/版本.txt");
				var remoteVersion = new Version(textStream);
				return remoteVersion > nowVerson;

			}
			else
			{
				await DownloadPlugin(dllPath);
				return true;
			};
		}

        private static async Task DownloadPlugin(string path) {
			//var client = new HttpClient();
			//client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

			//client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");//Set the User Agent to "request"
			//client.GetStringAsync("https://api.github.com/repos/TundraWork/FFXIV_ACT_Plugin_CN/releases")
			//	.ContinueWith(ExtractOpCode).Wait();
			//         string url = "";
			//HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://github.com/TundraWork/FFXIV_ACT_Plugin_CN/releases/latest");
			//req.Method = "HEAD";
			//req.AllowAutoRedirect = false;
			//HttpWebResponse myResp = (HttpWebResponse)req.GetResponse();
			//if (myResp.StatusCode == HttpStatusCode.Redirect)
			//	url = myResp.GetResponseHeader("Location");
			//var bcd = url.Split(@"/")[7];
			var pluginPath = Path.Combine(path, "FFXIV_ACT_Plugin.dll");
			var txtinPath = Path.Combine(path, "版本.txt");
			var httpClient = new HttpClient();
			await using var downloadStream = await httpClient.GetStreamAsync("https://cninact.diemoe.net/CN解析/FFXIV_ACT_Plugin.dll\r\n");
			await using var textStream = await httpClient.GetStreamAsync("https://cninact.diemoe.net/CN解析/版本.txt");
			//await using var downloadStream = await httpClient.GetStreamAsync($"https://github.com/TundraWork/FFXIV_ACT_Plugin_CN/releases/download/{bcd}/FFXIV_ACT_Plugin.dll");
			await using var zipFileStream = new FileStream(pluginPath, FileMode.Create);
			await downloadStream.CopyToAsync(zipFileStream);
			zipFileStream.Close();
			await using var zipFileStream1 = new FileStream(txtinPath, FileMode.Create);
			await textStream.CopyToAsync(zipFileStream1);
			zipFileStream1.Close();
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
