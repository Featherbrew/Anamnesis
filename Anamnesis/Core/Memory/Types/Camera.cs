﻿// Concept Matrix 3.
// Licensed under the MIT license.

namespace Anamnesis.Memory.Types
{
	using System;
	using System.Runtime.InteropServices;
	using System.Windows.Media.Media3D;
	using Anamnesis.ThreeD;
	using PropertyChanged;

	[StructLayout(LayoutKind.Explicit)]
	public struct Camera
	{
		[FieldOffset(0x130)] public Vector2D Angle;
		[FieldOffset(0x14C)] public float YMin;
		[FieldOffset(0x148)] public float YMax;
		[FieldOffset(0x150)] public Vector2D Pan;
		[FieldOffset(0x160)] public float Rotation;
		[FieldOffset(0x114)] public float Zoom;
		[FieldOffset(0x118)] public float MinZoom;
		[FieldOffset(0x11C)] public float MaxZoom;
		[FieldOffset(0x12C)] public float FieldOfView;
	}

	[AddINotifyPropertyChangedInterface]
	public class CameraViewModel : MemoryViewModelBase<Camera>
	{
		public CameraViewModel(IntPtr pointer, IStructViewModel? parent = null)
			: base(pointer, parent)
		{
		}

		[ModelField] public Vector2D Angle { get; set; }
		[ModelField] public float YMin { get; set; }
		[ModelField] public float YMax { get; set; }
		[ModelField] public Vector2D Pan { get; set; }
		[ModelField] public float Rotation { get; set; }
		[ModelField] public float Zoom { get; set; }
		[ModelField] public float MinZoom { get; set; }
		[ModelField] public float MaxZoom { get; set; }
		[ModelField] public float FieldOfView { get; set; }

		[AlsoNotifyFor(nameof(CameraViewModel.Angle), nameof(CameraViewModel.Rotation))]
		public Quaternion Rotation3d
		{
			get
			{
				Vector3D camEuler = default;
				camEuler.Y = (float)MathUtils.RadiansToDegrees((double)this.Angle.X) - 180;
				camEuler.Z = (float)-MathUtils.RadiansToDegrees((double)this.Angle.Y);
				camEuler.X = (float)MathUtils.RadiansToDegrees((double)this.Rotation);
				return camEuler.ToQuaternion();
			}
		}
	}
}
