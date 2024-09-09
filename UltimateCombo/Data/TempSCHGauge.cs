using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;
using ECommons.DalamudServices;
using UltimateCombo.Services;
using System;
using System.Runtime.InteropServices;

namespace UltimateCombo.Data;

public unsafe class TmpSCHGauge
{
	public byte Aetherflow
	{
		get
		{
			return Struct->Aetherflow;
		}
	}

	public byte FairyGauge
	{
		get
		{
			return Struct->FairyGauge;
		}
	}

	public short SeraphTimer
	{
		get
		{
			return Struct->SeraphTimer;
		}
	}

	public DismissedFairy DismissedFairy
	{
		get
		{
			return (DismissedFairy)Struct->DismissedFairy;
		}
	}

	private protected TmpScholarGauge* Struct;

	public TmpSCHGauge()
	{
		Struct = (TmpScholarGauge*)Service.JobGauges.Get<SCHGauge>().Address;
	}
}

public unsafe class TmpPCTGauge
{
	public byte PalleteGauge
	{
		get
		{
			return Struct->PalleteGauge;
		}
	}

	public byte Paint
	{
		get
		{
			return Struct->Paint;
		}
	}

	public bool CreatureMotifDrawn
	{
		get
		{
			return Struct->CreatureMotifDrawn;
		}
	}

	public bool WeaponMotifDrawn
	{
		get
		{
			return Struct->WeaponMotifDrawn;
		}
	}

	public bool LandscapeMotifDrawn
	{
		get
		{
			return Struct->LandscapeMotifDrawn;
		}
	}

	public bool MooglePortraitReady
	{
		get
		{
			return Struct->MooglePortraitReady;
		}
	}

	public bool MadeenPortraitReady
	{
		get
		{
			return Struct->MadeenPortraitReady;
		}
	}

	public CreatureFlags Flags
	{
		get
		{
			return Struct->CreatureFlags;
		}
	}

	private protected PictoGauge* Struct;

	public byte GetOffset(int offset)
	{
		nint val = IntPtr.Add(Address, offset);
		return Marshal.ReadByte(val);
	}

	private readonly nint Address;
	public TmpPCTGauge()
	{
		Address = Svc.SigScanner.GetStaticAddressFromSig("48 8B 3D ?? ?? ?? ?? 33 ED") + 0x8;
		Struct = (PictoGauge*)Address;
	}
}

[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct TmpScholarGauge
{
	[FieldOffset(0x08)] public byte Aetherflow;
	[FieldOffset(0x09)] public byte FairyGauge;
	[FieldOffset(0x0A)] public short SeraphTimer;
	[FieldOffset(0x0C)] public byte DismissedFairy;
}

[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct PictoGauge
{
	[FieldOffset(0x08)] public byte PalleteGauge;
	[FieldOffset(0x0A)] public byte Paint;
	[FieldOffset(0x0B)] public CanvasFlags CanvasFlags;
	[FieldOffset(0x0C)] public CreatureFlags CreatureFlags;

	public bool CreatureMotifDrawn
	{
		get
		{
			return CanvasFlags.HasFlag(CanvasFlags.Pom) || CanvasFlags.HasFlag(CanvasFlags.Wing) || CanvasFlags.HasFlag(CanvasFlags.Claw) || CanvasFlags.HasFlag(CanvasFlags.Maw);
		}
	}

	public bool WeaponMotifDrawn
	{
		get
		{
			return CanvasFlags.HasFlag(CanvasFlags.Weapon);
		}
	}

	public bool LandscapeMotifDrawn
	{
		get
		{
			return CanvasFlags.HasFlag(CanvasFlags.Landscape);
		}
	}

	public bool MooglePortraitReady
	{
		get
		{
			return CreatureFlags.HasFlag(CreatureFlags.MooglePortrait);
		}
	}

	public bool MadeenPortraitReady
	{
		get
		{
			return CreatureFlags.HasFlag(CreatureFlags.MadeenPortrait);
		}
	}
}

[Flags]
public enum CanvasFlags : byte
{
	Pom = 1,
	Wing = 2,
	Claw = 4,
	Maw = 8,
	Weapon = 16,
	Landscape = 32,
}

[Flags]
public enum CreatureFlags : byte
{
	Pom = 1,
	Wings = 2,
	Claw = 4,

	MooglePortrait = 16,
	MadeenPortrait = 32,
}
