using UnityEngine;

namespace NetworkMenu.Validators
{
    public class RoomCreateValidator : MonoBehaviour
    {
        public static bool IsVoidName(string roomName)
        {
            if (roomName == "")
                return true;

            return false;
        }
    }
}