﻿using GreyMagic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PostNamazu.Common
{
	internal abstract class NamazuModule
	{
		protected PostNamazu PostNamazu;
		protected FFXIV_ACT_Plugin.FFXIV_ACT_Plugin FFXIV_ACT_Plugin => PostNamazu?.FFXIV_ACT_Plugin;
		protected Process FFXIV => PostNamazu?.FFXIV;
		protected ExternalProcessMemory Memory => PostNamazu?.Memory;
		protected SigScanner SigScanner => PostNamazu?.SigScanner;

		internal bool isReady = false;

		public void Init(PostNamazu postNamazu) => PostNamazu = postNamazu;

		public void Setup()
		{
			try
			{
				GetOffsets();
			}
			catch (Exception ex)
			{
				Log(ex.ToString());
				isReady = false;
			}
			//Log("初始化完成");
			if (FFXIV_ACT_Plugin != null && FFXIV != null && Memory != null)
			{
				isReady = true;
			}
			else
			{
				isReady = false;
			}
		}

		public virtual void GetOffsets()
		{

		}

		public void Log(string msg)
		{
			MessageBox.Show(msg);
		}
	}
}
namespace PostNamazu.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class CommandAttribute : Attribute
	{
		public string Command { get; }

		public CommandAttribute(string command)
		{
			Command = command;
		}
	}
}
