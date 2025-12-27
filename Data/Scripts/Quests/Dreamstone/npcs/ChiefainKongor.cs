using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections;
using System.Collections.Generic;
using Server.Misc;

namespace Server.Mobiles
{
	public class ChieftainKongor : BaseCreature
	{

		[Constructable]
		public ChieftainKongor()  : base(AIType.AI_Thief, FightMode.None, 10, 1, 0.4, 1.6)
		{
            InitStats( 125, 55, 65 ); 
			Name = "Kongor, o Chefe";
			Body = 0x190;
			Hue = 1420;
			Blessed = true;
            SpeechHue = 1420;
            CantWalk = false;
            Utility.AssignRandomHair( this );
            FacialHairItemID = Utility.RandomList( 0, 8254, 8255, 8256, 8257, 8267, 8268, 8269 );
            AddItem( new PlateArms() );
            AddItem( new PlateLegs() );
            AddItem( new Cloak( Utility.RandomBirdHue() ) );
            AddItem( new PlateChest() );
            AddItem( new PlateGloves());
            AddItem( new Boots( Utility.RandomBirdHue() ) );
            AddItem( new PlateHelm());
		}


		public override void OnDoubleClick(Mobile from)
		{
			if (!from.InRange(this.Location, 4))
			{
				from.SendMessage("Você está muito longe para falar com Kongor.");
				return;
			}

            if (Server.Misc.PlayerSettings.GetKeys(from, "Kongor"))
	        {
	        	this.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Eu fiz tudo que pude por você, por enquanto. Volte depois de saciar a sede do martelo.", from.NetState);
	        	return;
	        }

			from.CloseGump(typeof(KongorDialogueGump));
			from.SendGump(new KongorDialogueGump(from, this));
		}

		public ChieftainKongor(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class KongorDialogueGump : Gump
	{
		private Mobile m_Player;
		private ChieftainKongor m_Kongor;

		public KongorDialogueGump(Mobile from, ChieftainKongor kongor) : base(100, 100)
		{
			m_Player = from;
			m_Kongor = kongor;

			double magicResist = from.Skills[SkillName.MagicResist].Base;
            int rawStr = from.RawStr;
			int karma = from.Karma;
			bool hasDreamstone = Server.Misc.PlayerSettings.GetKeys(from, "Dreamstone");

			string name = from.Name;
			string text = GetDialogueText(name, magicResist, karma, rawStr, hasDreamstone);

			AddBackground(0, 0, 420, 280, 9270);
			AddHtml(20, 20, 380, 200, text, true, true);


			if (ShouldOfferQuest(karma, magicResist, rawStr))
			{
				AddButton(160, 235, 4005, 4007, 1, GumpButtonType.Reply, 0);
				AddHtml(195, 237, 100, 20, "Continue", false, false);
			}
			else
			{
				AddButton(160, 235, 4005, 4007, 0, GumpButtonType.Reply, 0);
				AddHtml(195, 237, 100, 20, "Fechar", false, false);
			}
		}

		private string GetDialogueText(string name, double magicResist, int karma, double rawStr, bool hasDreamstone)
		{
            string intro = "Hai! " + name + ". Então você encontrou nosso lar.<br><br>" +
                        "Muito temos suportado longe dos confortos do que você chamaria de civilizado. Eh, não temos uso para isso aqui.<br><br>"+
                        "Sou o chefe deste povo e responsável por mantê-los seguros. Também herdei os segredos de nossa tenacidade. Você gostaria de aprendê-los, eh?";

            if (!hasDreamstone)
            {
                intro += "Nah, não adiantaria. Vocês civilizados são fracos.<br><br>Moles.<br><br>"+
                "Você discorda? Prove, então. Traga-me uma pedra do sonho e forjarei para você uma arma digna de nossos guerreiros.<br><br>";
            }
            else if (hasDreamstone)
                intro += "Você a tem... Como você conquistou aquela fera? Não importa. Não acho que você possa dominar uma arma como a nossa.<br><br>";

            if (karma < 0)
            {
                return intro + "Que garantias tenho de que você não usará esta arma contra meu povo, " + name + ". Sinto o lamento de muitas viúvas em seu rastro.<br><br>" +
                    "Fora! Não lhe oferecerei nenhum de meus serviços!<br><br>";
            }

            if (karma >= 0 && (magicResist < 111.0 || rawStr < 125))
            {
                return intro + "Você parece confiável, mas não acho que mereça uma arma como a nossa, " + name + "." +
                    "Fortaleça-se — Aprenda a resistir à sedução da magia maligna e torne-se forte matando muitas bestas vis, e considerarei você digno das armas do meu povo.<br><br>" +
                    "Não lhe oferecerei meus serviços hoje, receio.<br><br>";
            }

            if (magicResist >= 111.0 && rawStr >= 125 && karma >= 15000)
            {
                return intro + "Você fará a arma se orgulhar, " + name + ". Muito bem. Concederei a você este presente.<br><br>";
            }

            if (karma > 0 && (magicResist >= 111.0 || rawStr >= 125))
            {
                return intro + "Conheço seu povo, " + name + ".<br><br>" +
                    "Conheço sua linhagem e não confio neles. Prove para mim — prove-se digno.<br><br>" +
                    "Faça isso e lhe darei um presente digno de nossos ancestrais!<br><br>";
            }
            return intro;
        }

		private bool ShouldOfferQuest(int karma, double magicResist, double rawStr)
		{
			return (karma >= 15000 && magicResist >= 111.0 && rawStr >= 125);
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			if (info.ButtonID == 1)
			{
				m_Player.CloseGump(typeof(KongorConfirmationGump));
				m_Player.SendGump(new KongorConfirmationGump(m_Player, m_Kongor));
			}
		}
	}

	public class KongorConfirmationGump : Gump
	{
		private Mobile m_Player;
		private Mobile m_Kongor;

		public KongorConfirmationGump(Mobile from, Mobile kongor) : base(100, 100)
		{
			m_Player = from;
			m_Kongor = kongor;

			AddBackground(0, 0, 350, 160, 9270);
			AddHtml(20, 20, 310, 60, "Você deseja entregar sua Pedra do Sonho a Kongor?<br>Você não encontrará outra.", true, true);
			AddButton(60, 110, 4005, 4007, 1, GumpButtonType.Reply, 0);
			AddHtml(95, 112, 60, 20, "Sim", false, false);
			AddButton(190, 110, 4005, 4007, 0, GumpButtonType.Reply, 0);
			AddHtml(225, 112, 60, 20, "Não", false, false);
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			if (m_Player == null || m_Player.Deleted)
				return;

			if (info.ButtonID == 1)
			{
				bool hasUsableDreamstone = Server.Misc.PlayerSettings.GetKeys(m_Player, "Dreamstone") 
	                    && !Server.Misc.PlayerSettings.GetKeys(m_Player, "DreamstoneUsed");

                if(!hasUsableDreamstone)
                {
                	m_Player.SendMessage("Você não tem uma Pedra do Sonho para entregar.");
                	return;
                }

				Item reward = new LevelWarHammerKongor(m_Player.Name);
                m_Player.AddToBackpack(reward);

                m_Player.SendMessage("Kongor pega a Pedra do Sonho e acena com entusiasmo.");
                m_Player.SendMessage("Você recebe a lendária Fúria Imortal de Kongor!");
                if ( PlayerSettings.GetKeys( m_Player, "Kongor" ) )
                {
                    m_Player.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Kongor já lhe presenteou com uma arma!", m_Player.NetState);
                }
                else
                {
                    PlayerSettings.SetKeys( m_Player, "Kongor", true );
                    PlayerSettings.SetKeys( m_Player,"DreamstoneUsed", true);
                    m_Player.SendSound( 0x3D );
                    m_Player.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Você adquiriu a Fúria Imortal de Kongor.", m_Player.NetState);
                }
			}
			else
			{
				m_Player.SendMessage("Você decide manter sua Pedra do Sonho por enquanto.");
			}
		}
	}
}