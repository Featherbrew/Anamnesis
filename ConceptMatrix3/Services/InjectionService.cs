﻿// Concept Matrix 3.
// Licensed under the MIT license.

namespace ConceptMatrix.Injection
{
	using System;
	using System.Diagnostics;
	using System.Threading.Tasks;
	using System.Windows;
	using ConceptMatrix;
	using ConceptMatrix.GUI.Services;
	using ConceptMatrix.GUI.Windows;
	using ConceptMatrix.Memory;
	using ConceptMatrix.Memory.Offsets;
	using ConceptMatrix.Memory.Process;

	public class InjectionService : IInjectionService
	{
		private MarshalerService service;

		public string GamePath
		{
			get
			{
				return this.service.GamePath;
			}
		}

		public IMarshaler<T> GetMemory<T>(IBaseMemoryOffset baseOffset, params IMemoryOffset[] offsets)
		{
			return this.service.GetMarshaler<T>(baseOffset, offsets);
		}

		public IMarshaler<T> GetMemory<T>(IBaseMemoryOffset baseOffset, params IMemoryOffset<T>[] offsets)
		{
			return this.service.GetMarshaler<T>(baseOffset, offsets);
		}

		public IMarshaler<T> GetMemory<T>(IBaseMemoryOffset<T> baseOffset, params IMemoryOffset<T>[] offsets)
		{
			return this.service.GetMarshaler<T>(baseOffset, offsets);
		}

		public async Task Initialize()
		{
			this.service = new MarshalerService();
			this.service.ErrorCallback = this.OnError;
			this.service.LogCallback = this.OnLog;
			this.service.SelectProcessCallback = this.OnSelectProcess;

			await this.service.Initialize<WinProcess>();
		}

		public async Task Shutdown()
		{
			await this.service.Shutdown();
		}

		public Task Start()
		{
			return Task.CompletedTask;
		}

		public Task WaitForMemoryTick()
		{
			return this.service.WaitForMarshalerTick();
		}

		private void OnError(Exception ex)
		{
			Log.Write(ex, "Injection");
		}

		private void OnLog(string message)
		{
			Log.Write(message, "Injection", Log.Severity.Log);
		}

		private async Task<Process> OnSelectProcess()
		{
			return await App.Current.Dispatcher.InvokeAsync<Process>(() =>
			{
				Process proc = ProcessSelector.FindProcess();

				if (proc == null)
					Application.Current.Shutdown();

				return proc;
			});
		}
	}
}
