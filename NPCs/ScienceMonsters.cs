using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Science.NPCs
{
    public class ScienceIceMan_1 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Science Ice Man");
            DisplayName.AddTranslation(GameCulture.Chinese, "地底冰人");
            Main.npcFrameCount[npc.type] = 3;
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
            npc.aiStyle = 3;
            aiType = 73;
            
            
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if(spawnInfo.player.ZoneRockLayerHeight && spawnInfo.player.ZoneSnow)
            {
                return 1f;
            }
            return 0f;
        }



        public override void FindFrame(int frameHeight)
        {
            npc.spriteDirection = npc.direction;
            npc.frameCounter++;
            if(npc.frameCounter <= 10)
            {
                npc.frame.Y = 0;
            }
            else if(npc.frameCounter <= 20)
            {
                npc.frame.Y = frameHeight;
            }
            else if(npc.frameCounter <= 30)
            {
                npc.frame.Y = 2 * frameHeight;
            }
            else
            {
                npc.frameCounter = 0;
            }
        }

    }

    public class ScienceIceMan_2 : ScienceIceMan_1
    {
        public override string Texture => (GetType().Namespace + "." + Name).Replace('.', '/');
    }

    public class ScienceIceMan_3 : ScienceIceMan_1
    {
        public override string Texture => (GetType().Namespace + "." + Name).Replace('.', '/');
    }
}
