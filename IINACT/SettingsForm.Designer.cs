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
			checkBoxShield = new DarkCheckBox();
			logFileLabel = new DarkLabel();
			logFileButton = new DarkButton();
			darkLabel2 = new DarkLabel();
			comboBoxFilter = new DarkComboBox();
			comboBoxLang = new DarkComboBox();
			darkLabel1 = new DarkLabel();
			darkSectionPanel4 = new DarkSectionPanel();
			debugBox = new DarkTextBox();
			opPanel = new Panel();
			opLabel = new DarkLabel();
			logFolderBrowserDialog = new FolderBrowserDialog();
			darkSectionPanel2 = new DarkSectionPanel();
			Down = new DarkButton();
			Initi = new DarkButton();
			Rise = new DarkButton();
			listView1 = new ListView();
			darkSectionPanel1.SuspendLayout();
			darkSectionPanel4.SuspendLayout();
			opPanel.SuspendLayout();
			darkSectionPanel2.SuspendLayout();
			SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Dock = DockStyle.Bottom;
			label1.Location = new Point(0, 826);
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
			darkLabel14.Location = new Point(448, 43);
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
			// darkSectionPanel4
			// 
			darkSectionPanel4.Controls.Add(debugBox);
			darkSectionPanel4.Dock = DockStyle.Fill;
			darkSectionPanel4.Location = new Point(0, 571);
			darkSectionPanel4.Name = "darkSectionPanel4";
			darkSectionPanel4.SectionHeader = "Debug Log";
			darkSectionPanel4.Size = new Size(803, 255);
			darkSectionPanel4.TabIndex = 5;
			// 
			// debugBox
			// 
			debugBox.BackColor = Color.FromArgb(69, 73, 74);
			debugBox.BorderStyle = BorderStyle.None;
			debugBox.Dock = DockStyle.Fill;
			debugBox.ForeColor = Color.FromArgb(220, 220, 220);
			debugBox.Location = new Point(1, 25);
			debugBox.Multiline = true;
			debugBox.Name = "debugBox";
			debugBox.ReadOnly = true;
			debugBox.ScrollBars = ScrollBars.Vertical;
			debugBox.Size = new Size(801, 229);
			debugBox.TabIndex = 0;
			// 
			// opPanel
			// 
			opPanel.Controls.Add(opLabel);
			opPanel.Dock = DockStyle.Top;
			opPanel.Location = new Point(0, 249);
			opPanel.Name = "opPanel";
			opPanel.Size = new Size(803, 122);
			opPanel.TabIndex = 6;
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
			darkSectionPanel2.Controls.Add(Down);
			darkSectionPanel2.Controls.Add(Initi);
			darkSectionPanel2.Controls.Add(Rise);
			darkSectionPanel2.Controls.Add(listView1);
			darkSectionPanel2.Dock = DockStyle.Top;
			darkSectionPanel2.Location = new Point(0, 371);
			darkSectionPanel2.Name = "darkSectionPanel2";
			darkSectionPanel2.SectionHeader = "Adjust";
			darkSectionPanel2.Size = new Size(803, 200);
			darkSectionPanel2.TabIndex = 1;
			darkSectionPanel2.Paint += darkSectionPanel2_Paint;
			// 
			// Down
			// 
			Down.Location = new Point(152, 136);
			Down.Name = "Down";
			Down.Padding = new Padding(5);
			Down.Size = new Size(40, 23);
			Down.TabIndex = 2;
			Down.Text = "↓";
			Down.Click += Down_Click;
			// 
			// Initi
			// 
			Initi.Location = new Point(152, 83);
			Initi.Name = "Initi";
			Initi.Padding = new Padding(5);
			Initi.Size = new Size(40, 23);
			Initi.TabIndex = 4;
			Initi.Text = "Initi";
			Initi.Click += Initi_Click;
			// 
			// Rise
			// 
			Rise.Location = new Point(152, 28);
			Rise.Name = "Rise";
			Rise.Padding = new Padding(5);
			Rise.Size = new Size(40, 23);
			Rise.TabIndex = 1;
			Rise.Text = "↑";
			Rise.Click += Rise_Click;
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
			// SettingsForm
			// 
			AutoScaleDimensions = new SizeF(7F, 17F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(803, 843);
			Controls.Add(darkSectionPanel4);
			Controls.Add(darkSectionPanel2);
			Controls.Add(opPanel);
			Controls.Add(darkSectionPanel1);
			Controls.Add(label1);
			Icon = (Icon)resources.GetObject("$this.Icon");
			MinimumSize = new Size(815, 225);
			Name = "SettingsForm";
			Text = "IINACT";
			FormClosing += Form1_FormClosing;
			darkSectionPanel1.ResumeLayout(false);
			darkSectionPanel1.PerformLayout();
			darkSectionPanel4.ResumeLayout(false);
			darkSectionPanel4.PerformLayout();
			opPanel.ResumeLayout(false);
			darkSectionPanel2.ResumeLayout(false);
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
		private DarkSectionPanel darkSectionPanel4;
		private DarkTextBox debugBox;
		private DarkLabel darkLabel14;
		private Panel opPanel;
		private DarkLabel opLabel;
		private DarkLabel logFileLabel;
		private DarkButton logFileButton;
		private FolderBrowserDialog logFolderBrowserDialog;
		private DarkCheckBox AdjustOrer;
		private DarkSectionPanel darkSectionPanel2;
		private ListView listView1;
		private DarkButton Rise;
		private DarkButton Down;
		private DarkButton Initi;
	}
}