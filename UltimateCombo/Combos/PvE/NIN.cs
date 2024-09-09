using Dalamud.Game.ClientState.JobGauge.Types;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;
using UltimateCombo.Data;
using System.Collections.Generic;

namespace UltimateCombo.Combos.PvE
{
	internal class NIN
	{
		public const byte JobID = 30;

		public const uint
			SpinningEdge = 2240,
			ShadeShift = 2241,
			GustSlash = 2242,
			Hide = 2245,
			Assassinate = 2246,
			ThrowingDaggers = 2247,
			Mug = 2248,
			DeathBlossom = 2254,
			AeolianEdge = 2255,
			TrickAttack = 2258,
			Kassatsu = 2264,
			ArmorCrush = 3563,
			DreamWithinADream = 3566,
			TenChiJin = 7403,
			Bhavacakra = 7402,
			HakkeMujinsatsu = 16488,
			Meisui = 16489,
			Bunshin = 16493,
			PhantomKamaitachi = 25774,
			ForkedRaiju = 25777,
			FleetingRaiju = 25778,
			Hellfrog = 7401,
			HollowNozuchi = 25776,
			KunaisBane = 36958,
			Dokumori = 36957,
			Ninjutsu = 2260,
			Rabbit = 2272,
			Ten = 2259,
			Chi = 2261,
			Shukuchi = 2262,
			Jin = 2263,
			TenCombo = 18805,
			ChiCombo = 18806,
			JinCombo = 18807,
			FumaShuriken = 2265,
			Hyoton = 2268,
			Doton = 2270,
			Katon = 2266,
			Suiton = 2271,
			Raiton = 2267,
			Huton = 2269,
			GokaMekkyaku = 16491,
			HyoshoRanryu = 16492,
			TCJFumaShurikenTen = 18873,
			TCJFumaShurikenChi = 18874,
			TCJFumaShurikenJin = 18875,
			TCJKaton = 18876,
			TCJRaiton = 18877,
			TCJHyoton = 18878,
			TCJHuton = 2269,
			TCJDoton = 18880,
			TCJSuiton = 18881,
			DeathfrogMedium = 36960,
			ZeshoMeppo = 36960,
			TenriJindo = 36961;

		public static class Buffs
		{
			public const ushort
				Mudra = 496,
				Kassatsu = 497,
				Suiton = 507,
				Hidden = 614,
				TenChiJin = 1186,
				AssassinateReady = 1955,
				RaijuReady = 2690,
				PhantomReady = 2723,
				Meisui = 2689,
				Doton = 501,
				Bunshin = 1954,
				ShadowWalker = 3848,
				Higi = 3850,
				TenriJindo = 3851;
		}

		internal static class Debuffs
		{
			internal const ushort
				TrickAttack = 3254,
				KunaisBane = 3906,
				Mug = 3183,
				Dokumori = 3849;
		}

		internal static Dictionary<uint, ushort>
			TrickList = new() {
				{ TrickAttack,  Debuffs.TrickAttack  },
				{ KunaisBane, Debuffs.KunaisBane }
			};

		internal static Dictionary<uint, ushort>
			MugList = new() {
				{ Mug,  Debuffs.Mug  },
				{ Dokumori, Debuffs.Dokumori }
			};

		private static NINGauge Gauge
		{
			get
			{
				return CustomComboFunctions.GetJobGauge<NINGauge>();
			}
		}

		internal class NIN_ST_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.NIN_ST_DPS;

			protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
			{
				if ((actionID is SpinningEdge or GustSlash or AeolianEdge or ArmorCrush) && IsEnabled(CustomComboPreset.NIN_ST_DPS))
				{
					if (IsEnabled(CustomComboPreset.NIN_ST_Mudras) && !InCombat() && GetRemainingCharges(Ten) != GetMaxCharges(Ten)
						&& !HasEffect(Buffs.Mudra))
					{
						return Hide;
					}

					if (HasEffect(Buffs.Hidden))
					{
						return Shukuchi;
					}

					if (CanWeave(actionID) && !HasEffect(Buffs.Mudra) && !HasEffect(Buffs.TenChiJin))
					{
						if (IsEnabled(CustomComboPreset.NIN_ST_Kassatsu) && ActionReady(Kassatsu))
						{
							return Kassatsu;
						}

						if (ActionWatching.NumberOfGcdsUsed >= 2)
						{
							if (IsEnabled(CustomComboPreset.NIN_ST_Mug) && ActionReady(OriginalHook(Mug)))
							{
								return OriginalHook(Mug);
							}

							if (IsEnabled(CustomComboPreset.NIN_ST_Bunshin) && ActionReady(Bunshin) && Gauge.Ninki >= 50)
							{
								return Bunshin;
							}

							if (IsEnabled(CustomComboPreset.NIN_ST_Trick) && ActionReady(OriginalHook(TrickAttack)) && HasEffect(Buffs.ShadowWalker))
							{
								return OriginalHook(TrickAttack);
							}

							if (IsEnabled(CustomComboPreset.NIN_ST_Assassinate) && ActionReady(OriginalHook(Assassinate)))
							{
								return OriginalHook(Assassinate);
							}

							if (IsEnabled(CustomComboPreset.NIN_ST_TenChiJin) && HasEffect(Buffs.TenriJindo))
							{
								return TenriJindo;
							}

							if (IsEnabled(CustomComboPreset.NIN_ST_TenChiJin) && ActionReady(TenChiJin) && !HasEffect(Buffs.ShadowWalker)
								&& (GetCooldownRemainingTime(OriginalHook(TrickAttack)) <= 15
								|| (GetCooldownRemainingTime(Meisui) <= 15 && LevelChecked(Meisui))))
							{
								return TenChiJin;
							}

							if (IsEnabled(CustomComboPreset.NIN_ST_Meisui) && ActionReady(Meisui) && HasEffect(Buffs.ShadowWalker))
							{
								return Meisui;
							}

							if (IsEnabled(CustomComboPreset.NIN_ST_Bhav) && HasEffect(Buffs.Higi))
							{
								return ZeshoMeppo;
							}

							if (IsEnabled(CustomComboPreset.NIN_ST_Bhav) && ActionReady(Bhavacakra)
								&& (Gauge.Ninki >= 70
								|| (Gauge.Ninki >= 50 && (TargetHasEffect(TrickList[OriginalHook(TrickAttack)]) || TargetHasEffect(MugList[OriginalHook(Mug)])))))
							{
								return Bhavacakra;
							}
						}
					}

					if (IsEnabled(CustomComboPreset.NIN_ST_Mudras))
					{
						if (HasEffect(Buffs.TenChiJin))
						{
							if (OriginalHook(Jin) is TCJSuiton)
							{
								return TCJSuiton;
							}

							if (OriginalHook(Chi) is TCJRaiton)
							{
								return TCJRaiton;
							}

							if (OriginalHook(Ten) is TCJFumaShurikenTen)
							{
								return TCJFumaShurikenTen;
							}
						}

						if (HasEffect(Buffs.Kassatsu) && (TargetHasEffect(TrickList[OriginalHook(TrickAttack)]) || !LevelChecked(HyoshoRanryu)))
						{
							if (LevelChecked(HyoshoRanryu))
							{
								if (OriginalHook(Ninjutsu) is HyoshoRanryu)
								{
									return OriginalHook(Ninjutsu);
								}
								if (WasLastAbility(TenCombo))
								{
									return JinCombo;
								}
								return TenCombo;
							}

							if (OriginalHook(Ninjutsu) is Raiton)
							{
								return OriginalHook(Ninjutsu);
							}
							if (WasLastAbility(TenCombo))
							{
								return ChiCombo;
							}
							return TenCombo;
						}

						if (GetCooldownRemainingTime(OriginalHook(TrickAttack)) > 15
							&& (GetCooldownRemainingTime(Meisui) > 15 || !LevelChecked(Meisui))
							&& ActionWatching.NumberOfGcdsUsed >= 2 && (HasEffect(Buffs.Mudra) || HasCharges(Ten)))
						{
							if (OriginalHook(Ninjutsu) is Raiton)
							{
								return OriginalHook(Ninjutsu);
							}
							if (WasLastAbility(Ten))
							{
								return ChiCombo;
							}
							return Ten;
						}

						if (((GetCooldownRemainingTime(OriginalHook(TrickAttack)) <= 15)
							|| (GetCooldownRemainingTime(Meisui) <= 15 && LevelChecked(Meisui))) && !HasEffect(Buffs.ShadowWalker)
							&& (HasEffect(Buffs.Mudra) || HasCharges(Ten)))
						{
							if (OriginalHook(Ninjutsu) is Suiton)
							{
								return OriginalHook(Ninjutsu);
							}
							if (WasLastAbility(ChiCombo))
							{
								return JinCombo;
							}
							if (WasLastAbility(Ten))
							{
								return ChiCombo;
							}
							return Ten;
						}
					}

					if (IsEnabled(CustomComboPreset.NIN_ST_Bunshin) && HasEffect(Buffs.PhantomReady))
					{
						return PhantomKamaitachi;
					}

					if (IsEnabled(CustomComboPreset.NIN_ST_Raiju) && HasEffect(Buffs.RaijuReady))
					{
						if (!InActionRange(FleetingRaiju))
						{
							return ForkedRaiju;
						}
						return FleetingRaiju;
					}

					if (comboTime > 0)
					{
						if (lastComboActionID is GustSlash)
						{
							if ((ActionReady(AeolianEdge) && (Gauge.Kazematoi > 0 || !LevelChecked(ArmorCrush))
								&& (TargetHasEffect(TrickList[OriginalHook(TrickAttack)]) || TargetHasEffect(MugList[OriginalHook(Mug)])))
								|| Gauge.Kazematoi > 3 || !LevelChecked(ArmorCrush))
							{
								return AeolianEdge;
							}

							if (ActionReady(ArmorCrush))
							{
								return ArmorCrush;
							}
						}

						if (lastComboActionID is SpinningEdge)
						{
							return GustSlash;
						}
					}

					return SpinningEdge;
				}

				return actionID;
			}
		}

		internal class NIN_AoE_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.NIN_AoE_DPS;

			protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
			{
				if ((actionID is DeathBlossom or HakkeMujinsatsu) && IsEnabled(CustomComboPreset.NIN_AoE_DPS))
				{
					if (IsEnabled(CustomComboPreset.NIN_AoE_Mudras) && !InCombat() && GetRemainingCharges(Ten) != GetMaxCharges(Ten)
						&& !HasEffect(Buffs.Mudra))
					{
						return Hide;
					}

					if (WasLastAbility(Hide) || HasEffect(Buffs.Hidden))
					{
						return Shukuchi;
					}

					if (CanWeave(actionID) && !HasEffect(Buffs.Mudra) && !HasEffect(Buffs.TenChiJin))
					{
						if (IsEnabled(CustomComboPreset.NIN_AoE_Kassatsu) && ActionReady(Kassatsu))
						{
							return Kassatsu;
						}

						if (IsEnabled(CustomComboPreset.NIN_AoE_Trick) && ActionReady(OriginalHook(TrickAttack)) && HasEffect(Buffs.ShadowWalker))
						{
							return OriginalHook(TrickAttack);
						}

						if (IsEnabled(CustomComboPreset.NIN_AoE_TenChiJin) && HasEffect(Buffs.TenriJindo))
						{
							return TenriJindo;
						}

						if (IsEnabled(CustomComboPreset.NIN_AoE_TenChiJin) && ActionReady(TenChiJin) && !HasEffect(Buffs.ShadowWalker)
							&& (GetCooldownRemainingTime(OriginalHook(TrickAttack)) <= 15
							|| (GetCooldownRemainingTime(Meisui) <= 15 && LevelChecked(Meisui))))
						{
							return TenChiJin;
						}

						if (IsEnabled(CustomComboPreset.NIN_AoE_Meisui) && ActionReady(Meisui) && HasEffect(Buffs.ShadowWalker) && Gauge.Ninki <= 50)
						{
							return Meisui;
						}

						if (IsEnabled(CustomComboPreset.NIN_AoE_Bunshin) && ActionReady(Bunshin) && Gauge.Ninki >= 50)
						{
							return Bunshin;
						}

						if (IsEnabled(CustomComboPreset.NIN_AoE_Hellfrog) && HasEffect(Buffs.Higi))
						{
							return DeathfrogMedium;
						}

						if (IsEnabled(CustomComboPreset.NIN_AoE_Hellfrog) && ActionReady(Hellfrog) && Gauge.Ninki >= 50)
						{
							return Hellfrog;
						}

						if (IsEnabled(CustomComboPreset.NIN_AoE_Assassinate) && ActionReady(OriginalHook(Assassinate)))
						{
							return OriginalHook(Assassinate);
						}
					}

					if (IsEnabled(CustomComboPreset.NIN_AoE_Mudras))
					{
						if (HasEffect(Buffs.TenChiJin))
						{
							if (HasEffect(Buffs.Doton))
							{
								if (OriginalHook(Ten) is TCJHuton)
								{
									return TCJHuton;
								}

								if (OriginalHook(Chi) is TCJRaiton)
								{
									return TCJRaiton;
								}

								if (OriginalHook(Jin) is TCJFumaShurikenJin)
								{
									return TCJFumaShurikenJin;
								}
							}

							if (OriginalHook(Chi) is TCJDoton)
							{
								return TCJDoton;
							}

							if (OriginalHook(Ten) is TCJKaton)
							{
								return TCJKaton;
							}

							if (OriginalHook(Jin) is TCJFumaShurikenJin)
							{
								return TCJFumaShurikenJin;
							}
						}

						if (HasEffect(Buffs.Kassatsu)
							&& (TargetHasEffect(TrickList[OriginalHook(TrickAttack)]) || GetBuffRemainingTime(Buffs.Kassatsu) < 5))
						{
							if (OriginalHook(Ninjutsu) is GokaMekkyaku)
							{
								return OriginalHook(Ninjutsu);
							}
							if (WasLastAbility(ChiCombo))
							{
								return TenCombo;
							}
							return ChiCombo;
						}

						if (GetCooldownRemainingTime(OriginalHook(TrickAttack)) > 15
							&& (GetCooldownRemainingTime(Meisui) > 15 || !LevelChecked(Meisui))
							&& (HasEffect(Buffs.Mudra) || HasCharges(Ten)))
						{
							if (HasEffect(Buffs.Doton))
							{
								if (OriginalHook(Ninjutsu) is Katon)
								{
									return OriginalHook(Ninjutsu);
								}
								if (WasLastAbility(Chi))
								{
									return TenCombo;
								}
								return Chi;
							}

							if (OriginalHook(Ninjutsu) is Doton)
							{
								return OriginalHook(Ninjutsu);
							}
							if (WasLastAbility(Jin))
							{
								return ChiCombo;
							}
							if (WasLastAbility(Ten))
							{
								return JinCombo;
							}
							return Ten;
						}

						if ((GetCooldownRemainingTime(OriginalHook(TrickAttack)) <= 15
							|| (GetCooldownRemainingTime(Meisui) <= 15 && LevelChecked(Meisui))) && !HasEffect(Buffs.ShadowWalker)
							&& (HasEffect(Buffs.Mudra) || HasCharges(Ten)))
						{
							if (OriginalHook(Ninjutsu) is Huton)
							{
								return OriginalHook(Ninjutsu);
							}
							if (WasLastAbility(JinCombo))
							{
								return TenCombo;
							}
							if (WasLastAbility(Chi))
							{
								return JinCombo;
							}
							return Chi;
						}
					}

					if (IsEnabled(CustomComboPreset.NIN_AoE_Bunshin) && HasEffect(Buffs.PhantomReady))
					{
						return PhantomKamaitachi;
					}

					if (comboTime > 0)
					{
						if (lastComboActionID is DeathBlossom)
						{
							return HakkeMujinsatsu;
						}
					}

					return DeathBlossom;
				}
				return actionID;
			}
		}
	}
}