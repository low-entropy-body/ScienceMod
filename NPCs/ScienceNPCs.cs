using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
/// <summary>
/// 专门放所有友方NPC的文件
/// </summary>
namespace Science.NPCs
{

    //自动加载一个NPC的小地图图标用的，小地图图标的名字为：该NPC类名加_Head
    [AutoloadHead]

    class ScienceNPC1:ModNPC
    {
        public override string Texture => "Science/NPCs/ScienceNPC1";

        //开派对时候的替换材质,没有可不写
        //public override string[] AltTextures => new[] { "Science/NPCs/ScienceNPC1_Alt_1" };

        public override bool Autoload(ref string name)
        {
            return mod.Properties.Autoload;
        }

        /// <summary>
        /// NPC的名字（而非职业）
        /// </summary>
        public string CodeName;

        public override void SetStaticDefaults()
        {
            
            //该NPC的游戏职业
            DisplayName.SetDefault("ScienceNPC1");
            DisplayName.AddTranslation(GameCulture.Chinese, "联邦智能军需AI");


            //NPC总共帧图数，一般为16+下面两种帧的帧数
            Main.npcFrameCount[npc.type] = 25;


            //额外活动帧，一般为5
            //关于这个，一般来说是除了攻击帧以外的那几帧（包括坐下、谈话等动作），但实际上填写包含攻击帧在内的帧数也不影响（比如你写9），如果有知道的可以解释一下。
            NPCID.Sets.ExtraFramesCount[npc.type] = 5;
            
            //攻击帧，这个帧数取决于你的NPC攻击类型，射手填5，战士和投掷填4，法师填2
            //军火商一样的攻击类型
            NPCID.Sets.AttackFrameCount[npc.type] = NPCID.Sets.AttackFrameCount[NPCID.ArmsDealer];

            //巡敌范围，以像素为单位，这个似乎是半径
            NPCID.Sets.DangerDetectRange[npc.type] = 1000;

            //攻击类型，默认为0，想要模仿其他NPC就填他们的AttackType
            NPCID.Sets.AttackType[npc.type] = NPCID.Sets.AttackType[NPCID.ArmsDealer];


            //单次攻击持续时间，越短，则该NPC攻击越快（可以用来模拟长时间施法的NPC）
            NPCID.Sets.AttackTime[npc.type] = 6;



            //NPC遇敌的攻击优先度，该数值越大则NPC遇到敌怪时越会优先选择逃跑，反之则该NPC越好斗。
            //最小一般为1，你可以试试0或负数LOL~
            //NPCID.Sets.MagicAuraColor[base.npc.type] = Color.Purple;
            //如果该NPC属于法师类，你可以加上这个来改变NPC的魔法光环颜色
            NPCID.Sets.AttackAverageChance[npc.type] = 1;
            
        }


        public override void SetDefaults()
        {
            npc.townNPC = true;
            //必带项，没有为什么
            //加上这个后NPC就不会在退出地图后消失了，你可以灵活运用一下


            npc.friendly = true;
            //如果你想写敌对NPC也行


            npc.width = 22;
            //碰撞箱宽


            npc.height = 32;
            //碰撞箱高         
            

            npc.aiStyle = 7;
            //必带项，如果你能自己写出城镇NPC的AI可以不带
            //PS:这个决定了NPC是否能入住房屋


            npc.damage = 10;
            //碰撞伤害，由于城镇NPC没有碰撞伤害所以可以忽略


            npc.defense = 15;
            //防御力


            npc.lifeMax = 250;
            //生命值


            npc.HitSound = SoundID.NPCHit1;
            //受伤音效


            npc.DeathSound = SoundID.NPCDeath1;
            //死亡音效


            npc.knockBackResist = 0f;
            //抗击退性，数字越小抗性越高，0就是完全抗击退


            animationType = NPCID.ArmsDealer;
            //如果你的NPC属于除投掷类NPC以外的其他攻击类型，请带上，值可以填对应NPC的ID
        }


        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            //该入住条件为：击败史莱姆王
            if (NPC.downedSlimeKing)
            {
                return true;
            }
            return false;
        }
        

        //当npc生成时决定，也就是说如果在npc生成后改变语言，那么名字还是原来的语言，除非你杀掉当前npc重新生成
        public override string TownNPCName()
        {

            switch (WorldGen.genRand.Next(5)) 

            {
                case 0:
                    return " PZY-01-03-04 ";
                case 1:
                    return " WHT-01-07-08 ";
                case 2:
                    return " LYL-01-08-09 ";
                case 3:
                    return " ZYL-00-12-12 ";
                default:
                    return " LHY-02-01-06 ";
            }
        }


        public override string GetChat()
        {
            WeightedRandom<string> chat = new WeightedRandom<string>();
            string homeless_ch = "我想我们已经认识对方了，所以....能给个住的地方吗？";
            string homeless_default = "i think...i need a house";

            string hello_ch = "联邦标准型军需AI，代号"+
                Main.npc[NPC.FindFirstNPC(ModContent.NPCType<ScienceNPC1>())].GivenName
                + "向您致敬！";

            string hello_default = "hello";
            
            {
                if (!Main.bloodMoon && !Main.eclipse)
                {
                    //没有对应的房间时
                    if (npc.homeless)
                    {
                        if(GameCulture.Chinese.IsActive)
                            chat.Add(homeless_ch);
                        else
                            chat.Add(homeless_default);

                    }
                    else
                    {
                        if (GameCulture.Chinese.IsActive)
                            chat.Add(hello_ch);
                        else
                            chat.Add(hello_default);
                    }
                }
                //日食时
                //if (Main.eclipse)
                //{
                //    chat.Add("我相信你可以挺过去，让我没事就行！");
                //}
                //血月时
                //if (Main.bloodMoon)
                //{
                //    chat.Add("不！不要血月！！！");
                //}
                return chat;
            }
        }


        public override void SetChatButtons(ref string button, ref string button2)
        {
            //翻译“商店文本”
            button = Language.GetTextValue("LegacyInterface.28");
        }


        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            //如果按下第一个按钮，则开启商店
            if (firstButton)
            {
                shop = true;
            }
            //在if之后可以写第二个按钮的作用
        }


        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            //设置商品
            shop.item[nextSlot].SetDefaults(ItemID.StarCloak);
            //设置价格
            shop.item[nextSlot].value = 10;
            //切换到下一栏，否则你的NPC只能卖一件物品
            //在游戏中能注意到，如果通过卖东西来填满商店格子，还是会有一个格子始终无法填满
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.Wood);
            shop.item[nextSlot].value = 10;
            nextSlot++;
        }



        //设置该NPC的近战/抛射物伤害和击退（取决于NPC攻击类型）
        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 36;
            knockback = 20f;
        }

        //NPC攻击一次后的间隔
        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 1;
            randExtraCooldown = 1;
            //间隔的算法：实际间隔会大于或等于cooldown的值且总是小于cooldown+randExtraCooldown的总和（TR总整这些莫名其妙的玩意）
        }


        //弹幕设置
        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileID.Bullet;
            attackDelay = 1;
            //NPC在出招后多长时间才会发射弹幕
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
        }


        public override bool CheckDead()
        {
            string LastWords;
            if (GameCulture.Chinese.IsActive)
            {
                LastWords = "啊我死了";
            }
            else 
            {
                LastWords = "awsl";
            }
            Main.NewText(LastWords,Colors.RarityBlue);
            return true;
        }

    }


    
}
