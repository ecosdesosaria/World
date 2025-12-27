using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using Server.Gumps; 
using Server.Network; 
using Server.Targeting; 
using Server.ContextMenus;
using Server.Custom.DefenderOfTheRealm.Vow.VowOfTheScourge;

namespace Server.Custom.DefenderOfTheRealm.Scourge
{
    public class ScourgeOfRealm : BaseCreature
    {
        private DateTime m_NextSpeechTime;
        [Constructable]
        public ScourgeOfRealm() : base(AIType.AI_Thief, FightMode.None, 10, 1, 0.4, 1.6)
        {
            InitStats( 125, 55, 65 ); 
			Name = this.Female ? NameList.RandomName( "female" ) : NameList.RandomName( "male" );
			Title = "Flagelo do Reino";
            HairHue = Utility.RandomHairHue(); 
			Body = this.Female? 0x191: 0x190;
            SpeechHue = Utility.RandomTalkHue();
			Hue = Utility.RandomSkinHue(); 
			Utility.AssignRandomHair( this );
			if(( !this.Female ))
            {
                FacialHairItemID = Utility.RandomList( 0, 8254, 8255, 8256, 8257, 8267, 8268, 8269 );
            }
            AddItem( new Boots( Utility.RandomBirdHue() ) );
            AddItem( new Cloak( Utility.RandomBirdHue() ) );
            AddItem( new Artifact_ScourgeOfTheRealmArms());
            AddItem( new Artifact_ScourgeOfTheRealmChestpiece());
            AddItem( new Artifact_ScourgeOfTheRealmGloves());
            AddItem( new Artifact_ScourgeOfTheRealmGorget());
            AddItem( new Artifact_ScourgeOfTheRealmHelmet());
            AddItem( new Artifact_ScourgeOfTheRealmLeggings());
        }

        public override void OnMovement( Mobile m, Point3D oldLocation )
        {
            if ( InRange( m, 6 ) && !InRange( oldLocation, 2 ) )
            {
                if ( m is PlayerMobile && !m.Hidden ) 
                {
                    if ( DateTime.UtcNow >= m_NextSpeechTime )
                    {
                        switch (Utility.Random(11))
                        {
                            case 0: Say("Os Fracos cairão diante de nós!"); break;
                            case 1: Say("Sangue e fogo purificarão esta terra!"); break;
                            case 2: Say("A virtude do Rei não passa de uma frágil mentira!"); break;
                            case 3: Say("Aqueles que não se ajoelharem serão quebrados!"); break;
                            case 4: Say("Endurecei vosso coração, pois somos herdeiros da escuridão infinita!"); break;
                            case 5: Say("Toda a glória pertence a nós!"); break;
                            case 6: Say("Sosaria queimará!"); break;
                            case 7: Say("Erguei vossa lâmina em nome da vingança!"); break;
                            case 8: Say("Faremos pacto com os fantasmas desta terra!"); break;
                            case 9: Say("Removeremos a podridão deste reino!"); break;
                            case 10: Say("Salve o flagelo, ruína da virtude!"); break;
                        }

                        m_NextSpeechTime = DateTime.UtcNow + TimeSpan.FromSeconds(10);
                    }
                }
            }
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            list.Add(new GiveVowEntry(from, this));
        }

        private class GiveVowEntry : ContextMenuEntry
        {
            private Mobile m_From;
            private ScourgeOfRealm m_Npc;
            private static TimeSpan Delay = TimeSpan.FromHours(6);
			private static Dictionary<PlayerMobile,DateTime> LastUsers = new Dictionary<PlayerMobile,DateTime>();

            public GiveVowEntry(Mobile from, ScourgeOfRealm npc) : base(6146)
            {
                m_From = from;
                m_Npc = npc;
            }

            public override void OnClick()
            {
                if( !( m_From is PlayerMobile ) )
					return;
				
				if (m_From == null || m_From.Deleted || m_Npc == null || m_Npc.Deleted)
                    return;

                PlayerMobile mobile = (PlayerMobile) m_From;
                DateTime lastUse;

                if (!mobile.CheckAlive())
                {
                    mobile.SendMessage("Você deve estar vivo para receber um Voto do Flagelo.");
                    return;
                }
                else if (mobile.Backpack == null)
                {
                    mobile.SendMessage("Você não tem uma mochila para receber o Voto do Flagelo.");
                    return;
                }
                else if (LastUsers.TryGetValue(mobile, out lastUse))
                {
                    TimeSpan cooldown = Delay - (DateTime.UtcNow - lastUse);
                    if (cooldown > TimeSpan.Zero)
                    {
                        m_Npc.Say(String.Format("Eu terei outro Voto para você em {0} hora{1} e {2} minuto{3}.",
                        cooldown.Hours, cooldown.Hours == 1 ? "" : "s",
                        cooldown.Minutes, cooldown.Minutes == 1 ? "" : "s"));
                        return;
                    }
                }
                else if (mobile.Karma > 0)
                {
                    m_Npc.Say("Tu ainda não provaste teu valor! Não lidarei com aqueles que se envolvem em virtude insignificante!");
                    return;
                }
                if (CanGetVow(mobile))
                    {
                        LastUsers[mobile] = DateTime.UtcNow;
                        VowOfTheScourge vow = new VowOfTheScourge(mobile);
                        m_From.Backpack.DropItem(vow);

                        if (vow.Parent == mobile.Backpack)
                        {
                            mobile.SendGump(new SpeechGump(mobile, "Flagelo do Reino", SpeechFunctions.SpeechText(m_Npc, mobile, "Flagelo do Reino")));
                            mobile.SendMessage("Você recebe um Voto do Flagelo.");
                        }
                        else
                        {
                            vow.Delete();
                            mobile.SendMessage("Você não tem espaço suficiente no inventário para receber um Voto do Flagelo.");
                        }
                    }
            }
            private bool CanGetVow(PlayerMobile asker)
			{
				if(!LastUsers.ContainsKey(asker))
				{
					LastUsers.Add(asker,DateTime.UtcNow);
					return true;
				}
				else
				{
					if(DateTime.UtcNow-LastUsers[asker] < Delay)
					{
						return false;
					}
					else
					{
						LastUsers[asker]=DateTime.UtcNow;
						return true;
					}
				}
			}
        }
        
        public override bool HandlesOnSpeech( Mobile from ) 
		{ 
			return true; 
		} 

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (from == null || !(from is PlayerMobile))
                return;
          
            if( e.Mobile.InRange( this, 4 ))
			{
			    if ( e.Speech.IndexOf("reward") >= 0 )
                {
                    if (from.Karma < 0)
                    {
                        if (from.Karma < 0)
                        {
                            from.SendGump(new Server.Custom.DefenderOfTheRealm.RewardGump(from, 2, 0));
                            Say("Estas são as recompensas que posso oferecer-te.");
                        }
                        else
                        {
                            Say("Não oferecerei meus serviços a escravos da Virtude!");
                        }
                    }
                }
			    else 
			    { 
			        base.OnSpeech( e ); 
			    }
			}
        }

        public ScourgeOfRealm(Serial serial) : base(serial) { }

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
}