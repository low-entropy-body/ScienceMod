using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Science.Buffs;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Science.Items
{
    class ScienceInstancedGlobalItem:GlobalItem
    {
        public override bool InstancePerEntity => true;

        private bool IsVanillaRecallItem(Item item)
        {
            if (item.type == ItemID.CellPhone ||
               item.type == ItemID.IceMirror ||
               item.type == ItemID.MagicMirror ||
               item.type == ItemID.RecallPotion)
            {
                return true;
            }
            else
                return false;
        }

        public override bool CanUseItem(Item item, Player player)
        {
            if (IsVanillaRecallItem(item) && player.HasBuff(ModContent.BuffType<ScienceEnergyDepletion>()))
            {
                return false;
            }
            else
                return true;
            
        }


        public override bool UseItem(Item item, Player player)
        {
            if (IsVanillaRecallItem(item))
            {
                player.AddBuff(ModContent.BuffType<ScienceEnergyDepletion>(), ScienceEnergyDepletion.BuffTotalTime * 5);
                return true;
            }
            else
                return false;
        }

        public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (IsVanillaRecallItem(item))
            {
                Color color_draw;
                Player player = Main.player[Main.myPlayer];
                Vector2 draw_position = position + Main.inventoryBackTexture.Size() * Main.inventoryScale / 2f - Main.cdTexture.Size() * Main.inventoryScale / 2f;

                //必须要判断index是否为-1，不然会产生一个很奇怪的bug
                int index = player.FindBuffIndex(ModContent.BuffType<ScienceEnergyDepletion>());
                if (index != -1)
                    //透明度渐变
                    color_draw = item.GetAlpha(Color.White) * ((float)player.buffTime[index] / (ScienceEnergyDepletion.BuffTotalTime));
                else
                    color_draw = Color.White;

                //if(Main.expertMode)
                //	color_draw = item.GetAlpha(Color.White) * ((float)player.buffTime[player.FindBuffIndex(ModContent.BuffType<ScienceEnergyDepletion>())] / ScienceEnergyDepletion.BuffTotalTime * 2);
                //else
                //	color_draw = item.GetAlpha(Color.White) * ((float)player.buffTime[player.FindBuffIndex(ModContent.BuffType<ScienceEnergyDepletion>())] / ScienceEnergyDepletion.BuffTotalTime);

                if (!CanUseItem(item, player))
                    spriteBatch.Draw(Main.cdTexture, draw_position, null, color_draw, 0f, Main.cdTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
                else
                    return;
            }

            else
                base.PostDrawInInventory(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
    }


}
