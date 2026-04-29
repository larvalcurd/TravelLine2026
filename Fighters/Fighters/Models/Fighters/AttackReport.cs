namespace Fighters.Models.Fighters
{
    public record AttackReport(
        string AttackerName,
        string DefenderName,
        bool IsCritical,
        int DamageDealt,
        bool DefenderDied,
        int BaseDamage,
        double Multiplier
    );
}