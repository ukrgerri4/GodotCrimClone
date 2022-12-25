public class WeaponChangedEvent
{
  public WeaponChangedEvent(string weaponName)
  {
    WeaponName = weaponName;
  }

  public string WeaponName { get; private set; }
}