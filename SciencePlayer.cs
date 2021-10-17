using Microsoft.Xna.Framework;
using Science.Items;
using Science.Items.Weapons;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Science.Utils;
using Science.Projectiles;
using Terraria.GameInput;
using Science.Buffs;

namespace Science
{
    class SciencePlayer : ModPlayer
    {
        /// <summary>
        /// 是否处于机械地形
        /// </summary>
        public bool ZoneScience;
        /// <summary>
        /// 计算玩家背包中现实扭曲仪的数量
        /// </summary>
        public int NumberOfRealityDistortionDevice;
        /// <summary>
        /// 传递一个flag给现实扭曲仪，是否可以开始发射弹幕
        /// </summary>
        public bool BeginSpawnRealityDistortion;
        /// <summary>
        /// 现实扭曲仪的计时器
        /// </summary>
        public int RealityDistortionTimer = 0;
        

        public bool ShouldKillVoidWingsProjectile;
        public Vector2 ShootRelatePos;
        public Vector2 ShootPos;
        public Vector2 ShootVel;

        
        public override void UpdateBiomes()
        {
            ZoneScience = ScienceWorld.ScienceTiles >= 50;
        }


        //客机生物群落同步
        public override bool CustomBiomesMatch(Player other)
        {
            SciencePlayer modOther = other.GetModPlayer<SciencePlayer>();
            return ZoneScience == modOther.ZoneScience;
            // If you have several Zones, you might find the &= operator or other logic operators useful:
            // bool allMatch = true;
            // allMatch &= ZoneExample == modOther.ZoneExample;
            // allMatch &= ZoneModel == modOther.ZoneModel;
            // return allMatch;
            // Here is an example just using && chained together in one statemeny;
            // return ZoneExample == modOther.ZoneExample && ZoneModel == modOther.ZoneModel;
        }

        //初始物品
        public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
        {
            Item refItem = new Item();
            refItem.SetDefaults(ModContent.ItemType<ScienceItem1>());
            items.Add(refItem);
            refItem = new Item();
            refItem.SetDefaults(ModContent.ItemType<ScienceBrokenStandardPistol>());
            items.Add(refItem);
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            //当手持维度幻影弓的时候，如果箭命中敌人发射紫色维度之箭
            Item SelectedItem = player.inventory[player.selectedItem];
            Vector2 Direction = Vector2.Normalize(target.Center - player.Center);
            if (SelectedItem.type == ModContent.ItemType<ScienceDimensionalPhantom>() && proj.arrow && !ScienceUtils.IsDimensionalPhantomArrow(proj))
            {

                Vector2 RandomDelta1 = new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2));
                Vector2 RandomDelta2 = new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2));
                Projectile.NewProjectileDirect(player.Center + Direction * 16f + new Vector2(0f, 16f), Direction * 15f + RandomDelta1, ModContent.ProjectileType<ScienceDimensionalPhantom_Arrow_Purple>(), 10, 1f, Main.myPlayer);
                Projectile.NewProjectileDirect(player.Center + Direction * 16f - new Vector2(0f, 16f), Direction * 15f + RandomDelta2, ModContent.ProjectileType<ScienceDimensionalPhantom_Arrow_Purple>(), 10, 1f, Main.myPlayer);

            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            //玩家试图发动现实扭曲器
            if (ScienceUtils.HasGivenItem(player, ModContent.ItemType<ScienceRealityDistortionDevice>()) &&
                Science.RealityDistortionDeviceHotKey.JustPressed &&
                !player.HasBuff(ModContent.BuffType<ScienceRealityDistortionDeviceCD>()))
            {
                //传递flag
                BeginSpawnRealityDistortion = true;
                player.AddBuff(ModContent.BuffType<ScienceRealityDistortionDeviceCD>(), 60);
            }
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            
            if (BeginSpawnRealityDistortion)
            {
                
                damage = (int) (damage * 0.15);
            }
        }

        public override bool Shoot(Item item, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            //如果玩家手持初始枪，伤害有一半几率为0，并且如果不为0，则写死为1，不享受伤害加成
            //if (item.type == ModContent.ItemType<ScienceBrokenStandardPistol>())
            //{
            //    knockBack = 0f;
            //    damage = Main.rand.NextBool() ? 1 : 0;
            //}
            ShootPos = position;
            ShootRelatePos = position - player.Center;
            ShootVel = new Vector2(speedX, speedY);
            return base.Shoot(item, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
    }
}
