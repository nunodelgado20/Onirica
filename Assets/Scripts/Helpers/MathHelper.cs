using UnityEngine;

namespace Helpers
{
    public enum Plane
    {
        All,
        XZ,
        XY,
        YZ
    }
    
    public struct MathHelper
    {
        public static float Distance(Vector3 destination, Vector3 origin, Plane plane = Plane.All)
        {
            var distance = destination - origin;
            
            if(plane == Plane.All) return distance.magnitude;
            
            if (plane == Plane.XY) distance.z = 0f;
            else if (plane == Plane.XZ) distance.y = 0f;
            else if (plane == Plane.YZ) distance.x = 0f;

            return distance.magnitude;
        }

        public static Vector3 Direction(Vector3 destination, Vector3 origin, Plane plane = Plane.All)
        {
            var direction = destination - origin;
            
            if(plane == Plane.All) return direction;
            
            if (plane == Plane.XY) direction.z = 0f;
            else if (plane == Plane.XZ) direction.y = 0f;
            else if (plane == Plane.YZ) direction.x = 0f;

            return direction;
            
        }

        public static Vector3 VectorOffset(Vector3 destination, Vector3 origin, float offSet, bool offSetFromDestination = false)
        {
            Vector3 direction = Direction(destination, origin).normalized;
            Vector3 offSetVector = Vector3.zero;
            
            if (offSetFromDestination)
            {
                offSetVector = destination - direction * offSet;
            }
            else
            {
                offSetVector = origin + direction * offSet;
            }

            return offSetVector;
        }
        
        public static Quaternion QuaternionSmoothDamp(Quaternion rot, Quaternion target, ref Quaternion deriv, float time)
        {
            //FRom https://gist.github.com/maxattack/4c7b4de00f5c1b95a33b
            if (Time.deltaTime < Mathf.Epsilon) return rot;
            // account for double-cover
            var Dot = Quaternion.Dot(rot, target);
            var Multi = Dot > 0f ? 1f : -1f;
            target.x *= Multi;
            target.y *= Multi;
            target.z *= Multi;
            target.w *= Multi;
            // smooth damp (nlerp approx)
            var Result = new Vector4(
                Mathf.SmoothDamp(rot.x, target.x, ref deriv.x, time),
                Mathf.SmoothDamp(rot.y, target.y, ref deriv.y, time),
                Mathf.SmoothDamp(rot.z, target.z, ref deriv.z, time),
                Mathf.SmoothDamp(rot.w, target.w, ref deriv.w, time)
            ).normalized;

            // ensure deriv is tangent
            var derivError = Vector4.Project(new Vector4(deriv.x, deriv.y, deriv.z, deriv.w), Result);
            deriv.x -= derivError.x;
            deriv.y -= derivError.y;
            deriv.z -= derivError.z;
            deriv.w -= derivError.w;

            return new Quaternion(Result.x, Result.y, Result.z, Result.w);
        }
        
        
        public static float ClampAngle(float angle, float maxAngle, float minAngle, float intermediateAngle)
        {
            if (angle > intermediateAngle && angle < maxAngle)
            {
                return maxAngle;
            }

            if (angle < intermediateAngle && angle > minAngle)
            {
                return minAngle;
            }

            return angle;
        }
    }
}