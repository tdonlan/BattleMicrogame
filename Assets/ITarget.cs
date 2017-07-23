using UnityEngine;
using System.Collections;

public interface ITarget
{
	void AddEffect (ItemEffect effect);

	void RemoveEffect (ItemEffect effect);

	bool Hit (int damage);

	void Heal (int amount);

	void Cure (int amount);

}

