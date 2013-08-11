using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;

namespace FPSGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Fields

        public AudioManager audioManager;
        public List<SoundEmitter> soundEmitters;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        KeyboardInputState Input;
        FPSCamera camera;
        Player fpc;
        EffectManager effectManager;
        BasicEffect effect;
        //Sender sender;
        SpawnManager sm;
        SkySphere sky;
        Ground ground;
        int NetworkUpdateCounter = 0;
        Terrain terrain;
        Water water;

        // PacketWriter and PacketReader used to send and recieve game data
        public PacketWriter packetWriter = new PacketWriter();
        public PacketReader packetReader = new PacketReader();

        // List of sessions that you can join
        AvailableNetworkSessionCollection availableSessions;

        GamePadState currentGamePadState;
        GamePadState lastGamePadState;
        Random random = new Random();

        public enum GameState { MainMenu, CreateSession, FindSession, GameLobby, PlayingGame };
        public enum GameType { DeathMatch, CaptureTheFlag, FreeForAll };
        public enum SessionProperties { GameType, MapLevel, OtherCustomProperty };
        public enum MessageState { Update, Animation, Effects, Audio, Damage, Heal, RemoteAudio };

        public NetworkSession networkSession;

        public static GameState gameState = GameState.MainMenu;

        List<DisplayMessage> gameMessages = new List<DisplayMessage>();
        List<DummyPlayer> dummyPlayers = new List<DummyPlayer>();

        Texture2D backgroundTexture;
        Rectangle bgScreenRect;

        Song mainMenuMusic;
        bool songStart = false;

        // for health bar
        private Texture2D rect;
        private Color[] data;
        private Vector2 origin;
        private Vector2 screenpos;

        FreeCamera waterCamera;

        #endregion

        public Game1()
        {
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 100.0f);
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
            Components.Add(new GamerServicesComponent(this));
            camera = new FPSCamera(this);
            soundEmitters = new List<SoundEmitter>();
            Components.Add(camera);
            audioManager = new AudioManager(this);
            Components.Add(audioManager);
        }

        public EffectManager getEffectManager
        {
            get
            { 
                return this.effectManager;
            }
        }

        public struct DisplayMessage
        {
            public string Message;
            public TimeSpan DisplayTime;

            public DisplayMessage(string message, TimeSpan displayTime)
            {
                Message = message;
                DisplayTime = displayTime;
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        #region Intialize/Content
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Input = new KeyboardInputState();
            Services.AddService(typeof(KeyboardInputState), Input);
            this.Services.AddService(typeof(FPSCamera), camera);
            //fpc = new Player(this);         // MOVE THE CREATION OF THESE GUYS TO AFTER SESSION CREATED
            fpc = new Player(this);
            Components.Add(fpc);
            //sender = new Sender(this, fpc);
            //Components.Add(sender);
            //sm = new SpawnManager(this);
            //Components.Add(sm);
            //sm.SpawnGman();
            //ground = new Ground(this, new Vector3(0, 0, 0));
            //Components.Add(ground);
            Components.Add(new Tree(this, new Vector3(20, 70, -80)));
            Components.Add(new Tree(this, new Vector3(80, 50, 30)));
            Components.Add(new Tree(this, new Vector3(-50, 50, -210)));
            Components.Add(new Tree(this, new Vector3(-20, 70, -510)));
            Components.Add(new Tree(this, new Vector3(80, 70, 410)));
            Components.Add(new Tree(this, new Vector3(-120, 60, 40)));
            Components.Add(new Tree(this, new Vector3(840, 70, -170)));
            Components.Add(new Tree(this, new Vector3(-300, 80, 130)));
            Components.Add(new Tree(this, new Vector3(-80, 60, 160)));
            Components.Add(new Ship(this, new Vector3(1100, 40, 1100)));
            Components.Add(new Boulder(this, new Vector3(-1000, 60, -1040)));
            Components.Add(new Boulder(this, new Vector3(-2300, 60, -1200)));
            Components.Add(new Blimp(this, new Vector3(80, 520, 40)));
            //Components.Add(new Crate(this, new Vector3(0, 10, 0)));
            //Components.Add(new Crate(this, new Vector3(0, 25, 0)));
            //Components.Add(new Crate(this, new Vector3(0, 10, 0)));
            //Components.Add(new Crate(this, new Vector3(0, 25, 0)));
            Components.Add(new Crate(this, new Vector3(0, 100, -50)));
            graphics.ToggleFullScreen();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            waterCamera = new FreeCamera(new Vector3(1000, 2000, -2000),
                MathHelper.ToRadians(120), // Turned around 153 degrees
                MathHelper.ToRadians(-45), // Pitched up 13 degrees
                GraphicsDevice);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteFont = Content.Load<SpriteFont>("gamefont");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);
            BoundingBoxRenderer.InitializeGraphics(GraphicsDevice);
            CrosshairRenderer.initCrosshair(GraphicsDevice);
            effect = new BasicEffect(GraphicsDevice);
            effectManager = new EffectManager(this, GraphicsDevice, effect);
            Components.Add(effectManager);
            backgroundTexture = Content.Load<Texture2D>("bg");
            bgScreenRect = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            mainMenuMusic = Content.Load<Song>("MainMenuMusic");
            MediaPlayer.IsRepeating = true;

            terrain = new Terrain(Content.Load<Texture2D>("AssetCollection\\Terrain\\heightMap"), 8, 8,
                Content.Load<Texture2D>("AssetCollection\\Terrain\\grass"), 144f, new Vector3(1, -1, 0),
                GraphicsDevice, Content);
            terrain.WeightMap = Content.Load<Texture2D>("AssetCollection\\Terrain\\terrainMap");
            terrain.RTexture = Content.Load<Texture2D>("AssetCollection\\Terrain\\sand");
            terrain.GTexture = Content.Load<Texture2D>("AssetCollection\\Terrain\\rock0");
            terrain.BTexture = Content.Load<Texture2D>("AssetCollection\\Terrain\\snow");
            terrain.DetailTexture = Content.Load<Texture2D>("AssetCollection\\Terrain\\noiseTexture");
            sky = new SkySphere(Content, GraphicsDevice,
                Content.Load<TextureCube>("AssetCollection\\Sky\\skySphereCubeMapHalo"));
            water = new Water(Content, this, new Vector3(0, 20, 0), new Vector2(8000, 8000));
            water.Objects.Add(sky);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        #endregion

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Update the network session we need to check to
            // see if it is disposed since calling Update on
            // a disposed NetworkSession will throw an exception


            if (networkSession != null && !networkSession.IsDisposed)
            {
                networkSession.Update();
            }

            if (gameTime.TotalGameTime.Seconds > 0.1)
            {
                if (Gamer.SignedInGamers.Count == 0 && !Guide.IsVisible)
                {
                    Guide.ShowSignIn(1, false);
                }
            }

            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            switch (gameState)
            {
                case GameState.MainMenu:
                    MainMenuUpdate();
                    break;
                case GameState.CreateSession:
                    CreateSessionUpdate();
                    break;
                case GameState.GameLobby:
                    GameLobbyUpdate();
                    break;
                case GameState.PlayingGame:
                    PlayingGameUpdate(gameTime);
                    break;
                case GameState.FindSession:
                    FindSessionUpdate();
                    break;
            }

            if (songStart && gameState == GameState.PlayingGame)
            {
                MediaPlayer.Stop();
                songStart = false;
            }
            else if (!songStart && gameState != GameState.PlayingGame)
            {
                MediaPlayer.Play(mainMenuMusic);
                songStart = true;
            }

            lastGamePadState = currentGamePadState;

            if (gameMessages.Count > 0)
            {
                DisplayMessage currentMessage = gameMessages[0];
                currentMessage.DisplayTime -= gameTime.ElapsedGameTime;

                if (currentMessage.DisplayTime <= TimeSpan.Zero)
                {
                    gameMessages.RemoveAt(0);
                }
                else
                {
                    gameMessages[0] = currentMessage;
                }
            }
            // Allows the game to exit

            waterCamera.Position = fpc.Pose.WorldPosition;
            waterCamera.View = camera.ViewMatrix;
            waterCamera.Projection = camera.ProjMatrix;

            // TODO: Add your update logic here
            Input.Update();
            base.Update(gameTime);
        }

        public Terrain getTerrain()
        {
            return terrain;
        }

        // Update method for the FindSession GameState
        private void FindSessionUpdate()
        {
            // Go back to the main menu
            if (Input.IsKeyReleased(Keys.Back))
            {
                gameState = GameState.MainMenu;
            }
            // If the user presses the A button join the first session
            else if (Input.IsKeyReleased(Keys.A) && availableSessions.Count != 0)
            {
                JoinSession(0);
            }
        }

        public void AnimationUpdate()
        {
            foreach (LocalNetworkGamer gamer in networkSession.LocalGamers)
            {
                packetWriter.Write((short)MessageState.Animation);
                packetWriter.Write((short)fpc.playerAnimationState);
                gamer.SendData(packetWriter, SendDataOptions.Reliable);
            }
        }

        public void EffectUpdate(Vector3 start, Vector3 end, Color effectColor)
        {
            foreach (LocalNetworkGamer gamer in networkSession.LocalGamers)
            {
                packetWriter.Write((short)MessageState.Effects);
                packetWriter.Write(start);
                packetWriter.Write(end);
                packetWriter.Write(effectColor);
                gamer.SendData(packetWriter, SendDataOptions.Reliable);
            }
        }

        public void AudioUpdate(string SoundName, bool IsLooped)
        {
            foreach (LocalNetworkGamer gamer in networkSession.LocalGamers)
            {
                packetWriter.Write((short)MessageState.Audio);
                packetWriter.Write(SoundName);
                packetWriter.Write(IsLooped);
                gamer.SendData(packetWriter, SendDataOptions.Reliable);
            }
        }

        public void DamageUpdate(int newHealth, string targetName)
        {
            foreach (LocalNetworkGamer gamer in networkSession.LocalGamers)
            {
                packetWriter.Write((short)MessageState.Damage);
                packetWriter.Write(newHealth);
                packetWriter.Write(targetName);
                gamer.SendData(packetWriter, SendDataOptions.Reliable);
            }
        }

        public void RemoteAudioUpdate(string SoundName, bool IsLooped, Vector3 position)
        {
            foreach (LocalNetworkGamer gamer in networkSession.LocalGamers)
            {
                packetWriter.Write((short)MessageState.RemoteAudio);
                packetWriter.Write(position);
                packetWriter.Write(SoundName);
                gamer.SendData(packetWriter, SendDataOptions.Reliable);
            }
        }

        private void SoundEmitterUpdate()
        {
            for (int i = 0; i < soundEmitters.Count; )
            {
                if (soundEmitters[i].dead)
                {
                    soundEmitters[i] = null;
                    soundEmitters.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        public void HealUpdate(int newHealth)
        {
            foreach (LocalNetworkGamer gamer in networkSession.LocalGamers)
            {
                packetWriter.Write((short)MessageState.Heal);
                packetWriter.Write(newHealth);
                gamer.SendData(packetWriter, SendDataOptions.Reliable);
            }
        }

        // Creates a session using a AvailableNetworkSession index
        private void JoinSession(int sessionID)
        {
            // Join an existing NetworkSession
            try
            {
                networkSession = NetworkSession.Join(availableSessions[sessionID]);
            }
            catch (NetworkSessionJoinException ex)
            {
                gameMessages.Add(new DisplayMessage("Failed to connect to session: " +
                    ex.JoinError.ToString(),
                    TimeSpan.FromSeconds(2)));
                // Check for sessions again
                FindSession();
            }

            // Register for NetworkSession events
            networkSession.GameStarted += new EventHandler<GameStartedEventArgs>(networkSession_GameStarted);
            networkSession.GameEnded += new EventHandler<GameEndedEventArgs>(networkSession_GameEnded);
            networkSession.GamerJoined += new EventHandler<GamerJoinedEventArgs>(networkSession_GamerJoined);
            networkSession.GamerLeft += new EventHandler<GamerLeftEventArgs>(networkSession_GamerLeft);
            networkSession.SessionEnded += new EventHandler<NetworkSessionEndedEventArgs>(networkSession_SessionEnded);

            // Set the correct GameState. The NetworkSession may have already started a game.
            if (networkSession.SessionState == NetworkSessionState.Playing)
            {
                gameState = GameState.PlayingGame;
            }
            else
            {
                gameState = GameState.GameLobby;
            }
        }

        
        // Update method for the PlayingGame GameState
        private void PlayingGameUpdate(GameTime gameTime)
        {
            // tell audiomanager about camera position
            audioManager.Listener.Position = camera.WorldPosition;
            audioManager.Listener.Forward = camera.Pose.WorldTransform.Forward;
            audioManager.Listener.Up = camera.Pose.WorldTransform.Up;
            audioManager.Listener.Velocity = fpc.Velocity;

            // check to see if player wants to quit
            if (Input.IsKeyReleased(Keys.Back))
            {
                // If the player is the host then
                // the game is exited but the session
                // stays alive
                if (networkSession.IsHost)
                    networkSession.EndGame();
                // Other players leave the session
                else
                {
                    networkSession.Dispose();
                    networkSession = null;
                    gameState = GameState.MainMenu;
                }

                return;
            }

// --------------------------------------------------------------------------------------------------------//
// --------------------------------------------------------------------------------------------------------//
// --------------------------------------------------------------------------------------------------------//
// --------------------------------------------------------------------------------------------------------//

            if (NetworkUpdateCounter == 2)
            {
                // Loop all of the local gamers
                foreach (LocalNetworkGamer gamer in networkSession.LocalGamers)
                {
                    // Finds the player object associated with the local gamerTag
                    Player LocalPlayer = gamer.Tag as Player;

                    // Write all the data to be sent into one packet
                    // packetWriter does this by default, just call packetWriter.Write() as many times as necessary
                    // then use SendData to send all the current packet
                    // packetWriter.Write('1'); //some kind of message ID
                    // packetWriter.Write((int)fpc.playerAnimationState); //enum to int cast test
                    packetWriter.Write((short)MessageState.Update);
                    packetWriter.Write(fpc.WorldTransform);
                    // Send the data to everyone in your session
                    // SendDataOptions.None is like UDP because it just tries to update everyone with the most current data
                    // jeff - reliable is better, isnt terribly expensive and tries to guarantee delivery, order not too important
                    gamer.SendData(packetWriter, SendDataOptions.None);
                }
                NetworkUpdateCounter = 0;
            }
            NetworkUpdateCounter++;

            
            // Read data that is sent to local players
            // Read data that is sent to local players
            foreach (LocalNetworkGamer gamer in networkSession.LocalGamers)
            {
                // Read until there is no data left
                while (gamer.IsDataAvailable)
                {
                    NetworkGamer sender;
                    gamer.ReceiveData(packetReader, out sender);

                    // We only need to update the state of non local players
                    if (sender.IsLocal)
                        continue;

                    // Finds the player object associated with the REMOTE gamerTag
                    DummyPlayer RemotePlayer = sender.Tag as DummyPlayer;

                    // Read Data
                    // some kind of message ID parsing maybe? depends if we have multiple sending points for data
                    // ideally sending one packet would be fine

                    MessageState MessageID = (MessageState)packetReader.ReadInt16();
                    switch (MessageID)
                    {
                        case MessageState.Update:
                            Matrix worldTransform = packetReader.ReadMatrix();
                            RemotePlayer.UpdatePose(worldTransform);
                            break;

                        case MessageState.Animation:
                            RemotePlayer.UpdateAnimationState(packetReader.ReadInt16());
                            break;

                        case MessageState.Effects:
                            effectManager.addNewLaser(packetReader.ReadVector3(), packetReader.ReadVector3(), packetReader.ReadColor());
                            break;

                        case MessageState.Audio:
                            string lol = packetReader.ReadString();
                            Console.WriteLine(lol);
                            audioManager.Play3DSound(lol, packetReader.ReadBoolean(), RemotePlayer);
                            break;

                        case MessageState.RemoteAudio:
                            SoundEmitter tempEmitter = new SoundEmitter(packetReader.ReadVector3());
                            audioManager.Play3DSound(packetReader.ReadString(), false, tempEmitter);
                            break;

                        case MessageState.Damage:
                            int newHealth = packetReader.ReadInt32();
                            string victimName = packetReader.ReadString();
                            foreach (LocalNetworkGamer tempGamer in networkSession.LocalGamers)
                            {
                                Player VictimPlayer = tempGamer.Tag as Player;
                                if (VictimPlayer.playerName == victimName)
                                {
                                    VictimPlayer.health = newHealth;
                                    if (VictimPlayer.health <= 0)
                                        VictimPlayer.isDead = true;
                                }
                            }
                            break;

                        case MessageState.Heal:
                            RemotePlayer.health = packetReader.ReadInt32();
                            break;
                    }
                }
            }
// --------------------------------------------------------------------------------------------------------//
// --------------------------------------------------------------------------------------------------------//
// --------------------------------------------------------------------------------------------------------//
// --------------------------------------------------------------------------------------------------------//
// --------------------------------------------------------------------------------------------------------//
// --------------------------------------------------------------------------------------------------------//

        }

        // Update method for the GameLobby GameState
        private void GameLobbyUpdate()
        {
            // Move back to the main menu
            if (Input.IsKeyReleased(Keys.Back))
            {
                networkSession.Dispose();
                //Services.RemoveService(typeof(NetworkSession)); //sorry
                gameState = GameState.MainMenu;
            }

            // Set the ready state for the player
            else if (Input.IsKeyReleased(Keys.A))
            {
                networkSession.LocalGamers[0].IsReady = !networkSession.LocalGamers[0].IsReady;
            }
            // Only the host can start the game
            else if (Input.IsKeyReleased(Keys.B))
            {
                //for (int i=0, i<networkSession.LocalGamers[], i++)
                networkSession.StartGame();
                //if (Components.Contains(fpc)) { Components.Remove(fpc); }
                //fpc = new Player(this);
                //Components.Add(fpc);
            }
            // Invite other players
            else if (Input.IsKeyReleased(Keys.X))
            {
                Guide.ShowGameInvite(PlayerIndex.One, null);
            }

            // Show the party screen
            if (Input.IsKeyReleased(Keys.Y) && !Guide.IsVisible)
                Guide.ShowParty(PlayerIndex.One);

            // Register for the invite event
            // NetworkSession.InviteAccepted += new EventHandler<InviteAcceptedEventArgs>(NetworkSession_InviteAccepted);
        }

        void NetworkSession_InviteAccepted(object sender, InviteAcceptedEventArgs e)
        {
            if (networkSession != null && !networkSession.IsDisposed)
            {
                networkSession.Dispose();
            }

            if (!e.IsCurrentSession)
            {
                networkSession = NetworkSession.JoinInvited(1);
            }
        }

        private void CreateSessionUpdate()
        {
            if (Input.IsKeyReleased(Keys.Back))
                gameState = GameState.MainMenu;
            else if (Input.IsKeyReleased(Keys.A))
                CreateSession(GameType.DeathMatch);
                /*
            else if (Input.IsKeyReleased(Keys.B))
                CreateSession(GameType.CaptureTheFlag);
            else if (Input.IsKeyReleased(Keys.X))
                CreateSession(GameType.FreeForAll);*/
        }

        private void CreateSession(GameType gameType)
        {
            if (networkSession != null && !networkSession.IsDisposed)
                networkSession.Dispose();
            NetworkSessionProperties sessionProperties = new NetworkSessionProperties();

            // Other players will use these to search for a session
            sessionProperties[(int)SessionProperties.GameType] = (int)gameType;
            sessionProperties[(int)SessionProperties.MapLevel] = 0;
            sessionProperties[(int)SessionProperties.OtherCustomProperty] = 42;

            networkSession = NetworkSession.Create(NetworkSessionType.SystemLink, 1, 4, 0, sessionProperties);

            networkSession.AllowJoinInProgress = true;

            networkSession.GameStarted += new EventHandler<GameStartedEventArgs>(networkSession_GameStarted);
            networkSession.GameEnded += new EventHandler<GameEndedEventArgs>(networkSession_GameEnded);
            networkSession.GamerJoined += new EventHandler<GamerJoinedEventArgs>(networkSession_GamerJoined);
            networkSession.GamerLeft += new EventHandler<GamerLeftEventArgs>(networkSession_GamerLeft);
            networkSession.SessionEnded += new EventHandler<NetworkSessionEndedEventArgs>(networkSession_SessionEnded);


            Services.AddService(typeof(NetworkSession), networkSession);
            gameState = GameState.GameLobby;
        }

        #region Networking - Event Handlers

        //This is the most important EH, because this creates a PLAYER OBJECT associated with each gamer that joins!
        void networkSession_GamerJoined(object sender, GamerJoinedEventArgs e)
        {
            gameMessages.Add(new DisplayMessage("Gamer Joined: " + e.Gamer.Gamertag, TimeSpan.FromSeconds(2)));
            if (e.Gamer.IsLocal)
            {
                e.Gamer.Tag = fpc;
                fpc.playerName = e.Gamer.Gamertag;
                fpc.respawn();
            }
            else
            {
                DummyPlayer temp = new DummyPlayer(this, Vector3.One, e.Gamer.Gamertag);
                // spawn stuff
                e.Gamer.Tag = temp;
                Components.Add(temp);
                temp.respawn();
            }

        }

        void networkSession_GameStarted(object sender, GameStartedEventArgs e)
        {
            gameMessages.Add(new DisplayMessage("Game Started", TimeSpan.FromSeconds(2)));
            gameState = GameState.PlayingGame;
        }

        void networkSession_GameEnded(object sender, GameEndedEventArgs e)
        {
            gameMessages.Add(new DisplayMessage("Game Ended", TimeSpan.FromSeconds(2)));
            gameState = GameState.GameLobby;
        }

        void networkSession_GamerLeft(object sender, GamerLeftEventArgs e)
        {
            gameMessages.Add(new DisplayMessage("Gamer Left: " + e.Gamer.Gamertag, TimeSpan.FromSeconds(2)));
            DummyPlayer player = e.Gamer.Tag as DummyPlayer;
            if (e.Gamer.IsGuest)
                player.Dispose();
        }

        void networkSession_SessionEnded(object sender, NetworkSessionEndedEventArgs e)
        {
            gameMessages.Add(new DisplayMessage("Session Ended: " + e.EndReason.ToString(), TimeSpan.FromSeconds(2)));
            if (networkSession != null && !networkSession.IsDisposed)
                networkSession.Dispose();

            gameState = GameState.MainMenu;
        }

        #endregion

        private void MainMenuUpdate()
        {
            if (Gamer.SignedInGamers.Count != 0 && !Guide.IsVisible && gameState != GameState.PlayingGame)
            {
            if (Input.IsKeyReleased(Keys.Back))
            {
                Exit();
            }

            if (Input.IsKeyReleased(Keys.A))
            {
                gameState = GameState.CreateSession;
            }
            // Find a session
            if (Input.IsKeyReleased(Keys.B))
            {
                FindSession();
            }

            // Show a party sessions the player can join
            if (Input.IsKeyReleased(Keys.Y) && !Guide.IsVisible)
                Guide.ShowPartySessions(PlayerIndex.One);
            }
        }

        // Method to start the search for a NetworkSession
        private void FindSession()
        {
            // Dispose of any previous session
            if (networkSession != null && !networkSession.IsDisposed)
            {
                networkSession.Dispose();
            }

            // Define the type of session we want to search for using the 
            // NetworkSessionProperties
            // We only set the MapLevel and the OtherCustomProperty

            NetworkSessionProperties sessionProperties = new NetworkSessionProperties();
            sessionProperties[(int)SessionProperties.MapLevel] = 0;
            sessionProperties[(int)SessionProperties.OtherCustomProperty] = 42;

            // Find an available NetworkSession
            availableSessions = NetworkSession.Find(NetworkSessionType.SystemLink, 1, sessionProperties);

            // Move the game into the FindSEession state
            gameState = GameState.FindSession;
        }

        bool ButtonPressed(Buttons button)
        {
            if (Guide.IsVisible)
                return false;
            return currentGamePadState.IsButtonDown(button) &&
                lastGamePadState.IsButtonUp(button);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            switch (gameState)
            {
                case GameState.MainMenu:
                    MainMenuDraw();
                    break;
                case GameState.CreateSession:
                    CreateSessionDraw();
                    break;
                case GameState.GameLobby:
                    GameLobbyDraw();
                    break;
                case GameState.PlayingGame:
                    PlayingGameDraw(gameTime);
                    base.Draw(gameTime);
                    CrosshairRenderer.drawCrosshair(spriteBatch, Color.Red, fpc.currentWeapon.Recoil);
                    break;
                case GameState.FindSession:
                    FindSessionDraw();
                    break;
            }

            if (gameMessages.Count > 0)
            {
                DisplayMessage currentMessage = gameMessages[0];
                spriteBatch.Begin();
                Vector2 stringSize = spriteFont.MeasureString(gameMessages[0].Message);
                spriteBatch.DrawString(spriteFont, gameMessages[0].Message,
                    new Vector2((1280 - stringSize.X) / 2.0f, 500), Color.White);
                spriteBatch.End();
            }
            if (gameState == GameState.PlayingGame)
            {
                spriteBatch.Begin();
                int rectAndColor;
                if (fpc.health != 0)
                    rectAndColor = fpc.health;
                else
                    rectAndColor = 1;
                rect = new Texture2D(graphics.GraphicsDevice, 2 * rectAndColor, 30);
                data = new Color[2 * rectAndColor * 30];
                origin.X = rect.Width / 2;
                origin.Y = rect.Height / 2;
                screenpos.X = 1280 / 2;
                screenpos.Y = 720 / 2;
                for (int i = 0; i < data.Length; ++i)
                {
                    if (i % 2 == 0) { data[i] = Color.Red; }
                    else { data[i] = Color.Black; }
                }
                rect.SetData(data);
                float circle = MathHelper.Pi * 2;
                float rotationAngle = 18.75f;
                rotationAngle = rotationAngle % circle;
                Vector2 coor = new Vector2(40, 650);
                spriteBatch.Draw(rect, coor, null, Color.White, rotationAngle, new Vector2(0,0), 1.0f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(spriteFont, "" + fpc.health, coor, Color.White);
                rect = new Texture2D(graphics.GraphicsDevice, 20 * fpc.ammo, 30);
                data = new Color[20 * fpc.ammo * 30];
                for (int i = 0; i < data.Length; ++i)
                {
                    if (i % 2 == 0) { data[i] = Color.Blue; }
                    else { data[i] = Color.BlueViolet; }
                }
                rect.SetData(data);
                coor = new Vector2(1280-80,650);
                rotationAngle = 22.15f;
                rotationAngle = rotationAngle % circle;
                spriteBatch.Draw(rect, coor, null, Color.White, rotationAngle, new Vector2(30, 30), 1.0f, SpriteEffects.FlipHorizontally, 0f);
                spriteBatch.DrawString(spriteFont, "" + fpc.ammo, coor, Color.White);
                spriteBatch.End();
            }
        }
        
        // Draw method for the PlayingGame GameState
        private void PlayingGameDraw(GameTime gameTime)
        {
            spriteBatch.Begin();
            
            //DepthStencilState ds = new DepthStencilState();
            //ds.DepthBufferEnable = true;
            //GraphicsDevice.DepthStencilState = ds;
            //BoundingBoxRenderer.Render(GraphicsDevice, ((FPSCamera)Services.GetService(typeof(FPSCamera))).ViewMatrix, ((FPSCamera)Services.GetService(typeof(FPSCamera))).ProjMatrix, ground.worldBoundsBox);
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            //RasterizerState originalRasterizerState = graphics.GraphicsDevice.RasterizerState;
            //RasterizerState rasterizerState = new RasterizerState();
            //rasterizerState.CullMode = CullMode.None;
            //graphics.GraphicsDevice.RasterizerState = rasterizerState;
            
            water.PreDraw(waterCamera, gameTime);
            sky.Draw(waterCamera.View, waterCamera.Projection, waterCamera.Position);
            //graphics.GraphicsDevice.RasterizerState = originalRasterizerState;
            terrain.Draw(camera.ViewMatrix, camera.ProjMatrix);
            water.Draw(waterCamera.View, waterCamera.Projection, waterCamera.Position);

            // Draw each players name at their positions in the game
            foreach (NetworkGamer networkGamer in networkSession.AllGamers)
            {
                DummyPlayer AnyPlayer = networkGamer.Tag as DummyPlayer;
                //
            }
            spriteBatch.End();
        }

        // Draw method for the FindSession GameState
        private void FindSessionDraw()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, bgScreenRect, Color.White);
            spriteBatch.DrawString(spriteFont, "FIND SESSION", new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(spriteFont, "Exit - Press Back", new Vector2(10, 50), Color.White);

            // Write message if there are no sessions found
            if (availableSessions.Count == 0)
            {
                spriteBatch.DrawString(spriteFont, "NO SESSIONS FOUND", new Vector2(10, 90), Color.White);
            }
            else
            {
                // Print out a list of the avaiable sessions
                int sessionIndex = 0;
                foreach (AvailableNetworkSession session in availableSessions)
                {
                    spriteBatch.DrawString(spriteFont, session.HostGamertag + " " +
                        session.OpenPublicGamerSlots +
                        ((sessionIndex == 0) ? " (PRESS A)" : ""),
                        new Vector2(10, 90 + sessionIndex * 40),
                        Color.White);
                    sessionIndex++;
                }
            }

            spriteBatch.End();
        }

        // Draw method for the GameLobby GameState
        private void GameLobbyDraw()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, bgScreenRect, Color.White);
            spriteBatch.DrawString(spriteFont, "GAME LOBBY", new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(spriteFont, "Mark Ready Status - Press A", new Vector2(10, 50), Color.White);
            spriteBatch.DrawString(spriteFont, "Exit - Press Back", new Vector2(10, 90), Color.White);
            if (networkSession.IsHost)
            {
                spriteBatch.DrawString(spriteFont, "Start Game - Press B", new Vector2(10, 130), Color.White);
            }

            // Draw all games in the lobby
            spriteBatch.DrawString(spriteFont, "PLAYERS IN LOBBY", new Vector2(10, 220), Color.White);

            float drawOffset = 0;
            foreach (NetworkGamer networkGamer in networkSession.AllGamers)
            {
                spriteBatch.DrawString(spriteFont, networkGamer.Gamertag + " - " + ((networkGamer.IsReady) ? "READY" : "NOT READY") + " " + ((networkGamer.IsTalking) ? "TALKING" : ""),
                    new Vector2(10, 260 + drawOffset), Color.White);
                drawOffset += 40;
            }

            spriteBatch.End();
        }

        private void CreateSessionDraw()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, bgScreenRect, Color.White);
            spriteBatch.DrawString(spriteFont, "CREATE SESSION", new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(spriteFont, "Deathmatch - Press A", new Vector2(10, 50), Color.White);
            spriteBatch.DrawString(spriteFont, "Exit - Press Back", new Vector2(10, 90), Color.White);
            spriteBatch.End();
        }

        private void MainMenuDraw()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, bgScreenRect, Color.White);
            spriteBatch.DrawString(spriteFont, "MAIN MENU", new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(spriteFont, "Create Session - Press A", new Vector2(10, 50), Color.White);
            spriteBatch.DrawString(spriteFont, "Find Session - Press B", new Vector2(10, 90), Color.White);
            spriteBatch.DrawString(spriteFont, "Exit - Press Back", new Vector2(10, 130), Color.White);
            spriteBatch.End();
        }

    }
}

