using System;

using UltimateCombo.Combos;

namespace UltimateCombo.Attributes
{
	/// <summary> Attribute documenting conflicting presets for each combo. </summary>
	[AttributeUsage(AttributeTargets.Field)]
	internal class ConflictingCombosAttribute : Attribute
	{
		/// <summary> Initializes a new instance of the <see cref="ConflictingCombosAttribute"/> class. </summary>
		/// <param name="conflictingPresets"> Presets that conflict with the given combo. </param>
		internal ConflictingCombosAttribute(params CustomComboPreset[] conflictingPresets)
		{
			ConflictingPresets = conflictingPresets;
		}

		/// <summary> Gets the display name. </summary>
		public CustomComboPreset[] ConflictingPresets { get; }
	}
}
