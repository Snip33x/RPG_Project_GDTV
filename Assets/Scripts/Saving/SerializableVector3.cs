using UnityEngine;

namespace RPG.Saving
{
    [System.Serializable]  //serialization is proces of taking important data , and turning it into binary, and once data is in binary we can store it, send save etc - oposite process is deserialization
    public class SerializableVector3
    {
        float x, y, z;  // float (single precision) = 32 bits 6-9 digits // double (precision) = 64 bits 15-17 digits, double is much more precise, but takes longer for computer to utilize, thats why we mostly use float in game dev

        public SerializableVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public Vector3 ToVector()
        {
            return new Vector3(x, y, z);
        }
    }
}