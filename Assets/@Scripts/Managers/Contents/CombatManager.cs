using UnityEngine;
using static Define;

public class CombatManager
{
    
    public void PlayHit(Vector2 pos, bool flip = false)
    {
        Managers.Effect.Play("HitEffect", pos, flip);
    }
    public void PlayCrash(Vector2 pos, bool flip = false)
    {
        Managers.Effect.Play("CrashEffect", pos, flip);
    }
    public void PlaySlash(Vector2 pos, float angle, bool flip = false)
    {
        Managers.Effect.Play("SlashEffect", pos, angle, flip);
    }

    public void PlayArmorBreak(Vector2 pos, bool flip = false)
    {
        Managers.Effect.Play("ArmorBreak", pos, flip);
    }
    public void PlaySlashBreak(Vector2 pos, float angle, bool flip = false)
    {
        Managers.Effect.Play("SlashCrashEffect", pos, angle, flip);
    }

    public void PlayArmorCrash(Vector2 pos, bool flip = false)
    {
        Managers.Effect.Play("ArmorCrash", pos, flip);
        Managers.Effect.Play("ArmorIconCrash", pos, flip);
    }
}
