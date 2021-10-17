
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Science.Items;
using Science.Items.Weapons;
using Science.Tiles;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Science.NPCs
{
    class ScienceGlobalNPC : GlobalNPC
    {
        public static bool downedScienceBoss1 = false;

        public override bool CheckDead(NPC npc)
        {
            //史莱姆第一次死亡后刷出联邦AI
            if (npc.type == NPCID.KingSlime && !NPC.downedSlimeKing)
            {
                NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, ModContent.NPCType<ScienceNPC1>());
            }

            //邪恶怪物第一次死亡后生成亚克矿
            else if (!NPC.downedBoss2 &&(npc.type == NPCID.EaterofWorldsHead ||npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail ||npc.type == NPCID.BrainofCthulhu))
            {
                int EaterofWorldHealth = 0;
                if (!WorldGen.crimson)
                {
                    foreach (NPC localnpc in Main.npc)
                    {
                        int localtype = localnpc.type;
                        if (localtype == NPCID.EaterofWorldsHead || localtype == NPCID.EaterofWorldsBody || localtype == NPCID.EaterofWorldsTail)
                        {
                            EaterofWorldHealth += localnpc.life;
                        }
                    }
                    if(EaterofWorldHealth < 1)
                        GenYakeOre();
                }
                else
                    GenYakeOre();
            }

            return true;
        }

        //生成亚克矿
        private void GenYakeOre()
        {
            for (int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY) * 6E-05); k++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
                WorldGen.TileRunner(x, y,
                    WorldGen.genRand.Next(3, 6),
                    WorldGen.genRand.Next(2, 6),
                    ModContent.TileType<ScienceKillounOre>());
            }
            Main.NewText("邪恶的boss已经被击败");
            Main.NewText("亚克矿已经在地下生成");
        }

        /// <summary>
        /// 禁止在机械地形生成原版生物
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="spawnInfo"></param>
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            
            if (spawnInfo.player.GetModPlayer<SciencePlayer>().ZoneScience)
            {
                pool[0] = 0f;
            }
        }

        public override void NPCLoot(NPC npc)
        {
            if (npc.boss)
            {
                npc.DropItemInstanced(npc.position, npc.Size, ModContent.ItemType<ScienceQuarkPolymer>(), npc.lifeMax <= 10000 ? npc.lifeMax/100 : 100 );
            }
        }

        //测试shader
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Science.npcEffect.Parameters["uTime"].SetValue((float)Main.time);
            Science.npcEffect.Parameters["uImageSize"].SetValue(Main.npcTexture[npc.type].Size());
            //Science.npcEffect.CurrentTechnique.Passes["RainBow"].Apply();
            //Science.npcEffect.CurrentTechnique.Passes["Edge"].Apply();
            return true;
        }
    }
}
