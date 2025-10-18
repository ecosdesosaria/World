using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Items;

namespace Server.Gumps
{
    public class EidolonResearchGump : Gump
    {
        private Mobile m_From;
        private BaseCreature m_Npc;

        public EidolonResearchGump(Mobile from, BaseCreature npc) : base(50, 50)
        {
            m_From = from;
            m_Npc = npc;

            AddBackground(0, 0, 400, 260, 9270);
            AddLabel(120, 20, 1152, "Eidolon Research");

            AddHtml(30, 50, 340, 120,
                "If you wish to get started on your own Eidolon research, I'll need you to give me a sizeable quantity of arcane research materials in order to craft you a totem that will allow you to contact the higher planes and establish a relationship with an Eidolon.<br><br>" +
                "10 of them should do. I will also need 400 units of Arcane Dust, and about 10,000 gold pieces for spell components and to cover my costs.",
                true, true);

            AddButton(100, 190, 247, 248, 1, GumpButtonType.Reply, 0);
          
            AddButton(220, 190, 241, 242, 0, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 1)
            {
                TryConsumeMaterials();
            }
        }

        private void TryConsumeMaterials()
        {
            Container pack = m_From.Backpack;
            BankBox bank = m_From.BankBox;

            if (pack == null || bank == null)
                return;

            int materials = GetAmount<ArcaneResearchMaterials>(pack) + GetAmount<ArcaneResearchMaterials>(bank);
            int dust = GetAmount<ArcaneDust>(pack) + GetAmount<ArcaneDust>(bank);
            int gold = (int)(pack.GetAmount(typeof(Gold)) + bank.GetAmount(typeof(Gold)));

            if (materials >= 10 && dust >= 400 && gold >= 10000)
            {
                Consume<ArcaneResearchMaterials>(pack, bank, 10);
                Consume<ArcaneDust>(pack, bank, 400);
                Consume<Gold>(pack, bank, 10000);

                m_From.AddToBackpack(new EidolonTalisman(m_From.Name));
                if (m_Npc != null)
                    m_Npc.Say(true, "I shall grant you a token to call upon your Eidolon, and the spirit will help you understand the arrangement further. Good luck on your research, " + m_From.Name + "!");
 
            }
            else
            {
                m_Npc.Say(true, "You lack the materials, the dust, or the gold required for this research, " + m_From.Name + ".");
            }
        }

        private int GetAmount<T>(Container c)
        {
            int total = 0;
            if (c == null) return 0;

            foreach (Item i in c.Items)
            {
                if (i is T)
                    total += i.Amount;
            }
            return total;
        }

        private void Consume<T>(Container pack, Container bank, int amount)
        {
            amount -= ConsumeFrom(pack, typeof(T), amount);
            if (amount > 0)
                ConsumeFrom(bank, typeof(T), amount);
        }

        private int ConsumeFrom(Container c, Type t, int amount)
        {
            if (c == null) return 0;
            int removed = 0;

            foreach (Item i in c.Items)
            {
                if (i.GetType() == t)
                {
                    int take = Math.Min(i.Amount, amount - removed);
                    i.Consume(take);
                    removed += take;
                    if (removed >= amount)
                        break;
                }
            }
            return removed;
        }
    }
}
