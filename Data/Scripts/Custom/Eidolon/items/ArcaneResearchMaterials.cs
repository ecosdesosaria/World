using System;
using Server.Items;

namespace Server.Items
{
    public class ArcaneResearchMaterials : Item
    {
        [Constructable]
        public ArcaneResearchMaterials() : this(1)
        {
        }

        [Constructable]
        public ArcaneResearchMaterials(int amount) : base(0x1F4C)
        {
            Name = "Arcane research materials";
            Hue = 33;
            Stackable = true;
            Amount = amount;
            Weight = 1;
        }
        public override string DefaultDescription{ get{ return"These documents contain the a multitude os scribbled noted and diagrams related to the summoning and binding of Eidolons. They are very valuable to the Wizard's guild, and multiple of them can be used to initiate your own research in this subject."; } }

        public ArcaneResearchMaterials(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}