﻿using Dalamud.Interface;
using Dalamud.Interface.Textures.TextureWraps;
using ECommons.ImGuiMethods;
using ImGuiNET;
using System.Numerics;

namespace UltimateCombo.Window
{
	public static class IconButtons
	{
		private static Vector2 GetIconSize(FontAwesomeIcon icon)
		{
			ImGui.PushFont(UiBuilder.IconFont);
			Vector2 iconSize = ImGui.CalcTextSize(icon.ToIconString());
			ImGui.PopFont();
			return iconSize;
		}

		public static bool IconImageButton(IDalamudTextureWrap texture, string text, Vector2 size = new(), bool imageOnRight = false, float imageScale = 0)
		{
			bool buttonClicked = false;
			_ = Vector2.Zero;
			Vector2 imageSize = new(texture.Width, texture.Height);
			if (imageScale > 0)
			{
				imageSize.X *= imageScale;
				imageSize.Y *= imageScale;
			}
			Vector2 textSize = ImGui.CalcTextSize(text);
			Vector2 padding = ImGui.GetStyle().FramePadding;
			Vector2 spacing = ImGui.GetStyle().ItemSpacing;

			float buttonSizeX = imageSize.X + textSize.X + (padding.X * 2) + spacing.X;
			float buttonSizeY = (imageSize.Y > textSize.Y ? imageSize.Y : textSize.Y) + (padding.Y * 2);

			Vector2 buttonSize = size == Vector2.Zero ? new Vector2(buttonSizeX, buttonSizeY) : size;

			if (ImGui.Button("###" + text, buttonSize))
			{
				buttonClicked = true;
			}

			ImGui.SameLine();
			if (size == Vector2.Zero)
			{
				ImGui.SetCursorPosX(ImGui.GetCursorPosX() - buttonSize.X - padding.X);
			}
			else
			{
				ImGui.SetCursorPosX((ImGui.GetContentRegionMax().X - textSize.X - size.X) * 0.5f);
				ImGui.SetCursorPosY(ImGui.GetCursorPosY() + padding.Y);
			}
			if (imageOnRight)
			{
				ImGui.Text(text);
				ImGui.SameLine();
				if (size != Vector2.Zero)
				{
					ImGui.SetCursorPosY(ImGui.GetCursorPosY() + padding.Y);
				}

				ImGui.Image(texture.ImGuiHandle, imageSize);

			}
			else
			{

				ImGui.Image(texture.ImGuiHandle, imageSize);

				ImGui.SameLine();
				if (size != Vector2.Zero)
				{
					ImGui.SetCursorPosY(ImGui.GetCursorPosY() + padding.Y);
				}
				ImGui.Text(text);
			}


			return buttonClicked;
		}
		public static bool IconImageButton(string imageUrl, string text, Vector2 size = new(), bool imageOnRight = false, float imageScale = 0)
		{
			bool buttonClicked = false;

			if (ThreadLoadImageHandler.TryGetTextureWrap(imageUrl, out IDalamudTextureWrap? texture))
			{
				buttonClicked = IconImageButton(texture, text, size, imageOnRight, imageScale);
			}

			return buttonClicked;
		}
		public static bool IconTextButton(FontAwesomeIcon icon, string text, Vector2 size = new(), bool iconOnRight = false)
		{
			bool buttonClicked = false;
			_ = Vector2.Zero;
			Vector2 iconSize = GetIconSize(icon);
			Vector2 textSize = ImGui.CalcTextSize(text);
			Vector2 padding = ImGui.GetStyle().FramePadding;
			Vector2 spacing = ImGui.GetStyle().ItemSpacing;

			float buttonSizeX = iconSize.X + textSize.X + (padding.X * 2) + spacing.X;
			float buttonSizeY = (iconSize.Y > textSize.Y ? iconSize.Y : textSize.Y) + (padding.Y * 2);
			Vector2 buttonSize = size == Vector2.Zero ? new Vector2(buttonSizeX, buttonSizeY) : size;

			if (ImGui.Button("###" + icon.ToIconString() + text, buttonSize))
			{
				buttonClicked = true;
			}

			ImGui.SameLine();
			if (size == Vector2.Zero)
			{
				ImGui.SetCursorPosX(ImGui.GetCursorPosX() - buttonSize.X - padding.X);
			}
			else
			{
				ImGui.SetCursorPosX((ImGui.GetContentRegionMax().X - textSize.X - iconSize.X) * 0.5f);
				ImGui.SetCursorPosY(ImGui.GetCursorPosY() + padding.Y);
			}
			if (iconOnRight)
			{
				ImGui.Text(text);
				ImGui.SameLine();
				if (size != Vector2.Zero)
				{
					ImGui.SetCursorPosY(ImGui.GetCursorPosY() + padding.Y);
				}
				ImGui.PushFont(UiBuilder.IconFont);
				ImGui.Text(icon.ToIconString());
				ImGui.PopFont();
			}
			else
			{
				ImGui.PushFont(UiBuilder.IconFont);
				ImGui.Text(icon.ToIconString());
				ImGui.PopFont();
				ImGui.SameLine();
				if (size != Vector2.Zero)
				{
					ImGui.SetCursorPosY(ImGui.GetCursorPosY() + padding.Y);
				}
				ImGui.Text(text);
			}


			return buttonClicked;
		}
	}
}