namespace TechMec.Models
{
    public enum PatientStatus
    {
        Stable = 1,
        Observation = 2,
        Critical = 3,
        Discharged = 4
    }

    public enum HandoverShift
    {
        Morning = 1,
        Afternoon = 2,
        Night = 3
    }
}