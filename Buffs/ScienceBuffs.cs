using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;

namespace Science.Buffs
{

    //没卵用
    //class ScienceBuffTest : ModBuff
    //{
    //    public override void SetDefaults()
    //    {
    //        DisplayName.SetDefault("测试用buff，清除所有buff");
    //        Main.debuff[Type] = false;
    //        canBeCleared = true;
            
    //    }

    //    public override void Update(Player player, ref int buffIndex)
    //    {
    //        foreach (int id in player.buffType)
    //        {
    //            player.ClearBuff(id);
    //        }
    //    }
    //}


    class ScienceBuff1 : ModBuff
    {
        public override void SetDefaults()
        {
            // 设置buff名字和描述
            DisplayName.SetDefault("toxic");
            DisplayName.AddTranslation(GameCulture.Chinese, "发光剧毒");
            Description.SetDefault("good luck");
            Description.AddTranslation(GameCulture.Chinese, "你正在燃烧你的身体来释放光明");
            // 因为buff严格意义上不是一个TR里面自定义的数据类型，所以没有像buff.XXXX这样的设置属性方式了
            // 我们需要用另外一种方式设置属性

            // 这个属性决定buff在游戏退出再进来后会不会仍然持续，true就是不会，false就是会
            Main.buffNoSave[Type] = true;
            // 当然你也可以用这个属性让这个buff即使不是debuff也不能取消，设为false就是不能取消了
            this.canBeCleared = false;
            // 用来判定这个buff算不算一个debuff，如果设置为true会得到TR里对于debuff的限制，比如无法取消
            Main.debuff[Type] = true;
            
            // 决定这个buff是不是照明宠物的buff
            Main.lightPet[Type] = false;
            // 决定这个buff会不会显示持续时间，false就是会显示，true就是不会显示，一般宠物buff都不会显示
            Main.buffNoTimeDisplay[Type] = false;
            // 决定这个buff在专家模式会不会持续时间加长，false是不会，true是会
            this.longerExpertDebuff = false;
            // 如果这个属性为true，pvp的时候就可以给对手加上这个debuff/buff
            Main.pvpBuff[Type] = true;
            // 决定这个buff是不是一个装饰性宠物，用来判定的，比如消除buff的时候不会消除它
            Main.vanityPet[Type] = false;
        }
        // 注意这里我们选择的是对Player生效的Update，另一个是对NPC生效的Update
        public override void Update(Player player, ref int buffIndex)
        {
            // 把玩家的所有生命回复清除，否则可能会把debuff效果抵消掉
            //player.lifeRegenTime = 0;
            // 让玩家的减血速率增加
            player.lifeRegen -= 2;

            //让玩家发光
            // 这里是按照RGB的比例(0, 1, 0)在玩家中心发出绿光
            // 值越大发的光越强
            Lighting.AddLight(player.Center, 5f, 5f, 5f);
        }

        // 如果返回true就代表buff重新添加成功
        public override bool ReApply(Player player, int time, int buffIndex)
        {
            // 这段代码的作用就是当玩家被重新添加buff的时候延长buff时间
            player.buffTime[buffIndex] += time;
            return true;
        }
    }


    
    /// <summary>
    /// 通用传送物品的cdbuff
    /// </summary>
    public class ScienceEnergyDepletion : ModBuff 
    {

        public static int BuffTotalTime = 3600;
        
        public override void SetDefaults()
        {
            
            DisplayName.SetDefault("EnergyDepletionc");
            DisplayName.AddTranslation(GameCulture.Chinese, "能源枯竭");
            Description.SetDefault("have no energy");
            Description.AddTranslation(GameCulture.Chinese, "ScienceItem1的能源枯竭了");
            
            Main.buffNoSave[Type] = false;
            canBeCleared = false;
            Main.debuff[Type] = true;
            Main.lightPet[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
            this.longerExpertDebuff = false;
            Main.vanityPet[Type] = false;
            
        }
    }


    public class ScienceBuffSCP500 : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("SCP-500 Buff");
            DisplayName.AddTranslation(GameCulture.Chinese, "SCP-500 生效中");

            Description.SetDefault("SCP-500 is come into effect");
            Description.AddTranslation(GameCulture.Chinese, "scp500生效辣！");

        }

        public override void Update(Player player, ref int buffIndex)
        {

            //遍历玩家的Buff列表，清除所有除抗药性，发光剧毒和物品cdBuff以外的DeBuff
            foreach (int id in player.buffType)
            {
                if (
                    Main.debuff[id] == true
                    && id != BuffID.PotionSickness
                    && id != ModContent.BuffType<ScienceEnergyDepletion>()
                    && id != ModContent.BuffType<ScienceBuff1>()
                    ) 
                
                {
                    player.ClearBuff(id);
                }
            }

            //生命恢复速度加20,抵消发光剧毒buff的影响
            if (player.HasBuff(ModContent.BuffType<ScienceBuff1>()))
            {
                player.lifeRegen += 22;
            }

            else
            {
                player.lifeRegen += 20;
            }

        }

    }

    /// <summary>
    /// 现实扭曲装置的cdbuff
    /// </summary>
    public class ScienceRealityDistortionDeviceCD : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("RealityDistortionDeviceCD");
            DisplayName.AddTranslation(GameCulture.Chinese, "现实扭曲装置充能中");

            Description.SetDefault("RealityDistortionDeviceCD");
            DisplayName.AddTranslation(GameCulture.Chinese, "你将不能使用现实扭曲充能装置");

            Main.buffNoSave[Type] = false;
            canBeCleared = false;
            Main.debuff[Type] = true;
            Main.lightPet[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
            longerExpertDebuff = true;
            Main.vanityPet[Type] = false;
        }


    }
}
