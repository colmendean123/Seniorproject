using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCMaker
{
    class Slot
    {
        private String name;
        private int slots;

        public Slot()
        {

        }
        
        public void setName(String name)
        {
            this.name = name;
        }
        public String getName()
        {
            return this.name;
        }
        public void setSlots(int slots)
        {
            this.slots = slots;
        }
        public int getSlots()
        {
            return slots;
        }
    }
}
