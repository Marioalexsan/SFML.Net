using System;
using System.Runtime.InteropServices;
using System.Security;
using SFML.System;

namespace SFML.Audio
{
    ////////////////////////////////////////////////////////////
    /// <summary>
    /// Enumeration of all possible sound states
    /// </summary>
    ////////////////////////////////////////////////////////////
    public enum SoundStatus
    {
        /// <summary>Sound is not playing</summary>
        Stopped,

        /// <summary>Sound is paused</summary>
        Paused,

        /// <summary>Sound is playing</summary>
        Playing
    }

    ////////////////////////////////////////////////////////////
    /// <summary>
    /// Regular sound that can be played in the audio environment
    /// </summary>
    ////////////////////////////////////////////////////////////
    public class Sound : ObjectBase
    {
        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Default constructor (invalid sound)
        /// </summary>
        ////////////////////////////////////////////////////////////
        public Sound() :
            base(sfSound_create())
        {
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct the sound with a buffer
        /// </summary>
        /// <param name="buffer">Sound buffer containing the audio data to play with the sound</param>
        ////////////////////////////////////////////////////////////
        public Sound(SoundBuffer buffer) :
            base(sfSound_create()) => SoundBuffer = buffer;

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct the sound from another source
        /// </summary>
        /// <param name="copy">Sound to copy</param>
        ////////////////////////////////////////////////////////////
        public Sound(Sound copy) :
            base(sfSound_copy(copy.CPointer)) => SoundBuffer = copy.SoundBuffer;

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Start or resume playing the sound.
        ///
        /// This function starts the stream if it was stopped, resumes
        /// it if it was paused, and restarts it from beginning if it
        /// was it already playing.
        /// This function uses its own thread so that it doesn't block
        /// the rest of the program while the sound is played.
        /// </summary>
        ////////////////////////////////////////////////////////////
        public void Play() => sfSound_play(CPointer);

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Pause the sound.
        ///
        /// This function pauses the sound if it was playing,
        /// otherwise (sound already paused or stopped) it has no effect.
        /// </summary>
        ////////////////////////////////////////////////////////////
        public void Pause() => sfSound_pause(CPointer);

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Stop playing the sound.
        ///
        /// This function stops the sound if it was playing or paused,
        /// and does nothing if it was already stopped.
        /// It also resets the playing position (unlike pause()).
        /// </summary>
        ////////////////////////////////////////////////////////////
        public void Stop() => sfSound_stop(CPointer);

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Buffer containing the sound data to play through the sound.
        /// 
        /// It is important to note that the sound buffer is not copied,
        /// thus the SoundBuffer instance must remain alive as long
        /// as it is attached to the sound.
        /// </summary>
        ////////////////////////////////////////////////////////////
        public SoundBuffer SoundBuffer
        {
            get => _buffer;
            set { _buffer = value; sfSound_setBuffer(CPointer, value != null ? value.CPointer : IntPtr.Zero); }
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Current status of the sound (see SoundStatus enum)
        /// </summary>
        ////////////////////////////////////////////////////////////
        public SoundStatus Status => sfSound_getStatus(CPointer);

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Flag if the sound should loop after reaching the end.
        ///
        /// If set, the sound will restart from beginning after
        /// reaching the end and so on, until it is stopped or
        /// Loop = false is set.
        /// The default looping state for sounds is false.
        /// </summary>
        ////////////////////////////////////////////////////////////
        public bool Loop
        {
            get => sfSound_getLoop(CPointer);
            set => sfSound_setLoop(CPointer, value);
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Pitch of the sound.
        /// 
        /// The pitch represents the perceived fundamental frequency
        /// of a sound; thus you can make a sound more acute or grave
        /// by changing its pitch. A side effect of changing the pitch
        /// is to modify the playing speed of the sound as well.
        /// The default value for the pitch is 1.
        /// </summary>
        ////////////////////////////////////////////////////////////
        public float Pitch
        {
            get => sfSound_getPitch(CPointer);
            set => sfSound_setPitch(CPointer, value);
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Volume of the sound.
        /// 
        /// The volume is a value between 0 (mute) and 100 (full volume).
        /// The default value for the volume is 100.
        /// </summary>
        ////////////////////////////////////////////////////////////
        public float Volume
        {
            get => sfSound_getVolume(CPointer);
            set => sfSound_setVolume(CPointer, value);
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Current playing position of the sound.
        /// 
        /// The playing position can be changed when the sound is
        /// either paused or playing.
        /// </summary>
        ////////////////////////////////////////////////////////////
        public Time PlayingOffset
        {
            get => sfSound_getPlayingOffset(CPointer);
            set => sfSound_setPlayingOffset(CPointer, value);
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// 3D position of the sound in the audio scene.
        ///
        /// Only sounds with one channel (mono sounds) can be
        /// spatialized.
        /// The default position of a sound is (0, 0, 0).
        /// </summary>
        ////////////////////////////////////////////////////////////
        public Vector3f Position
        {
            get => sfSound_getPosition(CPointer);
            set => sfSound_setPosition(CPointer, value);
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Make the music's position relative to the listener or absolute.
        ///
        /// Making a sound relative to the listener will ensure that it will always
        /// be played the same way regardless the position of the listener.
        /// This can be useful for non-spatialized sounds, sounds that are
        /// produced by the listener, or sounds attached to it.
        /// The default value is false (position is absolute).
        /// </summary>
        ////////////////////////////////////////////////////////////
        public bool RelativeToListener
        {
            get => sfSound_isRelativeToListener(CPointer);
            set => sfSound_setRelativeToListener(CPointer, value);
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Minimum distance of the sound.
        ///
        /// The "minimum distance" of a sound is the maximum
        /// distance at which it is heard at its maximum volume. Further
        /// than the minimum distance, it will start to fade out according
        /// to its attenuation factor. A value of 0 ("inside the head
        /// of the listener") is an invalid value and is forbidden.
        /// The default value of the minimum distance is 1.
        /// </summary>
        ////////////////////////////////////////////////////////////
        public float MinDistance
        {
            get => sfSound_getMinDistance(CPointer);
            set => sfSound_setMinDistance(CPointer, value);
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Attenuation factor of the music.
        ///
        /// The attenuation is a multiplicative factor which makes
        /// the music more or less loud according to its distance
        /// from the listener. An attenuation of 0 will produce a
        /// non-attenuated sound, i.e. its volume will always be the same
        /// whether it is heard from near or from far. On the other hand,
        /// an attenuation value such as 100 will make the sound fade out
        /// very quickly as it gets further from the listener.
        /// The default value of the attenuation is 1.
        /// </summary>
        ////////////////////////////////////////////////////////////
        public float Attenuation
        {
            get => sfSound_getAttenuation(CPointer);
            set => sfSound_setAttenuation(CPointer, value);
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Provide a string describing the object
        /// </summary>
        /// <returns>String description of the object</returns>
        ////////////////////////////////////////////////////////////
        public override string ToString()
        {
            if (IsInvalid)
            {
                return MakeDisposedObjectString();
            }

            return "[Sound]" +
                   " Status(" + Status + ")" +
                   " Loop(" + Loop + ")" +
                   " Pitch(" + Pitch + ")" +
                   " Volume(" + Volume + ")" +
                   " Position(" + Position + ")" +
                   " RelativeToListener(" + RelativeToListener + ")" +
                   " MinDistance(" + MinDistance + ")" +
                   " Attenuation(" + Attenuation + ")" +
                   " PlayingOffset(" + PlayingOffset + ")" +
                   " SoundBuffer(" + SoundBuffer + ")";
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Handle the destruction of the object
        /// </summary>
        /// <param name="disposing">Is the GC disposing the object, or is it an explicit call ?</param>
        ////////////////////////////////////////////////////////////
        protected override void Destroy(bool disposing) => sfSound_destroy(CPointer);

        private SoundBuffer _buffer;

        #region Imports
        [DllImport(CSFML.Audio, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr sfSound_create();

        [DllImport(CSFML.Audio, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr sfSound_copy(IntPtr sound);

        [DllImport(CSFML.Audio, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern void sfSound_destroy(IntPtr sound);

        [DllImport(CSFML.Audio, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern void sfSound_play(IntPtr sound);

        [DllImport(CSFML.Audio, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern void sfSound_pause(IntPtr sound);

        [DllImport(CSFML.Audio, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern void sfSound_stop(IntPtr sound);

        [DllImport(CSFML.Audio, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern void sfSound_setBuffer(IntPtr sound, IntPtr buffer);

        [DllImport(CSFML.Audio, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern void sfSound_setLoop(IntPtr sound, bool loop);

        [DllImport(CSFML.Audio, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern bool sfSound_getLoop(IntPtr sound);

        [DllImport(CSFML.Audio, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern SoundStatus sfSound_getStatus(IntPtr sound);

        [DllImport(CSFML.Audio, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern void sfSound_setPitch(IntPtr sound, float pitch);

        [DllImport(CSFML.Audio, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern void sfSound_setVolume(IntPtr sound, float volume);

        [DllImport(CSFML.Audio, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern void sfSound_setPosition(IntPtr sound, Vector3f position);

        [DllImport(CSFML.Audio, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern void sfSound_setRelativeToListener(IntPtr sound, bool relative);

        [DllImport(CSFML.Audio, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern void sfSound_setMinDistance(IntPtr sound, float minDistance);

        [DllImport(CSFML.Audio, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern void sfSound_setAttenuation(IntPtr sound, float attenuation);

        [DllImport(CSFML.Audio, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern void sfSound_setPlayingOffset(IntPtr sound, Time timeOffset);

        [DllImport(CSFML.Audio, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern float sfSound_getPitch(IntPtr sound);

        [DllImport(CSFML.Audio, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern float sfSound_getVolume(IntPtr sound);

        [DllImport(CSFML.Audio, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern Vector3f sfSound_getPosition(IntPtr sound);

        [DllImport(CSFML.Audio, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern bool sfSound_isRelativeToListener(IntPtr sound);

        [DllImport(CSFML.Audio, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern float sfSound_getMinDistance(IntPtr sound);

        [DllImport(CSFML.Audio, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern float sfSound_getAttenuation(IntPtr sound);

        [DllImport(CSFML.Audio, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern Time sfSound_getPlayingOffset(IntPtr sound);
        #endregion
    }
}
