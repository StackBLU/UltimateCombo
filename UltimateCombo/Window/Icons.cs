using Dalamud.Interface.Textures;
using Dalamud.Interface.Textures.TextureWraps;
using Dalamud.Utility;
using ECommons.DalamudServices;
using Lumina.Data.Files;
using System.Collections.Generic;

namespace UltimateCombo.Window
{
	internal static class Icons
	{
		public static Dictionary<uint, IDalamudTextureWrap> CachedModdedIcons = [];
		public static IDalamudTextureWrap? GetJobIcon(uint jobId)
		{
			if ((jobId > 42 & jobId <= 50) || jobId >= 52)
			{
				return null;
			}

			uint iconNum = 62100;

			if (jobId is 51)
			{
				iconNum = 62118;
			}
			else
			{
				iconNum += jobId;
			}

			if (iconNum == 62100)
			{
				iconNum = 62146;
			}

			IDalamudTextureWrap? icon = GetTextureFromIconId(iconNum);

			return icon;
		}

		private static string ResolvePath(string path)
		{
			return Svc.TextureSubstitution.GetSubstitutedPath(path);
		}

		public static IDalamudTextureWrap? GetTextureFromIconId(uint iconId, uint stackCount = 0, bool hdIcon = true)
		{
			GameIconLookup lookup = new(iconId + stackCount, false, hdIcon);
			string path = Svc.Texture.GetIconPath(lookup);
			string resolvePath = ResolvePath(path);

			ISharedImmediateTexture wrap = Svc.Texture.GetFromFile(resolvePath);
			if (wrap.TryGetWrap(out IDalamudTextureWrap? icon, out _))
			{
				return icon;
			}

			try
			{
				if (CachedModdedIcons.ContainsKey(iconId))
				{
					return CachedModdedIcons[iconId];
				}

				TexFile tex = Svc.Data.GameData.GetFileFromDisk<TexFile>(resolvePath);
				IDalamudTextureWrap output = Svc.Texture.CreateFromRaw(RawImageSpecification.Rgba32(tex.Header.Width, tex.Header.Width), tex.GetRgbaImageData());
				if (output != null)
				{
					CachedModdedIcons[iconId] = output;
					return output;
				}
			}
			catch { }

			return Svc.Texture.GetFromGame(path).GetWrapOrDefault();
		}
	}
}