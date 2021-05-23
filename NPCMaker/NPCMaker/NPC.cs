using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
namespace NPCMaker
{
    //[Serializable()]
    class NPC 
    {
        
        private String Name;
        private int Attack;
        private int Defense;
        private int HP;
        private String png;
        private int Faction;
        private String dialogue;
        private String moves;
        private String isPlayable;
        private int speed;
        public NPC()
        {

        }
        public void setName(String name)
        {
            this.Name = name;
        }
        public String getName()
        {
            return this.Name;
        }
        public void setAttack(int attack)
        {
            this.Attack = attack;
        }
        public int getAttack()
        {
            return this.Attack;
        }
        public void setDefense(int defense)
        {
            this.Defense = defense;
        }
        public int getDefense()
        {
            return this.Defense;
        }
        public void setHP(int hp)
        {
            this.HP = hp;
        }
        public int getHP()
        {
            return this.HP;
        }
        public void setFaction(int move)
        {
            this.Faction = move;
        }
        public int getFaction()
        {
            return Faction;
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
        public void setIsPlayable(String IsPlayable)
        {
            this.isPlayable = IsPlayable;
        }
        public String getIsPlayable()
        {
            return this.isPlayable;
        }
        public void setSpeed(int speed)
        {
            this.speed = speed;
        }
        public int getSpeed()
        {
            return this.speed;
        }
        /*public void GetObjectData(SerializationInfo info, StreamingContext context)
        {

            info.AddValue("Name", this.Name);
            info.AddValue("HP", this.HP);
            info.AddValue("Attack", this.Attack);
            info.AddValue("Defense", this.Defense);
            info.AddValue("Move", this.Move);
            info.AddValue("png", this.png);
        }
        public NPC(SerializationInfo info, StreamingContext context)
        {
            this.Name = (string)info.GetValue("Name", typeof(string));
            this.HP = (int)info.GetValue("HP", typeof(int));
            this.Attack = (int)info.GetValue("Attack", typeof(int));
            this.Defense = (int)info.GetValue("Defense", typeof(int));
            this.Move = (int)info.GetValue("Move", typeof(int));
            this.png = (string)info.GetValue("png", typeof(string));

        }*/
    }
}
