using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeratDemirel_215040088_Final
{
    public class NPC
    {
        public string Name { get; private set; }
        public int Damage { get; private set; }
        public bool CanAttack { get; private set; }

        public NPC(string name, int damage)
        {
            Name = name;
            Damage = damage;
            CanAttack = true;
        }
    }
}
