namespace Terra.EffectsSystem
{
    /// <summary>
    ///     Enum indicates where the effectData can be stored at.
    ///     <br></br>
    ///     Example: PoisonMeleeEffect can be stored only on MeleeWeapons.
    /// </summary>
    /// <remarks>Effects on weapons will affect their given targets.
    /// Effects on Entity will be executed instantly on the source</remarks>
    public enum ContainerType
    {
        None,
        AllWeapons,
        MeleeWeapon,
        RangedWeapon,
    }
}