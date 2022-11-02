using System.IO.Compression;
using System.Net;

namespace FetchDependencies {
    public class FetchDependencies {
        private const string ActUserAgent =
            "ACT-Parser (v3.6.1     Release: 277 | .NET v4.8+ (533325) | OS Microsoft Windows NT 10.0.22000.0)";

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
            if (!File.Exists(dllPath)) return true;
            try {
                using var plugin = new TargetAssembly(dllPath);
                var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(1);
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(ActUserAgent);
                var remoteVersionString =
                    await httpClient.GetStringAsync("https://advancedcombattracker.com/versioncheck/pluginversion/73");
                var remoteVersion = new Version(remoteVersionString);
                return remoteVersion > plugin.Version;
            }
            catch {
                return false;
            }
        }

        private static async Task DownloadPlugin(string path) {
			string url = "";
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://github.com/TundraWork/FFXIV_ACT_Plugin_CN/releases/latest");
			req.Method = "HEAD";
			req.AllowAutoRedirect = false;
			HttpWebResponse myResp = (HttpWebResponse)req.GetResponse();
			if (myResp.StatusCode == HttpStatusCode.Redirect)
				url = myResp.GetResponseHeader("Location");
			var bcd = url.Split(@"/")[7];
			var httpClient = new HttpClient();
			await using var downloadStream = await httpClient.GetStreamAsync($"https://github.com/TundraWork/FFXIV_ACT_Plugin_CN/releases/download/{bcd}/FFXIV_ACT_Plugin.dll");
			await using var zipFileStream = new FileStream(path, FileMode.Create);
			await downloadStream.CopyToAsync(zipFileStream);
			zipFileStream.Close();

		}
    }
}
