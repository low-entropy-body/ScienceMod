
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria;
using Terraria.ID;
using Science.Buffs;

namespace Science.Items
{
    class ScienceSCP500:ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SCP-500");
            Tooltip.SetDefault("this is SCP-500");
            Tooltip.AddTranslation(GameCulture.Chinese, "活死人，肉白骨");
        }

        public override void SetDefaults()
        {
            item.width = 14;
            item.height = 24;
            item.useAnimation = 17;
            item.useTime = 17;
            item.maxStack = 5;
            item.rare = 13;
            item.value = Item.sellPrice(0, 50, 0, 0);

            item.useStyle = 2;
            item.UseSound = SoundID.Item3;
            item.consumable = true;
            item.useTurn = true;
            item.potion = true;
            item.healLife = 1;
            
        }
        //动态调整了
        public override void UpdateInventory(Player player)
        {
            item.healLife = player.statLifeMax2 / 2 - player.statLife;
        }

        public override bool UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer && CanUseItem(player) )
            {
                //if (player.statLife <= player.statLifeMax2 / 2)
                //{
                //    player.statLife = player.statLifeMax2 / 2;
                //}
                player.AddBuff(ModContent.BuffType<ScienceBuffSCP500>(), 3600);
                //一分半的抗药性
                player.AddBuff(21,5400);
            }
            
            return false;
        }
        
        public override void GetHealLife(Player player, bool quickHeal, ref int healValue)
        {
            if (quickHeal == true)
            {
                UseItem(player);
                ConsumeItem(player);
            }
        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }


}
