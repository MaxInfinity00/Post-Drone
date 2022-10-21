// using UnityEngine;
// using UnityEditor;
//
//     
// [CustomEditor (typeof(PackageRequestManager))]
// public class PackageRequestManagerEditor : Editor
// {
//     public override void OnInspectorGUI()
//     {
//         base.OnInspectorGUI();
//         PackageRequestManager manager = (PackageRequestManager)target;
//         
//         if(GUILayout.Button("Update Dropoff Points")){
//             manager.UpdateDropoffPoints();
//         }
//     }
// }