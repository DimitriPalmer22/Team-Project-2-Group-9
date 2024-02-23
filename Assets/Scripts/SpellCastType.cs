using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A list of the different spell types the player can use
/// </summary>
public enum SpellCastType
{
    None,
    Freeze,
    Invisibility,
    Teleport,
}

/// <summary>
/// Static helper class for the SpellCastType enum
/// </summary>
public static class SpellCastTypeHelper
{
    /// <summary>
    /// Returns a boolean based on if the spell is active for a duration or single use.
    /// </summary>
    /// <param name="spellType"></param>
    /// <returns></returns>
    public static bool IsDurationSpellType(this SpellCastType spellType)
    {
        return spellType == SpellCastType.Invisibility;
    }
}
