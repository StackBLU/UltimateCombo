using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using System;
using System.Numerics;

namespace UltimateCombo.Window;

internal class InfoBox
{
    internal Vector4 Color { get; set; } = Colors.White;
    internal Action ContentsAction { get; set; } = () => ImGui.TextUnformatted("Action Not Set");
    internal float CurveRadius { get; set; } = 1f;
    internal float ContentsOffset { get; set; }
    internal Vector2 Size { get; set; } = Vector2.Zero;
    internal float BorderThickness { get; set; } = 2f;
    internal int SegmentResolution { get; set; } = 10;
    internal Vector2 Offset { get; set; } = Vector2.Zero;
    internal string Label { get; set; } = "";
    internal bool AutoResize { get; set; } = true;
    internal bool Debug { get; set; }
    internal bool HasMaxWidth { get; set; }
    internal bool IsSubBox { get; set; }

    private static ImDrawListPtr DrawList => ImGui.GetWindowDrawList();
    private uint ColorU32 => ImGui.GetColorU32(Color);
    private Vector2 StartPosition { get; set; }

    internal void Draw()
    {
        StartPosition = ImGui.GetCursorScreenPos() + Offset;

        if (Size.X <= 0)
        {
            Size = Size with { X = ImGui.GetContentRegionAvail().X };
        }

        if (Debug)
        {
            DrawList.AddCircleFilled(StartPosition, 2f, ImGui.GetColorU32(Colors.Purple));
        }

        DrawContents();

        var contentWidth = ImGui.GetItemRectMax().X - ImGui.GetItemRectMin().X;
        var contentHeight = ImGui.GetItemRectMax().Y - ImGui.GetItemRectMin().Y;

        if (Size.Y <= 0 || AutoResize)
        {
            Size = Size with { Y = contentHeight + (CurveRadius * 2f) };
        }

        if (HasMaxWidth && contentWidth + 15f < ImGui.GetContentRegionAvail().X)
        {
            Size = Size with { X = contentWidth + 6f };
        }

        DrawCorners();
        DrawBorders();
    }

    internal void DrawCentered(float percentSize = 0.80f)
    {
        Vector2 region = ImGui.GetContentRegionAvail();
        Vector2 currentPosition = ImGui.GetCursorPos();
        var width = region.X * percentSize;

        ImGui.SetCursorPosX(currentPosition.X + ((region.X - width) / 2f));
        Size = Size with { X = width };
        Draw();
    }

    private void DrawContents()
    {
        var contentPosition = new Vector2(
            StartPosition.X + CurveRadius + ContentsOffset,
            StartPosition.Y + CurveRadius + ContentsOffset);

        ImGui.SetCursorScreenPos(contentPosition);
        ImGui.PushTextWrapPos(ImGui.GetCursorPosX() + Size.X - (CurveRadius * 2f) - (ContentsOffset * 2f));

        using (ImRaii.GroupDisposable group = ImRaii.Group())
        {
            ImGui.PushID(Label);
            ContentsAction();

            if (ContentsOffset > 0)
            {
                ImGuiHelpers.ScaledDummy(ContentsOffset);
            }

            ImGui.PopID();
        }

        ImGui.PopTextWrapPos();
    }

    private void DrawCorners()
    {
        var topLeft = new Vector2(StartPosition.X + CurveRadius, StartPosition.Y + CurveRadius);
        var topRight = new Vector2(StartPosition.X + Size.X - CurveRadius, StartPosition.Y + CurveRadius);
        var bottomLeft = new Vector2(StartPosition.X + CurveRadius, StartPosition.Y + Size.Y - CurveRadius);
        var bottomRight = new Vector2(StartPosition.X + Size.X - CurveRadius, StartPosition.Y + Size.Y - CurveRadius);

        DrawList.PathArcTo(topLeft, CurveRadius, DegreesToRadians(180), DegreesToRadians(270), SegmentResolution);
        DrawList.PathStroke(ColorU32, ImDrawFlags.None, BorderThickness);

        DrawList.PathArcTo(topRight, CurveRadius, DegreesToRadians(360), DegreesToRadians(270), SegmentResolution);
        DrawList.PathStroke(ColorU32, ImDrawFlags.None, BorderThickness);

        DrawList.PathArcTo(bottomLeft, CurveRadius, DegreesToRadians(90), DegreesToRadians(180), SegmentResolution);
        DrawList.PathStroke(ColorU32, ImDrawFlags.None, BorderThickness);

        DrawList.PathArcTo(bottomRight, CurveRadius, DegreesToRadians(0), DegreesToRadians(90), SegmentResolution);
        DrawList.PathStroke(ColorU32, ImDrawFlags.None, BorderThickness);

        if (Debug)
        {
            DrawList.AddCircleFilled(topLeft, 2f, ImGui.GetColorU32(Colors.Red));
            DrawList.AddCircleFilled(topRight, 2f, ImGui.GetColorU32(Colors.Green));
            DrawList.AddCircleFilled(bottomLeft, 2f, ImGui.GetColorU32(Colors.Blue));
            DrawList.AddCircleFilled(bottomRight, 2f, ImGui.GetColorU32(Colors.Orange));
        }
    }

    private void DrawBorders()
    {
        var color = Debug ? ImGui.GetColorU32(Colors.Red) : ColorU32;

        DrawList.AddLine(new Vector2(StartPosition.X - 0.5f, StartPosition.Y + CurveRadius - 0.5f), new Vector2(StartPosition.X - 0.5f, StartPosition.Y + Size.Y - CurveRadius + 0.5f), color, BorderThickness);
        DrawList.AddLine(new Vector2(StartPosition.X + Size.X - 0.5f, StartPosition.Y + CurveRadius - 0.5f), new Vector2(StartPosition.X + Size.X - 0.5f, StartPosition.Y + Size.Y - CurveRadius + 0.5f), color, BorderThickness);
        DrawList.AddLine(new Vector2(StartPosition.X + CurveRadius - 0.5f, StartPosition.Y + Size.Y - 0.5f), new Vector2(StartPosition.X + Size.X - CurveRadius + 0.5f, StartPosition.Y + Size.Y - 0.5f), color, BorderThickness);

        Vector2 textSize = ImGui.CalcTextSize(Label);
        var textPadding = textSize.X > 0 ? 7f * ImGuiHelpers.GlobalScale : 0f;
        var textVerticalOffset = textSize.X > 0 ? textSize.Y / 2f : 0f;

        DrawList.AddText(new Vector2(StartPosition.X + CurveRadius + textPadding, StartPosition.Y - textVerticalOffset), ColorU32, Label);
        DrawList.AddLine(new Vector2(StartPosition.X + CurveRadius + textPadding + textSize.X + textPadding, StartPosition.Y - 0.5f), new Vector2(StartPosition.X + Size.X - CurveRadius + 0.5f, StartPosition.Y - 0.5f), color, BorderThickness);
    }

    private static float DegreesToRadians(float degrees)
    {
        return MathF.PI / 180f * degrees;
    }
}

internal static class Colors
{
    internal static readonly Vector4 Purple = new(176 / 255f, 38 / 255f, 236 / 255f, 1f);
    internal static readonly Vector4 Blue = new(37 / 255f, 168 / 255f, 1f, 1f);
    internal static readonly Vector4 ForestGreen = new(0.133f, 0.545f, 0.1333f, 1f);
    internal static readonly Vector4 White = new(1f, 1f, 1f, 1f);
    internal static readonly Vector4 Red = new(1f, 0f, 0f, 1f);
    internal static readonly Vector4 Green = new(0f, 1f, 0f, 1f);
    internal static readonly Vector4 Black = new(0f, 0f, 0f, 1f);
    internal static readonly Vector4 HealerGreen = new(33 / 255f, 193 / 255f, 0f, 1f);
    internal static readonly Vector4 DPSRed = new(210 / 255f, 42 / 255f, 43 / 255f, 1f);
    internal static readonly Vector4 SoftRed = new(0.8f, 0.2f, 0.2f, 1f);
    internal static readonly Vector4 Grey = new(0.4f, 0.4f, 0.4f, 1f);
    internal static readonly Vector4 Orange = new(1f, 165 / 255f, 0f, 1f);
    internal static readonly Vector4 SoftGreen = new(0.2f, 0.8f, 0.2f, 1f);
}
