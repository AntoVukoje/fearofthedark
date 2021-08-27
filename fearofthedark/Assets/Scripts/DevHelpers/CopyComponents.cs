using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class CopyComponents : MonoBehaviour
{
	public enum Direction
	{
		Up, Down, Right, Left, Forward, Back
	}

	#region Display
	public Transform objectToCopyTransform;
	[Space]
	public Transform useAsParent;
	[Space]
	public Vector3 offset;
	public Direction direction;
	[Space]
	public int number;
	[Space]
	public MeshRenderer[] meshRenderers;
	public bool removeChanges;
	[Space]
	public bool apply;
	#endregion

	private Vector3 meshRendererSize;
	private List<GameObject> resetList = new List<GameObject>();
	private GameObject originalPrefab;

	private void Update()
	{
		if (objectToCopyTransform == null) return;

		Bounds objectBounds = new Bounds(objectToCopyTransform.position, Vector3.zero);

		originalPrefab = PrefabUtility.GetCorrespondingObjectFromSource(objectToCopyTransform.gameObject);

		meshRenderers = originalPrefab.GetComponentsInChildren<MeshRenderer>();

		foreach (var mesh in meshRenderers)
		{
			objectBounds.Encapsulate(mesh.bounds);
		}

		meshRendererSize = objectBounds.size;

		if (apply)
		{
			apply = false;

			var position = objectToCopyTransform.position;
			for (int i = 0; i < number + 1; i++)
			{

				resetList.Add(PrefabUtility.InstantiatePrefab(originalPrefab, useAsParent) as GameObject);
				resetList[i].transform.position = position;

				ChangeAxies(ref position, ref meshRendererSize);
				position += offset;
			}
			DestroyImmediate(resetList[0]);
			resetList.RemoveAt(0);
		}

		if (removeChanges)
		{
			removeChanges = false;
			for (int i = 0; i < resetList.Count; i++)
			{
				DestroyImmediate(resetList[i]);
			}
			resetList.Clear();
		}

		Draw();
	}


	private void Draw()
	{
		Vector3 drawEnd = objectToCopyTransform.position;

		for (int i = 0; i < number + 1; i++)
		{
			var lastPosition = drawEnd;
			ChangeAxies(ref drawEnd, ref meshRendererSize);
			Debug.DrawLine(lastPosition, drawEnd, Color.green);
			drawEnd += offset;
		}
	}

	private void ChangeAxies(ref Vector3 position, ref Vector3 meshSize)
	{
		switch (direction)
		{
			case Direction.Up:
				position.y += meshSize.y;
				break;
			case Direction.Down:
				position.y -= meshSize.y;
				break;
			case Direction.Right:
				position.x += meshSize.x;
				break;
			case Direction.Left:
				position.x -= meshSize.x;
				break;
			case Direction.Forward:
				position.z += meshSize.z;
				break;
			case Direction.Back:
				position.z -= meshSize.z;
				break;
		}
	}
}
