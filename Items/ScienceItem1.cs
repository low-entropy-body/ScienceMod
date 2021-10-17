
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Science.Buffs;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Science.Items
{
    public class ScienceItem1:ModItem
    {
        //暂时用的冰镜的材质
        public override string Texture => "Terraria/Item_" + ItemID.IceMirror;
        public override void SetStaticDefaults()
        {
            

            DisplayName.SetDefault("ScienceItem1");
            DisplayName.AddTranslation(GameCulture.Chinese, "神秘核心");

            Tooltip.SetDefault("ScienceItem1  tooltip");
            Tooltip.AddTranslation(GameCulture.Chinese, "某个神秘的核心");
        }


        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.IceMirror);
            item.color = Color.White;
        }


        public override bool CanUseItem(Player player)
        {
			if (player.HasBuff(ModContent.BuffType<ScienceEnergyDepletion>()))
			{
				return false;
			}

			else
				return true;
        }


        // UseStyle is called each frame that the item is being actively used.
        public override void UseStyle(Player player)
		{
			if (!player.HasBuff(ModContent.BuffType<ScienceEnergyDepletion>()))
			{
				// Each frame, make some dust
				if (Main.rand.NextBool())
			{
				Dust.NewDust(player.position, player.width, player.height, 15, 0f, 0f, 150, default(Color), 1.1f);
			}
				// This sets up the itemTime correctly.
				if (player.itemTime == 0)
				{
					player.itemTime = (int)(item.useTime / PlayerHooks.TotalUseTimeMultiplier(player, item));
				}
				else if (player.itemTime == (int)(item.useTime / PlayerHooks.TotalUseTimeMultiplier(player, item)) / 2)
				{
					// This code runs once halfway through the useTime of the item. You'll notice with magic mirrors you are still holding the item for a little bit after you've teleported.

					// Make dust 70 times for a cool effect.
					for (int d = 0; d < 70; d++)
					{
						Dust.NewDust(player.position, player.width, player.height, 15, player.velocity.X * 0.5f, player.velocity.Y * 0.5f, 150, default(Color), 1.5f);
					}
					// This code releases all grappling hooks and kills them.
					player.grappling[0] = -1;
					player.grapCount = 0;
					for (int p = 0; p < 1000; p++)
					{
						if (Main.projectile[p].active && Main.projectile[p].owner == player.whoAmI && Main.projectile[p].aiStyle == 7)
						{
							Main.projectile[p].Kill();
						}
					}
					// The actual method that moves the player back to bed/spawn.




					player.Spawn();
					player.AddBuff(ModContent.BuffType<ScienceEnergyDepletion>(), ScienceEnergyDepletion.BuffTotalTime);

					// Make dust 70 times for a cool effect. This dust is the dust at the destination.
					for (int d = 0; d < 70; d++)
					{
						Dust.NewDust(player.position, player.width, player.height, 15, 0f, 0f, 150, default(Color), 1.5f);
					}
				}
			}
		}


		//Color[] itemNameCycleColors = new Color[]{
		//	new Color(254, 105, 47),
		//	new Color(190, 30, 209),
		//	new Color(34, 221, 151),
		//	new Color(0, 106, 185)

		//public override void ModifyTooltips(List<TooltipLine> tooltips)
		//{
		//	// This code shows using Color.Lerp,  Main.GameUpdateCount, and the modulo operator (%) to do a neat effect cycling between 4 custom colors.
		//	foreach (TooltipLine line2 in tooltips)
		//	{
		//		if (line2.mod == 
		//};"Terraria" && line2.Name == "ItemName")
		//		{
		//			float fade = Main.GameUpdateCount % 60 / 60f;
		//			int index = (int)(Main.GameUpdateCount / 60 % 4);
		//			line2.overrideColor = Color.Lerp(itemNameCycleColors[index], itemNameCycleColors[(index + 1) % 4], fade);
		//		}
		//	}
		//}


		public override void UpdateInventory(Player player)
		{

			//为什么用int？   
			//猜测可能是根据不同的数字来决定不同的格式，比如12小时制和24小时制之类的

			//时间

			player.accWatch = 3;

			//深度计
			player.accDepthMeter = 1;

			//罗盘
			player.accCompass = 1;

			//鱼力
			player.accFishFinder = true;

			//天气（六分仪）
			player.accWeatherRadio = true;

			//日历（月象？）
			player.accCalendar = true;

			//雷达，周围敌人数
			player.accThirdEye = true;

			//杀怪计数器
			player.accJarOfSouls = true;

			//稀有生物
			player.accCritterGuide = true;

			//秒表，即速度
			player.accStopwatch = true;

			//金属探测
			player.accOreFinder = true;

			//DPS
			player.accDreamCatcher = true;
		
	}



		/// <summary>
		/// 给物品在cd的时候加一个红叉，并且透明度会越来越高
		/// </summary>
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Color color_draw;
			Player player = Main.player[Main.myPlayer];
			Vector2 draw_position = position + Main.inventoryBackTexture.Size() * Main.inventoryScale / 2f - Main.cdTexture.Size() * Main.inventoryScale / 2f;
			
			//必须要判断index是否为-1，不然会产生一个很奇怪的bug
			int index = player.FindBuffIndex(ModContent.BuffType<ScienceEnergyDepletion>());
			if (index != -1)
				//透明度渐变
				color_draw = item.GetAlpha(Color.White) * ((float)player.buffTime[index] / (ScienceEnergyDepletion.BuffTotalTime));
			else
				color_draw = Color.White;

			//if(Main.expertMode)
			//	color_draw = item.GetAlpha(Color.White) * ((float)player.buffTime[player.FindBuffIndex(ModContent.BuffType<ScienceEnergyDepletion>())] / ScienceEnergyDepletion.BuffTotalTime * 2);
			//else
			//	color_draw = item.GetAlpha(Color.White) * ((float)player.buffTime[player.FindBuffIndex(ModContent.BuffType<ScienceEnergyDepletion>())] / ScienceEnergyDepletion.BuffTotalTime);

			if (!CanUseItem(player))
				spriteBatch.Draw(Main.cdTexture, draw_position, null, color_draw, 0f, Main.cdTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
			else
				return;
		}

	}
}

