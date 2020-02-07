using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Threading;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class tts_android : MonoBehaviour
{
    //Get tts result, genereate audio clip from byte
    public class ActivityListener : AndroidJavaProxy
    {
        private List<AudioClip> audioClipQueue = new List<AudioClip>();

        public ActivityListener() : base("interfaces.UnityListener")
        {
        }

        public void onActivityResult(int requestCode, int resultCode, AndroidJavaObject data)
        {
            byte[] buffer = data.Call<byte[]>("getBytes");
            Debug.Log("unity onActivityResult data:" + buffer.Length);

            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Write(new byte[44], 0, 44);
            memoryStream.Write(buffer, 0, buffer.Length);
            WAVE_Header wave_Header = getWave_Header((int)memoryStream.Length);
            byte[] array2 = StructToBytes(wave_Header);
            memoryStream.Position = 0L;//set pointer to start
            memoryStream.Write(array2, 0, array2.Length);//存储结构体的字节数组
            memoryStream.Position = 0L;//set pointer to start 

            Debug.Log("unity onActivityResult buffer:"+memoryStream.GetBuffer().Length);

            AudioClip ac = FromWavData(memoryStream.GetBuffer());
            audioClipQueue.Add(ac);
        }

        public List<AudioClip> getAudioClipQueue(){
            return audioClipQueue;
        }
    }

    public ActivityListener activityListener;
    private List<AudioClip> audioList;

    private void Start()
    {
        activityListener = new ActivityListener();
        AndroidActivity.androidActivity.Call("setUnityListener", activityListener);

        audioList = activityListener.getAudioClipQueue();
    }

    private void Update()
    {
        
    }

    public void ClearAudioList(){
        audioList.Clear();
        //Debug.Log("tts_android ClearAudioList audioList length:"+audioList.Count);
    }

    public int GetAudioClipNum(){
        //Debug.Log("tts_android audiolist size:"+audioList.Count);
        return audioList.Count;
    }

    public AudioClip GetAudioClip(int i){
        return audioList[i];
    }

    private static byte[] StructToBytes(object structure)
    {
        int num = Marshal.SizeOf(structure);
        IntPtr intPtr = Marshal.AllocHGlobal(num);
        byte[] result;
        try
        {
            Marshal.StructureToPtr(structure, intPtr, false);
            byte[] array = new byte[num];
            Marshal.Copy(intPtr, array, 0, num);
            result = array;
        }
        finally
        {
            Marshal.FreeHGlobal(intPtr);
        }
        return result;
    }

    private static WAVE_Header getWave_Header(int data_len)
    {
        return new WAVE_Header
        {
            RIFF_ID = 1179011410,//相当于SDK例子中的"RIFF"，只是转换成了对应的ASCL值
            File_Size = data_len - 8,
            RIFF_Type = 1163280727,
            FMT_ID = 544501094,
            FMT_Size = 16,
            FMT_Tag = 1,
            FMT_Channel = 1,
            FMT_SamplesPerSec = 16000,
            AvgBytesPerSec = 32000,
            BlockAlign = 2,
            BitsPerSample = 16,
            DATA_ID = 1635017060,
            DATA_Size = data_len - 44
        };
    }

    private struct WAVE_Header
    {
        public int RIFF_ID;
        public int File_Size;
        public int RIFF_Type;
        public int FMT_ID;
        public int FMT_Size;
        public short FMT_Tag;
        public ushort FMT_Channel;
        public int FMT_SamplesPerSec;
        public int AvgBytesPerSec;
        public ushort BlockAlign;
        public ushort BitsPerSample;
        public int DATA_ID;
        public int DATA_Size;
    }

    public static AudioClip FromWavData(byte[] data)
    {
        WAV wav = new WAV(data);
        AudioClip audioClip = AudioClip.Create("wavclip", wav.SampleCount, 1, wav.Frequency, false);
        audioClip.SetData(wav.LeftChannel, 0);
        return audioClip;
    }

    public class WAV
    {
        // convert two bytes to one float in the range -1 to 1
        static float bytesToFloat(byte firstByte, byte secondByte)
        {
            // convert two bytes to one short (little endian)
            short s = (short)((secondByte << 8) | firstByte);
            // convert to range from -1 to (just below) 1
            return s / 32768.0F;
        }

        static int bytesToInt(byte[] bytes, int offset = 0)
        {
            int value = 0;
            for (int i = 0; i < 4; i++)
            {
                value |= ((int)bytes[offset + i]) << (i * 8);
            }
            return value;
        }
        // properties  
        public float[] LeftChannel { get; internal set; }
        public float[] RightChannel { get; internal set; }
        public int ChannelCount { get; internal set; }
        public int SampleCount { get; internal set; }
        public int Frequency { get; internal set; }

        public WAV(byte[] wav)
        {

            // Determine if mono or stereo  
            ChannelCount = wav[22];     // Forget byte 23 as 99.999% of WAVs are 1 or 2 channels  
            Debug.Log("audioclip ChannelCount:"+ChannelCount);

            // Get the frequency  
            Frequency = bytesToInt(wav, 24);
            Debug.Log("audioclip frequency:"+Frequency);
            // Get past all the other sub chunks to get to the data subchunk:  
            int pos = 12;   // First Subchunk ID from 12 to 16  

            // Keep iterating until we find the data chunk (i.e. 64 61 74 61 ...... (i.e. 100 97 116 97 in decimal))  
            while (!(wav[pos] == 100 && wav[pos + 1] == 97 && wav[pos + 2] == 116 && wav[pos + 3] == 97))
            {
                pos += 4;
                int chunkSize = wav[pos] + wav[pos + 1] * 256 + wav[pos + 2] * 65536 + wav[pos + 3] * 16777216;
                pos += 4 + chunkSize;
            }
            pos += 8;

            // Pos is now positioned to start of actual sound data.  
            SampleCount = (wav.Length - pos) / 2;     // 2 bytes per sample (16 bit sound mono)  
            if (ChannelCount == 2) SampleCount /= 2;        // 4 bytes per sample (16 bit stereo)
            Debug.Log("audioclip SampleCount:"+SampleCount);

            // Allocate memory (right will be null if only mono sound)  
            LeftChannel = new float[SampleCount];
            if (ChannelCount == 2) RightChannel = new float[SampleCount];
            else RightChannel = null;

            // Write to double array/s:  
            int i = 0;
            int maxInput = wav.Length - (RightChannel == null ? 1 : 3);
            // while (pos < wav.Length)  
            while ((i < SampleCount) && (pos < maxInput))
            {
                LeftChannel[i] = bytesToFloat(wav[pos], wav[pos + 1]);
                pos += 2;
                if (ChannelCount == 2)
                {
                    RightChannel[i] = bytesToFloat(wav[pos], wav[pos + 1]);
                    pos += 2;
                }
                i++;
            }
        }
    }
}
