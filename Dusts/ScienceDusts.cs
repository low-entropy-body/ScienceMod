using Terraria;
using Terraria.ModLoader;

namespace Science.Dusts
{
    public class ScienceDustBlueGas : ModDust
    {
        public int timeleft;
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = true;
            timeleft = 60;
        }

        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.scale *= 0.999f;
            timeleft--;
            Lighting.AddLight(dust.position, 0.5f, 0.5f, 0.5f);
            if (timeleft <= 0)
            {
                dust.active = false;
            }
            return false;
        }
    }
}
