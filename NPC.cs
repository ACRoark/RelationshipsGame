using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace _2_1
{
    class NPC{
        /// <summary>
        ///each position, Vector3       
        /// </summary>
        Vector3 pos;

        /// <summary>
        /// Name of npc, string
        /// </summary>
        String name;

        /// <summary>
        /// NPCs bio of themselves, string
        /// </summary>
        String bio;
        

        public Vector3 Pos
        {
            get { return pos; }
            set { pos = value; }
        }

        /// <summary>
        /// stored velocity for each npc
        /// </summary>
        Vector3 vel;

        public Vector3 Vel
        {
            get { return vel; }
            set { vel = value; }
        }

        public Vector3 Foward
        {
            get { return Vector3.Normalize(vel); }
        }

        public NPC(Vector3 p, Vector3 v){
            pos = p;
            vel = v;
        }

        public NPC(Vector3 p, Vector3 v, String nm, String bi){
            pos = p;
            vel = v;
            name = nm;
            bio = bi;
        }

        public String getBio() { return bio; }

        public String getName() { return name; }

        public void Update(GameTime gameTime)
        {
            // v += a * (delta)t;
            vel = vel + CONST.GRAVITY * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //p += v * (delta)t;
            pos = pos + vel * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}

