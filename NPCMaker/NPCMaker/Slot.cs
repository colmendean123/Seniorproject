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
        private String custom;
        public Slot()
        {

        }
        public void setCustom(String c)
        {
            this.custom = c;
        }
        public String getCustom()
        {
            return this.custom;
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
