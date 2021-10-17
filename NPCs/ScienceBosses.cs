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
            MonsterType = ModContent.NPCType<ScienceAutonomousCombatUnit_3>();
        }

        private int GuidedMissile;
        private int Laser;
        private AIState state;
        private int MonsterType;
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
            state = AIState.Stand;
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


        private enum AIState
        {
            Stand,
            Fly,
            Shoot,
            Dash,
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





        private int Timer;
        private Player target;
        /// <summary>
        /// 各种动作之间的cd，cd == 0的时候会倒飞3秒，大于0的时候会播放相应的动作
        /// </summary>
        private int actionCD;
        /// <summary>
        /// 倒飞的计时器，在倒飞的第一帧会进行下一次action是会射击导弹还是会冲刺还是会召唤小弟
        /// </summary>
        private int flytimer;
        private int nextaction;
        private float speed = 1f;
        private Vector2 AimingStart, AimingStop;
        private float liferate;
        public override void AI()
        {

            target = Main.player[npc.target];
            npc.dontTakeDamage = false;
            liferate = npc.life / npc.lifeMax;
            Timer++;
            if (Timer <= 120)
            {
                npc.dontTakeDamage = true;
                npc.alpha = (int)((1 - Timer / 120f) * 225);
                state = AIState.Stand;
            }
            else
            {
                npc.TargetClosest();
                if(actionCD <= 0)
                {
                    actionCD = 0;
                    flytimer++;
                }
                //1阶段
                //只会召唤小弟或者发射激光
                if (liferate >= 0.66f)
                {
                    if(flytimer <= 180 && flytimer >= 1)
                    {
                        
                        if(flytimer == 1)
                        {
                            nextaction = Main.rand.Next(0, 1);
                            npc.frameCounter = 0d;
                            state = AIState.Fly;
                        }

                        npc.velocity = Vector2.Normalize(npc.Center - target.Center) * speed;
                        

                        if (flytimer > 180)
                        {
                            //停止倒飞，进行动作
                            flytimer = 0;
                            actionCD = 120;
                        }
                        
                    }
                    else
                    {
                        //动作,0是召唤小弟，1是发射激光
                        state = AIState.Shoot;
                        npc.velocity = npc.velocity * 0.99f;
                        if(actionCD == 120 || actionCD == 119)
                        {
                            npc.frameCounter = 0d;
                            if (nextaction == 0)
                            {
                                for(int i = 0; i < 3; i++)
                                {
                                    NPC.NewNPC((int)target.position.X / 16 - npc.direction * (Main.screenWidth / 2) + Main.rand.Next(-3, 3),
                                        (int)target.position.Y / 16,
                                        MonsterType,0,0,0,0,npc.target);
                                }
                            }
                            else if(nextaction == 1)
                            {
                                
                                
                                //瞄准线写在PostDraw里面,1秒瞄准，1秒弹幕
                            }
                        }
                        if(actionCD == 60)
                        {
                            int max = (int)((AimingStop - AimingStart).Length() / 256);
                            for (int i = 0; i < max; i++)
                            {
                                Projectile.NewProjectileDirect(AimingStart + (AimingStop - AimingStart) * (i / (float)max), Vector2.Zero, Laser, npc.damage, 1f);
                            }
                        }
                        actionCD--;
                    }
                }

            }
        }
        
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if(liferate >= 0.66f && state == AIState.Shoot && nextaction == 1 && actionCD > 60)
            {
                AimingStart = npc.Center;
                AimingStop = target.Center + npc.direction * Vector2.Normalize(target.Center - AimingStart) * Main.screenWidth;
                for (int i = 0; i <= (int)((AimingStop.Y - AimingStart.Y) / 7.5f); i++)
                {
                    spriteBatch.Draw(Science.ScienceAimingLineBlue,
                        AimingStart + new Vector2(0, i * 7.5f) - Main.screenPosition,
                        new Rectangle(0, 0, 18, 30),
                        Color.White,
                        (AimingStop - AimingStart).ToRotation(),
                        Science.ScienceAimingLineBlue.Size() / 2,
                        0.25f,
                        SpriteEffects.None,
                        0f
                        );
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            npc.spriteDirection = npc.direction;
            npc.frameCounter++;
            npc.rotation = 0f;
            switch (state)
            {
                case AIState.Stand:
                    {
                        //每10帧切一次帧图
                        npc.frame.Y = frameHeight * (((int)npc.frameCounter /10) % 4);
                        break;
                    }
                case AIState.Fly:
                    {
                        //倒飞的时候稍微倾斜一点
                        npc.rotation = npc.direction * 0.5233f;
                        npc.frame.Y = frameHeight * ((((int)npc.frameCounter /5) % 2)+4);
                        break;
                    }
                case AIState.Shoot:
                    {
                        npc.frame.Y = frameHeight * (npc.frameCounter <= 10 ? 0 : 1 + 6);
                        break;
                    }
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
}
