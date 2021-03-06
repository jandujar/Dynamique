﻿using UnityEngine;
using System.Collections;

public class ScrollTexture : MonoBehaviour
{
	[SerializeField] float scrollSpeed = 0.5F;

	void Update()
	{
		float offset = Time.time * scrollSpeed;
		GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
	}
}