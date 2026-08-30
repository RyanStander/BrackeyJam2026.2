using UnityEngine;

namespace UI
{
    public static class GrievanceLabelResolver
    {
        public static string GetLabel(float grievance)
        {
            if (grievance < 12.5f) return "Content";
            if (grievance < 25f) return "Wary";
            if (grievance < 37.5f) return "Unsure";
            if (grievance < 50f) return "Suspicious";
            if (grievance < 62.5f) return "Resentful";
            if (grievance < 75f) return "Seething";
            if (grievance < 87.5f) return "Furious";
            return "Betrayed";
        }

        public static Color GetColor(float grievance)
        {
            float t = Mathf.Clamp01(grievance / 100f);
            return Color.Lerp(Color.green, Color.red, t);
        }
    }
}
