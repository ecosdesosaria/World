using System;
using Server;


namespace Server.Items
{
    public class MarksOfTheShadowbroker : Item
    {
        [Constructable]
        public MarksOfTheShadowbroker() : this(1)
        {
        }


        public override string DefaultDescription{ get{ return "A Marca do Shadowbroker representa sua habilidade como ladrão. Pode ser adquirido por ladrões enquanto eles se aventuram e vasculham os bolsos de suas vítimas. O mestre da guilda dos ladrões pode oferecer muitas bugigangas para aqueles que falam de recompensas com eles."; } }


        [Constructable]
        public MarksOfTheShadowbroker(int amount) : base(0x2ff8)
        {
            Stackable = true;
            Weight = 0.1;
            Hue = 0x455;
            Amount = amount;
            Name = "Mark of the Shadow Broker";
        }


        public MarksOfTheShadowbroker(Serial serial) : base(serial)
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
}