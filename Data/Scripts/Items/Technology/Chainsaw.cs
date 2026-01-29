using System;
using Server;
using Server.Network;
using System.Text;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
	public class Chainsaw : Item
	{
		private int m_Charges;

		[CommandProperty( AccessLevel.GameMaster )]
		public int Charges
		{
			get{ return m_Charges; }
			set{ m_Charges = value; InvalidateProperties(); }
		}

		[Constructable]
		public Chainsaw() : base( 0x5408 )
		{
			Name = "chainsaw";
			Technology = true;
			Weight = 5;
			ItemID = Utility.RandomMinMax( 0x5408, 0x5409 );
			Charges = Utility.RandomMinMax( 50, 100 );
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			if ( m_Charges > 1 ){ list.Add( 1070722, m_Charges.ToString() + " Uses Left"); }
			else { list.Add( 1070722, "1 Use Left"); }
            list.Add( 1049644, "Cut logs into boards");
        }

		public override void OnDoubleClick( Mobile from )
		{
			if ( !Movable )
				return;

			if ( !IsChildOf( from.Backpack ) ) 
			{
				from.SendMessage( "This must be in your backpack to use." );
				return;
			}
			else
			{
				from.SendMessage( "Select the logs you want to cut into boards." );
				from.Target = new InternalTarget( this );
			}
		}

		private class InternalTarget : Target
		{
			private Chainsaw m_Tool;

			public InternalTarget( Chainsaw tool ) :  base ( 2, false, TargetFlags.None )
			{
				m_Tool = tool;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is BaseLog )
				{
					BaseLog m_Log = (BaseLog)targeted;

					if ( m_Log.Deleted )
						return;

					if ( !from.InRange( m_Log.GetWorldLocation(), 2 ) )
					{
						from.SendMessage( "The logs are too far away." );
						return;
					}

					double difficulty;

					switch ( m_Log.Resource )
					{
						default: difficulty = 40.0; break;
						case CraftResource.AshTree: difficulty = 55.0; break;
						case CraftResource.CherryTree: difficulty = 60.0; break;
						case CraftResource.EbonyTree: difficulty = 65.0; break;
						case CraftResource.GoldenOakTree: difficulty = 70.0; break;
						case CraftResource.HickoryTree: difficulty = 75.0; break;
						case CraftResource.MahoganyTree: difficulty = 80.0; break;
						case CraftResource.DriftwoodTree: difficulty = 80.0; break;
						case CraftResource.OakTree: difficulty = 85.0; break;
						case CraftResource.PineTree: difficulty = 90.0; break;
						case CraftResource.GhostTree: difficulty = 90.0; break;
						case CraftResource.RosewoodTree: difficulty = 95.0; break;
						case CraftResource.WalnutTree: difficulty = 99.0; break;
						case CraftResource.PetrifiedTree: difficulty = 99.9; break;
						case CraftResource.ElvenTree: difficulty = 100.1; break;
					}

					double minSkill = difficulty - 25.0;
					double maxSkill = difficulty + 25.0;
					
					if ( difficulty > 50.0 && difficulty > from.Skills[SkillName.Lumberjacking].Value )
					{
						from.SendMessage( "Você não tem ideia de como melhor cortar este tipo de madeira!" );
						return;
					}

					if ( from.CheckTargetSkill( SkillName.Lumberjacking, targeted, minSkill, maxSkill ) )
					{
						if ( m_Log.Amount <= 0 )
						{
							from.SendMessage( "Não há madeira suficiente nesta pilha para fazer uma tábua." );
						}
						else
						{
							int amount = m_Log.Amount;
							BaseWoodBoard wood = m_Log.GetLog();
							m_Log.Delete();
							wood.Amount = amount;
							from.AddToBackpack( wood );
							from.PlaySound( 0x21C );
							from.SendMessage( "Você corta as toras e coloca algumas tábuas em sua mochila." );
							m_Tool.ConsumeCharge( from );
						}
					}
					else
					{
						int amount = m_Log.Amount;
						int lose = Utility.RandomMinMax( 1, amount );

						if ( amount < 2 || lose == amount )
						{
							m_Log.Delete();
							from.SendMessage( "Você tenta cortar as toras mas estraga toda a madeira." );
							m_Tool.ConsumeCharge( from );
						}
						else
						{
							m_Log.Amount = amount - lose;
							from.SendMessage( "Você tenta cortar as toras mas estraga parte da madeira." );
							m_Tool.ConsumeCharge( from );
						}

						from.PlaySound( 0x21C );
					}
				}
				else
				{
					from.SendMessage( "Você só pode usar isso em toras." );
				}
			}
		}

		public void ConsumeCharge( Mobile from )
		{
			--Charges;

			if ( Charges == 0 )
			{
				from.SendMessage( "A motosserra foi usada demais e quebrou." );
				Item MyJunk = new SciFiJunk();
			  	MyJunk.Hue = this.Hue;
			  	MyJunk.ItemID = this.ItemID;
				MyJunk.Name = Server.Items.SciFiJunk.RandomCondition() + " chainsaw";
				MyJunk.Weight = this.Weight;
			  	from.AddToBackpack ( MyJunk );
				this.Delete();
			}
		}

		public Chainsaw( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
			writer.Write( (int) m_Charges );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_Charges = (int)reader.ReadInt();
		}
	}
}