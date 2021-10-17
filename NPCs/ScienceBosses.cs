using Terraria.ModLoader;
using Terraria.Localization;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Science.Projectiles;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace Science.NPCs
{
    /// <summary>
    /// 宿敌boss第一阶段
    /// </summary>
    public class ScienceBoss1 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Boss1");
            DisplayName.AddTranslation(GameCulture.Chinese, "宿敌boss1");
            Main.npcFrameCount[npc.type] = 12;
        }

        private int GuidedMissile;
        private int Laser;
        public override void SetDefaults()
        {
            npc.boss = true; 
            npc.damage = 100;
            npc.defDefense = 25;
            npc.lifeMax = 4500;
            npc.aiStyle = -1;
            aiType = 0;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.knockBackResist = 0f;
            npc.width = 138;
            npc.height = 202;
            GuidedMissile = ModContent.ProjectileType<ScienceBoss1GuidedMissileProjectile>();
            Laser = ModContent.ProjectileType<ScienceBoss1LaserProjectile>();
        }


        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax *= (int)(bossLifeScale * 1.5 * numPlayers);
            
        }

        public override bool CheckDead()
        {
            if (!ScienceGlobalNPC.downedScienceBoss1)
            {
                ScienceGlobalNPC.downedScienceBoss1 = true;
            }
            return true;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;
        }

        /// <summary>
        /// 在6点钟进行判断
        /// </summary>
        public const double prespawnTime = 48600.0;//6:00pm
        public static NPC FindNPC(int npcType) => Main.npc.FirstOrDefault(npc => npc.type == npcType && npc.active);

        /// <summary>
        /// 生成boss
        /// </summary>
        public static void SpawnBoss1()
        {
            int intBoss = NPC.NewNPC((int)(Main.player[Main.myPlayer].position.X / 16), (int)(Main.player[Main.myPlayer].position.Y / 16), ModContent.NPCType<ScienceBoss1>());
            NPC Boss = Main.npc[intBoss];
            if (Main.netMode == NetmodeID.SinglePlayer) Main.NewText(Boss.GivenOrTypeName + " 已苏醒", 50, 125, 255);
            
            else NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(Boss.GivenOrTypeName + " 已苏醒"), new Color(50, 125, 255));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }





        private int tempcounter;
        private Player target;
        public override void AI()
        {
            npc.TargetClosest();
            target = Main.player[npc.target];
            tempcounter++;
            if(tempcounter % 30 == 0)
            {
                Main.NewText("try to spawn missile");
            }
        }
        private Vector2 start = Vector2.Zero, stop = Vector2.Zero;
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            
            if(tempcounter % 60 == 0)
            {
                start = new Vector2(target.Center.X, target.Center.Y - Main.screenHeight * 2);
                stop = new Vector2(start.X, target.Center.Y + Main.screenHeight * 2);
            }
            //spriteBatch.End();
            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            
                for(int i = 0; i <= (int)((stop.Y - start.Y) / 7.5f); i++)
                {
                    spriteBatch.Draw(Science.ScienceAimingLineBlue,
                        start + new Vector2(0, i * 7.5f) - Main.screenPosition,
                        new Rectangle(0,0,18,30),
                        Color.White,
                        0f,
                        Science.ScienceAimingLineBlue.Size() / 2,
                        0.25f,
                        SpriteEffects.None,
                        0f
                        );
                }
            //spriteBatch.End();
            //spriteBatch.Begin();
            
            if (tempcounter % 30 == 20)
            {
                Projectile.NewProjectileDirect(new Vector2(target.position.X, target.position.Y - Main.screenHeight / 2), new Vector2(0, 10f), GuidedMissile, npc.damage, 1f);
                Projectile.NewProjectileDirect(target.Center + new Vector2(0f, 30f), Vector2.Zero, Laser, npc.damage, 1f);
            }
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            changeframetemp(frameHeight);
        }

        private int ShootMissileTimer;
        private void ShootMissile()
        {
            Projectile.NewProjectileDirect(new Vector2(target.position.X, target.position.Y - Main.screenHeight / 2), new Vector2(0, 10f), GuidedMissile, npc.damage, 1f);
        }

        private void changeframetemp(int frameHeight)
        {
            int temp = (int)(npc.frameCounter % 240);
            npc.frame.Y = frameHeight * (temp / 20);
        }
    }

    public class ScienceBoss1Bag : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            DisplayName.AddTranslation(GameCulture.Chinese, "宝藏袋");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.consumable = true;
            item.width = 24;
            item.height = 24;
            item.rare = ItemRarityID.Cyan;
            item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            if (!ScienceGlobalNPC.downedScienceBoss1)
            {
                player.TryGettingDevArmor();
            }
            else if (Main.rand.NextBool(7))
            {
                player.TryGettingDevArmor();
            }

        }

        public override int BossBagNPC => ModContent.NPCType<ScienceBoss1>();

        
    }

    public class ScienceAutonomousCombatUnit_3 : ModNPC
    {
        public override string Texture => (GetType().Namespace + "." + "ScienceAutonomousCombatUnit_2").Replace('.', '/');
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("fuyoupao");
            DisplayName.AddTranslation(GameCulture.Chinese, "XI型浮游炮");
            Main.npcFrameCount[npc.type] = 6;
        }

        public override void SetDefaults()
        {
            npc.CloneDefaults(173);
            npc.damage = 20;
            npc.defDefense = 10;
            npc.lifeMax = 100;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.value = 0f;
            aiType = 173;
        }

        public override void PostAI()
        {
            
        }
    }
}
