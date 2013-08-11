//audiomanager.cs

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

// keep track of active sounds, update their 3d settings as world changes, remove used sound effect instances
namespace FPSGame
{
    public class AudioManager : Microsoft.Xna.Framework.GameComponent
    {
        // List all possible sounds to be loaded in array
        public string[] soundNames =
        {
            "AssetCollection/Audio/AwpSound",
            "AssetCollection/Audio/pl_step1",
            "AssetCollection/Audio/pl_step2",
            "AssetCollection/Audio/pl_step3",
            "AssetCollection/Audio/pl_step4",
            "AssetCollection/Audio/headshot1",
            "AssetCollection/Audio/headshot2",
            "AssetCollection/Audio/headshot3",
            "AssetCollection/Audio/die1",
            "AssetCollection/Audio/die2",
            "AssetCollection/Audio/die3",
            "AssetCollection/Audio/dryfire_rifle",
            "AssetCollection/Audio/ric_conc-1",
            "AssetCollection/Audio/ric_conc-2",
            "AssetCollection/Audio/ric_metal-1",
            "AssetCollection/Audio/ric_metal-2",
            "AssetCollection/Audio/zoom",
        };
        int i = 0;

        // set listener to match camera. this is necessary for multiplayer.
        public AudioListener Listener
        {
            get { return listener; }
        }
        AudioListener listener = new AudioListener();


        // The emitter is anything that makes a 3D sound
        // this is necessary if you intend to use Emitter as an interface for all objects that make a sound
        // this is probably actually very important because you need to be able to connect them to this audiomanager
        // and the only way to do that is to have them be all one object type
        // or else it gets complicated quick
        AudioEmitter emitter = new AudioEmitter();

        // Store all the sound effects that are available to be played.
        Dictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();

        // 3d sounds that are playing
        List<ActiveSound> activeSounds = new List<ActiveSound>();

        //constructor
        public AudioManager(Game game)
            : base(game)
        { }

        //helper class - keeps track of active sounds, remembers emitters
        // used in list and only in this manager
        private class ActiveSound
        {
            public SoundEffectInstance Instance;
            public AudioEmitterInterface Emitter;
        }

        public override void Initialize()
        {
            // set 3D audio scale, match with size of game world
            // DistanceScale-> volume change in sounds with distance
            // DopplerScale-> pitch change for doppler effect
            SoundEffect.DistanceScale = 50f;
            SoundEffect.DopplerScale = 1.0f;

            // load sfx
            foreach (string soundName in soundNames)
            {
                soundEffects.Add(soundName, Game.Content.Load<SoundEffect>(soundName));
            }

            base.Initialize();
        }

        // C# memory management lol
        protected override void Dispose(bool remove)
        {
            try
            {
                if (remove)
                {
                    foreach (SoundEffect soundEffect in soundEffects.Values)
                    {
                        soundEffect.Dispose();
                    }
                    soundEffects.Clear();
                }
            }
            finally
            {
                base.Dispose(remove);
            }
        }

        public override void Update(GameTime gameTime)
        {
            // check out all the sounds
            int index = 0;

            while (index < activeSounds.Count)
            {
                ActiveSound activeSound = activeSounds[index];

                if (activeSound.Instance.State == SoundState.Stopped)
                {
                    // delete old sounds, remove them from list of active sounds
                    activeSound.Instance.Dispose();
                    activeSounds.RemoveAt(index);
                }
                else
                {
                    // if still active, reapply 3dfx
                    ManagerApply3D(activeSound);
                    index++;
                }
            }

            base.Update(gameTime);
        }

        public SoundEffectInstance Play3DSound(string soundName, bool isLooped, AudioEmitterInterface emitter)
        {
            // "name of sound", whether or not its looped sound, and what object is emitting the sound)
            ActiveSound activeSound = new ActiveSound();
            
            // select the correct sound, create an instance of it, determine whether its looped, select correct emitter
            activeSound.Instance = soundEffects[soundName].CreateInstance();
            activeSound.Instance.IsLooped = isLooped;
            activeSound.Emitter = emitter;
            /*
            System.Console.WriteLine("activeSound.Instance Position and Velocity V3s");
            System.Console.WriteLine(activeSound.Emitter.Position);
            System.Console.WriteLine("listener position");
            System.Console.WriteLine(listener.Position);
            System.Console.WriteLine("==============================================");  
            */
            // apply 3dfx and play, add it to actives list
            ManagerApply3D(activeSound);
            activeSound.Instance.Play();
            activeSounds.Add(activeSound);

            return activeSound.Instance;
        }

        private void ManagerApply3D(ActiveSound activeSound)
        {
            //update the emitter's vector data (position, facing, velocity)
            emitter.Position = activeSound.Emitter.Position;
            emitter.Forward = activeSound.Emitter.Forward;
            emitter.Up = activeSound.Emitter.Up;
            emitter.Velocity = activeSound.Emitter.Velocity; 
            
            if (i==20)
            {
                /*
                System.Console.WriteLine("Apply3D Position and Velocity V3s");
                System.Console.WriteLine(emitter.Position);
                System.Console.WriteLine(emitter.Velocity);
                System.Console.WriteLine("==================================");
                 * */
                i = 0;
            }
            i++;
            //applies 3dfx to any sound, usually just started or still continuing sounds
            activeSound.Instance.Apply3D(listener, emitter);
            //this is different from manager apply 3d, this is the apply 3d effects method of soundeffectinstances
        }

        
    }
}

