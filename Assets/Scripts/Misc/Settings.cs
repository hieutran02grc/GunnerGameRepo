using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
	#region ROOM SETTINGS
	public const float fadeInTime = 0.5f; // time to fade in the room
	public const int maxChildCorridors = 3; // Max number of child corridors leading from a room. - maximum should be 3 although this is not recommended since it can cause the dungeon building to fail since the rooms are more likely to not fit together;
	public const float doorUnlockDelay = 1f;
	#endregion
}
