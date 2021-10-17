using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Science.Projectiles;
namespace Science.Items.Weapons
{
    public class ScienceGun:ModItem
    {

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            // 这个是物品名字，也就是忽略游戏语言的情况下显示的文字
            DisplayName.SetDefault("Gun");
            // 推荐通过AddTranslation的方式添加其在切换到中文的时候显示中文名字
            DisplayName.AddTranslation(GameCulture.Chinese, "科学手枪");

            // 物品的描述，加入换行符 '\n' 可以多行显示哦
            Tooltip.SetDefault("a gun");
            Tooltip.AddTranslation(GameCulture.Chinese, "穷则精准打击，富则火力覆盖");
        }


        public override void SetDefaults()
        {
            // 不然…… 你试试就知道了
            item.damage = 1;

            item.knockBack = 1f;

            // 物品的基础暴击几率，比正常物品少了 4% 呢
            item.crit = 96;

            // 物品的稀有度，由-1到13越来越高，具体参考维基百科或者我的博客
            item.rare = 13;

            // 攻击速度和攻击动画持续时间！
            // 这个数值越低越快，因为TR游戏速度每秒是60帧，这里的10就是
            // 10.0 / 60.0 = 0.1666... 秒使用1次！也就是一秒6次
            // 一般来说我们要把这两个值设成一样，但也有例外的时候，我们以后会讲
            item.useTime = 3;
            item.useAnimation = 3;

            item.useStyle = ItemUseStyleID.HoldingOut;

            item.autoReuse = true;

            item.ranged = true;

            // 物品的价格，这里用sellPrice，也就是卖出物品的价格作为基准
            item.value = Item.sellPrice(0, 1, 0, 0);

            // 设置这个物品使用时发出的声音，以后会讲到怎么调出其他声音
            // 在这里我用的是开枪的声音
            item.UseSound = SoundID.Item36;

            // 物品的碰撞体积大小，可以与贴图无关，但是建议设为跟贴图一样的大小
            // 不然鬼知道会不会发生奇怪的事情（无所谓的）
            item.width = 24;
            item.height = 24;

            // 让它变小一点
            item.scale = 0.85f;

            // 最大堆叠数量，唔，对于一般武器来说，即使你堆了99个，使用的时候还是只有一个的效果
            item.maxStack = 1;

            //-------------------------------------------------------------------------
            // 接下来就是枪械武器独特的属性

            // noMelee代表这个武器使用的时候贴图会不会造成伤害
            // 如果你希望开枪的时候你的手枪还能敲在敌人头上就把它设为false
            // 反正我不希望：（，就当枪本身没有伤害吧
            item.noMelee = true;

            // 决定枪射出点什么和射出的速度的量
            // 这里我让枪射出子弹，并且以 （像素 / 帧） 的速度射出去
            item.shoot = ModContent.ProjectileType<ScienceDimensionalPhantom_Arrow_Purple>();
            item.shootSpeed = 6f;

            // 选择这个枪射出（的时候消耗什么作为弹药，这里选择子弹
            // 你也可以删（或者注释）掉这一句，这样枪就什么都不消耗了
            //【重要】如果设置了消耗什么弹药，那么之前shoot设置的值就会被弹药物品的属性所覆盖
            // 也就是说，你到底射出的是什么就由弹药决定了
            //item.useAmmo = AmmoID.Bullet;

        }
        

        //60%几率不消耗弹药
        public override bool ConsumeAmmo(Player player)
        {
            // 为了有60的几率返回false就是40%几率返回true
            return Main.rand.Next(10) < 4;
            //Main.rand.Next(10)的意思就是在[0,10)的范围内随机选取一个整数。
        }

        //public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        //{
        //    Projectile refProjectile = new Projectile();
        //    refProjectile.SetDefaults(type);
        //    Dust dust = Dust.NewDustDirect(position, refProjectile.width, refProjectile.height, DustID.Flare_Blue, speedX, speedY, 225, default, 3);
        //    dust.noGravity = true;
        //    dust.fadeIn = 60;
        //    return true;
        //}
    }

    public class ScienceBrokenStandardPistol : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Broken Standard Pistol");
            DisplayName.AddTranslation(GameCulture.Chinese, "破旧的制式手枪");

            Tooltip.SetDefault("oooo");
            Tooltip.AddTranslation(GameCulture.Chinese, "许多模块被破坏了，但是还能使用\n" + "有一半的几率发射出假弹");
        }

        public override void SetDefaults()
        {
            item.height = 46;
            item.width = 72;
            item.scale = 0.5f;
            item.damage = 5;
            item.crit = -4;
            item.rare = ItemRarityID.White;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.autoReuse = true;
            item.useTurn = true;
            item.ranged = true;
            item.value = Item.sellPrice(0, 0, 0, 1);
            item.UseSound = SoundID.Item36;
            item.maxStack = 1;
            item.noMelee = true;
            item.shoot = ModContent.ProjectileType<ScienceBlueLaser>();
            item.shootSpeed = 6f;

        }

        public static ModTranslation Missfiring;

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (Main.rand.NextBool())
            {
                Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY), type, 5, 1f, Main.myPlayer);
            }
            else
            {
                CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), Color.Red,"哑火");
            }
            
            return false;
        }
    }

}
