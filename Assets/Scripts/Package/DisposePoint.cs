// using UnityEngine;
//
//
// [RequireComponent(typeof(Collider))]
// public class DisposePoint : MonoBehaviour {
//     
//     [SerializeField] private Transform _dropoffPosition;
//     
//     private void OnTriggerEnter(Collider other)
//     {
//         var player = other.GetComponent<PlayerPackageHandler>();
//         if (!player || !player.currentPackage) return;
//         player.currentPackage.DeliverPackage(transform,_dropoffPosition);
//         player.DropPackage();
//     }
// }