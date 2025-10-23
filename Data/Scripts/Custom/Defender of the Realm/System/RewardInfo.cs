using System;
using Server;
using Server.Items;

namespace Server.Custom.DefenderOfTheRealm
{
    public class RewardInfo
    {
        public Type ItemType;
        public int Cost;
        public int ItemID;
        public string Name;
        public bool Hueable;
        public int Hue;

        public RewardInfo(Type type, int cost, int itemID, string name, bool hueable, int hue)
        {
            ItemType = type;
            Cost = cost;
            ItemID = itemID;
            Name = name;
            Hueable = hueable;
            Hue = hue;
        }

        public Item CreateItem(int type)
        {
            Item item = (Item)Activator.CreateInstance(ItemType);

            if (Hueable)
            {
                if(type == 1)
                {
                    item.Hue = 53;
                }
                else if (type == 2)
                {
                    item.Hue = 37;
                }
                else if (type == 3)
                {
                    item.Hue = 1109;
                }
            }
            return item;
        }
    }
}
