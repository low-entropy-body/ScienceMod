using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Science.Tiles;
using Science.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria.DataStructures;

namespace Science.Items.Weapons
{
    public class ScienceDimensionalPhantom : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("ScienceDimensionalPhantom");
            DisplayName.AddTranslation(GameCulture.Chinese, "多维幻影");

            Tooltip.SetDefault("ScienceBowTip");
            Tooltip.AddTranslation(GameCulture.Chinese, "来自维度间隙");
        }

        public override void SetDefaults()
        {
            item.damage = 50;
            item.crit = 20;
            item.knockBack = 1f;
            item.rare = 13;
            item.useTime = 18;
            item.useAnimation = 18;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.autoReuse = true;
            item.value = Item.sellPrice(0, 50, 0, 0);
            item.ranged = true;
            item.UseSound = SoundID.Item5;
            item.width = 50;
            item.height = 82;
            item.scale = 0.85f;
            item.maxStack = 1;
            item.noMelee = true;
            item.shoot = ProjectileID.WoodenArrowFriendly;
            item.shootSpeed = 50f;
            item.useAmmo = AmmoID.Arrow;
            item.noUseGraphic = true;
        }





        public int ShootTime = 0;
        public Vector2 ShootPos;
        public float ShootSpeedX;
        public float ShootSpeedY;
        public int UsingType;

        private Projectile DimensionalPhantom;
        
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {

            ShootTime++;

            if(ShootTime == 1 && player.ownedProjectileCounts[ModContent.ProjectileType<ScienceDimensionalPhantomSpecial>()] >= 1)
            {
                DimensionalPhantom.Kill();
            }

            if (player.ownedProjectileCounts[ModContent.ProjectileType<ScienceDimensionalPhantomNormal>()] < 1)
            {
                DimensionalPhantom = Projectile.NewProjectileDirect(player.Center, Vector2.Zero, ModContent.ProjectileType<ScienceDimensionalPhantomNormal>(), 0, 0f, Main.myPlayer);
            }
            if(ShootTime == 6)
            {
                //ShootTime不能在这里清0,在usestyle里面
                DimensionalPhantom.Kill();
                DimensionalPhantom = Projectile.NewProjectileDirect(player.Center, Vector2.Zero, ModContent.ProjectileType<ScienceDimensionalPhantomSpecial>(), 0, 0f, Main.myPlayer);            
            }

            DimensionalPhantom.rotation = new Vector2(speedX, speedY).ToRotation();
            DimensionalPhantom.Center = player.Center;
            DimensionalPhantom.frameCounter = item.useAnimation - 6;
            

            ShootPos = position;
            ShootSpeedX = speedX;
            ShootSpeedY = speedY;
            UsingType = type;
            int ShootAmmoArrow = Main.rand.Next(8, 16);
            for (int i = 0; i < ShootAmmoArrow; i++)
            {
                Vector2 RandomDelta = new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2));
                Projectile.NewProjectileDirect(ShootPos, new Vector2(ShootSpeedX, ShootSpeedY) + RandomDelta, type, damage, 1f, Main.myPlayer).timeLeft = 300;
            }

            //同步位置和朝向
            //这里虽然同步了位置，但同步的实际上是碰撞盒，和绘制的位置无关(但是请不要删除同步位置的代码，因为我没删也能跑.
            foreach(Projectile proj in Main.projectile)
            {
                if(proj.type == ModContent.ProjectileType<ScienceDimensionalPhantomNormal>() || proj.type == ModContent.ProjectileType<ScienceDimensionalPhantomSpecial>())
                {
                    proj.rotation = new Vector2(speedX, speedY).ToRotation();
                    proj.Center = player.Center;
                }
            }
            //Main.NewText(ShootTime);
            return false;
        }

        /// <summary>
        /// 第六次发射时候的专用计时器
        /// </summary>
        private int usetime6;
        
        
        public override void UseStyle(Player player)
        {
            

            if (ShootTime >= 6)
            {
                usetime6++;

                ////删除普通动态弹幕，生成特殊动态弹幕
                //if(player.ownedProjectileCounts[ModContent.ProjectileType<ScienceDimensionalPhantomNormal>()] >= 1)
                //{
                //    foreach(Projectile proj in Main.projectile)
                //    {
                //        if(proj.type == ModContent.ProjectileType<ScienceDimensionalPhantomNormal>() && proj.owner == Main.myPlayer)
                //        {
                //            proj.Kill();
                //        }
                //    }
                //}

                //if (player.ownedProjectileCounts[ModContent.ProjectileType<ScienceDimensionalPhantomNormal>()] < 1)
                //{
                //    Projectile.NewProjectileDirect(player.Center, Vector2.Zero, ModContent.ProjectileType<ScienceDimensionalPhantomNormal>(), 0, 0f, Main.myPlayer);
                //}


                //以下代码是用来生成三种不同颜色的弹幕的
                if (usetime6 < 15 && usetime6 % 3 == 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 RandomDelta = new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2));
                        Projectile tempproj = Projectile.NewProjectileDirect(ShootPos + new Vector2(player.direction * -40f, 40f), (new Vector2(ShootSpeedX, ShootSpeedY * 1.3f) + RandomDelta).RotatedBy(Math.PI / 6), ModContent.ProjectileType<ScienceDimensionalPhantom_Arrow_Blue>(), 40, 1f, Main.myPlayer);

                    }
                    //tempproj.velocity = tempproj.velocity * 1.5f;
                }

                else if (usetime6 < 15 && usetime6 % 3 == 1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 RandomDelta = new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2));
                        Projectile tempproj = Projectile.NewProjectileDirect(ShootPos, new Vector2(ShootSpeedX, ShootSpeedY) + RandomDelta, ModContent.ProjectileType<ScienceDimensionalPhantom_Arrow_Orange>(), 40, 1f, Main.myPlayer);

                    }
                    //tempproj.velocity = tempproj.velocity * 1.5f;
                }

                else if (usetime6 < 15 && usetime6 % 3 == 2)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 RandomDelta = new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2));
                        Projectile tempproj = Projectile.NewProjectileDirect(ShootPos + new Vector2(player.direction * -40f, -40f), (new Vector2(ShootSpeedX, ShootSpeedY ) + RandomDelta).RotatedBy(- Math.PI / 6), ModContent.ProjectileType<ScienceDimensionalPhantom_Arrow_Green>(), 40, 1f, Main.myPlayer);

                    }
                    //tempproj.velocity = tempproj.velocity * 1.5f;

                }
                else
                {
                    ShootTime = 0;
                    usetime6 = 0;
                    
                }
            }

            //foreach (Projectile proj in Main.projectile)
            //{
            //    if (proj.type == UsingType && proj.owner == Main.myPlayer) 
            //    {
            //        Vector2 RandomDelta = new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2));
            //        Projectile.NewProjectileDirect(ShootPos + new Vector2(player.direction * -40f, -40f), new Vector2(ShootSpeedX, ShootSpeedY) + RandomDelta, ModContent.ProjectileType<ScienceDimeneionalPhantom_Arrow_Purple>(), 10, 1f, Main.myPlayer);
            //    }
            //}
        }

        public override void AddRecipes()
        {
            ScienceModRecipe recipe = new ScienceModRecipe(mod);
            recipe.AddIngredient(ItemID.Phantasm, 1);
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddIngredient(ItemID.FragmentVortex, 20);
            recipe.AddIngredient(ItemID.FragmentNebula, 20);
            recipe.AddIngredient(ItemID.FragmentSolar, 20);
            recipe.AddIngredient(ItemID.FragmentStardust, 20);
            recipe.AddTile(ModContent.TileType<ScienceHighDimensionalMatterProjector>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }


    public class ScienceVoidWings : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wings of void");
            DisplayName.AddTranslation(GameCulture.Chinese, "虚空之翼");

            Tooltip.SetDefault("temp");
            Tooltip.AddTranslation(GameCulture.Chinese, "裂隙之眼在注视着一切");

            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 8));
            
            //ItemID.Sets.ItemIconPulse[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.width = 58;
            item.height = 122;
            item.damage = 10;
            item.crit = 20;
            item.knockBack = 1f;
            item.rare = 13;
            item.useTime = 48;
            item.useAnimation = 48;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.autoReuse = true;
            item.useTurn = true;
            item.value = Item.sellPrice(1, 0, 0, 0);
            item.ranged = true;
            item.UseSound = SoundID.Item5;
            //item.scale = 0.85f;
            item.maxStack = 1;
            item.noMelee = true;
            item.shoot = ModContent.ProjectileType<Projectiles.ScienceVoidWings>();
            item.shootSpeed = 5f;
            item.useAmmo = AmmoID.Arrow;
            item.noUseGraphic = true;
        }



        private Projectile VoidWingsProjectile;
        private int TypeEnergyBullet = ModContent.ProjectileType<ScienceVoidWingsEnergyBullet>();
        private int TypeScienceVoidWings = ModContent.ProjectileType<Projectiles.ScienceVoidWings>();
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            
            if(player.ownedProjectileCounts[TypeScienceVoidWings] < 1)
            {
                VoidWingsProjectile = Projectile.NewProjectileDirect(player.Center, Vector2.Zero, TypeScienceVoidWings, 0, 0, Main.myPlayer);
                Main.NewText("VoidWingsProjCreated");
            }

            VoidWingsProjectile.rotation = new Vector2(speedX, speedY).ToRotation();
            VoidWingsProjectile.Center = player.Center;


            //todo: 优化一下生成位置，让他的角度根据发射角度变化
            //这个是在下面的能量弹
            Projectile EnergeBullet1 = Projectile.NewProjectileDirect(position + new Vector2(0, 16), new Vector2(speedX, speedY), TypeEnergyBullet, damage, 0f, Main.myPlayer);
            EnergeBullet1.GetGlobalProjectile<ScienceGlobalVoidWingsEnergeBullet>().IniUp = true;
            EnergeBullet1.GetGlobalProjectile<ScienceGlobalVoidWingsEnergeBullet>().IniSpeedX = new Vector2(speedX, speedY);

            //这个是在上面的能量弹
            Projectile EnergeBullet2 = Projectile.NewProjectileDirect(position - new Vector2(0, 16), new Vector2(speedX, speedY), TypeEnergyBullet, damage, 0f, Main.myPlayer);
            EnergeBullet2.GetGlobalProjectile<ScienceGlobalVoidWingsEnergeBullet>().IniUp = false;
            EnergeBullet2.GetGlobalProjectile<ScienceGlobalVoidWingsEnergeBullet>().IniSpeedX = new Vector2(speedX, speedY);

            return false;
        }
    }
}
