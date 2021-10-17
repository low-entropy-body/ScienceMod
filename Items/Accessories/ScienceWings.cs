using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Science.Items.Accessories
{
    [AutoloadEquip(EquipType.Wings)]
    public class ScienceWings : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.width = 22;
            item.height = 22;
            item.accessory = true;
            //item.defense = 2;
            item.rare = 8;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.expert = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);

            // 翅膀的最长持续时间
            player.wingTimeMax = 100;
            // 让翅膀的wingTime在每一帧都是一个固定的值,可以无限飞翔
            player.wingTime = player.wingTimeMax;

            //这段将会让人物悬停(类似ufo)
            //if (!player.controlJump && !player.controlDown)
            //{
            //    player.gravDir = 0f;
            //    player.velocity.Y = 0;
            //    player.gravity = 0;
            //    player.noFallDmg = true;
            //}
            //if (player.controlDown)
            //{
            //    player.gravity = Player.defaultGravity;
            //    player.gravDir = 1;
            //    player.noFallDmg = true;
            //}
            //
        }
        public override void AddRecipes()
        {
            base.AddRecipes();
        }


        // 控制翅膀垂直飞行的参数
        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            /*
             ascentWhenFalling这个属性是控制玩家在下落的时候开启翅膀的爬升率
            如果设为0那么下落的时候就需要比较长的时间才能 上去。
             
            ascentWhenRising    玩家切换到上升状态的时候开启翅膀的爬升率

            maxAscentMultiplier 是玩家可以到达的最大爬升率

            maxCanAscendMultiplier暂时不知道有什么用

            constantAscend是正常飞行时翅膀的爬升率
             */
        }
        // 控制翅膀水平飞行的参数
        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            /*
             speed就是你一直按着方向键能到多快，acceleration就是加速度
             */

        }
    }
}
