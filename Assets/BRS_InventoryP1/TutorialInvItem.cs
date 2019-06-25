using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialInvItem", menuName = "NewTutorialInvItem")]
public class TutorialInvItem : ScriptableObject
{

	public string Name;
	public string Description;

	public Texture2D Icon;
	public Texture2D Background;

	public enum ItemTypeEnum
	{
		Weapon, Healing, Throwable, Llama
	}

	public ItemTypeEnum ItemType;

	[Header("Weapon")]
	public float ReloadSpeed;
	public float FireRate;
	public float MaxAmmo;
	public float MagazineSize;
	public float Range;
}
