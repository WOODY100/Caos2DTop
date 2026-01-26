public interface IUsableItem
{
    float CooldownDuration { get; }
    bool CanUse(PlayerStats target);
    bool Use(PlayerStats target);
}