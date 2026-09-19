using Dalamud.Interface.Textures;
using Dalamud.Interface.Textures.TextureWraps;
using Dalamud.Utility;
using ECommons.DalamudServices;
using Lumina.Data.Files;
using System;
using System.Collections.Generic;
using UltimateCombo.Core;

namespace UltimateCombo.Window;

internal static class Icons
{
    private static readonly Dictionary<(uint IconId, uint StackCount, bool HdIcon), IDalamudTextureWrap> CachedModdedIcons = [];

    internal static IDalamudTextureWrap? GetJobIcon(uint jobId)
    {
        var iconID = jobId == 0
            ? 62146u
            : 62100u + jobId;

        return GetTextureFromIconId(iconID);
    }

    internal static IDalamudTextureWrap? GetTextureFromIconId(uint iconId, uint stackCount = 0, bool hdIcon = true)
    {
        var lookup = new GameIconLookup(iconId + stackCount, false, hdIcon);
        var gamePath = Svc.Texture.GetIconPath(lookup);
        var resolvedPath = Svc.TextureSubstitution.GetSubstitutedPath(gamePath);
        ISharedImmediateTexture sharedTexture = Svc.Texture.GetFromFile(resolvedPath);

        if (sharedTexture.TryGetWrap(out IDalamudTextureWrap? icon, out _))
        {
            return icon;
        }

        (uint iconId, uint stackCount, bool hdIcon) cacheKey = (iconId, stackCount, hdIcon);
        if (CachedModdedIcons.TryGetValue(cacheKey, out IDalamudTextureWrap? cachedIcon))
        {
            return cachedIcon;
        }

        try
        {
            TexFile? textureFile = Svc.Data.GameData.GetFileFromDisk<TexFile>(resolvedPath);
            if (textureFile != null)
            {
                IDalamudTextureWrap output = Svc.Texture.CreateFromRaw(
                    RawImageSpecification.Rgba32(textureFile.Header.Width, textureFile.Header.Height),
                    textureFile.GetRgbaImageData());

                CachedModdedIcons[cacheKey] = output;
                return output;
            }
        }
        catch (Exception ex)
        {
            Service.PluginLog.Verbose(ex, $"Unable to load substituted icon {iconId} from {resolvedPath}");
        }

        return Svc.Texture.GetFromGame(gamePath).GetWrapOrDefault();
    }

    internal static void Dispose()
    {
        foreach (IDalamudTextureWrap icon in CachedModdedIcons.Values)
        {
            icon.Dispose();
        }

        CachedModdedIcons.Clear();
    }
}
