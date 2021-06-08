using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCMaker
{
    class Structure
    {
        private String Name;
        private String png;
        private String dialogue;
        private String moves;

        public void setName(String name)
        {
            this.Name = name;
        }
        public String getName()
        {
            return this.Name;
        }
        public void setImage(String png)
        {
            this.png = png;
        }
        public String getImage()
        {
            return this.png;
        }
        public void setDialogue(String dialogue)
        {
            this.dialogue = dialogue;
        }
        public String getDialogue()
        {
            return this.dialogue;
        }
        public void setMoves(String moves)
        {
            this.moves = moves;
        }
        public String getMoves()
        {
            return this.moves;
        }
    }
}
