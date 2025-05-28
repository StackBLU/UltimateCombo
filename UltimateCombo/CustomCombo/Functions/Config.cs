using System;
using System.Linq;
using UltimateCombo.Core;
using UltimateCombo.Services;

namespace UltimateCombo.ComboHelper.Functions
{
	internal abstract partial class CustomComboFunctions
	{
		public static int GetOptionValue(string SliderID)
		{
			return PluginConfiguration.GetCustomIntValue(SliderID);
		}

		public static bool GetIntOptionAsBool(string SliderID)
		{
			return Convert.ToBoolean(GetOptionValue(SliderID));
		}

		public static bool GetOptionBool(string SliderID)
		{
			return PluginConfiguration.GetCustomBoolValue(SliderID);
		}

		public static float GetOptionFloat(string SliderID)
		{
			return PluginConfiguration.GetCustomFloatValue(SliderID);
		}
	}

	public class UserData(string v)
	{
		protected string pName = v;

		public static implicit operator string(UserData o)
		{
			return o.pName;
		}
	}

	public class UserFloat : UserData
	{
		// Constructor with only the string parameter
		public UserFloat(string v) : this(v, 0.0f) { }

		// Constructor with both string and float parameters
		public UserFloat(string v, float defaults) : base(v) // Overload constructor to preload data
		{
			if (!PluginConfiguration.CustomFloatValues.ContainsKey(pName)) // if it isn't there, set
			{
				PluginConfiguration.SetCustomFloatValue(pName, defaults);
				Service.Configuration.Save();
			}
		}

		// Implicit conversion to float
		public static implicit operator float(UserFloat o)
		{
			return PluginConfiguration.GetCustomFloatValue(o.pName);
		}
	}

	public class UserInt : UserData
	{
		// Constructor with only the string parameter
		public UserInt(string v) : this(v, 0) { } // Chaining to the other constructor with a default value

		// Constructor with both string and int parameters
		public UserInt(string v, int defaults) : base(v) // Overload constructor to preload data
		{
			if (!PluginConfiguration.CustomIntValues.ContainsKey(pName)) // if it isn't there, set
			{
				PluginConfiguration.SetCustomIntValue(pName, defaults);
				Service.Configuration.Save();
			}
		}

		// Implicit conversion to int
		public static implicit operator int(UserInt o)
		{
			return PluginConfiguration.GetCustomIntValue(o.pName);
		}
	}

	public class UserBool : UserData
	{
		// Constructor with only the string parameter
		public UserBool(string v) : this(v, false) { }

		// Constructor with both string and bool parameters
		public UserBool(string v, bool defaults) : base(v) // Overload constructor to preload data
		{
			if (!PluginConfiguration.CustomBoolValues.ContainsKey(pName)) // if it isn't there, set
			{
				PluginConfiguration.SetCustomBoolValue(pName, defaults);
				Service.Configuration.Save();
			}
		}

		// Implicit conversion to bool
		public static implicit operator bool(UserBool o)
		{
			return PluginConfiguration.GetCustomBoolValue(o.pName);
		}
	}

	public class UserIntArray(string v) : UserData(v)
	{
		public string Name
		{
			get
			{
				return pName;
			}
		}

		public int Count
		{
			get
			{
				return PluginConfiguration.GetCustomIntArrayValue(pName).Length;
			}
		}

		public bool Any(Func<int, bool> func)
		{
			return PluginConfiguration.GetCustomIntArrayValue(pName).Any(func);
		}

		public int[] Items
		{
			get
			{
				return PluginConfiguration.GetCustomIntArrayValue(pName);
			}
		}

		public int IndexOf(int item)
		{
			for (int i = 0; i < Count; i++)
			{
				if (Items[i] == item)
				{
					return i;
				}
			}
			return -1;
		}

		public void Clear(int maxValues)
		{
			int[] array = PluginConfiguration.GetCustomIntArrayValue(pName);
			Array.Resize<int>(ref array, maxValues);
			PluginConfiguration.SetCustomIntArrayValue(pName, array);
			Service.Configuration.Save();
		}

		public static implicit operator int[](UserIntArray o)
		{
			return PluginConfiguration.GetCustomIntArrayValue(o.pName);
		}

		public int this[int index]
		{
			get
			{
				if (index >= Count)
				{
					int[] array = PluginConfiguration.GetCustomIntArrayValue(pName);
					Array.Resize(ref array, index + 1);
					array[index] = 0;
					PluginConfiguration.SetCustomIntArrayValue(pName, array);
					Service.Configuration.Save();
				}
				return PluginConfiguration.GetCustomIntArrayValue(pName)[index];
			}
			set
			{
				if (index < Count)
				{
					int[] array = PluginConfiguration.GetCustomIntArrayValue(pName);
					array[index] = value;
					Service.Configuration.Save();
				}
			}
		}
	}

	internal class UserBoolArray(string v) : UserData(v)
	{
		public int Count
		{
			get
			{
				return PluginConfiguration.GetCustomBoolArrayValue(pName).Length;
			}
		}

		public static implicit operator bool[](UserBoolArray o)
		{
			return PluginConfiguration.GetCustomBoolArrayValue(o.pName);
		}

		public bool this[int index]
		{
			get
			{
				if (index >= Count)
				{
					bool[] array = PluginConfiguration.GetCustomBoolArrayValue(pName);
					Array.Resize(ref array, index + 1);
					array[index] = false;
					PluginConfiguration.SetCustomBoolArrayValue(pName, array);
					Service.Configuration.Save();
				}
				return PluginConfiguration.GetCustomBoolArrayValue(pName)[index];
			}
		}

		public bool All(Func<bool, bool> predicate)
		{
			bool[] array = PluginConfiguration.GetCustomBoolArrayValue(pName);
			return array.All(predicate);

		}
	}

}
