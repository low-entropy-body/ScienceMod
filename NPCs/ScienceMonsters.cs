using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Science.NPCs
{
    public class ScienceIceMan_1 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Science Ice Man");
            DisplayName.AddTranslation(GameCulture.Chinese, "地底冰人");
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void SetDefaults()
        {
            npc.width = 18;
            npc.height = 40;
            npc.damage = 14;
            npc.defense = 6;
            npc.lifeMax = 200;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.value = 60f;
            npc.knockBackResist = 0.5f;
            npc.aiStyle = 3;
            aiType = 73;
            
            
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if(spawnInfo.player.ZoneRockLayerHeight && spawnInfo.player.ZoneSnow)
            {
                return 1f;
            }
            return 0f;
        }



        public override void FindFrame(int frameHeight)
        {
            npc.spriteDirection = npc.direction;
            npc.frameCounter++;
            if(npc.frameCounter <= 10)
            {
                npc.frame.Y = 0;
            }
            else if(npc.frameCounter <= 20)
            {
                npc.frame.Y = frameHeight;
            }
            else if(npc.frameCounter <= 30)
            {
                npc.frame.Y = 2 * frameHeight;
            }
            else
            {
                npc.frameCounter = 0;
            }
        }

    }

    public class ScienceIceMan_2 : ScienceIceMan_1
    {
        public override string Texture => (GetType().Namespace + "." + Name).Replace('.', '/');
    }

    public class ScienceIceMan_3 : ScienceIceMan_1
    {
        public override string Texture => (GetType().Namespace + "." + Name).Replace('.', '/');
    }

    public class ScienceWormHead : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Science Worm");
            DisplayName.AddTranslation(GameCulture.Chinese, "科学蠕虫");
        }

        public override void SetDefaults()
        {
            npc.damage = 35;
            npc.npcSlots = 5f;
            npc.width = 30;
            npc.height = 60;
            npc.defense = 0;
            npc.lifeMax = 250;
            npc.aiStyle = -1;
            aiType = -1;
            animationType = 10;
            npc.knockBackResist = 0f;
            npc.alpha = 255;
            npc.behindTiles = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.netAlways = true; 
        }

		public override void AI()
		{
			float num = (Main.expertMode) ? 1.5f : 1.425f;
			float num2 = npc.life;
			float num3 = npc.lifeMax;
			speed = 15f * (num - num2 / num3);
			turnSpeed = 0.13f * (num - num2 / num3);
			if (npc.ai[3] > 0f)
			{
				npc.realLife = (int)npc.ai[3];
			}
			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead)
			{
				npc.TargetClosest(true);
			}
			npc.velocity.Length();
			npc.alpha -= 42;
			if (npc.alpha < 0)
			{
				npc.alpha = 0;
			}
			if (!TailSpawned)
			{
				int num4 = npc.whoAmI;
				for (int i = 0; i < maxLength; i++)
				{
					int num5;
					if (i >= 0 && i < minLength)
					{
						num5 = NPC.NewNPC((int)npc.position.X + npc.width / 2, (int)npc.position.Y + npc.height / 2, ModContent.NPCType<ScienceWormBody>(), npc.whoAmI, i, 0f, 0f, 0, 255);
					}
					else
					{
						num5 = NPC.NewNPC((int)npc.position.X + npc.width / 2, (int)npc.position.Y + npc.height / 2, ModContent.NPCType<ScienceWormTail>(), npc.whoAmI, 0f, 0f, 0f, 0f, 255);
					}
					Main.npc[num5].realLife = npc.whoAmI;
					Main.npc[num5].ai[2] = npc.whoAmI;
					Main.npc[num5].ai[1] = num4;
					npc.ai[0] = num5;
					NetMessage.SendData(23, -1, -1, null, num5, 0f, 0f, 0f, 0, 0, 0);
					num4 = num5;
				}
				TailSpawned = true;
			}
			int num6 = (int)(npc.position.X / 16f) - 1;
			int num7 = (int)((npc.position.X + npc.width) / 16f) + 2;
			int num8 = (int)(npc.position.Y / 16f) - 1;
			int num9 = (int)((npc.position.Y + npc.height) / 16f) + 2;
			if (num6 < 0)
			{
				num6 = 0;
			}
			if (num7 > Main.maxTilesX)
			{
				num7 = Main.maxTilesX;
			}
			if (num8 < 0)
			{
				num8 = 0;
			}
			if (num9 > Main.maxTilesY)
			{
				num9 = Main.maxTilesY;
			}
			bool flag = flies;
			if (!flag)
			{
				for (int j = num6; j < num7; j++)
				{
					for (int k = num8; k < num9; k++)
					{
						if (Main.tile[j, k] != null && ((Main.tile[j, k].nactive() && (Main.tileSolid[(int)Main.tile[j, k].type] || (Main.tileSolidTop[(int)Main.tile[j, k].type] && Main.tile[j, k].frameY == 0))) || Main.tile[j, k].liquid > 64))
						{
							Vector2 vector;
							vector.X = (j * 16);
							vector.Y = (k * 16);
							if (npc.position.X + npc.width > vector.X && npc.position.X < vector.X + 16f && npc.position.Y + npc.height > vector.Y && npc.position.Y < vector.Y + 16f)
							{
								flag = true;
								break;
							}
						}
					}
				}
			}
			if (!flag)
			{
				npc.localAI[1] = 1f;
				Rectangle rectangle = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
				int num10 = 200;
				bool flag2 = true;
				if (npc.position.Y > Main.player[npc.target].position.Y)
				{
					for (int l = 0; l < 255; l++)
					{
						if (Main.player[l].active)
						{
							Rectangle rectangle2 = new Rectangle((int)Main.player[l].position.X - num10, (int)Main.player[l].position.Y - num10, num10 * 2, num10 * 2);
							if (rectangle.Intersects(rectangle2))
							{
								flag2 = false;
								break;
							}
						}
					}
					if (flag2)
					{
						flag = true;
					}
				}
			}
			else
			{
				npc.localAI[1] = 0f;
			}
			float num11 = speed;
			float num12 = turnSpeed;
			Vector2 vector2 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
			float num13 = Main.player[npc.target].position.X + (Main.player[npc.target].width / 2);
			float num14 = Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2);
			num13 = ((int)(num13 / 16f) * 16);
			num14 = ((int)(num14 / 16f) * 16);
			vector2.X = (int)(vector2.X / 16f) * 16;
			vector2.Y = (int)(vector2.Y / 16f) * 16;
			num13 -= vector2.X;
			num14 -= vector2.Y;
			float num15 = (float)Math.Sqrt(num13 * num13 + (num14 * num14));
			if (npc.ai[1] > 0f && npc.ai[1] < Main.npc.Length)
			{
				try
				{
					vector2 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
					num13 = Main.npc[(int)npc.ai[1]].position.X + (Main.npc[(int)npc.ai[1]].width / 2) - vector2.X;
					num14 = Main.npc[(int)npc.ai[1]].position.Y + (Main.npc[(int)npc.ai[1]].height / 2) - vector2.Y;
				}
				catch
				{
				}
				npc.rotation = (float)(Math.Atan2(num14, num13) + 1.57f);
				num15 = (float)Math.Sqrt(num13 * num13 + num14 * num14);
				int width = npc.width;
				num15 = (num15 - width) / num15;
				return;
			}
			npc.rotation = (float)(Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f);
			if (flag)
			{
				if (npc.localAI[0] != 1f)
				{
					npc.netUpdate = true;
				}
				npc.localAI[0] = 1f;
			}
			else
			{
				if (npc.localAI[0] != 0f)
				{
					npc.netUpdate = true;
				}
				npc.localAI[0] = 0f;
			}
			if (((npc.velocity.X > 0f && npc.oldVelocity.X < 0f) || (npc.velocity.X < 0f && npc.oldVelocity.X > 0f) || (npc.velocity.Y > 0f && npc.oldVelocity.Y < 0f) || (npc.velocity.Y < 0f && npc.oldVelocity.Y > 0f)) && !npc.justHit)
			{
				npc.netUpdate = true;
				return;
			}
			if (Main.player[npc.target].dead)
			{
				flag = false;
				npc.velocity.Y = npc.velocity.Y + 0.05f;
				if (npc.position.Y > Main.worldSurface * 16.0)
				{
					npc.velocity.Y = npc.velocity.Y + 0.05f;
				}
				if (npc.position.Y > Main.rockLayer * 16.0)
				{
					for (int m = 0; m < 200; m++)
					{
						if (Main.npc[m].aiStyle == npc.aiStyle)
						{
							Main.npc[m].active = false;
						}
					}
				}
			}
			else
			{
				npc.aiStyle = -1;
				Player player = Main.player[npc.target];
				float x = npc.position.X + npc.width / 2 - player.Center.X;
				float y = npc.position.Y + npc.height - 59f - player.Center.Y;
				Vector2 vector = new Vector2(x, y);
				vector.Normalize();

				Vector2 R = new Vector2((float)(Math.Cos(npc.rotation - 1.57f)), (float)(Math.Sin(npc.rotation - 1.57f)));
				Timer++;

				if (Timer % 60 == 0)
				{
					Main.PlaySound(SoundID.NPCKilled, (int)npc.position.X, (int)npc.position.Y, 3);
				}
				if (!flies && npc.behindTiles && npc.soundDelay == 0)
				{
					float num16 = num15 / 40f;
					if (num16 < 10f)
					{
						num16 = 10f;
					}
					if (num16 > 20f)
					{
						num16 = 20f;
					}
					npc.soundDelay = (int)num16;
				}
				num15 = (float)Math.Sqrt(num13 * num13 + num14 * num14);
				float num17 = Math.Abs(num13);
				float num18 = Math.Abs(num14);
				float num19 = num11 / num15;
				num13 *= num19;
				num14 *= num19;
				if (!false)
				{
					if ((npc.velocity.X > 0f && num13 > 0f) || (npc.velocity.X < 0f && num13 < 0f) || (npc.velocity.Y > 0f && num14 > 0f) || (npc.velocity.Y < 0f && num14 < 0f))
					{
						if (npc.velocity.X < num13)
						{
							npc.velocity.X = npc.velocity.X + num12;
						}
						else if (npc.velocity.X > num13)
						{
							npc.velocity.X = npc.velocity.X - num12;
						}
						if (npc.velocity.Y < num14)
						{
							npc.velocity.Y = npc.velocity.Y + num12;
						}
						else if (npc.velocity.Y > num14)
						{
							npc.velocity.Y = npc.velocity.Y - num12;
						}
						if (Math.Abs(num14) < num11 * 0.2 && ((npc.velocity.X > 0f && num13 < 0f) || (npc.velocity.X < 0f && num13 > 0f)))
						{
							if (npc.velocity.Y > 0f)
							{
								npc.velocity.Y = npc.velocity.Y + num12 * 2f;
							}
							else
							{
								npc.velocity.Y = npc.velocity.Y - num12 * 2f;
							}
						}
						if (Math.Abs(num13) < num11 * 0.2 && ((npc.velocity.Y > 0f && num14 < 0f) || (npc.velocity.Y < 0f && num14 > 0f)))
						{
							if (npc.velocity.X > 0f)
							{
								npc.velocity.X = npc.velocity.X + num12 * 2f;
							}
							else
							{
								npc.velocity.X = npc.velocity.X - num12 * 2f;
							}
						}
					}
					else if (num17 > num18)
					{
						if (npc.velocity.X < num13)
						{
							npc.velocity.X = npc.velocity.X + num12 * 1.1f;
						}
						else if (npc.velocity.X > num13)
						{
							npc.velocity.X = npc.velocity.X - num12 * 1.1f;
						}
						if ((Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < num11 * 0.5)
						{
							if (npc.velocity.Y > 0f)
							{
								npc.velocity.Y = npc.velocity.Y + num12;
							}
							else
							{
								npc.velocity.Y = npc.velocity.Y - num12;
							}
						}
					}
					else
					{
						if (npc.velocity.Y < num14)
						{
							npc.velocity.Y = npc.velocity.Y + num12 * 1.1f;
						}
						else if (npc.velocity.Y > num14)
						{
							npc.velocity.Y = npc.velocity.Y - num12 * 1.1f;
						}
						if ((Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < num11 * 0.5)
						{
							if (npc.velocity.X > 0f)
							{
								npc.velocity.X = npc.velocity.X + num12;
							}
							else
							{
								npc.velocity.X = npc.velocity.X - num12;
							}
						}
					}
				}
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 5, hitDirection, -1f, 0, default(Color), 1f);
			}
			if (npc.life <= 0)
			{
				for (int j = 0; j < 10; j++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 5, hitDirection, -1f, 0, default(Color), 1f);
				}
			}
		}


		public override bool CheckDead()
		{
			for (int j = 0; j < 10; j++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 5, 0, -1f, 0, default(Color), 1f);
			}
			
			return base.CheckDead();
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 1f * bossLifeScale);
			npc.damage = (int)(npc.damage * 1.15f);
		}
		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			player.AddBuff(36, 240, true);
			player.AddBuff(30, 240, true);
		}
		private bool flies;
		private float speed = 19f;
		private float turnSpeed = 0.17f;
		private int minLength = 5;
		private int maxLength = 6;
		private bool TailSpawned;
		private bool TE;
		public int Timer;
		public int Timer2;
		public int Timer3;
		public int Timer4;
		public int Timer5;
	}

	public class ScienceWormBody : ModNPC
    {
        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("ScienceWormBody");
			DisplayName.AddTranslation(GameCulture.Chinese, "科学蠕虫身体");

        }

        public override void SetDefaults()
        {
			npc.damage = 15;
			npc.npcSlots = 5f;
			npc.width = 26;
			npc.height = 32;
			npc.defense = 22;
			npc.lifeMax = 250;
			npc.aiStyle = 6;
			aiType = -1;
			animationType = 10;
			npc.knockBackResist = 0f;
			npc.alpha = 255;
			npc.behindTiles = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.canGhostHeal = false;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.netAlways = true;
			npc.dontCountMe = true;
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return new bool?(false);
		}

		public int Timer;
		public int Timer2;
		public int Timer3;
		public int Timer4;
		public int Timer5;

		public override void AI()
		{
			if (!Main.npc[(int)npc.ai[1]].active)
			{
				npc.life = 0;
				npc.HitEffect(0, 10.0);
				npc.active = false;
			}
			if (Main.npc[(int)npc.ai[1]].alpha < 128)
			{
				npc.alpha -= 42;
				if (npc.alpha < 0)
				{
					npc.alpha = 0;
				}
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 5, (float)hitDirection, -1f, 0, default(Color), 1f);
			}
			if (npc.life <= 0)
			{
				for (int j = 0; j < 10; j++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 5, (float)hitDirection, -1f, 0, default(Color), 1f);
				}
				
			}
		}

		public override bool CheckActive()
		{
			return false;
		}

		public override bool PreNPCLoot()
		{
			return false;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)((float)npc.lifeMax * 0.7f * bossLifeScale);
			npc.damage = (int)((float)npc.damage * 0.7f);
		}


	}

	public class ScienceWormTail : ModNPC
    {
        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("ScienceWormTile");
			DisplayName.AddTranslation(GameCulture.Chinese, "科学蠕虫尾巴");
        }

		public override void SetDefaults()
		{
			npc.damage = 25;
			npc.npcSlots = 5f;
			npc.width = 30;
			npc.height = 40;
			npc.defense = 10;
			npc.lifeMax = 250;
			npc.aiStyle = 6;
			aiType = -1;
			animationType = 10;
			npc.knockBackResist = 0f;
			npc.alpha = 255;
			npc.behindTiles = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.canGhostHeal = false;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.netAlways = true;
			npc.dontCountMe = true;
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return new bool?(false);
		}

		public override void AI()
		{
			bool flag = Main.expertMode;
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
			}
			if (!Main.npc[(int)npc.ai[1]].active)
			{
				npc.life = 0;
				npc.HitEffect(0, 10.0);
				npc.active = false;
			}
			if (Main.npc[(int)npc.ai[1]].alpha < 128)
			{
				npc.alpha -= 42;
				if (npc.alpha < 0)
				{
					npc.alpha = 0;
				}
			}
		}

		public override bool CheckActive()
		{
			return false;
		}
		public override bool PreNPCLoot()
		{
			return false;
		}
		public override bool CheckDead()
		{
			for (int j = 0; j < 10; j++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 5, 0, -1f, 0, default(Color), 1f);
			}
			
			return base.CheckDead();
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 5, (float)hitDirection, -1f, 0, default(Color), 1f);
			}
			if (npc.life <= 0)
			{
				for (int j = 0; j < 10; j++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 5, (float)hitDirection, -1f, 0, default(Color), 1f);
				}
			}
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)((float)npc.lifeMax * 0.7f * bossLifeScale);
			npc.damage = (int)((float)npc.damage * 0.7f);
		}
	}
}