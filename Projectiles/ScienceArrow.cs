using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System;
using Science.Utils;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;

namespace Science.Projectiles
{
    /// <summary>
    /// 紫色维度之箭
    /// </summary>
    public class ScienceDimensionalPhantom_Arrow_Purple:ModProjectile
    {
        public override string Texture => (GetType().Namespace + "." + "ScienceDimensionalPhantom_Arrow_Purple").Replace('.', '/');
        public Texture2D texture;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("ScienceDimensionalPhantom_Arrow");
            DisplayName.AddTranslation(GameCulture.Chinese, "维度之箭：星云");
            texture = mod.GetTexture("Projectiles/ScienceDimensionalPhantom_Arrow_Purple");
        }
        public override void SetDefaults()
        {
            projectile.arrow = true;
            projectile.width = 11;
            projectile.height = 23;
            //projectile.aiStyle = 122;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.tileCollide = false;

            //这个ahpha没卵用，因为我已经在后面重新绘制了，要改就去改后面draw里面的color参数
            projectile.alpha = 75;
            projectile.ignoreWater = true;
            projectile.extraUpdates = 1;
            projectile.light = 0.5f;
            projectile.timeLeft = 300;
            //projectile.scale = 0.6f;
            //projectile.rotation = (float) Math.PI;
            //aiType = ProjectileID.PhantasmArrow;
            aiType = 0;
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 15f)
            {
                vector *= 15f / magnitude;
            }
        }

        public override void AI()
        {
            //if (projectile.alpha > 70)
            //{
            //    projectile.alpha -= 15;
            //    if (projectile.alpha < 70)
            //    {
            //        projectile.alpha = 70;
            //    }
            //}
            if (projectile.localAI[0] == 0f)
            {
                AdjustMagnitude(ref projectile.velocity);
                projectile.localAI[0] = 1f;
            }
            Vector2 move = Vector2.Zero;
            float distance = 400f;
            bool target = false;
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5)
                {
                    Vector2 newMove = Main.npc[k].Center - projectile.Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance)
                    {
                        move = newMove;
                        distance = distanceTo;
                        target = true;
                    }
                }
            }
            if (target)
            {
                AdjustMagnitude(ref move);
                projectile.velocity = (10 * projectile.velocity + move) / 11f;
                AdjustMagnitude(ref projectile.velocity);
            }

            projectile.rotation = Vector2.Normalize(projectile.velocity).ToRotation();
            //Lighting.AddLight(projectile.position, new Vector3(3f, 3f, 3f));
        }

        //public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        //{
        //    spriteBatch.End();
        //    spriteBatch.Begin(SpriteSortMode.Deferred,
        //        BlendState.Additive, 
        //        SamplerState.AnisotropicClamp,
        //        DepthStencilState.None,
        //        RasterizerState.CullNone, 
        //        null, 
        //        Main.GameViewMatrix.TransformationMatrix);
        //    return true;
        //}
        //public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        //{
        //    spriteBatch.End();
        //    spriteBatch.Begin(SpriteSortMode.Deferred,
        //        BlendState.AlphaBlend,
        //        SamplerState.AnisotropicClamp, 
        //        DepthStencilState.None,
        //        RasterizerState.CullNone, 
        //        null,
        //        Main.GameViewMatrix.TransformationMatrix);
        //}


        /// /////////////////////////////////////



        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.Draw(texture,
                projectile.Center - Main.screenPosition,
                new Rectangle(0, 0, (int)texture.Size().X,(int)texture.Size().Y),
                new Color(225,225,225,100),
                projectile.rotation,
                texture.Size() / 2,
                1f,
                SpriteEffects.None,
                0f);
            
        }
    }

    public class ScienceDimensionalPhantom_Arrow_Orange : ScienceDimensionalPhantom_Arrow_Purple
    {
        public override string Texture => (GetType().Namespace + "." + "ScienceDimensionalPhantom_Arrow_Orange").Replace('.', '/');
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("ScienceDimensionalPhantom_Arrow");
            DisplayName.AddTranslation(GameCulture.Chinese, "维度之箭：日耀");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            texture = mod.GetTexture("Projectiles/ScienceDimensionalPhantom_Arrow_Orange");
        }
    }

    public class ScienceDimensionalPhantom_Arrow_Green : ScienceDimensionalPhantom_Arrow_Purple
    {
        public override string Texture => (GetType().Namespace + "." + "ScienceDimensionalPhantom_Arrow_Green").Replace('.', '/');
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("ScienceDimeneionalPhantom_Arrow");
            DisplayName.AddTranslation(GameCulture.Chinese, "维度之箭：星璇");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            texture = mod.GetTexture("Projectiles/ScienceDimensionalPhantom_Arrow_Green");
        }
    }

    public class ScienceDimensionalPhantom_Arrow_Blue : ScienceDimensionalPhantom_Arrow_Purple
    {

        public override string Texture => (GetType().Namespace + "." + "ScienceDimensionalPhantom_Arrow_Blue").Replace('.', '/');
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("ScienceDimeneionalPhantom_Arrow");
            DisplayName.AddTranslation(GameCulture.Chinese, "维度之箭：星尘");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            texture = mod.GetTexture("Projectiles/ScienceDimensionalPhantom_Arrow_Blue");
        }
    }

    /// <summary>
    /// 多维幻象的动态弹幕
    /// </summary>
    public class ScienceDimensionalPhantomNormal : ModProjectile
    {
        public Texture2D texture;
        public Item refItem;
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            texture = mod.GetTexture("Projectiles/ScienceDimensionalPhantomNormal");
            refItem = new Item();
            refItem.SetDefaults(ModContent.ItemType<Items.Weapons.ScienceDimensionalPhantom>());
            projectile.width = texture.Width;
            projectile.height = texture.Height/4;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ranged = true;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            projectile.ai[0] += 1f;
            Player owner = Main.player[projectile.owner];

            owner.ChangeDir((int)ScienceUtils.Get01Vec(new Vector2((float)Math.Cos(projectile.rotation), 0)).X);

            //Main.NewText("ProjectileDimeneionalPhantomNormalAlive");
            //Loop through the 4 animation frames, spending x ticks on each.
            projectile.frameCounter++;
            if(projectile.frameCounter <= refItem.useAnimation - 7)
            {
                projectile.frame = 0;
            }
            else if(projectile.frameCounter <= refItem.useAnimation)
            {
                if (projectile.frameCounter % 2 == 1)
                {
                    projectile.frame++;
                }
            }
            else
            {
                projectile.frame = 0;
                projectile.frameCounter = 0;

            }

            if (projectile.ai[0] % refItem.useTime == 0 && Main.mouseLeftRelease)
            {
                //projectile.hide = true;
                projectile.Kill();
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }

        
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player owner = Main.player[projectile.owner];
            Color newColor = Color.White;
            if(owner.stealth <= 0.5)
            {
                newColor = new Color(50, 50, 50, 50);
            }
            Rectangle rectangle = new Rectangle(0, projectile.frame * (int)texture.Size().Y / 4, projectile.width, projectile.height);
            Vector2 pos = owner.Center - Main.screenPosition + Vector2.Normalize(owner.GetModPlayer<SciencePlayer>().ShootVel) * 16;
            spriteBatch.Draw(texture, 
                pos, 
                rectangle, 
                newColor, 
                projectile.rotation, 
                new Vector2(texture.Width, texture.Height / 4) / 2, 
                1f, 
                SpriteEffects.None, 
                0f);
            owner.heldProj = projectile.whoAmI;
        }

    }

    public class ScienceDimensionalPhantomSpecial : ScienceDimensionalPhantomNormal
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            texture = mod.GetTexture("Projectiles/ScienceDimensionalPhantomSpecial");
        }

    }



    /// <summary>
    /// 虚空之翼弓箭的动态弹幕
    /// </summary>
    public class ScienceVoidWings : ModProjectile
    {

        private Texture2D texture;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wings of void");
            DisplayName.AddTranslation(GameCulture.Chinese, "虚空之翼");
            Main.projFrames[projectile.type] = 8;
        }

        private Item refItem = new Item();
        public override void SetDefaults()
        {
            texture = mod.GetTexture("Projectiles/ScienceVoidWings");
            projectile.width = 58;//58
            projectile.height = 122;//122
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;

            projectile.ranged = true;
            projectile.ignoreWater = true;

            refItem.SetDefaults(ModContent.ItemType<Items.Weapons.ScienceVoidWings>());

            
        }

        public override void AI()
        {
            Player owner = Main.player[projectile.owner];
            owner.ChangeDir((int)ScienceUtils.Get01Vec(new Vector2((float)Math.Cos(projectile.rotation), 0)).X);
            projectile.ai[0] += 1f;
            Main.NewText("ProjectileVoidWingAlive");
            //Loop through the 8 animation frames, spending 6 ticks on each.
            if (++projectile.frameCounter >= 6)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 8)
                {
                    projectile.frame = 0;
                }
            }

            
            if (projectile.ai[0] % refItem.useTime == 0 && Main.mouseLeftRelease)
            {
                //projectile.hide = true;
                projectile.Kill();
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //画贴图
            Player owner = Main.player[projectile.owner];
            Rectangle rectangle = new Rectangle(0, projectile.frame * (int)texture.Size().Y/8, projectile.width, projectile.height);
            Vector2 pos = owner.Center - Main.screenPosition + Vector2.Normalize(owner.GetModPlayer<SciencePlayer>().ShootVel) * 16;
            spriteBatch.Draw(texture, pos, rectangle, Color.White, projectile.rotation, new Vector2(texture.Width,texture.Height/8)/2,1f, SpriteEffects.None, 0f);
            owner.heldProj = projectile.whoAmI;
        }
    }



    public class ScienceVoidWingsEnergyBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("EnergeBullet");
            DisplayName.AddTranslation(GameCulture.Chinese, "虚空能量弹");
        }


        public override void SetDefaults()
        {
            projectile.height = 16;
            projectile.width = 16;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            //projectile.extraUpdates = 1;
            projectile.light = 1f;
            projectile.timeLeft = 300;
        }


        
        
        private Vector2 IniSpeedY;
        private ScienceGlobalVoidWingsEnergeBullet global;
        public override void AI()
        {
            projectile.ai[0] += 1f;
            global = projectile.GetGlobalProjectile<ScienceGlobalVoidWingsEnergeBullet>();
            if (projectile.ai[0] == 1f)
            {
                //翻转90度
                IniSpeedY = global.IniSpeedX.RotatedBy((global.IniUp ? -1f : 1f) * Math.PI / 2d);
            }
            //在平行于初速度的方向上为定值，在垂直于初速度的方向上，速度为  Vy * cos（wt）
            projectile.velocity = global.IniSpeedX + IniSpeedY * (float)Math.Cos(projectile.ai[0] * (Math.PI / 60));
        }
    }
    /// <summary>
    /// 用来在不同文件中传递一个参数
    /// </summary>
    public class ScienceGlobalVoidWingsEnergeBullet : GlobalProjectile
    {
        
        public override bool InstancePerEntity => true;

        /// <summary>
        /// 是否开局获得一个向上的速度，否则会获得一个向下的速度
        /// </summary>
        public bool IniUp;
        /// <summary>
        /// 根据发射速度来获得水平初速度
        /// </summary>
        public Vector2 IniSpeedX;
    }
}
