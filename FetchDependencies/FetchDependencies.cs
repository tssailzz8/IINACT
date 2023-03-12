using System.IO.Compression;

namespace FetchDependencies {
    public class FetchDependencies {
        private const string UserAgent =
            "IINACT";

        public string DependenciesDir { get; }

        public FetchDependencies() {
            Environment.SetEnvironmentVariable("DOTNET_SYSTEM_NET_HTTP_SOCKETSHTTPHANDLER_HTTP3SUPPORT", "0");
            var assemblyDir = AppDomain.CurrentDomain.BaseDirectory;
            DependenciesDir = Path.Combine(assemblyDir, "external_dependencies");
        }

        public async Task GetFfxivPlugin() {
            Directory.CreateDirectory(DependenciesDir);
            var pluginZipPath = Path.Combine(DependenciesDir, "FFXIV_ACT_Plugin.zip");
            var pluginPath = Path.Combine(DependenciesDir, "FFXIV_ACT_Plugin.dll");

            if (!await NeedsUpdate(DependenciesDir))
                return;

            if (!File.Exists(pluginZipPath))
                await DownloadPlugin(DependenciesDir);

            //ZipFile.ExtractToDirectory(pluginZipPath, DependenciesDir, overwriteFiles: true);
            //File.Delete(pluginZipPath);

            var patcher = new Patcher(DependenciesDir);
            patcher.MainPlugin();
            patcher.LogFilePlugin();
            patcher.MemoryPlugin();
        }

        private static async Task<bool> NeedsUpdate(string dllPath) {
            if (!File.Exists(dllPath)) return true;
            try {
				var httpClient = new HttpClient();
				var txtPath = Path.Combine(dllPath, "版本.txt");
				if (File.Exists(txtPath))
				{
					using var txt = new StreamReader(txtPath);
					var nowVerson = new Version(txt.ReadToEnd());
					var textStream = await httpClient.GetStringAsync("https://cninact.diemoe.net/global/版本.txt");
					var remoteVersion = new Version(textStream);
					return remoteVersion > nowVerson;

				}
				else
				{
					await DownloadPlugin(dllPath);
					return true;
				};
			}
            catch {
                return false;
            }
        }

        private static async Task DownloadPlugin(string path) {
			var pluginPath = Path.Combine(path, "FFXIV_ACT_Plugin.dll");
			var txtinPath = Path.Combine(path, "版本.txt");
			var httpClient = new HttpClient();
			await using var downloadStream = await httpClient.GetStreamAsync("https://cninact.diemoe.net/global/FFXIV_ACT_Plugin.dll\r\n");
			await using var textStream = await httpClient.GetStreamAsync("https://cninact.diemoe.net/global/版本.txt");
			//await using var downloadStream = await httpClient.GetStreamAsync($"https://github.com/TundraWork/FFXIV_ACT_Plugin_CN/releases/download/{bcd}/FFXIV_ACT_Plugin.dll");
			await using var zipFileStream = new FileStream(pluginPath, FileMode.Create);
			await downloadStream.CopyToAsync(zipFileStream);
			zipFileStream.Close();
			await using var zipFileStream1 = new FileStream(txtinPath, FileMode.Create);
			await textStream.CopyToAsync(zipFileStream1);
			zipFileStream1.Close();
		}
    }
}
