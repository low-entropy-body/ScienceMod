using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
namespace Science.Items.Accessories
{
	[AutoloadEquip(EquipType.Shoes)]
	public class ScienceDashAccessory : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Science Dash Accesory");
			DisplayName.AddTranslation(GameCulture.Chinese, "冲刺饰品");

			Tooltip.SetDefault("This is a modded accessory." +
				"\nDouble tap in any cardinal direction to do a dash!");
			Tooltip.AddTranslation(GameCulture.Chinese, "双击冲刺");
		}

		public override void SetDefaults()
		{
			item.defense = 2;
			item.accessory = true;
			item.rare = ItemRarityID.Blue;
			item.value = Item.sellPrice(silver: 60);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			ScienceDashPlayer SciencePlayer = player.GetModPlayer<ScienceDashPlayer>();
			
			//If the dash is not active, immediately return so we don't do any of the logic for it
			if(!SciencePlayer.DashActive)
				return;
			//This is where we set the afterimage effect.  You can replace these two lines with whatever you want to happen during the dash
			//Some examples include:  spawning dust where the player is, adding buffs, making the player immune, etc.
			//Here we take advantage of "player.eocDash" and "player.armorEffectDrawShadowEOCShield" to get the Shield of Cthulhu's afterimage effect
			player.eocDash = SciencePlayer.DashTimer;
			player.armorEffectDrawShadowEOCShield = true;
			if(SciencePlayer.DashTimer > 0)
            {
				for (int i = 0; i < 100; i++)
					Dust.NewDustDirect(player.position, player.width, player.height, DustID.Fire, -player.direction * 2, 0f);
			}
			
			//If the dash has just started, apply the dash velocity in whatever direction we wanted to dash towards
			if (SciencePlayer.DashTimer == ScienceDashPlayer.MAX_DASH_TIMER)
			{
				Vector2 newVelocity = player.velocity;
					
				//Only apply the dash velocity if our current speed in the wanted direction is less than DashVelocity
				if((SciencePlayer.DashDir == ScienceDashPlayer.DashUp && player.velocity.Y > -SciencePlayer.DashVelocity) || 
					(SciencePlayer.DashDir == ScienceDashPlayer.DashDown && player.velocity.Y < SciencePlayer.DashVelocity))
				{
					//Y-velocity is set here
					//If the direction requested was DashUp, then we adjust the velocity to make the dash appear "faster" due to gravity being immediately in effect
					//This adjustment is roughly 1.3x the intended dash velocity
					float dashDirection = SciencePlayer.DashDir == ScienceDashPlayer.DashDown ? 1 : -1.3f;
					newVelocity.Y = dashDirection * SciencePlayer.DashVelocity;
				}
				else if((SciencePlayer.DashDir == ScienceDashPlayer.DashLeft && player.velocity.X > -SciencePlayer.DashVelocity) || 
					(SciencePlayer.DashDir == ScienceDashPlayer.DashRight && player.velocity.X < SciencePlayer.DashVelocity))
				{
					//X-velocity is set here
					int dashDirection = SciencePlayer.DashDir == ScienceDashPlayer.DashRight ? 1 : -1;
					newVelocity.X = dashDirection * SciencePlayer.DashVelocity;
				}

				player.velocity = newVelocity;
			}

			//Decrement the timers
			SciencePlayer.DashTimer--;
			SciencePlayer.DashDelay--;

			if(SciencePlayer.DashDelay == 0)
			{
				//The dash has ended.  Reset the fields
				SciencePlayer.DashDelay = ScienceDashPlayer.MAX_DASH_CD;
				SciencePlayer.DashTimer = ScienceDashPlayer.MAX_DASH_TIMER;
				SciencePlayer.DashActive = false;
			}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}



	public class ScienceDashPlayer : ModPlayer
    {
		/// <summary>
		/// 冲刺方向
		/// </summary>
		public static readonly int DashDown = 0;

		/// <summary>
		/// 冲刺方向
		/// </summary>
		public static readonly int DashUp = 1;

		/// <summary>
		/// 冲刺方向
		/// </summary>
		public static readonly int DashRight = 2;

		/// <summary>
		/// 冲刺方向
		/// </summary>
		public static readonly int DashLeft = 3;

		/// <summary>
		/// 目前冲刺的方向，如果没有冲刺默认为-1;
		/// </summary>
		public int DashDir = -1;

		public static readonly int MAX_DASH_CD = 120;
		public static readonly int MAX_DASH_TIMER = 60;


		public bool DashActive = false;
		public int DashDelay = MAX_DASH_CD;
		public int DashTimer = MAX_DASH_TIMER;

		public readonly float DashVelocity = 10f;


		public override void ResetEffects()
		{
			//ResetEffects() is called not long after player.doubleTapCardinalTimer's values have been set

			//Check if the ScienceDashAccessory is equipped and also check against this priority:
			// If the Shield of Cthulhu, Master Ninja Gear, Tabi and/or Solar Armour set is equipped, prevent this accessory from doing its dash effect
			//The priority is used to prevent undesirable effects.
			//Without it, the player is able to use the ExampleDashAccessory's dash as well as the vanilla ones
			bool dashAccessoryEquipped = false;

			//This is the loop used in vanilla to update/check the not-vanity accessories
			for (int i = 3; i < 8 + player.extraAccessorySlots; i++)
			{
				Item item = player.armor[i];

				//Set the flag for the ExampleDashAccessory being equipped if we have it equipped OR immediately return if any of the accessories are
				// one of the higher-priority ones
				if (item.type == ModContent.ItemType<ScienceDashAccessory>())
					dashAccessoryEquipped = true;
				else if (item.type == ItemID.EoCShield || item.type == ItemID.MasterNinjaGear || item.type == ItemID.Tabi)
					return;
			}

			//If we don't have the ExampleDashAccessory equipped or the player has the Solor armor set equipped, return immediately
			//Also return if the player is currently on a mount, since dashes on a mount look weird, or if the dash was already activated
			if (!dashAccessoryEquipped || player.setSolar || player.mount.Active || DashActive)
				return;

			//When a directional key is pressed and released, vanilla starts a 15 tick (1/4 second) timer during which a second press activates a dash
			//If the timers are set to 15, then this is the first press just processed by the vanilla logic.  Otherwise, it's a double-tap
			if (player.controlDown && player.releaseDown && player.doubleTapCardinalTimer[DashDown] < 15)
				DashDir = DashDown;
			else if (player.controlUp && player.releaseUp && player.doubleTapCardinalTimer[DashUp] < 15)
				DashDir = DashUp;
			else if (player.controlRight && player.releaseRight && player.doubleTapCardinalTimer[DashRight] < 15)
				DashDir = DashRight;
			else if (player.controlLeft && player.releaseLeft && player.doubleTapCardinalTimer[DashLeft] < 15)
				DashDir = DashLeft;
			else
				return;  //No dash was activated, return

			DashActive = true;

			//Here you'd be able to set an effect that happens when the dash first activates
			//Some examples include:  the larger smoke effect from the Master Ninja Gear and Tabi
		}
	}
}
