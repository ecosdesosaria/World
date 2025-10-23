using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.ContextMenus;
using Server.Mobiles;

namespace Server.Items
{
	[Flipable(0x1C10, 0x1CC6)]
	public class AlchemyPouch : LargeSack
	{
		public override CraftResource DefaultResource { get { return CraftResource.RegularLeather; } }

		[Constructable]
		public AlchemyPouch() : base()
		{
			Weight = 1.0;
			MaxItems = 50;
			Name = "alchemy rucksack";
			Hue = 0x89F;
		}

		public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			base.GetContextMenuEntries(from, list);

			if (from is PlayerMobile)
				list.Add(new OrganizeEntry(from, this));
		}

		private class OrganizeEntry : ContextMenuEntry
		{
			private readonly Mobile m_From;
			private readonly AlchemyPouch m_Pouch;

			public OrganizeEntry(Mobile from, AlchemyPouch pouch) : base(6172, 1)
			{
				m_From = from;
				m_Pouch = pouch;
				Enabled = true;
			}

			public override void OnClick()
			{
				m_Pouch.OrganizeItems(m_From);
			}
		}

		public void OrganizeItems(Mobile from)
		{
			if (from == null || from.Backpack == null)
				return;

			ArrayList toMove = new ArrayList();
			CollectAlchemyItems(from.Backpack, toMove);

			int moved = 0;

			for (int i = 0; i < toMove.Count; i++)
			{
				Item item = toMove[i] as Item;

				if (item == null || item.Deleted)
					continue;

				if (TryDropItem(from, item))
					moved++;
			}

			if (moved > 0)
				from.SendMessage("You organize {0} alchemical items into your rucksack.", moved);
			else
				from.SendMessage("You have no alchemical items to organize.");
		}

		private void CollectAlchemyItems(Container container, ArrayList list)
		{
			if (container == null)
				return;

			foreach (Item item in container.Items)
			{
				if (item == null || item.Deleted)
					continue;

				if (item == this)
					continue;

				if (item.Catalog == Catalogs.Reagent ||
					item is GodBrewing ||
					item is Bottle ||
					item is Jar ||
					item is MortarPestle ||
					item is DruidCauldron ||
					item is WitchCauldron)
				{
					list.Add(item);
				}

				else if (item is Container)
				{
					CollectAlchemyItems((Container)item, list);
				}
			}
		}

		private bool TryDropItem(Mobile from, Item item)
		{
		    if (item == null || item.Deleted)
		        return false;

		    if (!this.CheckHold(from, item, false, true, 0, 0))
		        return false;

		    if (item.Stackable)
		    {
		        foreach (Item existing in this.Items)
		        {
		            if (existing == null || existing.Deleted)
		                continue;
					//unsafe to check only for id
		            if (existing.GetType() == item.GetType() &&
		                existing.Hue == item.Hue &&
		                existing.Name == item.Name)
		            {
		                int toAdd = item.Amount;

		                existing.Amount += toAdd;
		                item.Delete();

		                return true;
		            }
		        }
		    }
		    this.DropItem(item);
		    return true;
		}


		public override bool OnDragDropInto(Mobile from, Item dropped, Point3D p)
		{
			if (dropped is Container && !(dropped is AlchemyPouch))
			{
				from.SendMessage("You can only use another alchemy rucksack within this sack.");
				return false;
			}
			else if (dropped.Catalog == Catalogs.Reagent ||
					 dropped is GodBrewing ||
					 dropped is Bottle ||
					 dropped is Jar ||
					 dropped is MortarPestle ||
					 dropped is DruidCauldron ||
					 dropped is WitchCauldron ||
					 dropped is AlchemyPouch)
			{
				return base.OnDragDropInto(from, dropped, p);
			}

			from.SendMessage("This rucksack is for small alchemical crafting items.");
			return false;
		}

		public override bool OnDragDrop(Mobile from, Item dropped)
		{
			if (dropped is Container && !(dropped is AlchemyPouch))
			{
				from.SendMessage("You can only use another alchemy rucksack within this sack.");
				return false;
			}
			else if (dropped.Catalog == Catalogs.Reagent ||
					 dropped is GodBrewing ||
					 dropped is Bottle ||
					 dropped is Jar ||
					 dropped is MortarPestle ||
					 dropped is DruidCauldron ||
					 dropped is WitchCauldron ||
					 dropped is AlchemyPouch)
			{
				return base.OnDragDrop(from, dropped);
			}

			from.SendMessage("This rucksack is for small alchemical crafting items.");
			return false;
		}

		public AlchemyPouch(Serial serial) : base(serial) { }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			Weight = 1.0;
			MaxItems = 50;
			Name = "alchemy rucksack";
		}

		public override int GetTotal(TotalType type)
		{
			if (type != TotalType.Weight)
				return base.GetTotal(type);
			else
				return (int)(TotalItemWeights() * 0.05);
		}

		public override void UpdateTotal(Item sender, TotalType type, int delta)
		{
			if (type != TotalType.Weight)
				base.UpdateTotal(sender, type, delta);
			else
				base.UpdateTotal(sender, type, (int)(delta * 0.05));
		}

		private double TotalItemWeights()
		{
			double weight = 0.0;
			foreach (Item item in Items)
				weight += (item.Weight * item.Amount);
			return weight;
		}
	}
}
