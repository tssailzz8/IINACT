using System.ComponentModel;
using DarkUI.Controls;

namespace IINACT
{
	partial class SettingsForm
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private IContainer components = null;


		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			ComponentResourceManager resources = new ComponentResourceManager(typeof(SettingsForm));
			label1 = new Label();
			darkSectionPanel1 = new DarkSectionPanel();
			AdjustOrer = new DarkCheckBox();
			darkLabel14 = new DarkLabel();
			checkBoxDebug = new DarkCheckBox();
			checkBoxDotTick = new DarkCheckBox();
			checkBoxDotCrit = new DarkCheckBox();
			checkBoxPets = new DarkCheckBox();
			checkBoxRpcap = new DarkCheckBox();
			checkBoxShield = new DarkCheckBox();
			logFileLabel = new DarkLabel();
			logFileButton = new DarkButton();
			darkLabel2 = new DarkLabel();
			comboBoxFilter = new DarkComboBox();
			comboBoxLang = new DarkComboBox();
			darkLabel1 = new DarkLabel();
			rpcapSectionPanel = new DarkSectionPanel();
			darkLabel12 = new DarkLabel();
			darkLabel11 = new DarkLabel();
			darkLabel10 = new DarkLabel();
			darkLabel9 = new DarkLabel();
			darkLabel8 = new DarkLabel();
			darkLabel7 = new DarkLabel();
			textBoxPassword = new DarkTextBox();
			textBoxUsername = new DarkTextBox();
			darkLabel6 = new DarkLabel();
			darkLabel5 = new DarkLabel();
			textBoxPort = new DarkTextBox();
			darkLabel4 = new DarkLabel();
			textBoxHost = new DarkTextBox();
			darkLabel3 = new DarkLabel();
			darkSectionPanel4 = new DarkSectionPanel();
			debugBox = new DarkTextBox();
			opPanel = new Panel();
			opLabel = new DarkLabel();
			logFolderBrowserDialog = new FolderBrowserDialog();
			darkSectionPanel2 = new DarkSectionPanel();
			Initi = new DarkButton();
			listView1 = new ListView();
			Down = new DarkButton();
			Rise = new DarkButton();
			PostNamazu = new DarkCheckBox();
			darkSectionPanel1.SuspendLayout();
			rpcapSectionPanel.SuspendLayout();
			darkSectionPanel4.SuspendLayout();
			opPanel.SuspendLayout();
			darkSectionPanel2.SuspendLayout();
			SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Dock = DockStyle.Bottom;
			label1.Location = new Point(0, 926);
			label1.Name = "label1";
			label1.Size = new Size(0, 17);
			label1.TabIndex = 1;
			// 
			// darkSectionPanel1
			// 
			darkSectionPanel1.Controls.Add(AdjustOrer);
			darkSectionPanel1.Controls.Add(darkLabel14);
			darkSectionPanel1.Controls.Add(checkBoxDebug);
			darkSectionPanel1.Controls.Add(checkBoxDotTick);
			darkSectionPanel1.Controls.Add(checkBoxDotCrit);
			darkSectionPanel1.Controls.Add(checkBoxPets);
			darkSectionPanel1.Controls.Add(checkBoxRpcap);
			darkSectionPanel1.Controls.Add(checkBoxShield);
			darkSectionPanel1.Controls.Add(logFileLabel);
			darkSectionPanel1.Controls.Add(logFileButton);
			darkSectionPanel1.Controls.Add(darkLabel2);
			darkSectionPanel1.Controls.Add(comboBoxFilter);
			darkSectionPanel1.Controls.Add(comboBoxLang);
			darkSectionPanel1.Controls.Add(darkLabel1);
			darkSectionPanel1.Dock = DockStyle.Top;
			darkSectionPanel1.Location = new Point(0, 0);
			darkSectionPanel1.Name = "darkSectionPanel1";
			darkSectionPanel1.SectionHeader = "Parse Settings";
			darkSectionPanel1.Size = new Size(803, 249);
			darkSectionPanel1.TabIndex = 2;
			// 
			// AdjustOrer
			// 
			AdjustOrer.AutoSize = true;
			AdjustOrer.Location = new Point(664, 151);
			AdjustOrer.Name = "AdjustOrer";
			AdjustOrer.Size = new Size(99, 21);
			AdjustOrer.TabIndex = 12;
			AdjustOrer.Text = "调整职业顺序";
			AdjustOrer.CheckedChanged += AdjustOrer_CheckedChanged;
			// 
			// darkLabel14
			// 
			darkLabel14.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			darkLabel14.AutoSize = true;
			darkLabel14.ForeColor = Color.Gray;
			darkLabel14.Location = new Point(471, 43);
			darkLabel14.Name = "darkLabel14";
			darkLabel14.Size = new Size(357, 17);
			darkLabel14.TabIndex = 9;
			darkLabel14.Text = "Changing Parse Settings may requires an application restart";
			// 
			// checkBoxDebug
			// 
			checkBoxDebug.AutoSize = true;
			checkBoxDebug.Location = new Point(12, 206);
			checkBoxDebug.Name = "checkBoxDebug";
			checkBoxDebug.Size = new Size(279, 21);
			checkBoxDebug.TabIndex = 8;
			checkBoxDebug.Text = "(DEBUG) Forward Debug Fields to Overlays";
			checkBoxDebug.CheckedChanged += checkBoxDebug_CheckedChanged;
			// 
			// checkBoxDotTick
			// 
			checkBoxDotTick.AutoSize = true;
			checkBoxDotTick.Location = new Point(416, 178);
			checkBoxDotTick.Name = "checkBoxDotTick";
			checkBoxDotTick.Size = new Size(237, 21);
			checkBoxDotTick.TabIndex = 7;
			checkBoxDotTick.Text = "(DEBUG) Also Show 'Real' DoT Ticks";
			checkBoxDotTick.CheckedChanged += checkBoxDotTick_CheckedChanged;
			// 
			// checkBoxDotCrit
			// 
			checkBoxDotCrit.AutoSize = true;
			checkBoxDotCrit.Location = new Point(12, 178);
			checkBoxDotCrit.Name = "checkBoxDotCrit";
			checkBoxDotCrit.Size = new Size(248, 21);
			checkBoxDotCrit.TabIndex = 6;
			checkBoxDotCrit.Text = "(DEBUG) Simulate Individual DoT Crits";
			checkBoxDotCrit.CheckedChanged += checkBoxDotCrit_CheckedChanged;
			// 
			// checkBoxPets
			// 
			checkBoxPets.AutoSize = true;
			checkBoxPets.Location = new Point(416, 150);
			checkBoxPets.Name = "checkBoxPets";
			checkBoxPets.Size = new Size(223, 21);
			checkBoxPets.TabIndex = 5;
			checkBoxPets.Text = "Disable Combine Pets with Owner";
			checkBoxPets.CheckedChanged += checkBoxPets_CheckedChanged;
			// 
			// checkBoxRpcap
			// 
			checkBoxRpcap.AutoSize = true;
			checkBoxRpcap.Location = new Point(416, 206);
			checkBoxRpcap.Name = "checkBoxRpcap";
			checkBoxRpcap.Size = new Size(164, 21);
			checkBoxRpcap.TabIndex = 11;
			checkBoxRpcap.Text = "(EXPERT) Enable RPCAP";
			checkBoxRpcap.CheckedChanged += RpcapCheckBox_CheckedChanged;
			// 
			// checkBoxShield
			// 
			checkBoxShield.AutoSize = true;
			checkBoxShield.Location = new Point(12, 150);
			checkBoxShield.Name = "checkBoxShield";
			checkBoxShield.Size = new Size(221, 21);
			checkBoxShield.TabIndex = 4;
			checkBoxShield.Text = "Disable Damage Shield Estimates";
			checkBoxShield.CheckedChanged += checkBoxShield_CheckedChanged;
			// 
			// logFileLabel
			// 
			logFileLabel.AutoSize = true;
			logFileLabel.ForeColor = Color.FromArgb(220, 220, 220);
			logFileLabel.Location = new Point(12, 111);
			logFileLabel.Name = "logFileLabel";
			logFileLabel.Size = new Size(103, 17);
			logFileLabel.TabIndex = 0;
			logFileLabel.Text = "Logfile Location:";
			// 
			// logFileButton
			// 
			logFileButton.Location = new Point(172, 108);
			logFileButton.Name = "logFileButton";
			logFileButton.Padding = new Padding(5, 6, 5, 6);
			logFileButton.Size = new Size(454, 27);
			logFileButton.TabIndex = 1;
			logFileButton.Text = "Log Directory";
			// 
			// darkLabel2
			// 
			darkLabel2.AutoSize = true;
			darkLabel2.ForeColor = Color.FromArgb(220, 220, 220);
			darkLabel2.Location = new Point(12, 77);
			darkLabel2.Name = "darkLabel2";
			darkLabel2.Size = new Size(75, 17);
			darkLabel2.TabIndex = 3;
			darkLabel2.Text = "Parse Filter:";
			// 
			// comboBoxFilter
			// 
			comboBoxFilter.DrawMode = DrawMode.OwnerDrawVariable;
			comboBoxFilter.FormattingEnabled = true;
			comboBoxFilter.Location = new Point(172, 74);
			comboBoxFilter.Name = "comboBoxFilter";
			comboBoxFilter.Size = new Size(227, 24);
			comboBoxFilter.TabIndex = 2;
			// 
			// comboBoxLang
			// 
			comboBoxLang.DrawMode = DrawMode.OwnerDrawVariable;
			comboBoxLang.FormattingEnabled = true;
			comboBoxLang.Location = new Point(172, 40);
			comboBoxLang.Name = "comboBoxLang";
			comboBoxLang.Size = new Size(227, 24);
			comboBoxLang.TabIndex = 1;
			comboBoxLang.SelectedIndexChanged += comboBoxLang_SelectedIndexChanged_1;
			// 
			// darkLabel1
			// 
			darkLabel1.AutoSize = true;
			darkLabel1.ForeColor = Color.FromArgb(220, 220, 220);
			darkLabel1.Location = new Point(12, 43);
			darkLabel1.Name = "darkLabel1";
			darkLabel1.Size = new Size(106, 17);
			darkLabel1.TabIndex = 0;
			darkLabel1.Text = "Game Language:";
			// 
			// rpcapSectionPanel
			// 
			rpcapSectionPanel.Controls.Add(darkLabel12);
			rpcapSectionPanel.Controls.Add(darkLabel11);
			rpcapSectionPanel.Controls.Add(darkLabel10);
			rpcapSectionPanel.Controls.Add(darkLabel9);
			rpcapSectionPanel.Controls.Add(darkLabel8);
			rpcapSectionPanel.Controls.Add(darkLabel7);
			rpcapSectionPanel.Controls.Add(textBoxPassword);
			rpcapSectionPanel.Controls.Add(textBoxUsername);
			rpcapSectionPanel.Controls.Add(darkLabel6);
			rpcapSectionPanel.Controls.Add(darkLabel5);
			rpcapSectionPanel.Controls.Add(textBoxPort);
			rpcapSectionPanel.Controls.Add(darkLabel4);
			rpcapSectionPanel.Controls.Add(textBoxHost);
			rpcapSectionPanel.Controls.Add(darkLabel3);
			rpcapSectionPanel.Dock = DockStyle.Top;
			rpcapSectionPanel.Location = new Point(0, 249);
			rpcapSectionPanel.Name = "rpcapSectionPanel";
			rpcapSectionPanel.SectionHeader = "RPCAP";
			rpcapSectionPanel.Size = new Size(803, 263);
			rpcapSectionPanel.TabIndex = 3;
			rpcapSectionPanel.Paint += rpcapSectionPanel_Paint;
			// 
			// darkLabel12
			// 
			darkLabel12.AutoSize = true;
			darkLabel12.ForeColor = Color.FromArgb(220, 220, 220);
			darkLabel12.Location = new Point(12, 212);
			darkLabel12.Name = "darkLabel12";
			darkLabel12.Size = new Size(60, 17);
			darkLabel12.TabIndex = 15;
			darkLabel12.Text = "Warning:";
			// 
			// darkLabel11
			// 
			darkLabel11.AutoSize = true;
			darkLabel11.ForeColor = Color.LightCoral;
			darkLabel11.Location = new Point(79, 212);
			darkLabel11.Name = "darkLabel11";
			darkLabel11.Size = new Size(579, 34);
			darkLabel11.TabIndex = 14;
			darkLabel11.Text = "The username and password are sent over the network to the capture server ***IN CLEAR TEXT***\r\nBecause of this credentials are also stored unenecrypted for now.";
			// 
			// darkLabel10
			// 
			darkLabel10.AutoSize = true;
			darkLabel10.ForeColor = Color.Gray;
			darkLabel10.Location = new Point(315, 176);
			darkLabel10.Name = "darkLabel10";
			darkLabel10.Size = new Size(507, 17);
			darkLabel10.TabIndex = 13;
			darkLabel10.Text = "Specifies the password that has to be used on the remote machine for authentication.";
			// 
			// darkLabel9
			// 
			darkLabel9.AutoSize = true;
			darkLabel9.ForeColor = Color.Gray;
			darkLabel9.Location = new Point(315, 143);
			darkLabel9.Name = "darkLabel9";
			darkLabel9.Size = new Size(507, 17);
			darkLabel9.TabIndex = 12;
			darkLabel9.Text = "Specifies the username that has to be used on the remote machine for authentication.";
			// 
			// darkLabel8
			// 
			darkLabel8.AutoSize = true;
			darkLabel8.ForeColor = Color.FromArgb(220, 220, 220);
			darkLabel8.Location = new Point(12, 176);
			darkLabel8.Name = "darkLabel8";
			darkLabel8.Size = new Size(67, 17);
			darkLabel8.TabIndex = 10;
			darkLabel8.Text = "Password:";
			// 
			// darkLabel7
			// 
			darkLabel7.AutoSize = true;
			darkLabel7.ForeColor = Color.FromArgb(220, 220, 220);
			darkLabel7.Location = new Point(12, 143);
			darkLabel7.Name = "darkLabel7";
			darkLabel7.Size = new Size(70, 17);
			darkLabel7.TabIndex = 9;
			darkLabel7.Text = "Username:";
			// 
			// textBoxPassword
			// 
			textBoxPassword.BackColor = Color.FromArgb(69, 73, 74);
			textBoxPassword.BorderStyle = BorderStyle.FixedSingle;
			textBoxPassword.ForeColor = Color.FromArgb(220, 220, 220);
			textBoxPassword.Location = new Point(82, 173);
			textBoxPassword.Name = "textBoxPassword";
			textBoxPassword.Size = new Size(227, 23);
			textBoxPassword.TabIndex = 8;
			textBoxPassword.UseSystemPasswordChar = true;
			textBoxPassword.TextChanged += TextBoxPassword_TextChanged;
			// 
			// textBoxUsername
			// 
			textBoxUsername.BackColor = Color.FromArgb(69, 73, 74);
			textBoxUsername.BorderStyle = BorderStyle.FixedSingle;
			textBoxUsername.ForeColor = Color.FromArgb(220, 220, 220);
			textBoxUsername.Location = new Point(82, 141);
			textBoxUsername.Name = "textBoxUsername";
			textBoxUsername.Size = new Size(227, 23);
			textBoxUsername.TabIndex = 7;
			textBoxUsername.TextChanged += TextBoxUsername_TextChanged;
			// 
			// darkLabel6
			// 
			darkLabel6.AutoSize = true;
			darkLabel6.ForeColor = Color.Gray;
			darkLabel6.Location = new Point(315, 110);
			darkLabel6.Name = "darkLabel6";
			darkLabel6.Size = new Size(469, 17);
			darkLabel6.TabIndex = 6;
			darkLabel6.Text = "Specifies the network port (e.g. \"2002\") we want to use for the RPCAP protocol.";
			// 
			// darkLabel5
			// 
			darkLabel5.AutoSize = true;
			darkLabel5.ForeColor = Color.FromArgb(220, 220, 220);
			darkLabel5.Location = new Point(12, 110);
			darkLabel5.Name = "darkLabel5";
			darkLabel5.Size = new Size(35, 17);
			darkLabel5.TabIndex = 5;
			darkLabel5.Text = "Port:";
			// 
			// textBoxPort
			// 
			textBoxPort.BackColor = Color.FromArgb(69, 73, 74);
			textBoxPort.BorderStyle = BorderStyle.FixedSingle;
			textBoxPort.ForeColor = Color.FromArgb(220, 220, 220);
			textBoxPort.Location = new Point(82, 108);
			textBoxPort.Name = "textBoxPort";
			textBoxPort.Size = new Size(227, 23);
			textBoxPort.TabIndex = 4;
			textBoxPort.TextChanged += TextBoxPort_TextChanged;
			// 
			// darkLabel4
			// 
			darkLabel4.AutoSize = true;
			darkLabel4.ForeColor = Color.Gray;
			darkLabel4.Location = new Point(315, 77);
			darkLabel4.Name = "darkLabel4";
			darkLabel4.Size = new Size(367, 17);
			darkLabel4.TabIndex = 3;
			darkLabel4.Text = "Specifies the host (e.g. \"foo.bar.com\") we want to connect to. \r\n";
			// 
			// textBoxHost
			// 
			textBoxHost.BackColor = Color.FromArgb(69, 73, 74);
			textBoxHost.BorderStyle = BorderStyle.FixedSingle;
			textBoxHost.ForeColor = Color.FromArgb(220, 220, 220);
			textBoxHost.Location = new Point(82, 75);
			textBoxHost.Name = "textBoxHost";
			textBoxHost.Size = new Size(227, 23);
			textBoxHost.TabIndex = 2;
			textBoxHost.TextChanged += TextBoxHost_TextChanged;
			// 
			// darkLabel3
			// 
			darkLabel3.AutoSize = true;
			darkLabel3.ForeColor = Color.FromArgb(220, 220, 220);
			darkLabel3.Location = new Point(12, 77);
			darkLabel3.Name = "darkLabel3";
			darkLabel3.Size = new Size(38, 17);
			darkLabel3.TabIndex = 1;
			darkLabel3.Text = "Host:";
			// 
			// darkSectionPanel4
			// 
			darkSectionPanel4.Controls.Add(debugBox);
			darkSectionPanel4.Dock = DockStyle.Fill;
			darkSectionPanel4.Location = new Point(0, 839);
			darkSectionPanel4.Name = "darkSectionPanel4";
			darkSectionPanel4.SectionHeader = "Debug Log";
			darkSectionPanel4.Size = new Size(803, 87);
			darkSectionPanel4.TabIndex = 5;
			// 
			// debugBox
			// 
			debugBox.AllowDrop = true;
			debugBox.BackColor = Color.FromArgb(69, 73, 74);
			debugBox.BorderStyle = BorderStyle.None;
			debugBox.Dock = DockStyle.Fill;
			debugBox.ForeColor = Color.FromArgb(220, 220, 220);
			debugBox.Location = new Point(1, 25);
			debugBox.Multiline = true;
			debugBox.Name = "debugBox";
			debugBox.ReadOnly = true;
			debugBox.ScrollBars = ScrollBars.Vertical;
			debugBox.Size = new Size(801, 61);
			debugBox.TabIndex = 0;
			debugBox.TextChanged += debugBox_TextChanged;
			// 
			// opPanel
			// 
			opPanel.Controls.Add(opLabel);
			opPanel.Dock = DockStyle.Top;
			opPanel.Location = new Point(0, 512);
			opPanel.Name = "opPanel";
			opPanel.Size = new Size(803, 126);
			opPanel.TabIndex = 6;
			opPanel.Paint += opPanel_Paint;
			// 
			// opLabel
			// 
			opLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			opLabel.ForeColor = Color.FromArgb(220, 220, 220);
			opLabel.Location = new Point(569, 39);
			opLabel.Name = "opLabel";
			opLabel.RightToLeft = RightToLeft.Yes;
			opLabel.Size = new Size(222, 24);
			opLabel.TabIndex = 0;
			opLabel.Text = "...Searching for game";
			opLabel.TextAlign = ContentAlignment.MiddleLeft;
			// 
			// darkSectionPanel2
			// 
			darkSectionPanel2.Controls.Add(PostNamazu);
			darkSectionPanel2.Controls.Add(Initi);
			darkSectionPanel2.Controls.Add(listView1);
			darkSectionPanel2.Controls.Add(Down);
			darkSectionPanel2.Controls.Add(Rise);
			darkSectionPanel2.Dock = DockStyle.Top;
			darkSectionPanel2.Location = new Point(0, 638);
			darkSectionPanel2.Name = "darkSectionPanel2";
			darkSectionPanel2.SectionHeader = "Adjust";
			darkSectionPanel2.Size = new Size(803, 201);
			darkSectionPanel2.TabIndex = 16;
			darkSectionPanel2.Paint += darkSectionPanel2_Paint;
			// 
			// Initi
			// 
			Initi.Location = new Point(152, 71);
			Initi.Name = "Initi";
			Initi.Padding = new Padding(5);
			Initi.Size = new Size(40, 23);
			Initi.TabIndex = 4;
			Initi.Text = "Initi";
			Initi.Click += Initi_Click;
			// 
			// listView1
			// 
			listView1.BackColor = Color.FromArgb(60, 63, 65);
			listView1.ForeColor = SystemColors.Info;
			listView1.Location = new Point(211, 6);
			listView1.Name = "listView1";
			listView1.Size = new Size(80, 189);
			listView1.TabIndex = 3;
			listView1.UseCompatibleStateImageBehavior = false;
			listView1.View = View.SmallIcon;
			listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
			// 
			// Down
			// 
			Down.Location = new Point(152, 123);
			Down.Name = "Down";
			Down.Padding = new Padding(5);
			Down.Size = new Size(40, 23);
			Down.TabIndex = 2;
			Down.Text = "↓";
			Down.Click += Down_Click;
			// 
			// Rise
			// 
			Rise.Location = new Point(152, 6);
			Rise.Name = "Rise";
			Rise.Padding = new Padding(5);
			Rise.Size = new Size(40, 23);
			Rise.TabIndex = 1;
			Rise.Text = "↑";
			Rise.Click += Rise_Click;
			// 
			// PostNamazu
			// 
			PostNamazu.AutoSize = true;
			PostNamazu.Location = new Point(340, 24);
			PostNamazu.Name = "PostNamazu";
			PostNamazu.Size = new Size(147, 21);
			PostNamazu.TabIndex = 5;
			PostNamazu.Text = "触发器里面使用鲶鱼精";
			PostNamazu.CheckedChanged += PostNamazu_CheckedChanged;
			// 
			// SettingsForm
			// 
			AutoScaleDimensions = new SizeF(7F, 17F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(803, 943);
			Controls.Add(darkSectionPanel4);
			Controls.Add(darkSectionPanel2);
			Controls.Add(opPanel);
			Controls.Add(rpcapSectionPanel);
			Controls.Add(darkSectionPanel1);
			Controls.Add(label1);
			Icon = (Icon)resources.GetObject("$this.Icon");
			MinimumSize = new Size(819, 244);
			Name = "SettingsForm";
			Text = "IINACT";
			FormClosing += Form1_FormClosing;
			darkSectionPanel1.ResumeLayout(false);
			darkSectionPanel1.PerformLayout();
			rpcapSectionPanel.ResumeLayout(false);
			rpcapSectionPanel.PerformLayout();
			darkSectionPanel4.ResumeLayout(false);
			darkSectionPanel4.PerformLayout();
			opPanel.ResumeLayout(false);
			darkSectionPanel2.ResumeLayout(false);
			darkSectionPanel2.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private Label label1;
		private DarkSectionPanel darkSectionPanel1;
		private DarkCheckBox checkBoxDebug;
		private DarkCheckBox checkBoxDotTick;
		private DarkCheckBox checkBoxDotCrit;
		private DarkCheckBox checkBoxPets;
		private DarkCheckBox checkBoxShield;
		private DarkLabel darkLabel2;
		private DarkComboBox comboBoxFilter;
		private DarkComboBox comboBoxLang;
		private DarkLabel darkLabel1;
		private DarkSectionPanel rpcapSectionPanel;
		private DarkLabel darkLabel6;
		private DarkLabel darkLabel5;
		private DarkTextBox textBoxPort;
		private DarkLabel darkLabel4;
		private DarkTextBox textBoxHost;
		private DarkLabel darkLabel3;
		private DarkSectionPanel darkSectionPanel4;
		private DarkLabel darkLabel12;
		private DarkLabel darkLabel11;
		private DarkLabel darkLabel10;
		private DarkLabel darkLabel9;
		private DarkCheckBox checkBoxRpcap;
		private DarkLabel darkLabel8;
		private DarkLabel darkLabel7;
		private DarkTextBox textBoxPassword;
		private DarkTextBox textBoxUsername;
		private DarkTextBox debugBox;
		private DarkLabel darkLabel14;
		private Panel opPanel;
		private DarkLabel opLabel;
		private DarkLabel logFileLabel;
		private DarkButton logFileButton;
		private FolderBrowserDialog logFolderBrowserDialog;
		private DarkSectionPanel darkSectionPanel2;
		private DarkCheckBox AdjustOrer;
		private DarkButton Down;
		private DarkButton Rise;
		private ListView listView1;
		private DarkButton Initi;
		private DarkCheckBox PostNamazu;
	}
}