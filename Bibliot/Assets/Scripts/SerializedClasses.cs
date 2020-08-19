
using System;

namespace Serializables
{


    [Serializable]
    public class Vector3
    {
        public float x;
        public float y;
        public float z;

        public static implicit operator Vector3(UnityEngine.Vector3 vector3)
        {
            return new Vector3()
            {
                x = vector3.x,
                y = vector3.y,
                z = vector3.z,
            };
        }

        public static implicit operator UnityEngine.Vector3(Vector3 vector3)
        {
            return new UnityEngine.Vector3()
            {
                x = vector3.x,
                y = vector3.y,
                z = vector3.z,
            };
        }
    }

    [Serializable]
    public class Quaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public static implicit operator Quaternion(UnityEngine.Quaternion vector3)
        {
            return new Quaternion()
            {
                x = vector3.x,
                y = vector3.y,
                z = vector3.z,
                w = vector3.w,
            };
        }

        public static implicit operator UnityEngine.Quaternion(Quaternion vector3)
        {
            return new UnityEngine.Quaternion()
            {
                x = vector3.x,
                y = vector3.y,
                z = vector3.z,
                w = vector3.w,
            };
        }
    }
}