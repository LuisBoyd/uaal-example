using DataStructures;
using UnityEngine.UIElements;

namespace RCR.Systems.ProgressSystem
{
    public interface IProgressSystem
    {
        int Start(string name, string description,ProgressBar bar,
            int parentID = -1);

        int Remove(int id);
        void Cancel(int id);
        void Report(int id, float progress);
        void Report(int id, float progress, string description);
        void GetStartDateTime(int id);
        UnityDateTime? GetRemainingTime(int id);
        int GetParentID(int id);
        string GetDescription(int id);
        string GetName(int id);
        bool Exists(int id);
    }
}