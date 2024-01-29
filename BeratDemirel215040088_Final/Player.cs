using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeratDemirel_215040088_Final
{
    public class Player(string name, int? health = null, Inventory? inventory = null)
    {
        public string Name { get; private set; } = name;
        public int Health { get; set; } = health ?? 100;
        public Inventory Inventory { get; private set; } = inventory ?? new Inventory();

        public void TakeItem(Item item)
        {
            Inventory.AddItem(item);
        }
    }
}
