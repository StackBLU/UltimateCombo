using Dalamud.Bindings.ImGui;
using Dalamud.Interface;
using Dalamud.Interface.Textures.TextureWraps;
using ECommons.ImGuiMethods;
using System;
using System.Numerics;

namespace UltimateCombo.Window;

internal static class IconButtons
{
    private static Vector2 GetIconSize(FontAwesomeIcon icon)
    {
        ImGui.PushFont(UiBuilder.IconFont);
        Vector2 iconSize = ImGui.CalcTextSize(icon.ToIconString());
        ImGui.PopFont();
        return iconSize;
    }

    internal static bool IconImageButton(IDalamudTextureWrap texture, string text, Vector2 size = default,
        bool imageOnRight = false, float imageScale = 0, string? id = null)
    {
        var imageSize = new Vector2(texture.Width, texture.Height);
        if (imageScale > 0)
        {
            imageSize *= imageScale;
        }

        Vector2 textSize = ImGui.CalcTextSize(text);
        Vector2 padding = ImGui.GetStyle().FramePadding;
        Vector2 spacing = ImGui.GetStyle().ItemSpacing;
        var contentWidth = imageSize.X + spacing.X + textSize.X;
        var contentHeight = MathF.Max(imageSize.Y, textSize.Y);
        Vector2 buttonSize = size == Vector2.Zero
            ? new Vector2(contentWidth + (padding.X * 2f), contentHeight + (padding.Y * 2f))
            : size;

        ImGui.PushID(id ?? text);
        var clicked = ImGui.Button("##IconImageButton", buttonSize);
        ImGui.PopID();

        Vector2 buttonMin = ImGui.GetItemRectMin();
        Vector2 cursorAfterButton = ImGui.GetCursorScreenPos();
        var contentStartX = buttonMin.X + ((buttonSize.X - contentWidth) / 2f);
        var imageY = buttonMin.Y + ((buttonSize.Y - imageSize.Y) / 2f);
        var textY = buttonMin.Y + ((buttonSize.Y - textSize.Y) / 2f);

        if (imageOnRight)
        {
            DrawText(text, new Vector2(contentStartX, textY));
            ImGui.SetCursorScreenPos(new Vector2(contentStartX + textSize.X + spacing.X, imageY));
            ImGui.Image(texture.Handle, imageSize);
        }
        else
        {
            ImGui.SetCursorScreenPos(new Vector2(contentStartX, imageY));
            ImGui.Image(texture.Handle, imageSize);
            DrawText(text, new Vector2(contentStartX + imageSize.X + spacing.X, textY));
        }

        ImGui.SetCursorScreenPos(cursorAfterButton);
        return clicked;
    }

    internal static bool IconImageButton(string imageUrl, string text, Vector2 size = default,
        bool imageOnRight = false, float imageScale = 0, string? id = null)
    {
        return ThreadLoadImageHandler.TryGetTextureWrap(imageUrl, out IDalamudTextureWrap? texture)
            && texture != null
            && IconImageButton(texture, text, size, imageOnRight, imageScale, id);
    }

    internal static bool IconTextButton(FontAwesomeIcon icon, string text, Vector2 size = default,
        bool iconOnRight = false, string? id = null)
    {
        Vector2 iconSize = GetIconSize(icon);
        Vector2 textSize = ImGui.CalcTextSize(text);
        Vector2 padding = ImGui.GetStyle().FramePadding;
        Vector2 spacing = ImGui.GetStyle().ItemSpacing;
        var contentWidth = iconSize.X + spacing.X + textSize.X;
        var contentHeight = MathF.Max(iconSize.Y, textSize.Y);
        Vector2 buttonSize = size == Vector2.Zero
            ? new Vector2(contentWidth + (padding.X * 2f), contentHeight + (padding.Y * 2f))
            : size;

        ImGui.PushID(id ?? (icon.ToIconString() + text));
        var clicked = ImGui.Button("##IconTextButton", buttonSize);
        ImGui.PopID();

        Vector2 buttonMin = ImGui.GetItemRectMin();
        Vector2 cursorAfterButton = ImGui.GetCursorScreenPos();
        var contentStartX = buttonMin.X + ((buttonSize.X - contentWidth) / 2f);
        var iconY = buttonMin.Y + ((buttonSize.Y - iconSize.Y) / 2f);
        var textY = buttonMin.Y + ((buttonSize.Y - textSize.Y) / 2f);

        if (iconOnRight)
        {
            DrawText(text, new Vector2(contentStartX, textY));
            DrawIcon(icon, new Vector2(contentStartX + textSize.X + spacing.X, iconY));
        }
        else
        {
            DrawIcon(icon, new Vector2(contentStartX, iconY));
            DrawText(text, new Vector2(contentStartX + iconSize.X + spacing.X, textY));
        }

        ImGui.SetCursorScreenPos(cursorAfterButton);
        return clicked;
    }

    private static void DrawText(string text, Vector2 position)
    {
        ImGui.SetCursorScreenPos(position);
        ImGui.TextUnformatted(text);
    }

    private static void DrawIcon(FontAwesomeIcon icon, Vector2 position)
    {
        ImGui.SetCursorScreenPos(position);
        ImGui.PushFont(UiBuilder.IconFont);
        ImGui.TextUnformatted(icon.ToIconString());
        ImGui.PopFont();
    }
}
