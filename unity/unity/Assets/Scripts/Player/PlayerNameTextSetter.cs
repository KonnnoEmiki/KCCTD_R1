using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class PlayerNameTextSetter : MonoBehaviour
{
	void Start()
	{
		var playerNameText = GetComponent<TextMeshPro>();
		if (playerNameText == null) return;
		playerNameText.text = string.Empty;
		
		var player = GetComponentInParent<Player>();
		playerNameText.text = player.OwnerName;
	}

}
