using System;
using Server.Items;

namespace Server.Items
{
    public class ArcaneDust : Item
    {
        [Constructable]
        public ArcaneDust() : this(1)
        {
        }

        [Constructable]
        public ArcaneDust(int amount) : base(12265)
        {
            Name = "arcane dust";
            Hue = 33;
            Stackable = true;
            Amount = amount;
            Weight = 0.1;
        }

        public ArcaneDust(Serial serial) : base(serial)
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
