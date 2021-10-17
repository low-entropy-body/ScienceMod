using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Science.Projectiles;
using Science;
using System;

namespace Science.NPCs
{
    public class ScienceAutonomousCombatUnit_1 : ModNPC
    {


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Autonomous Combat Unit");
            DisplayName.AddTranslation(GameCulture.Chinese, "自主作战单元_近战");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Zombie];
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
            //aiType = NPCID.Zombie;
            aiType = 0;
            npc.aiStyle = -1;
            //animationType = NPCID.Zombie;

            npc.direction = Main.rand.NextBool() ? -1 : 1;
            //banner = Item.NPCtoBanner(NPCID.Zombie);
            //bannerItem = Item.BannerToItem(banner);
        }


        //和原版生物比较的刷新权重，如果是一的话就是刷新概率和所有原版生物一样
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<SciencePlayer>().ZoneScience)
            {
                return 1;
            }
            else
                return 0;
        }



        private const int AI_State_Slot = 0;
        private const int AI_Timer_Slot = 1;
        private const int AI_Flutter_Time_Slot = 3;

        private const int State_Ini = 0;
        private const int State_Asleep = 1;
        private const int State_HangOut = 2;
        private const int State_Notice = 3;
        private const int State_Attack = 4;
        private const int State_Jump = 5;
        private const int State_Fall = 6;

        private const float MoveSpeed = 2f;

        public float AI_State
        {
            get => npc.ai[AI_State_Slot];
            set => npc.ai[AI_State_Slot] = value;
        }

        public float AI_Timer
        {
            get => npc.ai[AI_Timer_Slot];
            set => npc.ai[AI_Timer_Slot] = value;
        }

        //public float AI_FlutterTime
        //{
        //    get => npc.ai[AI_Flutter_Time_Slot];
        //    set => npc.ai[AI_Flutter_Time_Slot] = value;
        //}

        internal int jumptime;
        /// <summary>
        /// 当水平速度为0时npc跳跃
        /// </summary>
        /// <returns></returns>
        private bool CanAndTryToJumpOut()
        {
            if (npc.velocity.X == 0)
            {
                jumptime++;
                if (jumptime <= 30)
                {
                    npc.velocity = new Vector2(npc.direction * MoveSpeed, -5f);

                }
                if (jumptime >= 60)
                {
                    jumptime = 0;
                }

                return true;
            }
            else
                return false;
        }

        public override void AI()
        {

            if (AI_State == State_Ini)
            {
                AI_State = Main.rand.NextBool() ? State_HangOut : State_Asleep;

            }

            else if (AI_State == State_Asleep)
            {
                npc.TargetClosest(false);
                if (npc.HasValidTarget && Main.player[npc.target].Distance(npc.Center) < 500f)
                {
                    AI_State = State_Notice;
                    AI_Timer = 0;
                }
                //Main.NewText("状态：Asleep");
                //Main.NewText("Direction:" + npc.direction);
            }


            else if (AI_State == State_HangOut)
            {
                AI_Timer++;
                npc.TargetClosest(false);
                Main.NewText("状态：HangOut");
                if (AI_Timer == 1)
                {
                    npc.velocity = new Vector2(npc.direction * MoveSpeed, 0);
                }
                //如果NPC走完三秒
                if (AI_Timer >= 180)
                {
                    npc.direction = -npc.direction;
                    AI_Timer = 0;
                }

                //npc.direction = (int)npc.velocity.X / (int)System.Math.Abs(npc.velocity.X);

                //Main.NewText("Direction:" + npc.direction);
                if (npc.HasValidTarget && Main.player[npc.target].Distance(npc.Center) <= 500f)
                {
                    AI_State = State_Notice;
                    AI_Timer = 0;
                }
                CanAndTryToJumpOut();

            }


            else if (AI_State == State_Notice)
            {
                npc.TargetClosest(true);
                Main.NewText("状态：Notice");
                //Main.NewText("Direction:" + npc.direction);

                if (Main.player[npc.target].Distance(npc.Center) < 250f)
                {
                    AI_State = State_Attack;
                    AI_Timer = 0;
                }

                else if (Main.player[npc.target].Distance(npc.Center) < 450f)
                {

                    AI_State = State_Jump;
                    AI_Timer = 0;

                }

                else
                {
                    if (!npc.HasValidTarget || Main.player[npc.target].Distance(npc.Center) > 500f)
                    {
                        AI_State = State_HangOut;
                        AI_Timer = 0;
                    }
                }
                CanAndTryToJumpOut();
            }

            else if (AI_State == State_Jump)
            {
                Main.NewText("状态：Jump");
                npc.TargetClosest(true);
                AI_Timer++;
                if (AI_Timer == 1)
                {
                    npc.velocity = new Vector2(npc.direction * MoveSpeed, -5f);
                }
                if (AI_Timer > 60 || npc.velocity.Y == 0)
                {
                    AI_State = State_Fall;
                    AI_Timer = 0;
                }

            }

            else if (AI_State == State_Fall)
            {
                Main.NewText("状态：Fall");
                if (npc.velocity.Y == 0)
                {
                    AI_State = State_Notice;
                    npc.velocity.X = 0;
                    AI_Timer = 0;
                }
            }

            else if (AI_State == State_Attack)
            {
                //Main.NewText("状态：Attack");
                npc.TargetClosest(true);
                AI_Timer++;
                if (AI_Timer == 1)
                    npc.velocity = new Vector2(npc.direction * MoveSpeed * 2, 0);
                if (npc.direction != npc.oldDirection)
                {
                    AI_Timer = 0;
                }
                //if (AI_Timer >= 30)
                //{
                //    Vector2 DirectionToPlayer = Main.player[npc.target].position - npc.position;
                //    DirectionToPlayer.Normalize();
                //    Projectile.NewProjectile(npc.position,
                //        DirectionToPlayer * 3,
                //        ModContent.ProjectileType<ScienceBullet>(),
                //        10,
                //        3f);
                //    AI_Timer = 0;
                //}

                if (!npc.HasValidTarget || Main.player[npc.target].Distance(npc.Center) > 500f)
                {
                    AI_State = State_HangOut;
                    AI_Timer = 0;
                }
                CanAndTryToJumpOut();
            }
        }

        private const int Frame_Asleep = 0;
        private const int Frame_Notice = 1;
        private const int Frame_Falling = 1;
        private const int Frame_HangOut_1 = 0;
        private const int Frame_HangOut_2 = 1;
        private const int Frame_HangOut_3 = 2;
        private const int Frame_Attack_1 = 0;
        private const int Frame_Attack_2 = 1;
        private const int Frame_Attack_3 = 2;
        private const int Frame_Jump_1 = 0;
        private const int Frame_Jump_2 = 1;
        private const int Frame_Jump_3 = 2;

        public override void FindFrame(int frameHeight)
        {
            npc.spriteDirection = npc.direction;
            if (AI_State == State_Asleep || AI_State == State_Ini)
            {
                // npc.frame.Y is the goto way of changing animation frames. npc.frame starts from the top left corner in pixel coordinates, so keep that in mind.
                npc.frame.Y = Frame_Asleep * frameHeight;
            }

            else if (AI_State == State_HangOut)
            {
                npc.frameCounter++;
                if (npc.frameCounter < 10)
                {
                    npc.frame.Y = Frame_HangOut_1 * frameHeight;
                }
                else if (npc.frameCounter < 20)
                {
                    npc.frame.Y = Frame_HangOut_2 * frameHeight;
                }
                else if (npc.frameCounter < 30)
                {
                    npc.frame.Y = Frame_HangOut_3 * frameHeight;
                }
                else
                {
                    npc.frameCounter = 0;
                }
            }

            else if (AI_State == State_Notice)
            {
                // Going from Notice to Asleep makes our npc look like it's crouching to jump.
                if (AI_Timer < 10)
                {
                    npc.frame.Y = Frame_Notice * frameHeight;
                }
                else
                {
                    npc.frame.Y = Frame_Asleep * frameHeight;
                }
            }
            else if (AI_State == State_Attack)
            {
                npc.frameCounter++;
                if (npc.frameCounter < 10)
                {
                    npc.frame.Y = Frame_Attack_1 * frameHeight;
                }
                else if (npc.frameCounter < 20)
                {
                    npc.frame.Y = Frame_Attack_2 * frameHeight;
                }
                else if (npc.frameCounter < 30)
                {
                    npc.frame.Y = Frame_Attack_3 * frameHeight;
                }
                else
                {
                    npc.frameCounter = 0;
                }
            }
            else if (AI_State == State_Jump)
            {
                npc.frameCounter++;
                if (npc.frameCounter < 10)
                {
                    npc.frame.Y = Frame_Jump_1 * frameHeight;
                }
                else if (npc.frameCounter < 20)
                {
                    npc.frame.Y = Frame_Jump_2 * frameHeight;
                }
                else if (npc.frameCounter < 30)
                {
                    npc.frame.Y = Frame_Jump_3 * frameHeight;
                }
                else
                {
                    npc.frameCounter = 0;
                }
            }

            else if (AI_State == State_Fall)
            {
                npc.frame.Y = Frame_Falling * frameHeight;
            }

        }
    }

    /// <summary>
    /// 这是远程的自主作战单元，可以飞，并且可以发射远程的可以摧毁物块的弹幕
    /// ai以外的东西基本全抄史莱姆
    /// AI行为打算类似于沙漠秃鹫，但是有所不同。
    /// 等一手画师
    /// </summary>
    public class ScienceAutonomousCombatUnit_2 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Science Autonomous Combat Unit :ranged");
            DisplayName.AddTranslation(GameCulture.Chinese, "自主作战单元：远程");
            Main.npcFrameCount[npc.type] = 6;
        }


        public override void SetDefaults()
        {
            npc.width = 32;
            npc.height = 32;
            npc.aiStyle = -1; // This npc has a completely unique AI, so we set this to -1. The default aiStyle 0 will face the player, which might conflict with custom AI code.
            npc.damage = 7;
            npc.defense = 2;
            npc.lifeMax = 25;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            //npc.alpha = 175;
            //npc.color = new Color(0, 80, 255, 100);
            npc.value = 25f;
            //npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.Confused] = false;
        }
        //刷新机会，只会刷新在特殊地形上
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<SciencePlayer>().ZoneScience)
            {
                return 1f;
            }
            else
                return 0f;
        }


        private const int AI_State_Slot = 0;
        private const int AI_Timer_Slot = 1;
        private const int AI_Shoot_Timer_Slot = 2;
        private const int State_Asleep = 0;
        private const int State_Fly = 1;
        private const int State_Hover = 2;
        private const int State_Fall = 3;

        public float AI_State
        {
            get => npc.ai[AI_State_Slot];
            set => npc.ai[AI_State_Slot] = value;
        }

        public float AI_Timer
        {
            get => npc.ai[AI_Timer_Slot];
            set => npc.ai[AI_Timer_Slot] = value;
        }

        public float AI_Shoot_Timer
        {
            get => npc.ai[AI_Shoot_Timer_Slot];
            set => npc.ai[AI_Shoot_Timer_Slot] = value;
        }

        //类似秃鹫的ai？
        public override void AI()
        {
            if (AI_State == State_Asleep)
            {
                Main.NewText("Asleep");
                npc.TargetClosest(false);
                if (npc.HasValidTarget && Main.player[npc.target].Distance(npc.Center) < 500f)
                {
                    AI_State = State_Fly;
                    AI_Timer = 0;
                }
            }


            else if (AI_State == State_Fly)
            {
                Main.NewText("fly");
                AI_Timer++;
                npc.TargetClosest(true);
                if(AI_Timer == 1)
                {
                    npc.velocity = new Vector2(npc.direction * 2f, -10f);
                }
                else if(npc.velocity.Y >= 0)
                {
                    AI_State = State_Hover;
                    AI_Timer = 0;
                }
            }



            else if(AI_State == State_Hover)
            {
                AI_Timer++;
                AI_Shoot_Timer++;
                Main.NewText("hover");
                npc.TargetClosest(true);
                float distance = Main.player[npc.target].Distance(npc.Center);
                if(distance <= 500f)
                {
                    npc.velocity += new Vector2(0, -.5f);
                }
                if (Math.Abs(npc.velocity.X) <= 3)
                {
                    npc.velocity += new Vector2(npc.direction * .2f, 0);
                }
                if (AI_Shoot_Timer >= 60)
                {
                    Vector2 ShootDirection = Vector2.Normalize(Main.player[npc.target].Center - npc.Center);

                    Projectile.NewProjectileDirect(
                        npc.position,
                        ShootDirection * 3,
                        ModContent.ProjectileType<ScienceBullet>(),
                        10,
                        3f);

                    AI_Shoot_Timer = 0;
                }
                if(!npc.HasValidTarget || distance >= 600f)
                {
                    AI_State = State_Fall;
                    AI_Timer = 0;
                    AI_Shoot_Timer = 0;
                }
            }


            else if(AI_State == State_Fall)
            {
                Main.NewText("fall");
                if (npc.velocity.Y == 0)
                {
                    npc.velocity.X = 0;
                    AI_State = State_Asleep;
                }
            }

        }

        private const int Frame_Asleep = 0;
        private const int Frame_Shoot = 1;
        private const int Frame_Falling = 2;
        private const int Frame_Fly_1 = 3;
        private const int Frame_Fly_2 = 4;
        private const int Frame_Fly_3 = 5;

        public override void FindFrame(int frameHeight)
        {
            npc.spriteDirection = npc.direction;
            if(AI_State == State_Asleep)
            {
                npc.frame.Y = Frame_Asleep * frameHeight;
            }
            else if(AI_State == State_Fly || AI_State == State_Hover)
            {
                npc.frameCounter++;
                if(npc.frameCounter < 10)
                {
                    npc.frame.Y = Frame_Fly_1 * frameHeight;
                }
                else if(npc.frameCounter < 20)
                {
                    npc.frame.Y = Frame_Fly_2 * frameHeight;
                }
                else if(npc.frameCounter < 30)
                {
                    npc.frame.Y = Frame_Fly_3 * frameHeight;
                }
                else
                {
                    npc.frameCounter = 0;
                }
            }

            else if(AI_State == State_Fall)
            {
                npc.frame.Y = Frame_Falling * frameHeight;
            }

            //else if(AI_State == State_Shoot)
            //{
            //    npc.frame.Y = Frame_Shoot * frameHeight;
            //}
        }
    }
}
