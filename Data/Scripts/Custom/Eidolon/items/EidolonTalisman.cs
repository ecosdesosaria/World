using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.ContextMenus;

namespace Server.Items
{
    public class EidolonTalisman : BaseTrinket
    {
        private string m_Owner;
        private string m_ActiveForm;
        private ArrayList m_UnlockedForms = new ArrayList();

        [CommandProperty(AccessLevel.GameMaster)]
        public string Owner { get { return m_Owner; } set { m_Owner = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public string ActiveForm { get { return m_ActiveForm; } set { m_ActiveForm = value; InvalidateProperties(); } }

        [Constructable]
        public EidolonTalisman(string ownerName) : base(0x2F5B)
        {
            Name = "Eidolon Talisman";
            Hue = 33;
            Weight = 1.0;
            m_Owner = ownerName;

            m_UnlockedForms.Add("Wisp");
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (!String.IsNullOrEmpty(m_Owner))
                list.Add("Belongs to {0}", m_Owner);

            if (!String.IsNullOrEmpty(m_ActiveForm))
                list.Add("Active Form: {0}", m_ActiveForm);
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile && Parent == from)
                list.Add(new ExamineEntry(from, this));
        }

        public override bool OnEquip(Mobile from)
        {
            if(from.Skills[SkillName.Magery].Base < 50.0)
            {
                from.SendMessage("You are not skilled enough in Magery to equip this item (requires 50 base Magery).");
                return false;
            }
            if(from.Skills[SkillName.Spiritualism].Base < 50.0)
            {
                from.SendMessage("You are not skilled enough in Spiritualism to equip this item (requires 50 base Spiritualism).");
                return false;
            }
            if (from.Skills[SkillName.Necromancy].Base > 0.0)
            {
                from.SendMessage("Your knowledge of Necromancy leaves you unattuned to the higher planes (requires a base skill of Necromancy equal to zero).");
                return false;
            }
            if (m_Owner != null && m_Owner.Length > 0 && from.Name != m_Owner)
            {
                from.SendMessage("This Eidolon is not bound to you.");
                return false;
            }
            return base.OnEquip(from);
        }

        private class ExamineEntry : ContextMenuEntry
        {
            private Mobile m_From;
            private EidolonTalisman m_Talisman;

            public ExamineEntry(Mobile from, EidolonTalisman talisman)
                : base(6146, 3)
            {
                m_From = from;
                m_Talisman = talisman;
            }

            public override void OnClick()
            {
                if (m_From == null || m_Talisman == null)
                    return;

                m_From.CloseGump(typeof(EidolonFormGump));
                m_From.SendGump(new EidolonFormGump(m_From, m_Talisman));
            }
        }

        public EidolonTalisman(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);

            writer.Write(m_Owner);
            writer.Write(m_ActiveForm);
            writer.Write(m_UnlockedForms.Count);
            for (int i = 0; i < m_UnlockedForms.Count; i++)
                writer.Write((string)m_UnlockedForms[i]);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_Owner = reader.ReadString();
            m_ActiveForm = reader.ReadString();

            int count = reader.ReadInt();
            m_UnlockedForms = new ArrayList();
            for (int i = 0; i < count; i++)
                m_UnlockedForms.Add(reader.ReadString());
        }

        public bool IsUnlocked(string name)
        {
            return m_UnlockedForms.Contains(name);
        }

        public void Unlock(string name)
        {
            if (!m_UnlockedForms.Contains(name))
                m_UnlockedForms.Add(name);
        }
    }

    public class EidolonFormGump : Gump
    {
        private Mobile m_From;
        private EidolonTalisman m_Talisman;

        private class FormInfo
        {
            public string Name;
            public int Cost;
            public double ReqSkill;
            public int Graphic;
            public int Hue;
            public string Tooltip;
            public FormInfo(string name, int cost, double req, int graphic, int hue, string tip)
            { Name = name; Cost = cost; ReqSkill = req; Graphic = graphic; Hue = hue; Tooltip = tip; }
        }

        private static FormInfo[] m_Forms = new FormInfo[]
        {
            new FormInfo("Wisp", 0, 0, 0x3a, 0x4001, "Grants the caster increased mana regeneration."),
            new FormInfo("Drake", 150, 60, 0x24e, 0, "Breath weapon."),
            new FormInfo("Sprite", 350, 70, 0x16b, 0, "Reveals hidden enemies and treasures."),
            new FormInfo("Sphinx", 500, 80, 0x328, 0, "Turns enemies to stone."),
            new FormInfo("Phoenix", 850, 90, 0xF3, 0, "Grants the caster increased health regeneration."),
            new FormInfo("Tyranasaur", 1100, 100, 0x299, 0x4001, "Bleed attack."),
            new FormInfo("Cerberus", 1450, 110, 0x8D, 0, "Breath weapon."),
            new FormInfo("Angel", 1450, 110, 0x159, 0, "Heals the caster."),
            new FormInfo("Marilith", 1900, 120, 0x3f, 0, "Reflects magic."),
            new FormInfo("Archangel", 1900, 120, 0x15a, 0, "Cleanses curses and heals the caster."),
            new FormInfo("Shadow Wyrm", 2500, 125, 0x6A, 0x4001, "Breath weapon; powerful spellcaster.")
        };

        
        public EidolonFormGump(Mobile from, EidolonTalisman talisman) : base(60, 60)
        {
            m_From = from;
            m_Talisman = talisman;

            Closable = true;
            Dragable = true;
            AddPage(0);

            AddBackground(0, 0, 540, 440, 9270);
            AddLabel(200, 20, 1152, "Eidolon Forms");

            for (int i = 0; i < m_Forms.Length; i++)
            {
                FormInfo info = m_Forms[i];
                int y = 60 + (i * 32);

                bool unlocked = m_Talisman.IsUnlocked(info.Name);
                bool active = (m_Talisman.ActiveForm == info.Name);

                AddImage(40, y - 4, 9750 + info.Graphic, info.Hue);
                AddLabel(100, y, 0, info.Name);
                AddLabel(220, y, 0, string.Format("Req. Skill: {0}", info.ReqSkill));

                AddHtml(100, y + 16, 250, 20, String.Format("<BASEFONT COLOR=#AAAAAA>{0}</BASEFONT>", info.Tooltip), false, false);

                int buttonID = 0;
                string buttonText = "";
                
                if (!unlocked && info.Cost > 0)
                {
                    buttonID = 1000 + i;
                    buttonText = "Unlock";
                }
                else if (unlocked && !active)
                {
                    buttonID = 2000 + i;
                    buttonText = "Set Active";
                }
                else if (active)
                {
                    buttonText = "Active";
                }
                
                if (buttonID > 0)
                    AddButton(370, y, 4011, 4013, buttonID, GumpButtonType.Reply, 0);
                AddLabel(400, y, 0, buttonText);
            }
        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            int id = info.ButtonID;
            if (id == 0)
                return;

            if (id >= 1000 && id < 2000)
            {
                int idx = id - 1000;
                if (idx < 0 || idx >= m_Forms.Length)
                    return;

                FormInfo f = m_Forms[idx];
                TryUnlock(f);
            }
            else if (id >= 2000 && id < 3000)
            {
                int idx = id - 2000;
                if (idx < 0 || idx >= m_Forms.Length)
                    return;

                FormInfo f = m_Forms[idx];
                m_Talisman.ActiveForm = f.Name;
                m_From.SendMessage("You attune your Eidolon talisman to the form of the {0}.", f.Name);
                m_From.CloseGump(typeof(EidolonFormGump));
                m_From.SendGump(new EidolonFormGump(m_From, m_Talisman));
            }
        }

        private void TryUnlock(FormInfo f)
        {
            Container pack = m_From.Backpack;
            BankBox bank = m_From.BankBox;

            int total = 0;
            if (pack != null) total += pack.GetAmount(typeof(ArcaneDust));
            if (bank != null) total += bank.GetAmount(typeof(ArcaneDust));

            if (total < f.Cost)
            {
                m_From.SendMessage("You need {0} units of Arcane Dust to unlock the {1} form.", f.Cost, f.Name);
                return;
            }

            int remaining = f.Cost;
            if (pack != null) remaining -= ConsumeFrom(pack, typeof(ArcaneDust), remaining);
            if (remaining > 0 && bank != null) ConsumeFrom(bank, typeof(ArcaneDust), remaining);

            m_Talisman.Unlock(f.Name);
            m_From.SendMessage("You have unlocked the {0} form!", f.Name);
            m_From.CloseGump(typeof(EidolonFormGump));
            m_From.SendGump(new EidolonFormGump(m_From, m_Talisman));
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
