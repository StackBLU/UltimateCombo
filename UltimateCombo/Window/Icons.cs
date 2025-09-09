using Dalamud.Interface.Textures;
using Dalamud.Interface.Textures.TextureWraps;
using Dalamud.Utility;
using ECommons.DalamudServices;
using Lumina.Data.Files;
using System.Collections.Generic;

namespace UltimateCombo.Window;

internal static class Icons
{
    internal static Dictionary<uint, IDalamudTextureWrap> CachedModdedIcons = [];

    internal static IDalamudTextureWrap? GetJobIcon(uint jobId)
    {
        if ((jobId > 42 & jobId <= 50) || jobId >= 52)
        {
            return null;
        }

        var iconNum = 62100u;
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

    internal static IDalamudTextureWrap? GetTextureFromIconId(uint iconId, uint stackCount = 0, bool hdIcon = true)
    {
        var lookup = new GameIconLookup(iconId + stackCount, false, hdIcon);
        var path = Svc.Texture.GetIconPath(lookup);
        var resolvePath = ResolvePath(path);
        ISharedImmediateTexture wrap = Svc.Texture.GetFromFile(resolvePath);

        if (wrap.TryGetWrap(out IDalamudTextureWrap? icon, out _))
        {
            return icon;
        }

        try
        {
            if (CachedModdedIcons.TryGetValue(iconId, out IDalamudTextureWrap? value))
            {
                return value;
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
