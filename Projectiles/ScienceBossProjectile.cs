using Terraria.ModLoader;
using Terraria.Localization;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Science.Dusts;
using System;
using Terraria.GameContent.Achievements;

namespace Science.Projectiles
{
    public class ScienceBoss1GuidedMissileProjectile : ModProjectile
    {
        public Texture2D texture;
        private int BlueGasDustType;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Guided Missile");
            DisplayName.AddTranslation(GameCulture.Chinese, "导弹");
            Main.projFrames[projectile.type] = 3;
            texture = mod.GetTexture("Projectiles/ScienceBoss1GuidedMissileProjectile");
            BlueGasDustType = ModContent.DustType<ScienceDustBlueGas>();
            
        }

        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 80;
            projectile.friendly = false;
            projectile.aiStyle = -1;
            projectile.hostile = true;
            projectile.ranged = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 600;
            projectile.alpha = 0;
            projectile.light = 0.1f;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.rotation =  3.1415f;
            projectile.light = 1f;
        }

        public override void AI()
        {
            projectile.frameCounter++;

            if(projectile.frameCounter == 1)
            {
                projectile.rotation = projectile.velocity.ToRotation() + 3.1415f / 2;
            }
            if(projectile.frameCounter % 30 < 10)
            {
                projectile.frame = 0;
            }
            else if(projectile.frameCounter % 30 < 20)
            {
                projectile.frame = 1;
            }
            else
            {
                projectile.frame = 2;
            }
            
        }

        public override void Kill(int timeLeft)
        {
            // Play explosion sound
            Main.PlaySound(SoundID.Item15, projectile.position);
            // Smoke Dust spawn
            for (int i = 0; i < 50; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Smoke, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }
            // Fire Dust spawn
            for (int i = 0; i < 80; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Fire, 0f, 0f, 100, default(Color), 3f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 5f;
                dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Fire, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 3f;
            }
            // Large Smoke Gore spawn
            for (int g = 0; g < 2; g++)
            {
                int goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
                goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
                goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
                goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
            }
        }
        public override void PostAI()
        {
            projectile.timeLeft--;
            for(int i = -25;i <= 25; i++)
            {
                //Dust.NewDustDirect(projectile.Center + new Vector2(projectile.width * (i / 25), -projectile.height/2),5,5, BlueGasDustType, 0f,projectile.velocity.Y).noGravity = true;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return true;
        }



        //public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        //{
        //    spriteBatch.Draw(texture,
        //        projectile.Center - Main.screenPosition,
        //        new Rectangle(0, projectile.frame * (int)texture.Size().Y / 3, projectile.width, projectile.height),
        //        Color.White,
        //        projectile.rotation,
        //        new Vector2(texture.Width, texture.Height / 3) / 2,
        //        1f,
        //        SpriteEffects.None,
        //        0f);

        //}
    }

    /// <summary>
    /// boss1小姐姐激光
    /// </summary>
    public class ScienceBoss1LaserProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Deadly Laser");
            DisplayName.AddTranslation(GameCulture.Chinese, "泯灭激光");
            Main.projFrames[projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            projectile.width = 25;
            projectile.height = 128;
            projectile.friendly = false;
            projectile.aiStyle = -1;
            projectile.hostile = true;
            projectile.ranged = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 60;
            projectile.alpha = 0;
            projectile.light = 0.1f;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.rotation = 1.57f;
            projectile.scale = 2f;
        }

        public override void AI()
        {
            projectile.timeLeft--;
            projectile.frameCounter++;


            if (projectile.frameCounter % 50 <= 5)
            {
                projectile.frame = 0;
            }
            else if (projectile.frameCounter % 50 <= 10)
            {
                projectile.frame = 1;
            }

            else if (projectile.frameCounter % 50 <= 15)
            {
                projectile.frame = 2;
            }
            else if (projectile.frameCounter % 50 <= 20)
            {
                projectile.frame = 3;
            }
            else if(projectile.frameCounter % 50 <= 25)
            {
                projectile.frame = 4;
            }
            else if(projectile.frameCounter % 50 <= 30)
            {
                projectile.frame = 3;
            }
            else if (projectile.frameCounter % 50 <= 35)
            {
                projectile.frame = 2;
            }
            else
            {
                projectile.frame = 1;
            }
        }
    }
}
