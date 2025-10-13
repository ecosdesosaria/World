using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x27A2, 0x27ED )]
    public class LevelNoDachiMasamune : BaseLevelSword
	{
		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.WhirlwindAttack; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ArmorIgnore; } }
		public override WeaponAbility ThirdAbility{ get{ return WeaponAbility.DeathBlow; } }
		public override WeaponAbility FourthAbility{ get{ return WeaponAbility.DefenseMastery; } }
		public override WeaponAbility FifthAbility{ get{ return WeaponAbility.MagicProtection2; } }

		public override int AosStrengthReq{ get{ return 40; } }
		public override int AosMinDamage{ get{ return (int)(21 * GetDamageScaling()); } }
		public override int AosMaxDamage{ get{ return (int)(26 * GetDamageScaling()); } }
		public override int AosSpeed{ get{ return 35; } }
		public override float MlSpeed{ get{ return 3.50f; } }

		public override int OldStrengthReq{ get{ return 40; } }
		public override int OldMinDamage{ get{ return 16; } }
		public override int OldMaxDamage{ get{ return 18; } }
		public override int OldSpeed{ get{ return 35; } }

		public override int DefHitSound{ get{ return 0x23B; } }
		public override int DefMissSound{ get{ return 0x23A; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 90; } }

		[Constructable]
		public LevelNoDachiMasamune() : base( 0x27A2 )
		{
            Name = "Masamune";
            Hue = 0x373;
			Weight = 10.0;
			Layer = Layer.TwoHanded;
            Slayer = SlayerName.Repond;
            Attributes.WeaponDamage = 100;
		}

		public LevelNoDachiMasamune( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}