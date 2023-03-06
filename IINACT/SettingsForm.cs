using System.ComponentModel;
using System.Diagnostics;
using Advanced_Combat_Tracker;
using CactbotSelf;
using DarkUI.Controls;
using DarkUI.Forms;
using FFXIV_ACT_Plugin.Common;
using FFXIV_ACT_Plugin.Config;
using IINACT.Properties;
using PostNamazu;
using RainbowMage.OverlayPlugin;

namespace IINACT
{
	public partial class SettingsForm : DarkForm
	{
		public List<string> shunxu = new List<string> { "黑骑", "枪刃", "战士", "骑士", "白魔", "占星", "贤者", "学者", "武士", "武僧", "镰刀", "龙骑", "忍者", "机工", "舞者", "诗人", "黑魔", "召唤", "赤魔" };
		public SettingsForm(int targetPid)
		{
			InitializeComponent();
			comboBoxLang.DataSource = Enum.GetValues(typeof(Language));
			if (Settings.Default.shunxu.Count < shunxu.Count)
			{
				Settings.Default.shunxu = shunxu;

			}
			for (int i = 0; i < Settings.Default.shunxu.Count; i++)
			{
				listView1.Items.Add($@"[{i}]{Settings.Default.shunxu[i]}");
			}
			comboBoxLang.SelectedIndex = Settings.Default.Language - 1;
			comboBoxLang.SelectedIndexChanged += comboBoxLang_SelectedIndexChanged;
			comboBoxFilter.DataSource = Enum.GetValues(typeof(ParseFilterMode));
			comboBoxFilter.SelectedIndex = Settings.Default.ParseFilterMode;
			comboBoxFilter.SelectedIndexChanged += comboBoxFilter_SelectedIndexChanged;
			checkBoxShield.Checked = Settings.Default.DisableDamageShield;
			checkBoxPets.Checked = Settings.Default.DisableCombinePets;
			checkBoxDotCrit.Checked = Settings.Default.SimulateIndividualDoTCrits;
			checkBoxDotTick.Checked = Settings.Default.ShowRealDoTTicks;
			checkBoxDebug.Checked = Settings.Default.ShowDebug;
			checkBoxRpcap.Checked = Settings.Default.RPcap;
			textBoxHost.Text = Settings.Default.RPcapHost;
			textBoxPort.Text = $@"{Settings.Default.RPcapPort}";
			textBoxUsername.Text = Settings.Default.RPcapUsername;
			textBoxPassword.Text = Settings.Default.RPcapPassword;
			rpcapSectionPanel.Height = Settings.Default.RPcap ? 200 : 0;
			darkSectionPanel2.Height = Settings.Default.AdjustOrder ? 200 : 0;
			logFileButton.Click += logFileButton_Clicked;
			if (Directory.Exists(Settings.Default.LogFilePath))
				ActGlobals.oFormActMain.LogFilePath = Settings.Default.LogFilePath;
			logFileButton.Text = ActGlobals.oFormActMain.LogFilePath;
			AdjustOrer.Checked = Settings.Default.AdjustOrder;
			//create window handle
			Opacity = 0;
			Show();
			Hide();
			Opacity = 1;
#if DEBUG
			Show();
#endif
			Task.Run(CheckForUpdate);
			Task.Run(() =>
			{
				var ffxivActPlugin = new FfxivActPluginWrapper(targetPid);
				Invoke(new MethodInvoker(InitOverlayPlugin));
				while (ffxivActPlugin.ProcessManager.Verify())
					Thread.Sleep(2000);
				Invoke(new MethodInvoker(Application.Exit));
			});
		}

		private static void CheckForUpdate()
		{
			try
			{
				//var currentVersion = new Version(Application.ProductVersion);
				//var remoteVersionString =
				//	new HttpClient().GetStringAsync("https://github.com/marzent/IINACT/raw/main/version").Result;
				//var remoteVersion = new Version(remoteVersionString);
				//if (remoteVersion <= currentVersion) return;
				//if (MessageBox.Show("An newer version of IINACT is available. Would you like to download it?",
				//		"Update available", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
				//{
				//	Process.Start("explorer", "https://github.com/marzent/IINACT/releases/latest");
				//}
			}
			catch
			{
				//MessageBox.Show("Failed to check for updates.");
			}
		}
		CactbotSelf.CactbotSelf cactboSelf;
		private void InitOverlayPlugin()
		{
			var container = new TinyIoCContainer();
			var logger = new Logger();
			container.Register(logger);
			container.Register<ILogger>(logger);

			var pluginMain = new PluginMain(Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "OverlayPlugin"), logger, container);
			container.Register(pluginMain);
			ActGlobals.oFormActMain.OverlayPluginContainer = container;

			pluginMain.InitPlugin(opPanel, opLabel);
			Settings.Default.SettingsSaving += Save;
			var post = new PostNamazu.PostNamazu();
			post.InitPlugin(opPanel, opLabel);
			cactboSelf = new CactbotSelf.CactbotSelf(Settings.Default.shunxu, Settings.Default.PostNamazuUse);
			cactboSelf.InitPlugin(opPanel, opLabel);
		}

		private void Save(object sender, CancelEventArgs e)
		{
			if (sender is Settings set)
			{
				cactboSelf.ChangeSetting(set.shunxu, set.PostNamazuUse);
			}


		}

		protected override void OnHandleCreated(EventArgs e)
		{
			Trace.Listeners.Add(new TextBoxTraceListener(debugBox));
		}

		private void comboBoxLang_SelectedIndexChanged(object? sender, EventArgs e)
		{
			Settings.Default.Language = comboBoxLang.SelectedIndex + 1;
			Settings.Default.Save();
		}

		private void comboBoxFilter_SelectedIndexChanged(object? sender, EventArgs e)
		{
			Settings.Default.ParseFilterMode = comboBoxFilter.SelectedIndex;
			Settings.Default.Save();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason != CloseReason.UserClosing) return;
			e.Cancel = true;
			Hide();
		}

		private void checkBoxShield_CheckedChanged(object sender, EventArgs e)
		{
			Settings.Default.DisableDamageShield = checkBoxShield.Checked;
			Settings.Default.Save();
		}

		private void checkBoxPets_CheckedChanged(object sender, EventArgs e)
		{
			Settings.Default.DisableCombinePets = checkBoxPets.Checked;
			Settings.Default.Save();
		}

		private void checkBoxDotCrit_CheckedChanged(object sender, EventArgs e)
		{
			Settings.Default.SimulateIndividualDoTCrits = checkBoxDotCrit.Checked;
			Settings.Default.Save();
		}

		private void checkBoxDotTick_CheckedChanged(object sender, EventArgs e)
		{
			Settings.Default.ShowRealDoTTicks = checkBoxDotTick.Checked;
			Settings.Default.Save();
		}

		private void checkBoxDebug_CheckedChanged(object sender, EventArgs e)
		{
			Settings.Default.ShowDebug = checkBoxDebug.Checked;
			Settings.Default.Save();
		}

		private void RpcapCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			Settings.Default.RPcap = checkBoxRpcap.Checked;
			Settings.Default.Save();
			rpcapSectionPanel.Height = Settings.Default.RPcap ? 200 : 0;
		}

		private void TextBoxHost_TextChanged(object sender, EventArgs e)
		{
			Settings.Default.RPcapHost = textBoxHost.Text;
			Settings.Default.Save();
		}

		private void TextBoxPort_TextChanged(object sender, EventArgs e)
		{
			if (!int.TryParse(textBoxPort.Text, out var port)) return;
			Settings.Default.RPcapPort = port;
			Settings.Default.Save();
		}

		private void TextBoxUsername_TextChanged(object sender, EventArgs e)
		{
			Settings.Default.RPcapUsername = textBoxUsername.Text;
			Settings.Default.Save();
		}

		private void TextBoxPassword_TextChanged(object sender, EventArgs e)
		{
			Settings.Default.RPcapPassword = textBoxPassword.Text;
			Settings.Default.Save();
		}

		private void logFileButton_Clicked(object? sender, EventArgs e)
		{
			// Show the FolderBrowserDialog.
			var result = logFolderBrowserDialog.ShowDialog();
			if (result != DialogResult.OK)
				return;
			var newPath = logFolderBrowserDialog.SelectedPath ?? "";
			if (!Directory.Exists(newPath))
				return;
			ActGlobals.oFormActMain.LogFilePath = newPath;
			Settings.Default.LogFilePath = newPath;
			Settings.Default.Save();
		}

		private void debugBox_TextChanged(object sender, EventArgs e)
		{

		}

		private void darkSectionPanel2_Paint(object sender, PaintEventArgs e)
		{

		}

		private void rpcapSectionPanel_Paint(object sender, PaintEventArgs e)
		{

		}

		private void AdjustOrer_CheckedChanged(object sender, EventArgs e)
		{
			Settings.Default.AdjustOrder = AdjustOrer.Checked;
			Settings.Default.Save();
			darkSectionPanel2.Height = Settings.Default.AdjustOrder ? 200 : 0;
		}

		private void opPanel_Paint(object sender, PaintEventArgs e)
		{

		}

		private void darkListView1_Click(object sender, EventArgs e)
		{

		}

		private void comboBoxLang_SelectedIndexChanged_1(object sender, EventArgs e)
		{

		}

		private void Rise_Click(object sender, EventArgs e)
		{
			if (listView1.SelectedIndices != null && listView1.SelectedIndices.Count > 0)
			{
				var c = listView1.SelectedIndices;
				var b = listView1.Items[c[0]].Text;
				var 选中的 = c[0];
				if (选中的 >= 1)
				{
					listView1.BeginUpdate();
					var 交换 = Settings.Default.shunxu[选中的];
					Settings.Default.shunxu[选中的] = Settings.Default.shunxu[选中的 - 1];
					Settings.Default.shunxu[选中的 - 1] = 交换;
					listView1.Items.Clear();
					for (int i = 0; i < Settings.Default.shunxu.Count; i++)
					{
						listView1.Items.Add($@"[{i}]{Settings.Default.shunxu[i]}");
					}
					listView1.EndUpdate();
					listView1.Focus();
					listView1.Items[选中的 - 1].Focused = true;
					listView1.Items[选中的 - 1].Selected = true;
					Settings.Default.Save();
				}


			}
		}

		private void Down_Click(object sender, EventArgs e)
		{
			if (listView1.SelectedIndices != null && listView1.SelectedIndices.Count > 0)
			{
				var c = listView1.SelectedIndices;
				var b = listView1.Items[c[0]].Text;
				var 选中的 = c[0];
				if (选中的 >= 1)
				{
					listView1.BeginUpdate();
					var 交换 = Settings.Default.shunxu[选中的];
					Settings.Default.shunxu[选中的] = Settings.Default.shunxu[选中的 + 1];
					Settings.Default.shunxu[选中的 - 1] = 交换;
					listView1.Items.Clear();
					for (int i = 0; i < Settings.Default.shunxu.Count; i++)
					{
						listView1.Items.Add($@"[{i}]{Settings.Default.shunxu[i]}");
					}
					listView1.EndUpdate();
					listView1.Focus();
					listView1.Items[选中的 - 1].Focused = true;
					listView1.Items[选中的 - 1].Selected = true;
					Settings.Default.Save();
				}


			}
		}

		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void Initi_Click(object sender, EventArgs e)
		{
			Settings.Default.shunxu = shunxu;
			listView1.Items.Clear();
			for (int i = 0; i < Settings.Default.shunxu.Count; i++)
			{
				listView1.Items.Add($@"[{i}]{Settings.Default.shunxu[i]}");
			}
			Settings.Default.Save();

		}

		private void PostNamazu_CheckedChanged(object sender, EventArgs e)
		{
			Settings.Default.PostNamazuUse = PostNamazu.Checked;
			Settings.Default.Save();
		}
	}
}