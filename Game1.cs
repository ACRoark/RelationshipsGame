using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using GameJoltAPI;
using System.Drawing;


namespace _2_1
{
    public class CONST
    {
        /// <summary>
        /// Gravity constant, Vector3(0, -9.81f, 0)
        /// </summary>
        public static readonly Vector3 GRAVITY = new Vector3(0, -9.81f, 0);

        
    }

    public class dialogHandler {// helpful little buddy to move data around
        public bool flag;
        public String disName;
        public String disBio;
        public String curNPC;
        public String mate;

        /// <summary>
        /// 0 = game not over
        /// 1 = game over, lost
        /// 2 = game over, won
        /// </summary>
        public int gameOver; 
            
        }



    public class Game1 : Game
    {
        /// <summary>
        /// Graphics Device Manager
        /// </summary>
        GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;
        dialogHandler helper;

        Texture2D hopemeter, hopemeterfill, textbox, winscreen, killscreen;

        float currHope;
        float maxHope = 100f;

        Model earth, sky, ground, player, npc
            //A, B, C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z
            ;
        //List<Letter> letters;
        //Texture2D logo;

        // SpriteFont letterfont;

        SpriteFont fontTimes;

        //SoundEffect boing;

        Song bgtrack;
        SoundEffect crowdTrack;


        public bool textboxFlag = false;

        int maxNPCs;




        //Vector2 or Point?  (screen coordinates are always "int" when setting positions)
        /// <summary>
        /// screen space center of window (in pixels)
        /// </summary>
        Point windowCenter;

        /// <summary>
        /// field of view (radians)
        /// </summary>
        float fov;

        /// <summary>
        /// The 3D position of the camera in world space.
        /// </summary>
        Vector3 camPos;

        /// <summary>
        /// camera Y rotation amount around player
        /// </summary>
        float rotY;  //dont need to rotate X or Z
        float rotX; // dont need to rotate Y or Z

        /// <summary>
        /// Float multiplier  for look sens
        /// </summary>
        float lookSensitivity = 0.002f;

        /// <summary>
        /// Player position in world, vector3
        /// </summary>
        Vector3 playerPos;


        int numNPCs;

        /// <summary>
        /// List handler for NPCs
        /// </summary>
        List<NPC> NPClist;

        /// <summary>
        /// List of names for NPCs
        /// </summary>
        List<String> npcNames;

        /// <summary>
        /// List of bios for NPCs
        /// </summary>
        List<String> npcBios;

        /// <summary>
        ///flag var for if player is close to NPC </summary>
        bool playerNPCflag = false;

        /// <summary>
        //flag var for if NPC is activated by player </summary>
        bool NPCdialog = false;

        //random num gen
        Random rand;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            helper = new dialogHandler();

            #region ////////////////// variables
            currHope = maxHope;

            camPos = new Vector3(0, 25, -75);
            fov = MathHelper.PiOver4;

            rotY = 0;
            rotX = 0;

            rand = new Random();
            #endregion



            ///////////////////////////////////////////////////////////////////////// NPC HANDLING
            NPClist = new List<NPC>();
            npcNames = new List<String>();
            npcBios = new List<String>();

            numNPCs = 5;


            
            #region CREATE NPCs

            npcNames.Add("Sofia");
            npcBios.Add("Caffeine addict and part-time dragonslayer with a comprehensive \n" +
                        "grasp of every swear word in the english language.");

            npcNames.Add("Carolyn");
            npcBios.Add("Semi-responsible adult with lame, old lady hobbies (cross-stitching) \n" +
                "who has lapses of teenage irresponsibility (getting too drunk and eating out all the time).");

            npcNames.Add("Jeff");
            npcBios.Add("I'm your biological father.");

            npcNames.Add("Libby");
            npcBios.Add("Memelord fan girl trash who spends more time than she cares to admit reading fanfic.\n\n" +
                "I'm also gullible and overly trusting.");

            npcNames.Add("Katherine");
            npcBios.Add("The cool grandma you never had!");

            npcNames.Add("Zachary");
            npcBios.Add("A red head who wants to read more and play more diverse games \n but can't stop playing rocket league.");

            npcNames.Add("CJ");
            npcBios.Add("Degenerate with sleep issues who wants to go home when he is already at home.");

            npcNames.Add("Matthew");
            npcBios.Add("Alcoholic, tries to mask his depression behind a facade of humor and upbeat positivity. \n Calls himself a writer but has never actually written anything.");

            npcNames.Add("Jake");
            npcBios.Add("Who the hell do you think I am!?");

            npcNames.Add("David");
            npcBios.Add("Musically inclined hipster capable of growing more facial hair than the average Asian man.");

            npcNames.Add("Christian");
            npcBios.Add("Egotistical perfectionist with small margin for error and incontrolable \n" +
                " desire for success that does not meet the same levels of willpower and determination.");

            npcNames.Add("Nicholas");
            npcBios.Add("I am a stranger in this world. I don't need directions.");

            npcNames.Add("Nick");
            npcBios.Add("There's a typo in Christian's bio.");

            npcNames.Add("John");
            npcBios.Add("Neurotic softboy trying to make the world a better place.");

            maxNPCs = npcNames.Count;
            #endregion


            #region /////////////////////////////// CREATE NPCs //////////////////////////////////////////////////////
            //if(numNPCs <= maxNPCs)

            int r = rand.Next(0, NPClist.Count);

            for (int i = 0; i < numNPCs; i++) {

                r = rand.Next(0, NPClist.Count);

                ///// Randomly select from NPC data
                String newName = npcNames[r];
                String newBio = npcBios[r];

                if (i == 0) {
                    helper.mate = npcNames[r];
                    }

                NPClist.Add(// add NPC to NPC list
                        new NPC(
                            new Vector3(rand.Next(-50, 50), 0, rand.Next(-50, 50)),
                            new Vector3(rand.Next(-10, 10), 0, rand.Next(-10, 10)),
                            newName,
                            newBio
                            )//end new NPC
                );// end add new npc

            npcBios.RemoveAt(r);
                npcNames.RemoveAt(r);  



        } //end for
    
           

            #endregion



            windowCenter = new Point(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);

            Window.AllowUserResizing = true;
            base.Initialize();


        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            fontTimes = Content.Load<SpriteFont>("TimesNewRoman");

            player = Content.Load<Model>("playerUnit");
            npc = Content.Load<Model>("enemyUnit");
            earth = Content.Load<Model>("earth");
            sky = Content.Load<Model>("sky");
            ground = Content.Load<Model>("floor");
            //letterfont = Content.Load<SpriteFont>("LetterFont");
            crowdTrack = Content.Load<SoundEffect>("crowdwav");

            // Hope Meter sprites
            hopemeter = Content.Load<Texture2D>("hopemeter");
            hopemeterfill = Content.Load<Texture2D>("hopemeterfill");

            // Text Box Sprite
            textbox = Content.Load<Texture2D>("textbox");

            killscreen = Content.Load<Texture2D>("killscreen");
            winscreen = Content.Load<Texture2D>("winscreen");

            bgtrack = Content.Load<Song>("crowdbohren");
            //crowdTrack = Content.Load<Song>("crowd");

            //MediaPlayer.Play(crowdTrack);
            MediaPlayer.Play(bgtrack);
            crowdTrack.Play();
            MediaPlayer.IsRepeating = true;
            SoundEffect.MasterVolume = 0.1f;
            playerPos = new Vector3(0, ground.Meshes[0].BoundingSphere.Radius - 3f, 0);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,                                            UPDATE
        /// checking for collisions, gathering input, and playing audio.                                        UPDATE
        /// </summary>                                                                                          UPDATE
        /// <param name="gameTime">Provides a snapshot of timing values.</param>                                UPDATE
        protected override void Update(GameTime gameTime)
        {
            
            //textboxFlag = true;



            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Vector3 move = new Vector3(0, 0, 1);
            rotY -= (Mouse.GetState().X - windowCenter.X) * lookSensitivity;
            rotX += (Mouse.GetState().Y - windowCenter.Y) * lookSensitivity;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))

                rotX -= 0.01f;
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))

                rotX += 0.01f;


            Vector3 forward = Vector3.Transform(move, Matrix.CreateRotationY(rotY));
            Vector3 up = Vector3.Up;
            Vector3 right = Vector3.Cross(forward, up);

            if (Keyboard.GetState().IsKeyDown(Keys.A))////////////////////// LEFT
                //playerPos -= new Vector3(-1, 0, 0);
                playerPos -= right * 0.7f;

            else if (Keyboard.GetState().IsKeyDown(Keys.D))///////////////// RIGHT
                //playerPos -= new Vector3(1, 0, 0);
                playerPos += right * 0.7f;

            if (Keyboard.GetState().IsKeyDown(Keys.W))//////////////////////// UP
                playerPos += Vector3.Transform(move, Matrix.CreateRotationY(rotY)) * 0.8f;

            else if (Keyboard.GetState().IsKeyDown(Keys.S))//////////////// DOWN
                playerPos -= Vector3.Transform(move, Matrix.CreateRotationY(rotY)) * 0.8f;

            if (Keyboard.GetState().IsKeyDown(Keys.Y) && textboxFlag == true)
            {
                if (helper.curNPC == helper.mate) { 
                helper.gameOver = 2; //player win
                    textboxFlag = false;
            }
                else { 
                currHope = currHope - 25;
                textboxFlag = false;
            }
        }
            else if (Keyboard.GetState().IsKeyDown(Keys.N) && textboxFlag==true)
            {
                textboxFlag = false;
            }


            //////////////////////////////////////////////////////////////// TEST HOPE METER INC/DEC
            //if (Keyboard.GetState().IsKeyDown(Keys.OemPlus)){
            //    if (currHope <= 95) currHope += 5;
            //     }

            //else if (Keyboard.GetState().IsKeyDown(Keys.OemMinus)){
            //    if (currHope >= 5) currHope -= 5;
            //    }

            if (Keyboard.GetState().IsKeyDown(Keys.Space)) {

                if (textboxFlag) {



                    textboxFlag = false;
                }// If a text box is up, close it

                if (playerNPCflag)
                    textboxFlag = true;
                else
                    textboxFlag = false;

                

            
            }



            //Vector...  direction only
            Vector3 playerForward = Vector3.Backward;// new Vector3((float)Math.Sin(rotY), 0, (float)Math.Cos(rotY));

            playerForward = Vector3.Transform(playerForward, Matrix.CreateRotationX(rotX) * Matrix.CreateRotationY(rotY));

           
            ////////////////////////////////////////////////////////// ENEMY //////////////////////////////////////////////////////////

            playerNPCflag = false;
            NPCdialog = false;

            foreach (NPC npc in NPClist)
            {

                ////////////////////////////// Player and Enemy Collision ///////////////////////////////

                //if the player is in acceptable radius of an NPC
                if (new BoundingSphere(playerPos, player.Meshes[0].BoundingSphere.Radius / 1.5f).Intersects(
                        new BoundingSphere(npc.Pos, this.npc.Meshes[0].BoundingSphere.Radius / 1.5f))
                        )
                {
                    playerNPCflag = true;
                    helper.disName = npc.getName();
                    helper.disBio = npc.getBio();
                    helper.curNPC = npc.getName();


                    
                }



                //update the enemy position based on its velocity
                npc.Update(gameTime);

                ///////////////////////////////////////////////////////////////////////// FLOOR COLLISION
                if (npc.Pos.Y < 0 + ground.Meshes[0].BoundingSphere.Radius)
                {
                    //enemy has collided with floor
                    //fix position first
                    npc.Pos = new Vector3(npc.Pos.X, ground.Meshes[0].BoundingSphere.Radius - 3.0f, npc.Pos.Z);
                    //fix velocity 2* (-V dot N) * N + V
                    Vector3 slower = new Vector3(0, 8.0f, 0);
                    //e.Vel = 2 * Vector3.Dot(-e.Vel, Vector3.Up) * Vector3.Up + (e.Vel - slower);

                }

                /////////////////////////////////////////////////////////////////////// SKY DOME COLISION 

                float skydomeRadius = 200;// Where to end movement

                if (skydomeRadius - earth.Meshes[0].BoundingSphere.Radius <
                       Vector3.Distance(npc.Pos, Vector3.Zero))
                {
                    Vector3 skydomeNormal = Vector3.Zero - npc.Pos;
                    // Fix position first.
                    // Figure how far the enemy is outside the skydome
                    float howFarPast =
                        skydomeNormal.Length() - skydomeRadius + earth.Meshes[0].BoundingSphere.Radius;

                    npc.Pos += howFarPast * Vector3.Normalize(skydomeNormal);

                    skydomeNormal.Normalize();// Normalize so that the new Vel vector
                                              // does not add more speed

                    // Fix velocity
                    npc.Vel = 2 * Vector3.Dot(-npc.Vel, skydomeNormal) * skydomeNormal + npc.Vel;



                }// end sky dome

                ///////////////////////////////////////////////////////////ENEMY ON ENEMY COLISION
                foreach (NPC npc2 in NPClist)
                {

                    if (npc != npc2)
                    {//do not check against self
                        Vector3 axis = npc2.Pos - npc.Pos;

                        float dist = 2 * earth.Meshes[0].BoundingSphere.Radius;

                        if (axis.Length() < dist)
                        {

                            float move2 = (dist - axis.Length()) / 2.0f;

                            axis.Normalize(); // turn into direction only
                            npc.Pos -= axis * move2;
                            npc2.Pos += axis * move2;

                            //fix direction
                            Vector3 U1x = axis * Vector3.Dot(axis, npc.Vel);
                            Vector3 U2x = -axis * Vector3.Dot(-axis, npc2.Vel);

                            Vector3 U1y = npc.Vel - U1x;
                            Vector3 U2y = npc2.Vel - U2x;

                            npc.Vel = U2x + U1y;
                            npc2.Vel = U1x + U2y;
                        }
                    }
                }

            }//end enemy


            //foreach (Letter let in letters) {
            //    let.Update(gameTime);
            //    }

            if (currHope <= 0)
                helper.gameOver = 1;



            Mouse.SetPosition(windowCenter.X, windowCenter.Y);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;


            GraphicsDevice.Clear(Color.CornflowerBlue);

            //how we place the model in the world
            Matrix world = Matrix.Identity;

            /////////////////////////////////////// CAMERA VECTOR AS MATRIX
            Matrix view = Matrix.CreateLookAt(
                Vector3.Transform(
                    camPos,
                    Matrix.CreateRotationX(rotX) *
                    Matrix.CreateRotationY(rotY)) + playerPos,
                playerPos,
                Vector3.Up);

            ///////////////////////////////////// FOV VECTOR AS MATRIX
            Matrix proj = Matrix.CreatePerspectiveFieldOfView(
                fov,
                1,
                0.1f,
                1000.0f);
            world = Matrix.CreateScale(100);
            ground.Draw(world, view, proj);

            //SRT
            //rotate then translate
            world = Matrix.CreateRotationY(rotY) * Matrix.CreateTranslation(playerPos);
            player.Draw(world, view, proj);

            ///////////////////////////////////////////////////////////////////////// DRAW ENEMIES
            #region
            for (int i = 0; i < NPClist.Count; i++)
            {//foreach (Vector3 p in pos)

                world = Matrix.CreateTranslation(NPClist[i].Pos);
                Vector2 drawPos;
                //enemy[i].Pos is a 3D position need to figure out 2D screen coords
                //drawPos = new Vector2(enemy[i].Pos.X, enemy[i].Pos.Y);
                Vector3 tempPos = Vector3.Transform(NPClist[i].Pos, proj * view);
                drawPos = new Vector2(tempPos.X, tempPos.Y);


                foreach (ModelMesh mesh in npc.Meshes) { // mesh is collection of triangles
               
                    foreach (BasicEffect effect in mesh.Effects) {
                        effect.Projection = proj;
                        effect.World = world;
                        effect.View = view;
                    }
                    mesh.Draw();
                }
            }// end NPClist for
            #endregion

            world = Matrix.CreateScale(100);
            sky.Draw(world, view, proj);
            world = Matrix.CreateScale(100) * Matrix.CreateRotationX(MathHelper.Pi);
            sky.Draw(world, view, proj);

            ////////////////////////////////// IF PLAYER CLOSE TO NPC, DISPLAY [SPACE] 
            #region
            if (playerNPCflag){
                spriteBatch.Begin();

                spriteBatch.DrawString(
                        fontTimes, 
                        "Interact [SPACE]",
                        new Vector2(
                            GraphicsDevice.Viewport.Width / 2.3f,
                            GraphicsDevice.Viewport.Height / 1.3f
                        ),
                        Color.White
                        );

                spriteBatch.End();
            }
            #endregion

            //////////////////////////////////////////// DRAW HUD //////////////
            #region
            spriteBatch.Begin();

            //healthbar frame
            spriteBatch.Draw(
                hopemeterfill,
                new Rectangle( 130,10, // position on screen in pixels

                        (int)((int)(hopemeter.Width / 2 - 120) * (currHope / maxHope)),//width
                        (int)hopemeter.Height / 2),                                     //height
                        Color.White //change no colors
                );

            //health meter amount...
            spriteBatch.Draw(hopemeter,
                
                new Rectangle(10, 10, (int)hopemeter.Width / 2, (int)hopemeter.Height / 2),
                
                new Rectangle(0, 0, hopemeter.Width, hopemeter.Height), // where I'm reading from in the original image
                Color.White);


            //spriteBatch.Draw(heart, new Rectangle(10 - (int)(2 * Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / (50 * healthValue + 1))), 10 - (int)(2 * Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / (50 * healthValue + 1))), 200 + (int)(2 * Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / (50 * healthValue + 1))), 50 + (int)(2 * Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / (50 * healthValue + 1)))), Color.White);

            spriteBatch.End();

            //textboxFlag = true;



            if (textboxFlag)
                drawTextBox( helper.disName, helper.disBio);
            #endregion

           

            if (helper.gameOver != 0) {
                if (helper.gameOver == 1) {

                    spriteBatch.Begin();

                    int width = GraphicsDevice.Viewport.Width;
                    int height = GraphicsDevice.Viewport.Height;
                    int xpoint = 0;
                    int ypoint = 0;

                    Rectangle textBoxrect = new Rectangle(xpoint, ypoint, width, height);

                    spriteBatch.Draw(
                        killscreen, //source texture - killscreen
                        textBoxrect,// pre-assembled rectangle for on screen
                        Color.White
                    );

                    spriteBatch.End();


                }

                else if (helper.gameOver == 2) {
                    spriteBatch.Begin();

                    int width = GraphicsDevice.Viewport.Width;
                    int height = GraphicsDevice.Viewport.Height;
                    int xpoint = 0;
                    int ypoint = 0;

                    Rectangle textBoxrect = new Rectangle(xpoint, ypoint, width, height);

                    spriteBatch.Draw(
                        winscreen, //source texture - winscreen
                        textBoxrect,// pre-assembled rectangle for on screen
                        Color.White
                    );

                    spriteBatch.End();
                }

           }



            base.Draw(gameTime);
        }// end DRAW

        public void drawTextBox() {
            spriteBatch.Begin();

            int width = 700;
            int height = 150;
            int xpoint = 50;
            int ypoint = GraphicsDevice.Viewport.Height * 2/3;

            Rectangle textBoxrect = new Rectangle(xpoint, ypoint, width, height);

            spriteBatch.Draw(
                textbox, //source texture
                textBoxrect,// pre-assembled rectangle for on screen
                Color.White
            );

            spriteBatch.DrawString(
                        fontTimes,
                        "N A M E",
                        new Vector2(
                            xpoint+65,
                            ypoint+23
                        ),
                        Color.White
                        );

            spriteBatch.End();
        }//end text box


        public void drawTextBox(String n, String b)
        {
            spriteBatch.Begin();

            int width = 700;
            int height = 150;
            int xpoint = 50;
            int ypoint = GraphicsDevice.Viewport.Height * 2 / 3;

            Rectangle textBoxrect = new Rectangle(xpoint, ypoint, width, height);

            spriteBatch.Draw(
                textbox, //source texture
                textBoxrect,// pre-assembled rectangle for on screen
                Color.White
            );

            spriteBatch.DrawString(
                        fontTimes,
                        n, // name string
                        new Vector2(
                            xpoint + 65,
                            ypoint + 23
                        ),
                        Color.Gold
                        );

            spriteBatch.DrawString(
                        fontTimes,
                        b,
                        new Vector2(
                            xpoint + 65,
                            ypoint + 46
                        ),
                        Color.White
                        );


            spriteBatch.DrawString(
                        fontTimes,
                        " Is this person your mate?              [Y]es               [N]o  \n"+
                        "                                                                                                                  [Space] Close",
                        new Vector2(
                            xpoint + 65,
                            ypoint + 100
                        ),
                        Color.White
                        );


            spriteBatch.End();
        }//end text box

    }// end GAME
}//end NAMESPACE
