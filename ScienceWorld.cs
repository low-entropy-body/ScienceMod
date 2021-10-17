using Terraria.Utilities;
using Terraria.Localization;
using Science.Tiles;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.World.Generation;
using System;
using Microsoft.Xna.Framework;
using Science.NPCs;
using Terraria.ID;

namespace Science
{
	class ScienceWorld : ModWorld
	{
		//地形方块数
		public static int ScienceTiles;


		public override void TileCountsAvailable(int[] tileCounts)
		{
			ScienceTiles = tileCounts[ModContent.TileType<ScienceBlock>()];
			//甚至可以在这里改变对原版地形的判断条件
		}

		private int timer1;
		private int timer2;
		private bool canspawnboss1 = false;
        public override void PreUpdate()
        {
			if(Main.time >= ScienceBoss1.prespawnTime)
            {
				timer1++;
				if(timer1 == 1)
                {
					canspawnboss1 = Main.rand.NextBool(10);
					if (Main.netMode == NetmodeID.SinglePlayer) Main.NewText("Boss1" + " 要苏醒", 50, 125, 255);
					else NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Boss1" + " 要苏醒"), new Color(50, 125, 255));
				}
            }
			
			if (!Main.dayTime)
			{
				timer2++;
				if (canspawnboss1)
				{
					if (timer2 == 1)
					{
						ScienceBoss1.SpawnBoss1();
					}
				}
			}
        }




        //在游戏本体生成矿物的任务顺序中插入一个生成科学方块的任务
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
		{
			// 找到矿物生成的层
			//  =>  可以理解成一个匿名方法，具体去看lambda表达式
			int genoreLayer = tasks.FindIndex((pass) => pass.Name.Equals("Shinies"));
			if (genoreLayer != -1)
			{
				tasks.Insert(genoreLayer + 1, new PassLegacy("ScienceBlockOres", ScienceModOres));
			}
			int genoreDungeon = tasks.FindIndex((pass) => pass.Name.Equals("Dungeon"));
			if(genoreDungeon != -1)
            {
				tasks.Insert(genoreDungeon + 1, new PassLegacy("InfectPurity", InfectPurity));
            }
		}


		//生成科学方块(生成矿物）任务的具体实现
		private void ScienceModOres(GenerationProgress progress)
		{
			// progress.Message is the message shown to the user while the following code is running.
			//Try to make your message clear. You can be a little bit clever,
			//but make sure it is descriptive enough for troubleshooting purposes. 
			if (GameCulture.Chinese.IsActive)
			{
				progress.Message = "生成科学方块中";
			}
			else
			{
				progress.Message = "ohhhhhhhhhhhhhhh";
			}

			// 生成数量是地图总物块数量的0.006%
			for (int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY) * 6E-05); k++)
			{
				int x = WorldGen.genRand.Next(0, Main.maxTilesX);
				int y = WorldGen.genRand.Next((int)WorldGen.worldSurfaceLow, Main.maxTilesY); 

				// 原版自带物块生成函数
				//strength代表的是这个生成区域的最大大小
				//steps代表这个生成区域连接的紧密程度
				//方块类型
				WorldGen.TileRunner(x, y, 
					WorldGen.genRand.Next(10, 30), 
					WorldGen.genRand.Next(20, 40), 
					ModContent.TileType<ScienceBlock>());

				//只在雪原生成
				// Tile tile = Framing.GetTileSafely(x, y);
				// if (tile.active() && tile.type == TileID.SnowBlock)
				// {
				// 	WorldGen.TileRunner(.....);
				// }
			}
		}

		//将地牢对应的另外一侧感染成机械地形
		private void InfectPurity(GenerationProgress progress) 
		{
			UnifiedRandom genRand = WorldGen.genRand;

			if (GameCulture.Chinese.IsActive)
			{
				progress.Message = "格式化无机生物群落中...";
			}
			else
			{
				progress.Message = "ohhhhhhhhh";
			}

			int dungeonSide = CheckDungeonSide();
			
			int startX;
			int deltaX = dungeonSide *= 250;
			int startY = (int)((Main.worldSurface + Main.rockLayer) / 2.0) + genRand.Next(0, 200);

			startX = Main.maxTilesX - Main.dungeonX;
			startX += deltaX;

			int depth = genRand.Next((int)Main.rockLayer * 2) + genRand.Next(-200,200);
			int length1 = genRand.Next(200, 300);
			int length2 = genRand.Next(200, 300);
			for (int i = 0; i<= depth; i++) 
			{
				length1 += genRand.Next(-3, 3);
				length2 += genRand.Next(-3, 3);
				TileLine(new Point(startX - length1, startY - i), new Point(startX + length2, startY - i),ModContent.TileType<ScienceBlock>());
			}
		}




		/// <summary>
		/// Bresenham算法绘制直线
		/// 教程上抄的代码，属实是看不懂
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="type"></param>
		private void TileLine(Microsoft.Xna.Framework.Point start, Microsoft.Xna.Framework.Point end, int type)
		{
			int x1 = start.X, y1 = start.Y;
			int x2 = end.X, y2 = end.Y;
			bool steep = Math.Abs(y2 - y1) > Math.Abs(x2 - x1);
			if (steep)
			{
                // 反转坐标轴
                Swap(ref x1, ref y1);
				Swap(ref x2, ref y2);
			}
			if (x1 > x2)
			{
                Swap(ref x1, ref x2);
				Swap(ref y1, ref y2);
			}
			int dx = x2 - x1;
			int dy = Math.Abs(y2 - y1);
			int error = 0;
			int ystep = (y1 < y2) ? 1 : -1;
			int y = y1;
			for (int x = x1; x <= x2; ++x)
			{
				if (steep)
				{
					if (Valid(y, x))
					{
						Main.tile[y, x].type = (ushort)type;
						Main.tile[y, x].active();
					}
				}
				else
				{
					if (Valid(x, y))
					{
						Main.tile[x, y].type = (ushort)type;
						Main.tile[x, y].active();
					}
				}
				if (2 * (error + dy) < dx)
					error += dy;
				else
				{
					y += ystep;
					error = error + dy - dx;
				}
			}
		}

		/// <summary>
		/// 检测坐标的合法性
		/// </summary>
		/// <param name="x">横坐标</param>
		/// <param name="y">纵坐标</param>
		/// <returns></returns>
		private bool Valid(int x, int y)
		{
			return x >= 0 && x < Main.maxTilesX && y >= 0 && y < Main.maxTilesY;
		}

		/// <summary>
		/// 调换二者位置
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		private void Swap<T>(ref T x1, ref T y1)
        {
			T mid = x1;
			x1 = y1;
			y1 = mid;
        }

		/// <summary>
		/// 检查地牢生成在哪边
		/// </summary>
		/// <returns></returns>
		private int CheckDungeonSide()
        {
			int checkSide = ((int)(Main.maxTilesX * 0.095) + (int)(Main.maxTilesX * 0.05)) / 2;
			if (Main.dungeonX < checkSide)
			{
				return -1;
			}
			else
			{
				return  1;
			}
		}

	}
}
