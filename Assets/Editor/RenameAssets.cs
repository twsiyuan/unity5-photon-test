using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Linq;
using UnityEngine.Assertions;
using System.Collections.Generic;

namespace AnimationClipExtensions
{

	public class RenameAssets : EditorWindow
	{
		private string oldName = string.Empty, newName = string.Empty, folderPath = null;
		bool isOpenTargets = true;

		const string MenuName = "Assets/RenameFiles";

		Vector2 scroll;

		[MenuItem (MenuName)]
		static void Init ()
		{
			var window = RenameAssets.GetWindow<RenameAssets> (true);
			window.folderPath = GetSelectedFolderPath ();
			window.Show ();
		}

		[MenuItem (MenuName, true)]
		static bool ValidateLogSelectedInit ()
		{
			var isSelectProjectView = (Selection.assetGUIDs != null && Selection.assetGUIDs.Length > 0);
			var isSelectDirectry = Directory.Exists (AssetDatabase.GetAssetPath (Selection.activeObject));

			return isSelectProjectView && isSelectDirectry;
		}

		void OnGUI ()
		{
			EditorGUILayout.LabelField ("path", folderPath);
			oldName = EditorGUILayout.TextField ("old Name", oldName);
			newName = EditorGUILayout.TextField ("new Name",newName);

			if (GUILayout.Button ("Rename")) {
				Rename (folderPath, oldName, newName);
				AssetDatabase.Refresh (ImportAssetOptions.ForceUpdate);
			}

			if (isOpenTargets) {

				using( var scrollScope = new EditorGUILayout.ScrollViewScope(scroll) ){
					scroll = scrollScope.scrollPosition;
					var files = GetSelectedPath(folderPath, oldName);
					foreach (var file in files) {
						var obj = AssetDatabase.LoadAssetAtPath<Object> (file);
						Assert.IsTrue(obj != null, file);
						EditorGUILayout.ObjectField (obj, obj.GetType (), false);
					}
				}
			}
		}

		static IEnumerable<string> GetSelectedPath (string path, string keyword)
		{
			return Directory.GetFiles (path)
				.Where( c=> Path.GetFileName( c ) != ".DS_Store")
				.Where (c => Path.GetExtension (c) != ".meta")
				.Where (c => Path.GetFileName (c).Contains (keyword));
		}

		static string GetSelectedFolderPath ()
		{
			var guid = Selection.assetGUIDs [0];
			return AssetDatabase.GUIDToAssetPath (guid);
		}

		static void Rename (string folderPath, string originalName, string newName)
		{
			var files = GetSelectedPath(folderPath, originalName);
			foreach (var file in files) {
				var newFileName = Path.GetFileName (file).Replace (originalName, newName);
				AssetDatabase.RenameAsset (file, newFileName);
			}
		}
	}

}