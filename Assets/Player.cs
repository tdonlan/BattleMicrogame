using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : ITarget
{
	private GameControllerScript gameController;

	public string Name;

	public int XP;

	public int MaxLevel = 50;

	public int Level;
	public int TotalHP;
	public int HP;

	public int Gold = 0;
	public List<Item> itemList;
	public List<Item> usableItemList = new List<Item> ();
	public Weapon weapon = null;
	public Armor armor = null;

	public int Damage {
		get {
			if (weapon == null) {
				return Level * 5;
			} else {
				return weapon.Damage;
			}
		}
	}

	public int Defense {
		get { 
			if (armor == null) {
				return 0;
			} else {
				return armor.Defense;
			}
		}
	}

	public List<ItemEffect> effectList = new List<ItemEffect> ();

	private List<int> XPCurve = new List<int> () {
		0, 
		0,
		1000,
		2569,
		4922,
		8059,
		11980,
		16686,
		22176,
		28451,
		35510,
		43353,
		51980,
		61392,
		71588,
		82569,
		94333,
		106882,
		120216,
		134333,
		149235,
		164922,
		181392,
		198647,
		216686,
		235510,
		255118,
		275510,
		296686,
		318647,
		341392,
		364922,
		389235,
		414333,
		440216,
		466882,
		494333,
		522569,
		551588,
		581392,
		611980,
		643353,
		675510,
		708451,
		742176,
		776686,
		811980,
		848059,
		884922,
		922569,
		1000000
	};

	public int HPSliderValue {
		get {
			return Mathf.RoundToInt (((float)HP / (float)TotalHP) * 100);
		}
	}

	public Player ()
	{
		this.Name = "Player";
		this.TotalHP = 100;
		this.HP = this.TotalHP;
		this.Level = 1;
		this.XP = 0;

		itemList = new List<Item> ();

		itemList.Add (ItemFactory.getHealingPotion ());
		itemList.Add (ItemFactory.getRegenPotion ());
		itemList.Add (ItemFactory.getGrenade ());
		itemList.Add (ItemFactory.getPoison ());
	}

	public void AttachGameController (GameControllerScript gameController)
	{
		this.gameController = gameController;
	}

	public string GetStats ()
	{
		return string.Format ("Level: {3}\nXP: {4}/{5}\nHP: {0}/{1}\n" +
		"Dmg:{2}\n" +
		"Def:{7} \n" +
		"Gold: {6}", HP, TotalHP, Damage, Level, XP, getXPNextLevel (), Gold, Defense);
	}

	public override string ToString ()
	{
		return string.Format ("{0}\n{1}", this.Name, GetStats ());
	}

	public void AddEffect (ItemEffect effect)
	{
		var newEffect = effect.GetCopy ();
		newEffect.ApplyEffect (this);
		if (newEffect.Duration > 0) {
			this.effectList.Add (newEffect);
		}
	}

	public void RemoveEffect (ItemEffect effect)
	{
		this.effectList.Remove (effect);	
	}

	public bool Hit (int damage)
	{
		damage = Mathf.Clamp (damage - Defense, 0, 999999); //directly apply defense for now
		gameController.DisplayDmg (false, damage);

		this.HP -= damage;
		if (this.HP <= 0) {
			return true;
		}
		return false;
	}

	public void Heal (int amt)
	{
		gameController.DisplayDmg (false, amt * -1);

		this.HP += amt;
		if (this.HP > this.TotalHP) {
			this.HP = this.TotalHP;
		}
	}

	public void Cure (int amount)
	{
		while (amount > 0 && effectList.Count > 0) {
			amount--;
			effectList.RemoveAt (0);
		}
	}

	public void UpdateEffects ()
	{
		for (int i = effectList.Count - 1; i >= 0; i--) {
			effectList [i].ApplyEffect (this);
		}
	}

	//-----Inventory

	public List<Item> GetWeapons ()
	{

		var wepList = from i in itemList
		              where i is Weapon
		              select i;
		return wepList.ToList ();

	}

	public List<Item> GetArmor ()
	{
		var armorList = from i in itemList
		                where i is Armor
		                select i;
		return armorList.ToList ();

	}

	public List<Item> GetItems ()
	{
		var iList = from i in itemList
		            where (!(i is Armor)) && (!(i is Weapon))
		            select i;
		return iList.ToList ();
	}

	public Item EquipItem (Item i)
	{
		Item oldItem = null;
		if (i is Weapon) {
			oldItem = equipWeapon (i);
		} else if (i is Armor) {
			oldItem = equipArmor (i);
		} else {
			oldItem = equipItem (i);
		}
		return oldItem;
	}

	private Item equipWeapon (Item i)
	{
		Item oldItem = null;
		if (weapon != null) {
			oldItem = weapon;
			this.itemList.Add (weapon);
		}
		weapon = (Weapon)i;
		itemList.Remove (i);
		return oldItem;
	}

	private Item equipArmor (Item i)
	{
		Item oldItem = null;
		if (armor != null) {
			oldItem = armor;
			this.itemList.Add (armor);
		}
		armor = (Armor)i;
		itemList.Remove (i);
		return oldItem;
	}

	private Item equipItem (Item i)
	{
		Item oldItem = null;
		if (usableItemList.Count >= 4) {
			oldItem = usableItemList [0];
			usableItemList.RemoveAt (0);
		}

		itemList.Remove (i);
		usableItemList.Add (i);
		return oldItem;
	}

	//-------- XP Calculations


	//Not used
	private List<int> getXPCurve ()
	{
		List<int> xpCurve = new List<int> ();
		xpCurve.Add (0); //level 0
		xpCurve.Add (0); //level 1

		int xpNeeded = 100;
		for (int i = 2; i < this.MaxLevel / 4; i++) {
			xpCurve.Add (xpNeeded);
			xpNeeded += Mathf.RoundToInt ((float)xpNeeded * 1.01f);
		}
		for (int i = this.MaxLevel / 4; i < this.MaxLevel / 2; i++) {
			xpCurve.Add (xpNeeded);
			xpNeeded += Mathf.RoundToInt ((float)xpNeeded * .5f);
		}
		for (int i = this.MaxLevel / 2; i <= this.MaxLevel; i++) {
			xpCurve.Add (xpNeeded);
			xpNeeded += Mathf.RoundToInt ((float)xpNeeded * .1f);
		}
		return xpCurve;
	}

	public void GetXP (int amount)
	{
		this.XP += amount;
		while (this.Level <= this.MaxLevel && this.XP >= XPCurve [this.Level + 1]) {
			this.LevelUp ();
		}
	}

	private int getXPNextLevel ()
	{
		if (this.Level <= this.MaxLevel) {
			return XPCurve [Level + 1];
		}
		return XP;
	}

	public void LevelUp ()
	{
		if (this.Level < this.MaxLevel) {
			this.Level++;
			this.TotalHP = this.Level * 50;
			this.HP = this.TotalHP;

		}
	}

}
